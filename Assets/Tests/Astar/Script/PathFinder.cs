using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassedPoint
{
    public Vector2Int pos;
    public int gCost;
    public int hCost;
    public int fCost;
    public Vector2Int parent;
}

public class PathFinder : MonoBehaviour
{
    public GameObject target;
    private Vector2Int _startPos;
    public Vector2Int StartPos
    {
        get => _startPos; 
        set
        {
            _startPos = value;    
        }
    }
    public PriorityQueue<Vector2Int> openQueue = new PriorityQueue<Vector2Int>();
    public List<PassedPoint> closedList = new List<PassedPoint>();

    private int[] _eightX = new int[8] { 0, 1, 1, 0, -1, -1, 1, -1 };
    private int[] _eightY = new int[8] { 1, 0, 1, -1, 0, -1, -1, 1 };

    public List<Vector2Int> PathFinding()
    {
        var curPos = new Vector2Int((int)Mathf.Round(gameObject.transform.position.x), (int)Mathf.Round(gameObject.transform.position.y));
        StartPos = curPos;
        var targetPos = new Vector2Int((int)Mathf.Round(target.transform.position.x), (int)Mathf.Round(target.transform.position.y));

        openQueue.Clear();
        closedList.Clear();

        var openDict = new Dictionary<Vector2Int, PassedPoint>();
        var closedDict = new Dictionary<Vector2Int, PassedPoint>();

        var startNode = new PassedPoint
        {
            pos = curPos,
            gCost = 0,
            hCost = (int)Vector2Int.Distance(curPos, targetPos),
            fCost = (int)Vector2Int.Distance(curPos, targetPos),
            parent = curPos
        };

        openQueue.Enqueue(curPos, startNode.fCost);
        openDict[curPos] = startNode;

        while (openQueue.Count > 0)
        {
            curPos = openQueue.Dequeue();
            var curNode = openDict[curPos];
            openDict.Remove(curPos);
            closedDict[curPos] = curNode;

            if (curPos == targetPos)
                break;

            for (int i = 0; i < 8; i++)
            {
                var newPos = new Vector2Int(curPos.x + _eightX[i], curPos.y + _eightY[i]);
                var mapInfos = TileMapManager.Instance.mapInfos;

                if (!mapInfos.ContainsKey(new Vector3Int(newPos.x, newPos.y, 0)) || !mapInfos[new Vector3Int(newPos.x, newPos.y, 0)].ableToGo)
                {
                    continue;
                }
                if (closedDict.ContainsKey(newPos))
                {
                    continue;
                }

                int g = curNode.gCost + 1;
                int h = (int)Vector2Int.Distance(newPos, targetPos);
                int f = g + h;

                if (openDict.TryGetValue(newPos, out var existingNode))
                {
                    if (g < existingNode.gCost)
                    {
                        existingNode.gCost = g;
                        existingNode.fCost = f;
                        existingNode.parent = curPos;
                        openQueue.Enqueue(newPos, f);
                    }
                }
                else
                {
                    var newNode = new PassedPoint
                    {
                        pos = newPos,
                        gCost = g,
                        hCost = h,
                        fCost = f,
                        parent = curPos
                    };
                    openQueue.Enqueue(newPos, f);
                    openDict[newPos] = newNode;
                }
            }
        }

        if (closedDict.ContainsKey(targetPos))
        {
            var path = new List<Vector2Int>();
            var trace = targetPos;
            while (trace != StartPos)
            {
                path.Add(trace);
                trace = closedDict[trace].parent;
            }
            path.Add(StartPos);
            path.Reverse();

            /*Debug.Log("경로 출력:");
            foreach (var p in path)
            {
                Debug.Log(p);
            }*/

            closedList.Clear();
            foreach (var pos in path)
            {
                closedList.Add(new PassedPoint { pos = pos });
            }
            return path;
        }
        return null;
    }
}
