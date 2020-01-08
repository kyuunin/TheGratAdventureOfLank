using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGen : MonoBehaviour
{
    public GameObject mainChar;
    public Vector3 charPos;
    public Quaternion charDir = Quaternion.identity;
    public GameObject LevelStart;
    public GameObject LevelEnd;
    public GameObject[] rooms;
    public float[] weight;
    public int targetSize;
    public float space;
    public float bridgeProbability;
    public GameObject CamPrefeb;
    public Shader DefaultShader;
    public int numberOptimizerPasses = 100;
    public (List<Room>, Dictionary<Room, int>) InScene;
    public GameObject PointDisplay;
    public Vector2 MapPosition;
    public Vector2 MapScale;

    void AddToScene(Room room)
    {
        var n = InScene.Item1.Count;
        InScene.Item1.Add(room);
        InScene.Item2.Add(room, n);
    }

    float[,] InitMatrix()
    {
        var n = InScene.Item1.Count;
        float[,] matrix = new float[n, n];
        for (var i = 0; i < n; ++i)
        {
            for (var j = 0; j < n; ++j)
            {
                matrix[i, j] = i == j ? 0 : float.PositiveInfinity;
            }
        }
        for (var i = 0; i < n; ++i)
        {
            foreach (var plane in InScene.Item1[i].planes)
            {
                var j = InScene.Item2[plane.Brother.Parent];
                matrix[i, j] = 1;
            }
        }
        return matrix;
    }

    float[,] DistMatrix()
    {
        var n = InScene.Item1.Count;
        float[,] matrix = InitMatrix();
        float[,] buffer = (float[,])matrix.Clone();
        for (var l = 1; l < n; l *= 2)
        {
            for (var i = 0; i < n; ++i)
            {
                for (var j = 0; j < n; ++j)
                {
                    if (matrix[i, j] != float.PositiveInfinity)
                    {
                        buffer[i, j] = matrix[i, j];
                        continue;
                    }
                    for (var k = 0; k < n; ++k)
                    {
                        var tmp = matrix[i, k] + matrix[k, j];
                        if (buffer[i, j] > tmp)
                        {
                            buffer[i, j] = tmp;
                        }
                    }
                }
            }
            (matrix, buffer) = (buffer, matrix);
        }
        PrintMatrix(matrix);
        return matrix;
    }

    void PrintMatrix(float[,] matrix)
    {
        var n = InScene.Item1.Count;
        var res = "";
        for (var i = 0; i < n; ++i)
        {
            for (var j = 0; j < n; ++j)
            {
                res += (matrix[i, j] == float.PositiveInfinity ? "_," : matrix[i, j] + ",");
            }
            res += "\n";
        }
        Debug.Log(res);
    }

    Vector2[] CreateGraph(float[,] matrix)
    {
        var n = InScene.Item1.Count;
        Vector2[] graph = new Vector2[n];
        for (var i = 0; i < n; ++i)
        {
            graph[i].x = Random.Range(0f, 1f);
            graph[i].y = Random.Range(0f, 1f);
        }
        Vector2[] buffer = new Vector2[n];
        for (var k = 0; k < numberOptimizerPasses; ++k)
        {
            for (var i = 0; i < n; ++i)
            {
                buffer[i] = Vector2.zero;
                for (var j = 0; j < n; ++j)
                {
                    var tmp = graph[i] - graph[j];
                    tmp.Normalize();
                    buffer[i] += matrix[i, j] * tmp / n;
                }
            }
            (graph, buffer) = (buffer, graph);
        }
        var x = "";
        for (var i = 0; i < n; ++i)
        {
            x += graph[i] + "\n";
        }
        Debug.Log(x);
        return graph;
    }

    void Lank(Plane p1, Plane p2)
    {
        p1.Brother = p2;
        p2.Brother = p1;
    }

    static Plane RandomPop(List<Plane> plane)
    {
        int i = Random.Range(0, plane.Count);
        Plane tmp = plane[i];
        plane[i] = plane[plane.Count - 1];
        plane.RemoveAt(plane.Count - 1);
        return tmp;
    }

    void InitWeight()
    {
        if (weight.Length != rooms.Length) throw new System.Exception("weight and rooms should have the same length");
        for (var i = 1; i < weight.Length; i++)
        {
            weight[i] += weight[i - 1];
        }
    }

    GameObject GetRoom()
    {
        var val = Random.Range(0, weight[weight.Length - 1]);
        for (var i = 0; i < weight.Length; ++i)
        {
            if (weight[i] >= val)
            {
                return rooms[i];
            }
        }
        throw new System.Exception("something didn't work");
    }

    private Room generatedStartRoom;

    Vector3 CreateVector(int i)
    {
        int width = (int)Mathf.Ceil(Mathf.Pow(targetSize, 0.33f));
        int hight = width;

        var y = i / width % hight;
        var x = i % width;
        var z = i / width / hight;

        return new Vector3(space * x, space * y, space * z);
    }

    void PushAll(List<Plane> queue, Room room, int block = -1)
    {

        for (var j = 0; j < room.planes.Length; ++j)
        {
            room.planes[j].Parent = room;
            room.planes[j].cam = CamPrefeb;
            if (j != block)
            {
                queue.Add(room.planes[j]);
            }
        }
    }

    (GameObject, Room) CreateRoom(GameObject RoomPrefab, int i)
    {
        GameObject obj = Object.Instantiate(RoomPrefab, CreateVector(i), Quaternion.identity);
        Room room = obj.GetComponent<Room>();
        room.CamPrefeb = CamPrefeb;
        room.DefaultShader = DefaultShader;
        return (obj, room);
    }

    private void Awake()
    {
        InScene.Item1 = new List<Room>();
        InScene.Item2 = new Dictionary<Room, int>();
        InitWeight();
        int i;
        List<Plane> queue = new List<Plane>();
        {
            (var obj, var room) = CreateRoom(LevelStart, 0);
            room.isFirst = true;
            PushAll(queue, room);
            generatedStartRoom = room;
            AddToScene(room);
        }

        for (i = 1; i < targetSize; ++i)
        {

            var exitPlane = RandomPop(queue);
            Plane entPlane;
            if (queue.Count > 1 && Random.Range(0.0f, 1.0f) < bridgeProbability) //Connect 2 existing rooms
            {
                entPlane = RandomPop(queue);
            }
            else //Create New Room
            {
                GameObject obj;
                Room room;
                int currIter = 0;
                while (true)
                {
                    (obj, room) = CreateRoom(GetRoom(), i);
                    //check if posible Level Layout
                    if (queue.Count == 0 && room.planes.Length == 1)
                    {
                        Destroy(obj);
                        if (currIter++ == 100)
                        {
                            throw new System.Exception("coundn't get next room");
                        }
                    }
                    else
                    {
                        AddToScene(room);
                        break;
                    }
                }
                int entId = Random.Range(0, room.planes.Length); //choose entrance
                entPlane = room.planes[entId];
                PushAll(queue, room, entId);
            }
            Lank(entPlane, exitPlane);
        }
        if (queue.Count == 0)
            throw new System.Exception("Es gibt keine Räume mehr");
        var currIter2 = 0;
        while (true)
        {
            if (queue.Count % 2 == 1)
            {
                break;
            }
            if (currIter2++ == 100)
            {
                throw new System.Exception("coundn't get next room");
            }
            var exitPlane = RandomPop(queue);
            (var obj, var room) = CreateRoom(GetRoom(), i);
            AddToScene(room);
            int entId = Random.Range(0, room.planes.Length); //choose entrance
            var entPlane = room.planes[entId];
            PushAll(queue, room, entId);
            Lank(entPlane, exitPlane);
            ++i;
        }
        {
            var exitPlane = RandomPop(queue);
            (var obj, var room) = CreateRoom(LevelEnd, i);
            AddToScene(room);
            var entPlane = room.planes[0];
            entPlane.Parent = room;
            Lank(entPlane, exitPlane);
            ++i;
        }

        while (queue.Count != 0)
        {

            var exitPlane = RandomPop(queue);
            var entPlane = RandomPop(queue);
            Lank(entPlane, exitPlane);

        }

        Object.Instantiate(mainChar, charPos, charDir);
        var graph = CreateGraph(DistMatrix());
        foreach (var p in graph)
        {
            var tmp = Object.Instantiate(PointDisplay, new Vector3(0, 0, 0), Quaternion.identity);
            Debug.Log(p);
            p.Scale(MapScale);
            tmp.transform.Find("Point").GetComponent<RectTransform>().anchoredPosition = p+MapPosition;
            // = new Vector3(p.x * MapScale.x + MapPosition.x,0, p.y * MapScale.y * MapPosition.y);
        }
    }

    private void Start()
    {
        Room.CurrentPlayerRoom = generatedStartRoom;
    }
}
