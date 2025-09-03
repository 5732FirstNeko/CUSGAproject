using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//半山腰太挤，你总得去山顶看看//
public class MapGenerator : MonoBehaviour
{
    public GameMapDataSo gameMapData;
    public int maxPlacementAttempts = 20;

    private Queue<ConnectDoorData> pendingDoors;
    private Queue<ConnectDoorData> previousDoors;

    private List<MapUnit> placedCorridor;
    private Dictionary<GameObject,int> placedRoom;
    private HashSet<Vector2Int> occupiedGrids;

    private void DungeonMapGenerate()
    {
        occupiedGrids.Clear();
        int doorNumber = 2 * gameMapData.roomNumber - 2;
        bool isHaveRewardRoom = false;
        bool isHaveShopRoom = false;

        MapUnit startRoom = gameMapData.GetRandomMatchingMapUnit(r => r.roomType == RoomType.Start);

        PlaceRoom(startRoom, Vector3.zero, 0);

        foreach (DoorData data in startRoom.doors)
        {
            pendingDoors.Enqueue(new ConnectDoorData(data, startRoom));
            doorNumber--;
        }

        while (doorNumber > 0 && pendingDoors.Count > 0)
        {
            while (pendingDoors.Count > 0)
            {
                ConnectDoorData doorData = pendingDoors.Dequeue();
                List<MapUnit> usedTempUnit = new List<MapUnit>();
                bool isSucceedPlaceRoom = false;
                int attemptCount = 0;

                while (!isSucceedPlaceRoom && attemptCount < maxPlacementAttempts)
                {
                    attemptCount++;
                    MapUnit newCorridor = gameMapData.GetRandomMatchingMapUnit(r => r.roomType == RoomType.Corridor &&
                    r.doors.Any(d => d.doorDirection != doorData.connectDoor.doorDirection) &&
                    !usedTempUnit.Contains(r) &&
                    !placedCorridor.Contains(r));

                    if (newCorridor == null) break;

                    foreach (DoorData connectDoor in newCorridor.GetAllMatchDoor(d => d.doorDirection != doorData.connectDoor.doorDirection))
                    {
                        Vector3 tempPlacePosition = GetMapUnitPosition(doorData.connectDoor, newCorridor.transform.position, connectDoor);

                        if (!IsOverlapWithMap(newCorridor, tempPlacePosition))
                        {
                            PlaceCorridor(newCorridor, tempPlacePosition);
                            foreach (DoorData d in newCorridor.GetAllMatchDoor(d => d != connectDoor))
                            {
                                previousDoors.Enqueue(new ConnectDoorData(d, doorData.lastRoom));
                            }
                            isSucceedPlaceRoom = true;
                            break;
                        }
                    }

                    if (!isSucceedPlaceRoom)
                    {
                        usedTempUnit.Add(newCorridor);
                    }
                    else
                    {
                        break;
                    }
                }

                if (!isSucceedPlaceRoom)
                {
                    Debug.LogWarning($"Failed to place corridor for doorData after {maxPlacementAttempts} attempts");
                }
            }

            while (previousDoors.Count > 0)
            {
                pendingDoors.Enqueue(previousDoors.Dequeue());
            }


            while (pendingDoors.Count > 0)
            {
                ConnectDoorData door = pendingDoors.Dequeue();
                List<MapUnit> usedTempUnit = new List<MapUnit>();
                bool isSucceedPlaceRoom = false;
                int attemptCount = 0;

                while (!isSucceedPlaceRoom && attemptCount < maxPlacementAttempts)
                {
                    attemptCount++;
                    RoomType nextRoomType = RoomType.Normal;

                    float randomIndex = UnityEngine.Random.Range(0, 1);

                    if (doorNumber == 1)
                    {
                        nextRoomType = RoomType.End;
                    }
                    else if (!isHaveRewardRoom && !isHaveShopRoom)
                    {
                        if (placedRoom.Count + 1 == gameMapData.roomNumber)
                        {
                            nextRoomType = RoomType.Reward;
                            isHaveRewardRoom = true;
                        }
                        else
                        {
                            if (randomIndex <= 0.4)
                            {
                                nextRoomType = RoomType.Reward;
                                isHaveRewardRoom = true;
                            }
                        }

                        if (!isHaveRewardRoom)
                        {
                            if (placedRoom.Count + 1 == gameMapData.roomNumber)
                            {
                                nextRoomType = RoomType.Shop;
                                isHaveShopRoom = true;
                            }
                            else
                            {
                                if (randomIndex <= 0.4)
                                {
                                    nextRoomType = RoomType.Shop;
                                    isHaveShopRoom = true;
                                }
                            }
                        }
                    }
                    else if (!isHaveRewardRoom && isHaveShopRoom)
                    {
                        if (placedRoom.Count + 1 == gameMapData.roomNumber)
                        {
                            nextRoomType = RoomType.Shop;
                            isHaveShopRoom = true;
                        }
                        else
                        {
                            if (randomIndex <= 0.4)
                            {
                                nextRoomType = RoomType.Shop;
                                isHaveShopRoom = true;
                            }
                        }
                    }
                    else if (isHaveRewardRoom && !isHaveShopRoom)
                    {
                        if (placedRoom.Count + 1 == gameMapData.roomNumber)
                        {
                            nextRoomType = RoomType.Reward;
                            isHaveRewardRoom = true;
                        }
                        else
                        {
                            if (randomIndex <= 0.4)
                            {
                                nextRoomType = RoomType.Reward;
                                isHaveRewardRoom = true;
                            }
                        }
                    }

                    MapUnit newRoom = gameMapData.GetRandomMatchingMapUnit(r => r.roomType == nextRoomType &&
                        !placedRoom.ContainsKey(r.gameObject) &&
                        !usedTempUnit.Contains(r));

                    if (newRoom == null) break;

                    foreach (DoorData connectDoor in newRoom.GetAllMatchDoor(d => d.doorDirection != door.connectDoor.doorDirection))
                    {
                        Vector3 tempPlacePosition = GetMapUnitPosition(door.connectDoor, newRoom.transform.position, connectDoor);

                        if (!IsOverlapWithMap(newRoom, tempPlacePosition))
                        {
                            doorNumber--;
                            PlaceRoom(newRoom, tempPlacePosition, placedRoom[door.lastRoom.gameObject] + 1);
                            foreach (DoorData d in newRoom.GetAllMatchDoor(d => d != connectDoor))
                            {
                                previousDoors.Enqueue(new ConnectDoorData(d, newRoom));
                                doorNumber--;
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

                if (!isSucceedPlaceRoom)
                {
                    Debug.LogWarning($"Failed to place room for doorData after {maxPlacementAttempts} attempts");
                }
            }

            while (previousDoors.Count > 0)
            {
                pendingDoors.Enqueue(previousDoors.Dequeue());
            }
        }
    }

    private void FieldsMapGenerate()
    {
        occupiedGrids.Clear();

        int deletRoomNumber = gameMapData.roomNumber;

        MapUnit startRoom = gameMapData.GetRandomMatchingMapUnit(r => r.roomType == RoomType.Start);

        PlaceRoom(startRoom, Vector3.zero, 0);
        deletRoomNumber--;

        ConnectDoorData lastDoorData = new ConnectDoorData(startRoom.RightDoor,startRoom);
        bool isHaveRewardRoom = false;
        bool isHaveShopRoom = false;

        while (gameMapData.roomNumber > 0)
        {
            MapUnit newCorridor = gameMapData.GetRandomMatchingMapUnit(r => r.roomType == RoomType.Corridor && 
            r.doors.Count == 2 &&
            r.doors.Any(d => d.doorDirection == Direction.Left) && 
            r.doors.Any(d => d.doorDirection == Direction.Right));

            if (newCorridor == null) break;

            Vector3 tempPosition = GetMapUnitPosition(lastDoorData.connectDoor, newCorridor.transform.position, newCorridor.LeftDoor);
            PlaceCorridor(newCorridor,tempPosition);
            lastDoorData = new ConnectDoorData(newCorridor.RightDoor,startRoom);

            RoomType nextRoomType = RoomType.Normal;
            float randomIndex = UnityEngine.Random.Range(0, 1);

            if (deletRoomNumber == 1)
            {
                nextRoomType = RoomType.End;
            }
            else if (!isHaveRewardRoom && !isHaveShopRoom)
            {
                if (deletRoomNumber > 0.75f * gameMapData.roomNumber && randomIndex <= 0.2f)
                {
                    nextRoomType = RoomType.Reward;
                    isHaveRewardRoom = true;
                }
                else if (deletRoomNumber > 0.25f * gameMapData.roomNumber && randomIndex <= 0.4f)
                {
                    nextRoomType = RoomType.Reward;
                    isHaveRewardRoom = true;
                }
                else if (deletRoomNumber == 2)
                {
                    nextRoomType = RoomType.Reward;
                    isHaveRewardRoom = true;
                }

                if (!isHaveRewardRoom)
                {
                    if (randomIndex <= 0.3f)
                    {
                        nextRoomType = RoomType.Shop;
                        isHaveShopRoom = true;
                    }

                    if (deletRoomNumber == 3)
                    {
                        nextRoomType = RoomType.Shop;
                        isHaveShopRoom = true;
                    }
                }
            }
            else if (!isHaveRewardRoom && isHaveShopRoom)
            {
                if (deletRoomNumber > 0.75f * gameMapData.roomNumber && randomIndex <= 0.2f)
                {
                    nextRoomType = RoomType.Reward;
                    isHaveRewardRoom = true;
                }
                else if (deletRoomNumber > 0.25f * gameMapData.roomNumber && randomIndex <= 0.4f)
                {
                    nextRoomType = RoomType.Reward;
                    isHaveRewardRoom = true;
                }
                else if (deletRoomNumber == 2)
                {
                    nextRoomType = RoomType.Reward;
                    isHaveRewardRoom = true;
                }
            }
            else if (isHaveRewardRoom && !isHaveShopRoom)
            {
                if (randomIndex <= 0.3f)
                {
                    nextRoomType = RoomType.Shop;
                    isHaveShopRoom = true;
                }

                if (deletRoomNumber == 3)
                {
                    nextRoomType = RoomType.Shop;
                    isHaveShopRoom = true;
                }
            }


            MapUnit newRoom = gameMapData.GetRandomMatchingMapUnit(r => r.roomType == nextRoomType &&
            r.doors.Count == 2 &&
            r.doors.Any(d => d.doorDirection == Direction.Left) &&
            r.doors.Any(d => d.doorDirection == Direction.Right));

            if (newRoom == null) break;
            tempPosition = GetMapUnitPosition(lastDoorData.connectDoor, newRoom.transform.position, newRoom.LeftDoor);
            PlaceRoom(newRoom, tempPosition, placedRoom[lastDoorData.lastRoom.gameObject] + 1);
            deletRoomNumber--;
            lastDoorData = new ConnectDoorData(newRoom.RightDoor, newRoom);
        }
    }

    private Vector3 GetMapUnitPosition(DoorData door,Vector3 mapPosition,DoorData mapunitDoor)
    {
        Vector3 connectPosition = door.GetMidPoint();

        Vector3 interpolation = mapPosition - mapunitDoor.GetMidPoint();

        return connectPosition + interpolation;
    }
    
    private void PlaceRoom(MapUnit mapUnit,Vector3 worldPosition,int roomDepth)
    {
        Instantiate(mapUnit.roomPrefab, worldPosition, Quaternion.identity);
        placedRoom.Add(mapUnit.gameObject, roomDepth);

        HashSet<Vector2Int> newRoomGrids = CalculateWorldGridPositions(mapUnit, worldPosition);
        occupiedGrids.UnionWith(newRoomGrids);
    }

    private void PlaceCorridor(MapUnit mapUnit, Vector3 worldPosition)
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