using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelCreator : MonoBehaviour
{
    public GameObject _gunBoxPrototype;
    public LevelCreationData _levelCreationData;
    public Tilemap _tilemapToDrawFloor;
    public Tilemap _tilemapToDrawEdge;

    public TileBase _fillTile;
    public TileBase _edgeTile;

    private HashSet<Vector2Int> _dungeonTiles;

    private void Start ()
    {
        _dungeonTiles = DrunkWalkerDungeonCreator.CreateDungeon(_levelCreationData._numberOfWalkers, _levelCreationData._numberOfIterations, _levelCreationData._overlapAllowed);

        List<Vector2Int> dungeonTiles = ReturnListOfScaledTiles(_dungeonTiles);
        List<Vector2Int> edgeTiles = ReturnEdgeTiles(dungeonTiles);
        HashSet<Vector2Int> fillTiles = new HashSet<Vector2Int>(dungeonTiles.Except(edgeTiles));

        // a vector indicating places to draw grass or random environment details
        // grass
        // water
        // flowers / mushrooms etc

        DrawLevelOut(fillTiles, edgeTiles);

        // spawn enemies
        SpawnEnemies(fillTiles.ToList());
        SpawnItems(fillTiles.ToList());

        // set player position
        GameObject player = GameObject.FindWithTag("Player");
        Vector2 newPlayerPosition = FindPlayerStartPosition(edgeTiles, fillTiles);
        player.transform.position = new Vector3(newPlayerPosition.x, newPlayerPosition.y, player.transform.position.z);
    }

    private void DrawLevelOut(HashSet<Vector2Int> fillTiles, List<Vector2Int> edgeTiles)
    {
        DrawDungeonTiles(fillTiles, _tilemapToDrawFloor, _fillTile);
        DrawDungeonTiles(edgeTiles, _tilemapToDrawEdge, _edgeTile);
    }

    private void SpawnItems(List<Vector2Int> map)
    {
        HashSet<int> placesUsed = new HashSet<int>();
        int newItemLocation = Random.Range(0, map.Count);

        for (int i = 0; i < _levelCreationData._maxGunBoxesToSpawn; i++)
        {
            if (Random.value <= _levelCreationData._gunBoxSpawnPercentage)
            {
                while (placesUsed.Contains(newItemLocation))
                {
                    newItemLocation = Random.Range(0, map.Count);
                }

                Vector3 location = new Vector3(map[newItemLocation].x + .5f, map[newItemLocation].y + .5f, -1f);
                GunBox newGunBox = Instantiate(_gunBoxPrototype, location, Quaternion.identity).GetComponent<GunBox>();
                newGunBox._weaponsList = _levelCreationData._weaponsList;
                newGunBox._weaponRates = _levelCreationData._weaponRates;
            }
        }
    }

    private void SpawnEnemies(List<Vector2Int> map)
    {
        HashSet<int> placesUsed = new HashSet<int>();

        for (int i = 0; i < _levelCreationData._numberOfEnemies; i++)
        {
            int typeOfEnemy = Random.Range(0, _levelCreationData._enemyPrototypes.Count);
            int newEnemyLocationIndex = Random.Range(0, map.Count);

            while (placesUsed.Contains(newEnemyLocationIndex))
            {
                newEnemyLocationIndex = Random.Range(0, map.Count);
            }

            Vector3 location = new Vector3(map[newEnemyLocationIndex].x + .5f, map[newEnemyLocationIndex].y + .5f, -1f);
            Instantiate(_levelCreationData._enemyPrototypes[typeOfEnemy], location, Quaternion.identity);
            placesUsed.Add(newEnemyLocationIndex);
        }
    }

    private Vector2 FindPlayerStartPosition(List<Vector2Int> edgeTiles, HashSet<Vector2Int> fillTiles)
    {
        // take a random edge tile
        Vector2Int edgeTileSelected = edgeTiles[Random.Range(0, edgeTiles.Count)];
        Vector2Int positionToCheck;

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                positionToCheck = edgeTileSelected + new Vector2Int(i, j);
                if (fillTiles.Contains(positionToCheck))
                {
                    return positionToCheck;
                }
            }
        }

        Debug.Log("no valid player position found");
        return edgeTileSelected;
    }

    private void DrawDungeonTiles(IEnumerable<Vector2Int> tiles, Tilemap tilemapToUse, TileBase tilesToUse)
    {
        foreach (Vector2Int tileLocation in tiles)
        {
            tilemapToUse.SetTile(new Vector3Int(tileLocation.x, tileLocation.y, 0), tilesToUse);
        }
    }

    private List<Vector2Int> ReturnListOfScaledTiles(IEnumerable<Vector2Int> tiles)
    {
        List<Vector2Int> scaledTiles = new List<Vector2Int>();

        foreach (Vector2Int tileLocation in tiles)
        {
            Vector2Int startPosition = tileLocation * _levelCreationData._tileSize;
            Vector2Int newPosition;

            for (int i = 0; i < _levelCreationData._tileSize; i++)
            {
                for (int j = 0; j < _levelCreationData._tileSize; j++)
                {
                    newPosition = new Vector2Int(i, j) + new Vector2Int(startPosition.x, startPosition.y);
                    scaledTiles.Add(newPosition);
                }
            }
        }

        return scaledTiles;
    }

    private List<Vector2Int> ReturnEdgeTiles(IEnumerable<Vector2Int> dungeonPositions)
    {
        List<Vector2Int> edgeTiles = new List<Vector2Int>();
        HashSet<Vector2Int> tiles = new HashSet<Vector2Int>();

        foreach (Vector2Int tilePosition in dungeonPositions)
        {
            tiles.Add(tilePosition);
        }

        foreach (Vector2Int tilePosition in dungeonPositions)
        {
            if (IsTileSurroundedOnAllSides(tiles, tilePosition) == false)
            {
                edgeTiles.Add(tilePosition);
            }
        }

        return edgeTiles;
    }

    private bool IsTileSurroundedOnAllSides(HashSet<Vector2Int> dungeonPositions, Vector2Int positionToCheck)
    {
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (dungeonPositions.Contains(new Vector2Int(positionToCheck.x + i, positionToCheck.y + j)) == false)
                {
                    return false;
                }
            }
        }

        return true;
    }
}



























