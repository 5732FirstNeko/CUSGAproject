using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameMapDataSo gameMapData;

    private Queue<ConnectDoorData> pendingDoors;
    private Queue<ConnectDoorData> previousDoors;

    private List<MapUnit> placedCorridor;
    private Dictionary<MapUnit,int> placedRoom;
    private HashSet<Vector2Int> occupiedGrids;

    private int doorNumber = 0;

    public void GenerateMap()
    {
        occupiedGrids.Clear();

        MapUnit startRoom = gameMapData.GetRandomMatchingMapUnit(r => r.roomType == RoomType.Start);

        PlaceRoom(startRoom, Vector3.zero, 0);

        foreach (DoorData data in startRoom.doors)
        {
            pendingDoors.Enqueue(new ConnectDoorData(data, startRoom));
        }

        //第一次链接 : startRoom -> corrldor
        while (pendingDoors.Count > 0)
        {
            ConnectDoorData door = pendingDoors.Dequeue();
            List<MapUnit> usedTempUnit = new List<MapUnit>();
            bool isSucceedPlaceRoom = false;

            while (!isSucceedPlaceRoom)
            {
                MapUnit corridor = gameMapData.GetRandomMatchingMapUnit(r => r.roomType == RoomType.Corridor &&
                !placedCorridor.Contains(r) &&
                !usedTempUnit.Contains(r));

                foreach (DoorData connectDoor in corridor.GetAllMatchDoor(d => d.doorDirection != door.connectDoor.doorDirection))
                {
                    Vector3 tempPlacePosition = GetMapUnitPosition(door.connectDoor, corridor.transform.position, connectDoor);

                    if (!IsOverlapWithMap(corridor, tempPlacePosition))
                    {
                        PlaceCorridor(corridor, tempPlacePosition);
                        foreach (DoorData d in corridor.GetAllMatchDoor(d => d != connectDoor))
                        {
                            previousDoors.Enqueue(new ConnectDoorData(d, door.lastRoom));
                        }
                        isSucceedPlaceRoom = true;
                        break;
                    }
                }

                if (!isSucceedPlaceRoom)
                {
                    usedTempUnit.Add(corridor);
                }
                else
                {
                    break;
                }
            }
        }

        while (previousDoors.Count > 0)
        {
            pendingDoors.Enqueue(previousDoors.Dequeue());
        }

        //第二次链接 : corridor(2) -> normallRoom

        while (pendingDoors.Count > 0)
        {
            ConnectDoorData door = pendingDoors.Dequeue();
            List<MapUnit> usedTempUnit = new List<MapUnit>();
            bool isSucceedPlaceRoom = false;

            while (!isSucceedPlaceRoom)
            {
                RoomType nextRoomType = RoomType.Normal;
                if (placedRoom[door.lastRoom] + 1 >= gameMapData.RewardDepth)
                {
                    nextRoomType = RoomType.Reward;
                }

                MapUnit newRoom = gameMapData.GetRandomMatchingMapUnit(r => r.roomType == nextRoomType &&
                !placedCorridor.Contains(r) &&
                !usedTempUnit.Contains(r));

                foreach (DoorData connectDoor in newRoom.GetAllMatchDoor(d => d.doorDirection != door.connectDoor.doorDirection))
                {
                    Vector3 tempPlacePosition = GetMapUnitPosition(door.connectDoor, newRoom.transform.position, connectDoor);

                    if (!IsOverlapWithMap(newRoom,tempPlacePosition))
                    {
                        PlaceRoom(newRoom, tempPlacePosition, placedRoom[door.lastRoom] + 1);
                        foreach (DoorData d in newRoom.GetAllMatchDoor(d => d != connectDoor))
                        {
                            previousDoors.Enqueue(new ConnectDoorData(d, newRoom));
                        }
                        isSucceedPlaceRoom = true;
                        break;
                    }
                }

                if (!isSucceedPlaceRoom)
                {
                    usedTempUnit.Add(newRoom);
                }
                else
                {
                    break;
                }
            }
        }

        while (previousDoors.Count > 0)
        {
            pendingDoors.Enqueue(previousDoors.Dequeue());
        }
    }

    public Vector3 GetMapUnitPosition(DoorData door,Vector3 mapPosition,DoorData mapunitDoor)
    {
        Vector3 connectPosition = door.GetMidPoint();

        Vector3 interpolation = mapPosition - mapunitDoor.GetMidPoint();

        return connectPosition + interpolation;
    }

    public void PlaceRoom(MapUnit mapUnit,Vector3 worldPosition,int roomDepth)
    {
        Instantiate(mapUnit.roomPrefab, worldPosition, Quaternion.identity);
        placedRoom.Add(mapUnit, roomDepth);

        doorNumber++;
        HashSet<Vector2Int> newRoomGrids = CalculateWorldGridPositions(mapUnit, worldPosition);
        occupiedGrids.UnionWith(newRoomGrids);
    }

    public void PlaceCorridor(MapUnit mapUnit, Vector3 worldPosition)
    {
        Instantiate(mapUnit.roomPrefab, worldPosition, Quaternion.identity);
        placedCorridor.Add(mapUnit);

        HashSet<Vector2Int> newCorridorGrids = CalculateWorldGridPositions(mapUnit, worldPosition);
        occupiedGrids.UnionWith(newCorridorGrids);
    }

    private bool IsOverlapWithMap(MapUnit newRoom, Vector3 worldPosition)
    {
        HashSet<Vector2Int> newRoomGrids = CalculateWorldGridPositions(newRoom, worldPosition);
        return newRoomGrids.Any(occupiedGrids.Contains);
    }

    /// <summary>
    /// 计算房间在世界坐标下的网格位置集合
    /// </summary>
    /// <param name="room">房间实例</param>
    /// <param name="worldPosition">TileMap物体的坐标，房间放置的世界位置</param>
    /// <returns>世界坐标下的网格位置哈希表</returns>
    private HashSet<Vector2Int> CalculateWorldGridPositions(MapUnit room, Vector3 worldPosition)
    {
        HashSet<Vector2Int> worldGridPositions = new HashSet<Vector2Int>();

        Vector3Int roomGridPosition = room.tilemap.WorldToCell(worldPosition);

        foreach (Vector2Int localPos in room.GridPositions)
        {
            Vector2Int worldPos = new Vector2Int(
                roomGridPosition.x + localPos.x,
                roomGridPosition.y + localPos.y
            );

            worldGridPositions.Add(worldPos);
        }

        return worldGridPositions;
    }

    private class ConnectDoorData
    {
        public DoorData connectDoor;
        public MapUnit lastRoom;

        public ConnectDoorData(DoorData doorData, MapUnit lastRoom)
        {
            this.connectDoor = doorData;
            this.lastRoom = lastRoom;
        }
    }
}