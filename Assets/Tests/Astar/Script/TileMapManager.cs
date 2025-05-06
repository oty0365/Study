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
    public GameObject floor;
    public GameObject wall;

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
                    //Instantiate(floor, pos, Quaternion.identity);
                    mapInfos[pos].ableToGo = true;
                }
                else
                {
                    //Instantiate(wall, pos, Quaternion.identity);
                    mapInfos[pos].ableToGo = false;
                    mapInfos[pos].tileType = tile;
                }
            }
        }
    }

    void Start()
    {

    }
}
