using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class MapUnit : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject roomPrefab;
    public RoomType roomType;

    public List<DoorData> doors;

    public HashSet<Vector2Int> GridPositions { get; private set; }

    private void Awake()
    {
        if (tilemap != null)
        {
            GridPositions = GetTilemapGridPositions(tilemap);
        }
        else
        {
            GridPositions = new HashSet<Vector2Int>();
            Debug.LogError("MapUnitµÄTilemapÎ´¸³Öµ£¡");
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
    public Transform position_X;
    public Transform position_Y;
    public Direction doorDirection;

    public Vector3 GetMidPoint()
    {
        return new Vector3((position_X.position.x + position_Y.position.x) / 2, (position_X.position.y + position_Y.position.y) / 2);
    }
}

public enum RoomType
{
    Start,
    Normal,
    Reward,
    Boss,
    Shop,
    Corridor
}

public enum Direction
{
    Left,
    Right
}

