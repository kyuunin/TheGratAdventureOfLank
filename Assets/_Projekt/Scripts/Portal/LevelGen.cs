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
        InitWeight();
        int i;
        List<Plane> queue = new List<Plane>();
        {
            (var obj, var room) = CreateRoom(LevelStart, 0);
            room.isFirst = true;
            PushAll(queue, room);
            generatedStartRoom = room;
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
            int entId = Random.Range(0, room.planes.Length); //choose entrance
            var entPlane = room.planes[entId];
            PushAll(queue, room, entId);
            Lank(entPlane, exitPlane);
            ++i;
        }
        {
            var exitPlane = RandomPop(queue);
            (var obj, var room) = CreateRoom(LevelEnd, i);
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

    }

    private void Start()
    {
        Room.CurrentPlayerRoom = generatedStartRoom;
    }
}
