using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [Header("Prefabs & References")]
    [SerializeField] private MazeCell _cellPrefab;
    [SerializeField] private MazeCell _simpleCellPrefab;
    private MazeCell _optimolCellPrefab;
    [SerializeField] private Transform _mazeParent;

    [Header("Maze Settings")]
    [SerializeField, Range(10, 250)] private int _width = 10;
    [SerializeField, Range(10, 250)] private int _height = 10;
    [SerializeField, Min(0f)] private float _cellSpacing = 1f;
    [SerializeField, Min(0f)] private float _generationDelay = 0f;

    public event Action onMazeGenerated;

    private MazeCell[,] _mazeGrid;

    // Exposed properties for UI
    public int Width { get => _width; set => _width = Mathf.Clamp(value, 10, 250); }
    public int Height { get => _height; set => _height = Mathf.Clamp(value, 10, 250); }
    public float CellSpacing { get => _cellSpacing; set => _cellSpacing = Mathf.Max(0f, value); }
    public float GenerationDelay { get => _generationDelay; set => _generationDelay = Mathf.Max(0f, value); }


    /// <summary>
    /// Public method to (re)generate the maze.
    /// </summary>
    public void GenerateMaze()
    {
        StopAllCoroutines();
        ClearMaze();
        StartCoroutine(PerformGeneration());
    }

    /// <summary>
    /// Clears existing maze cells.
    /// </summary>
    private void ClearMaze()
    {
        if (_mazeParent == null) return;
        foreach (Transform child in _mazeParent)
            Destroy(child.gameObject);
    }

    /// <summary>
    /// Coroutine performing a randomized DFS (backtracking) maze generation.
    /// </summary>
    private IEnumerator PerformGeneration()
    {
        // Use the simple cell prefab for large mazes to optimize performance
        if (_width * _height < 1000) _optimolCellPrefab = _cellPrefab; 
        else _optimolCellPrefab = _simpleCellPrefab;

        _mazeGrid = new MazeCell[_width, _height];
        // 1. Instantiate grid of cells
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var cell = Instantiate(_optimolCellPrefab, _mazeParent);
                cell.Initialize(x, y);
                cell.transform.localPosition = new Vector3(x * _cellSpacing, 0, y * _cellSpacing);
                _mazeGrid[x, y] = cell;
            }
            // Yield per row to keep UI responsive
            yield return null;
        }

        onMazeGenerated?.Invoke();

        // 2. Depth-first search with stack
        var stack = new Stack<MazeCell>();
        var start = _mazeGrid[0, 0];
        start.Visit();
        stack.Push(start);

        while (stack.Count > 0)
        {
            var current = stack.Peek();
            var neighbors = GetUnvisitedNeighbors(current);
            if (neighbors.Count > 0)
            {
                var next = neighbors[UnityEngine.Random.Range(0, neighbors.Count)];
                RemoveWallBetween(current, next);
                next.Visit();
                stack.Push(next);
            }
            else
            {
                stack.Pop();
            }

            yield return new WaitForSeconds(_generationDelay);
           
        }
    }

    /// <summary>
    /// Returns a list of unvisited neighboring cells.
    /// </summary>
    private List<MazeCell> GetUnvisitedNeighbors(MazeCell cell)
    {
        var list = new List<MazeCell>();
        int x = cell.Coordinates.x;
        int y = cell.Coordinates.y;
        if (y < _height - 1 && !_mazeGrid[x, y + 1].IsVisited) list.Add(_mazeGrid[x, y + 1]);
        if (x < _width - 1 && !_mazeGrid[x + 1, y].IsVisited) list.Add(_mazeGrid[x + 1, y]);
        if (y > 0 && !_mazeGrid[x, y - 1].IsVisited) list.Add(_mazeGrid[x, y - 1]);
        if (x > 0 && !_mazeGrid[x - 1, y].IsVisited) list.Add(_mazeGrid[x - 1, y]);
        return list;
    }


    /// <summary>
    /// Disables walls between two adjacent cells.
    /// </summary>
    private void RemoveWallBetween(MazeCell a, MazeCell b)
    {
        int dx = b.Coordinates.x - a.Coordinates.x;
        int dy = b.Coordinates.y - a.Coordinates.y;
        if (dx == 1)
        {
            a.SetWall(Direction.East, false);
            b.SetWall(Direction.West, false);
        }
        else if (dx == -1)
        {
            a.SetWall(Direction.West, false);
            b.SetWall(Direction.East, false);
        }
        else if (dy == 1)
        {
            a.SetWall(Direction.North, false);
            b.SetWall(Direction.South, false);
        }
        else if (dy == -1)
        {
            a.SetWall(Direction.South, false);
            b.SetWall(Direction.North, false);
        }
    }
}