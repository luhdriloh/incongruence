using System.Collections.Generic;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    public GameObject _transporterPrototype;

    private BoxMethodDungeonGeneration _boxDungeonCreator;
    private Dictionary<Vector2Int, Room> _rooms;

	private void Start ()
    {
        _boxDungeonCreator = GetComponentInChildren<BoxMethodDungeonGeneration>();
        _rooms = _boxDungeonCreator.CreateBoxLevelDungeon();
        SpawnTransporters();
	}

    private void SpawnTransporters()
    {
        Vector2 roomCenterForTransporterToAdd;
        Vector2 newTransporterPosition;

        Dictionary<Direction, Vector2Int> roomConnections = new Dictionary<Direction, Vector2Int>();

        foreach (Vector2Int roomPosition in _rooms.Keys)
        {
            roomConnections = GetRoomsInCardinalDirections(roomPosition);

            foreach (Direction direction in roomConnections.Keys)
            {
                roomCenterForTransporterToAdd = _rooms[roomConnections[direction]].ReturnRoomCenter();
                newTransporterPosition = ReturnTransporterPlacement(_rooms[roomPosition], direction);

                GameObject newTransporter = Instantiate(_transporterPrototype, newTransporterPosition, Quaternion.identity);
                newTransporter.GetComponent<Transporter>().SetTransporterCoordinates(roomCenterForTransporterToAdd);
            }
        }
    }

    private void SpawnEnemies()
    { 
    }



    private Dictionary<Direction, Vector2Int> GetRoomsInCardinalDirections(Vector2Int roomPosition)
    {
        Dictionary<Direction, Vector2Int> roomsInCardinalDirections = new Dictionary<Direction, Vector2Int>();
        Direction[] directionsToCheck =
        {
            Direction.NORTH,
            Direction.SOUTH,
            Direction.WEST,
            Direction.EAST
        };

        Vector2Int positionToCheckForRoom;

        foreach (Direction direction in directionsToCheck)
        {
            positionToCheckForRoom = GetNewDirection(roomPosition, direction);
            if (_rooms.ContainsKey(positionToCheckForRoom))
            {
                roomsInCardinalDirections.Add(direction, positionToCheckForRoom);
            }
        }

        return roomsInCardinalDirections;
    }

    private Vector2Int GetNewDirection(Vector2Int position, Direction direction)
    {
        switch (direction)
        {
            case Direction.NORTH:
                return position + Vector2Int.up;
            case Direction.EAST:
                return position + Vector2Int.right;
            case Direction.SOUTH:
                return position + Vector2Int.down;
            default:
                return position + Vector2Int.left;
        }
    }


    private Vector2 ReturnTransporterPlacement(Room room, Direction inDirection)
    {
        Vector2 center = room.ReturnRoomCenter();
        float widthToAdd = room.ReturnRoomSize().x / 2 - 1.5f;
        float heightToAdd = room.ReturnRoomSize().y / 2 - 1.5f;

        switch (inDirection)
        { 
            case Direction.NORTH:
                return center + new Vector2(0f, heightToAdd);
            case Direction.WEST:
                return center + new Vector2(-widthToAdd, 0f);
            case Direction.SOUTH:
                return center + new Vector2(0f, -heightToAdd);
            default:
                return center + new Vector2(widthToAdd, 0f);
        }
    }
}
