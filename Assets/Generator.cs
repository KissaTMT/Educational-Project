using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Generator : MonoBehaviour
{
    [SerializeField] private int _weight;
    [SerializeField] private int _height;
    [SerializeField] private int _epoch;
    [SerializeField] private int _pathLength;

    [SerializeField] private Player _player;
    [SerializeField] private GameObject _barier;

    private bool[,] _cells;

    private void Awake()
    {
        _cells = new bool[_weight, _height];

        for(var x = 0; x < _weight; x++)
        {
            for(var y=0; y < _height; y++)
            {
                _cells[x, y] = true;
            }
        }
    }
    private void Start()
    {
        Project();
        SpawnBariers();
    }
    private void Project()
    {
        var path = new List<Vector2Int>();
        for(var i = 0; i < _epoch; i++)
        {
            var currentPosition = new Vector2Int(_cells.GetLength(0) / 2 + Random.Range(0, _epoch), _cells.GetLength(1) / 2 + Random.Range(0, _epoch));
            for (var j = 0; j < _pathLength; j++) {
                var neighbors = GetFreeNeighbors(currentPosition.x, currentPosition.y);
                path.Add(currentPosition);
                if(neighbors.Count == 0)
                {
                    for(var k = path.Count-1; k >= 0; k--)
                    {
                        neighbors = GetFreeNeighbors(path[k].x, path[k].y);
                        if (neighbors.Count > 0) break;
                    }
                    if (neighbors.Count == 0) break;    
                }
                currentPosition = neighbors[Random.Range(0, neighbors.Count)];
                path.Add(currentPosition);
            }
        }
    }
    private List<Vector2Int> GetFreeNeighbors(int x, int y)
    {
        _cells[x, y] = false;
        var neighbors = new List<Vector2Int>();
        var freeNeighbors = new List<Vector2Int>();
        var offset = 4;

        if (x + offset < _cells.GetLength(0)) neighbors.Add(new Vector2Int(x + 1, y));
        if (x - offset > 0) neighbors.Add(new Vector2Int(x - 1, y));
        if (y + offset < _cells.GetLength(1)) neighbors.Add(new Vector2Int(x, y + 1));
        if (y - offset > 0) neighbors.Add(new Vector2Int(x, y - 1));

        for (var i = 0; i < neighbors.Count; i++)
        {
            if (_cells[neighbors[i].x, neighbors[i].y])
            {
                freeNeighbors.Add(new Vector2Int(neighbors[i].x, neighbors[i].y));
            }
        }
        return freeNeighbors;
    }
    private void SpawnBariers()
    {
        var freeCells = new List<Vector2>();
        var sizeX = _cells.GetLength(0);
        var sizeY = _cells.GetLength(1);
        var barierSizeModifier = 25;
        for (var x = 0; x < sizeX; x++)
        {
            for (var y = 0; y < sizeY; y++)
            {
                var position = GetPositionByCenter(new Vector2(x,y),new Vector2(sizeX,sizeY)) * barierSizeModifier;
                if (_cells[x, y])
                {
                    var barier = Instantiate(_barier, position, Quaternion.identity);
                }
                else freeCells.Add(position);
            }
        }
        MovePlayerToFreeCell(freeCells[Random.Range(0,freeCells.Count)]);
    }
    private void MovePlayerToFreeCell(Vector2 position)
    {
        _player.Transform.position = position;
    }
    private Vector2 GetPositionByCenter(Vector2 vector, Vector2 center)
    {
        return new Vector2(vector.x - (center.x/2),vector.y-(center.y/2));
    }
}
