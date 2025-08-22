using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

//半山腰太挤，你总得去山顶看看//
[System.Serializable]
public class MapUnit : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject roomPrefab;
    public RoomType roomType;
    public DoorData LeftDoor { get => doors.First(d => d.doorDirection == Direction.Left); }
    public DoorData RightDoor { get => doors.First(d => d.doorDirection == Direction.Right); }

    public List<DoorData> doors;

    public HashSet<Vector2Int> GridPositions { get; private set; }

    private void OnEnable()
    {
        tilemap = GetComponent<Tilemap>();

        if (tilemap != null)
        {
            GridPositions = GetTilemapGridPositions(tilemap);
        }
        else
        {
            GridPositions = new HashSet<Vector2Int>();
            Debug.LogError("MapUnit的Tilemap未赋值！");
        }
    }

    private HashSet<Vector2Int> GetTilemapGridPositions(Tilemap targetTilemap)
    {
        HashSet<Vector2Int> positions = new HashSet<Vector2Int>();

        BoundsInt bounds = targetTilemap.cellBounds;

        for (int x = bounds.min.x; x < bounds.max.x; x++)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);
                TileBase tile = targetTilemap.GetTile(cellPosition);

                if (tile != null)
                {
                    positions.Add(new Vector2Int(x, y));
                }
            }
        }

        return positions;
    }

    public DoorData GetRandomMatchingDoor(Func<DoorData, bool> condition)
    {
        var matches = doors.Where(condition).ToList();

        if (matches.Count == 0)
            return null;

        var random = new System.Random();
        int index = random.Next(matches.Count);
        return matches[index];
    }

    public List<DoorData> GetAllMatchDoor()
    {
        return doors.ToList();
    }

    public List<DoorData> GetAllMatchDoor(Func<DoorData, bool> condition)
    {
        var matches = doors.Where(condition).ToList();

        return matches;
    }
}

[System.Serializable]
public class DoorData
{
    public Transform position_up;
    public Transform position_down;
    public Direction doorDirection;

    public Vector3 GetMidPoint()
    {
        return new Vector3((position_up.position.x + position_down.position.x) / 2, (position_up.position.y + position_down.position.y) / 2);
    }
}

public enum RoomType
{
    Start,
    Normal,
    Reward,
    Boss,
    Shop,
    End,
    Corridor
}

public enum Direction
{
    Left,
    Right
}
