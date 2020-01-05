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
    public int targetSize;
    public float space;
    public float bridgeProbability;

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

    private Room generatedStartRoom;

    private void Awake()
    {

        int i;
        List<Plane> queue = new List<Plane>();
        {
            var obj = Object.Instantiate(LevelStart, Vector3.zero, Quaternion.identity);
            var room = obj.GetComponent<Room>();
            foreach (Plane plane in room.planes)
            {
                queue.Add(plane);
            }
            generatedStartRoom = room;
        }
        int width = (int)Mathf.Sqrt(targetSize);
        for (i = 1; i < targetSize; ++i)
        {
            var x = i / width;
            var y = i % width;
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
                    obj = Object.Instantiate(rooms[Random.Range(0, rooms.Length)], new Vector3(space * x, 0, space * y), Quaternion.identity);
                    room = obj.GetComponent<Room>();
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
                for (var j = 0; j < room.planes.Length; ++j)
                {
                    if (j != entId)
                    {
                        queue.Add(room.planes[j]);
                    }
                }
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
            var x = i / width;
            var y = i % width;
            var exitPlane = RandomPop(queue);
            var obj = Object.Instantiate(rooms[Random.Range(0, rooms.Length)], new Vector3(space * x, 0, space * y), Quaternion.identity);
            var room = obj.GetComponent<Room>();
            int entId = Random.Range(0, room.planes.Length); //choose entrance
            var entPlane = room.planes[entId];
            for (var j = 0; j < room.planes.Length; ++j)
            {
                if (j != entId)
                {
                    queue.Add(room.planes[j]);
                }
            }
            Lank(entPlane, exitPlane);
            ++i;
        }
        {
            var x = i / width;
            var y = i % width;
            var exitPlane = RandomPop(queue);
            var obj = Object.Instantiate(LevelEnd, new Vector3(space * x, 0, space * y), Quaternion.identity);
            var room = obj.GetComponent<Room>();
            var entPlane = room.planes[0];
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
        //generatedStartRoom.SetRoomActiveExclusively();
        Room.CurrentPlayerRoom = generatedStartRoom;
    }
}
