using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum Direction
{
    NORTH = 0,
    EAST = 1,
    SOUTH = 2,
    WEST = 3,
    UNKOWN = 4
}

public class BoxMethodDungeonGeneration : MonoBehaviour
{
    public Tilemap _tilemapToDrawOn;
    public TileBase _tileToUse;

    public int _roomsToCreate;
    public int _walkersToCreate;

    private struct Walker
    {
        public Vector2Int _position;
        public Vector2Int _direction;
    };

    private List<Walker> _walkers;
    private Dictionary<Vector2Int, Room> _rooms;
    private bool _ready;

	private void Awake()
    {
        InitializeVariables();
	}

    private void InitializeVariables()
    {
        _rooms = new Dictionary<Vector2Int, Room>();
        _walkers = new List<Walker>();

        for (int i = 0; i < _walkersToCreate; i++)
        {
            Walker walkerToAdd = new Walker
            {
                _position = Vector2Int.zero
            };
            _walkers.Add(walkerToAdd);
        }
    }

    private void CreateRooms()
    {
        int roomsCreated = 0;
        Direction lastCreatedDirection = Direction.UNKOWN;

        // first add room (0, 0)
        _rooms.Add(Vector2Int.zero, new Room(Vector2Int.zero, GetRoomSize()));
        roomsCreated++;

        while (roomsCreated <= _roomsToCreate)
        {
            for (int i = 0; i < _walkersToCreate; i++)
            {
                // create new direction for walkers
                Walker walker = _walkers[i];
                walker._direction = ReturnDirection(lastCreatedDirection);

                // move walker and add a room if we haven't added it before
                walker._position += walker._direction;
                _walkers[i] = walker;
                if (!_rooms.ContainsKey(walker._position))
                {
                    _rooms.Add(walker._position, new Room(walker._position, GetRoomSize()));
                    roomsCreated++;
                }
            }
        }
    }

    private Vector2Int GetRoomSize()
    {
        return new Vector2Int(Random.Range(10, 20), Random.Range(10, 20));
    }

    private Vector2Int ReturnDirection(Direction lastCreatedDirection)
    {
        Direction directionToGo;
        Vector2Int vectorToReturn;

        do
        {
            directionToGo = ReturnCardinalDirection();

            switch (directionToGo)
            {
                case Direction.NORTH:
                    vectorToReturn = Vector2Int.up;
                    break;
                case Direction.EAST:
                    vectorToReturn = Vector2Int.right;
                    break;
                case Direction.SOUTH:
                    vectorToReturn = Vector2Int.down;
                    break;
                default:
                    vectorToReturn = Vector2Int.left;
                    break;
            }
        }
        while (directionToGo == lastCreatedDirection);

        return vectorToReturn;
    }
	
    private Direction ReturnCardinalDirection()
    {
        int value = Mathf.FloorToInt(Random.Range(0f, 3.99f));
        return (Direction)value;
	}

    public Dictionary<Vector2Int, Room> CreateBoxLevelDungeon()
    {
        CreateRooms();
        DrawRooms();
        return _rooms;
    }

    private void DrawRooms()
    {
        foreach (Room room in _rooms.Values)
        {
            DrawRoom(room);
        }
    }

    private void DrawRoom(Room room)
    {
        Vector2Int roomSize = room.ReturnRoomSize();
        Vector2Int offset = room.ReturnRoomPosition() * 20;

        for (int row = 0; row < roomSize.y; row++)
        {
            for (int column = 0; column < roomSize.x; column++)
            {
                if (row == 0 || row == roomSize.y - 1 || column == 0 || column == roomSize.x - 1)
                {
                    Vector3Int tilePosition = new Vector3Int(column, row, 0) + new Vector3Int(offset.x, offset.y, 0);
                    _tilemapToDrawOn.SetTile(tilePosition, _tileToUse);
                }
            }
        }
    }
}
