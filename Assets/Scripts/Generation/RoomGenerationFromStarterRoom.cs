using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RoomGenerationFromStarterRoom : MonoBehaviour
{
    public GameObject[] rooms;
    public int roomCount;
    public GameObject grid;
    public GameObject lastRoom;
    void Start()
    {
        grid = GameObject.Find("Grid");
        lastRoom = gameObject;
        GenerateRooms();
    }

    void Update()
    {
        
    }

    private void GenerateRooms()
    {
        Console.WriteLine(lastRoom);
        System.Random gen = new System.Random();
        for (int i = 0; i < roomCount; i++)
        {
            int roomIndex = gen.Next(0, rooms.Length);
            Vector3 offset1 = rooms[roomIndex].transform.Find("Joint Start").localPosition;
            Vector3 offset2 = rooms[roomIndex].transform.Find("Joint Start/Joint").localPosition;
            Vector3 finalOffset = offset1 + offset2;
            lastRoom = Instantiate(rooms[roomIndex], lastRoom.transform.Find("Joint End/Joint").transform.position - finalOffset, lastRoom.transform.rotation);
            lastRoom.transform.SetParent(grid.transform);
        }
    }
}
