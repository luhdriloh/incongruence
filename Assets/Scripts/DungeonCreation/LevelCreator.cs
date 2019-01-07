using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelCreator : MonoBehaviour
{
    public Tilemap _tilemapToDrawFloor;
    public Tilemap _tilemapToDrawEdge;

    public TileBase _fillTile;
    public TileBase _edgeTile;

    public bool _overlapAllowed;
    public int _tileSize;
    public int _numberOfWalkers;
    public int _numberOfIterations;
    public int _numberOfEnemies;
    public List<GameObject> _enemyPrototypes;

    private HashSet<Vector2Int> _dungeonTiles;

    private void Start ()
    {
        _dungeonTiles = DrunkWalkerDungeonCreator.CreateDungeon(_numberOfWalkers, _numberOfIterations, _overlapAllowed);

        List<Vector2Int> dungeonTiles = ReturnListOfScaledTiles(_dungeonTiles);
        List<Vector2Int> edgeTiles = ReturnEdgeTiles(dungeonTiles);
        HashSet<Vector2Int> fillTiles = new HashSet<Vector2Int>(dungeonTiles.Except(edgeTiles));

        // a vector indicating places to draw grass or random environment details
        // grass
        // water
        // flowers / mushrooms etc

        DrawLevelOut(fillTiles, edgeTiles);
        DecorateLevel(fillTiles, edgeTiles);

        // spawn enemies
        SpawnEnemies(fillTiles.ToList());
        SpawnItems(fillTiles, edgeTiles);

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

    private void DecorateLevel(HashSet<Vector2Int> fillTiles, List<Vector2Int> edgeTiles)
    { 
    
    }

    private void SpawnItems(HashSet<Vector2Int> map, List<Vector2Int> edgeTiles)
    { 
    }

    private void SpawnEnemies(List<Vector2Int> map)
    {
        HashSet<int> placesUsed = new HashSet<int>();

        for (int i = 0; i < _numberOfEnemies; i++)
        {
            int typeOfEnemy = Random.Range(0, _enemyPrototypes.Count);
            int newEnemyLocationIndex = Random.Range(0, map.Count);

            while (placesUsed.Contains(newEnemyLocationIndex))
            {
                newEnemyLocationIndex = Random.Range(0, map.Count);
            }

            Vector3 location = new Vector3(map[newEnemyLocationIndex].x + .5f, map[newEnemyLocationIndex].y + .5f, -1f);
            Instantiate(_enemyPrototypes[typeOfEnemy], location, Quaternion.identity);
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
            Vector2Int startPosition = tileLocation * _tileSize;
            Vector2Int newPosition;

            for (int i = 0; i < _tileSize; i++)
            {
                for (int j = 0; j < _tileSize; j++)
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



























