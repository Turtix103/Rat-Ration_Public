using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RoomGenerationWithBranchingPaths : MonoBehaviour
{
    public List<GameObject> normalRooms = new List<GameObject>();
    public List<GameObject> normalOffshootRooms = new List<GameObject>();
    public List<GameObject> specialRooms = new List<GameObject>();
    private List<int> specialRoomIndexes = new List<int>();
    public List<GameObject> offshoots = new List<GameObject>();
    private List<GameObject> generatedOffshoots = new List<GameObject>();
    private List<int> offShootIndexes = new List<int>();
    public List<GameObject> endRooms = new List<GameObject>();
    public List<GameObject> offshootEndRooms = new List<GameObject>();

    public bool uniqueOffshootRooms;
    public bool generateDiffrentRoomsForOffshoots;
    private bool generatingOffshoots;

    public int mainRoomCount;
    public int offshootCount;
    public int offshootRoomCount;
    private GameObject grid;
    private GameObject lastRoom;
    private System.Random gen = new System.Random();
    void Start()
    {
        grid = GameObject.Find("Grid");
        lastRoom = gameObject;
        GenerateRooms();
        offshootEndRooms.Shuffle();
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
                int roomIndex = gen.Next(1, mainRoomCount);
                if (!specialRoomIndexes.Contains(roomIndex))
                {
                    specialRoomIndexes.Add(roomIndex);
                    repeat = false;
                }
            } while (repeat);
        }
        for (int i = 0; i < (uniqueOffshootRooms ? offshoots.Count : offshootCount); i++)
        {
            bool repeat = true;
            do
            {
                int roomIndex = gen.Next(1, mainRoomCount);
                if (!offShootIndexes.Contains(roomIndex))
                {
                    offShootIndexes.Add(roomIndex);
                    repeat = false;
                }
            } while (repeat);
        }


        for (int i = 0; i < mainRoomCount; i++)
        {
            if (offShootIndexes.Contains(i))
            {
                if (!uniqueOffshootRooms)
                    GenerateOffshoots();
                else
                    GenerateDiffrentOffShoots(i);
            }
            if (specialRoomIndexes.Contains(i))
            {
                GenerateSpecialRoom(i);
            }
            GenerateRoom();
        }

        GenerateEndRoom();

        generatingOffshoots = true;

        for (int i = 0; i < generatedOffshoots.Count; i++)
        {
            GenerateOffshootRoom(i);
            for (int j = 0; j < offshootRoomCount - 1; j++)
            {
                GenerateRoom();
            }
            GeneratePOI(i);
        }

    }
    private void GenerateRoom()
    {
        List<GameObject> rooms;
        if (!generatingOffshoots || !generateDiffrentRoomsForOffshoots)
        {
            rooms = normalRooms;
        }
        else
        {
            rooms = normalOffshootRooms;
        }


       /*if (generatingOffshoots && diffrentOffshootRooms)
            rooms = normalOffshootRooms; */

        int roomIndex = gen.Next(0, rooms.Count);
        Vector3 offset1 = rooms[roomIndex].transform.Find("Joint Start").localPosition;
        Vector3 offset2 = rooms[roomIndex].transform.Find("Joint Start/Joint").localPosition;
        Vector3 finalOffset = offset1 + offset2;
        lastRoom = Instantiate(rooms[roomIndex], lastRoom.transform.Find("Joint End/Joint").transform.position - finalOffset, lastRoom.transform.rotation);
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
        generatedOffshoots.Add(lastRoom);
        lastRoom.transform.SetParent(grid.transform);
    }

    private void GenerateDiffrentOffShoots(int i)
    {
        int roomIndex = offShootIndexes.FindIndex(item => item == i);
        Vector3 offset1 = offshoots[roomIndex].transform.Find("Joint Start").localPosition;
        Vector3 offset2 = offshoots[roomIndex].transform.Find("Joint Start/Joint").localPosition;
        Vector3 finalOffset = offset1 + offset2;
        lastRoom = Instantiate(offshoots[roomIndex], lastRoom.transform.Find("Joint End/Joint").transform.position - finalOffset, lastRoom.transform.rotation);
        generatedOffshoots.Add(lastRoom);
        lastRoom.transform.SetParent(grid.transform);
    }

    private void GenerateOffshootRoom(int i)
    {
        lastRoom = generatedOffshoots[i];
        if (!generateDiffrentRoomsForOffshoots)
        {
            int roomIndex = gen.Next(0, normalRooms.Count);
            Vector3 offset1 = normalRooms[roomIndex].transform.Find("Joint Start").localPosition;
            Vector3 offset2 = normalRooms[roomIndex].transform.Find("Joint Start/Joint").localPosition;
            Vector3 finalOffset = offset1 + offset2;
            lastRoom = Instantiate(normalRooms[roomIndex], lastRoom.transform.Find("Offjoint End/Joint").transform.position - finalOffset, lastRoom.transform.rotation);
        }
        else
        {
            int roomIndex = gen.Next(0, normalOffshootRooms.Count);
            Vector3 offset1 = normalOffshootRooms[roomIndex].transform.Find("Joint Start").localPosition;
            Vector3 offset2 = normalOffshootRooms[roomIndex].transform.Find("Joint Start/Joint").localPosition;
            Vector3 finalOffset = offset1 + offset2;
            lastRoom = Instantiate(normalOffshootRooms[roomIndex], lastRoom.transform.Find("Offjoint End/Joint").transform.position - finalOffset, lastRoom.transform.rotation);
        }
        lastRoom.transform.SetParent(grid.transform);
    }

    private void GenerateEndRoom()
    {
        int roomIndex = gen.Next(0, endRooms.Count);
        Vector3 offset1 = endRooms[roomIndex].transform.Find("Joint Start").localPosition;
        Vector3 offset2 = endRooms[roomIndex].transform.Find("Joint Start/Joint").localPosition;
        Vector3 finalOffset = offset1 + offset2;
        lastRoom = Instantiate(endRooms[roomIndex], lastRoom.transform.Find("Joint End/Joint").transform.position - finalOffset, lastRoom.transform.rotation);
        lastRoom.transform.SetParent(grid.transform);
    }

    private void GeneratePOI(int i)
    {
        Vector3 offset1 = offshootEndRooms[i].transform.Find("Joint Start").localPosition;
        Vector3 offset2 = offshootEndRooms[i].transform.Find("Joint Start/Joint").localPosition;
        Vector3 finalOffset = offset1 + offset2;
        lastRoom = Instantiate(offshootEndRooms[i], lastRoom.transform.Find("Joint End/Joint").transform.position - finalOffset, lastRoom.transform.rotation);
        lastRoom.transform.SetParent(grid.transform);
    }
}
