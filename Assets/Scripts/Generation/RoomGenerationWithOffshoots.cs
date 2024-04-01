using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RoomGenerationWithOffshoots : MonoBehaviour
{
    public List<GameObject> normalRooms = new List<GameObject>();
    public List<GameObject> specialRooms = new List<GameObject>();
    public List<int> specialRoomIndexes = new List<int>();
    public List<GameObject> offshoots = new List<GameObject>();
    public List<int> offShootIndexes = new List<int>();
    public int roomCount;
    public int offshootCount;
    public GameObject grid;
    public GameObject lastRoom;
    private System.Random gen = new System.Random();
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
        for (int i = 0; i < specialRooms.Count; i++)
        {
            bool repeat = true;
            do
            {
                int roomIndex = gen.Next(1, roomCount);
                if (!specialRoomIndexes.Contains(roomIndex))
                {
                    specialRoomIndexes.Add(roomIndex);
                    repeat = false;
                }
            } while (repeat);
        }
        for (int i = 0; i < offshootCount; i++)
        {
            bool repeat = true;
            do
            {
                int roomIndex = gen.Next(1, roomCount);
                if (!offShootIndexes.Contains(roomIndex))
                {
                    offShootIndexes.Add(roomIndex);
                    repeat = false;
                }
            } while (repeat);
        }


        for (int i = 0; i < roomCount; i++)
        {
            if (offShootIndexes.Contains(i))
            {
                GenerateOffshoots();
            }
            if (specialRoomIndexes.Contains(i))
            {
               GenerateSpecialRoom(i);
            }
            GenerateRoom();
        }
    }
    private void GenerateRoom()
    {
        int roomIndex = gen.Next(0, normalRooms.Count);
        Vector3 offset1 = normalRooms[roomIndex].transform.Find("Joint Start").localPosition;
        Vector3 offset2 = normalRooms[roomIndex].transform.Find("Joint Start/Joint").localPosition;
        Vector3 finalOffset = offset1 + offset2;
        lastRoom = Instantiate(normalRooms[roomIndex], lastRoom.transform.Find("Joint End/Joint").transform.position - finalOffset, lastRoom.transform.rotation);
        lastRoom.transform.SetParent(grid.transform);
    }


    private void GenerateSpecialRoom(int i)
    {
        int roomIndex = specialRoomIndexes.FindIndex(item => item == i);
        Vector3 offset1 = specialRooms[roomIndex].transform.Find("Joint Start").localPosition;
        Vector3 offset2 = specialRooms[roomIndex].transform.Find("Joint Start/Joint").localPosition;
        Vector3 finalOffset = offset1 + offset2;
        lastRoom = Instantiate(specialRooms[roomIndex], lastRoom.transform.Find("Joint End/Joint").transform.position - finalOffset, lastRoom.transform.rotation);
        lastRoom.transform.SetParent(grid.transform);
    }
    private void GenerateOffshoots()
    {
        int roomIndex = gen.Next(0, offshoots.Count);
        Vector3 offset1 = offshoots[roomIndex].transform.Find("Joint Start").localPosition;
        Vector3 offset2 = offshoots[roomIndex].transform.Find("Joint Start/Joint").localPosition;
        Vector3 finalOffset = offset1 + offset2;
        lastRoom = Instantiate(offshoots[roomIndex], lastRoom.transform.Find("Joint End/Joint").transform.position - finalOffset, lastRoom.transform.rotation);
        lastRoom.transform.SetParent(grid.transform);
    }
}
