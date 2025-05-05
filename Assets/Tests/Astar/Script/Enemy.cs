using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public PathFinder pathFinder;
    public float speed;
    private List<Vector2Int> _wayPoints;
    private Coroutine _currentMove;
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
                }
                else
                {
                    _currentMove = StartCoroutine(Move());
                }
            }
        }
    }
        
    void Start()
    {
        WayPoints=pathFinder.PathFinding();
    }


    void Update()
    {
    }

    private IEnumerator Move()
    {
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
