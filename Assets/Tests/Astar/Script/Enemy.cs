using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public PathFinder pathFinder;
    public float speed;
    private List<Vector2Int> _wayPoints;
    private Coroutine _currentMove;
    private Vector2 currentTargetPos;
    public List<Vector2Int> WayPoints
    {
        get => _wayPoints;
        set
        {
            if (_wayPoints != value)
            {
                _wayPoints = value;

                if (_currentMove != null)
                {
                    StopCoroutine(_currentMove);
                    _currentMove = null;
                }

                if (_wayPoints != null && _wayPoints.Count > 0)
                {
                    _currentMove = StartCoroutine(Move());
                }
            }
        }
    }

    void Start()
    {
        currentTargetPos = pathFinder.target.transform.position;
        WayPoints=pathFinder.PathFinding();
    }


    void Update()
    {
        if (Vector2.Distance(currentTargetPos, (Vector2)pathFinder.target.transform.position) > 1) 
        {
            WayPoints = pathFinder.PathFinding();
            currentTargetPos = pathFinder.target.transform.position;
        }
    }

    private IEnumerator Move()
    {
        if (_wayPoints == null || _wayPoints.Count == 0)
            yield break;

        foreach (var i in WayPoints)
        {
            while (Vector2.Distance(gameObject.transform.position, i) > 0.01f)
            {
                gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, i, Time.deltaTime * speed);
                yield return null;
            }
        }
    }

}
