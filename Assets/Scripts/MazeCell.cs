using UnityEngine;

public enum Direction { North, East, South, West }


public class MazeCell : MonoBehaviour
{
    [Header("Wall Objects")]
    [SerializeField] private GameObject _wallNorth;
    [SerializeField] private GameObject _wallEast;
    [SerializeField] private GameObject _wallSouth;
    [SerializeField] private GameObject _wallWest;

    [Header("Cell State")]
    [SerializeField] private GameObject _unvisitedCell;
    [SerializeField] private GameObject _visitedCell;

    public Vector2Int Coordinates { get; private set; }
    public bool IsVisited { get; private set; }


    /// <summary>
    /// Initializes the cell with grid coordinates and default state.
    /// </summary>
    public void Initialize(int x, int y)
    {
        Coordinates = new Vector2Int(x, y);
        IsVisited = false;
        _unvisitedCell.SetActive(true);
        _visitedCell.SetActive(false);
    }

    /// <summary>
    /// Marks this cell as visited and updates its visual state.
    /// </summary>
    public void Visit()
    {
        IsVisited = true;
        _unvisitedCell.SetActive(false);
        _visitedCell.SetActive(true);
    }

    /// <summary>
    /// Enables or disables the wall in the given direction.
    /// </summary>
    public void SetWall(Direction dir, bool state)
    {
        switch (dir)
        {
            case Direction.North: _wallNorth?.SetActive(state); break;
            case Direction.East: _wallEast?.SetActive(state); break;
            case Direction.South: _wallSouth?.SetActive(state); break;
            case Direction.West: _wallWest?.SetActive(state); break;
        }
    }
}
