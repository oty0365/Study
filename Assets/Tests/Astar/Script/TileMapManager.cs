using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileInfo
{
    public TileBase tileType;
    public bool ableToGo;
}

public class TileMapManager : MonoBehaviour
{
    public static TileMapManager Instance { get; private set; }
    public Tilemap tileMap;
    public Dictionary<Vector3Int,TileInfo> mapInfos = new Dictionary<Vector3Int, TileInfo>();

    private void Awake()
    {
        Instance = this;
        SetMap();
    }

    public void SetMap()
    {
        mapInfos.Clear();
        BoundsInt bounds = tileMap.cellBounds;

        for (int y = bounds.yMin; y < bounds.yMax; y++)
        {
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                TileInfo tileInfo = new TileInfo();
                mapInfos.Add(pos, tileInfo);
                TileBase tile = tileMap.GetTile(pos);
                if (tile == null)
                {
                    mapInfos[pos].ableToGo = true;
                }
                else
                {
                    mapInfos[pos].ableToGo = false;
                    mapInfos[pos].tileType = tile;
                }
            }
        }
        
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Debug.Log(mapInfos[new Vector3Int(x, y)]?.tileType?.name + "," + mapInfos[new Vector3Int(x, y)].ableToGo);
            }
        }
    }

    void Start()
    {

    }
}
