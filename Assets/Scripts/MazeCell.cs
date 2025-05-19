using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    [SerializeField]
    private GameObject _wallN;

    [SerializeField]
    private GameObject _wallE;

    [SerializeField]
    private GameObject _wallS;

    [SerializeField]
    private GameObject _wallW;

    [SerializeField]
    private GameObject _unvisitedCell;

    [SerializeField]
    private GameObject _visitedCell;

    public bool IsVisited { get; private set; }

    public void Visit()
    {
        IsVisited = true;
        _unvisitedCell.SetActive(false);
        _visitedCell.SetActive(true);
    }

    public void SetWall(int direction, bool state)
    {
        switch (direction)
        {
            case 0:
                _wallN.SetActive(state);
                break;
            case 1:
                _wallE.SetActive(state);
                break;
            case 2:
                _wallS.SetActive(state);
                break;
            case 3:
                _wallW.SetActive(state);
                break;
            default:
                Debug.LogError("Invalid wall direction");
                break;
        }
    }
}
