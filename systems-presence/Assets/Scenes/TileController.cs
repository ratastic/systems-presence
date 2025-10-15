using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Tilemaps;
// using UnityEngine.UI;
// using TMPro;

public class TileController : MonoBehaviour
{
    private Tilemap tilemap;
    public Tile testTile;
    public float waitTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        StartCoroutine(DrawTile());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator DrawTile()
    {
        while (true)
        {
            BoundsInt bounds = tilemap.cellBounds;
            TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

            var changes = new HashSet<(int, int, string)>();

            for (int x = 0; x < bounds.size.x; x++)
            {
                for (int y = 0; y < bounds.size.y; y++)
                {
                    TileBase tile = allTiles[x + y * bounds.size.x];
                    if (tile != null)
                    {
                        CheckRules(x, y, bounds, allTiles, changes);
                        Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
                    }
                }
            }

            foreach ((int, int, string) change in changes)
            {
                if (change.Item3.Equals("create"))
                {
                    tilemap.SetTile(new Vector3Int(change.Item1, change.Item2, 0), testTile);
                }
                else if (change.Item3.Equals("kys"))
                {
                    tilemap.SetTile(new Vector3Int(change.Item1, change.Item2, 0), null);
                }
            }

            yield return new WaitForSeconds(waitTime);
        }
    }
    
    private int OneDimension(int x, int y, int rowLength)
    {
        return (x + y * rowLength);
    }

    private void CheckRules(int x, int y, BoundsInt bounds, TileBase[] allTiles, HashSet<(int, int, string)> changes)
    {
        if (x < bounds.size.x - 1)
        {
            TileBase tile = allTiles[x + 1 + y * bounds.size.x];
            if (tile != null)
            {
                changes.Add((x + 1 + bounds.xMin, y + bounds.yMin, "kys"));
            }
            else
            {
                changes.Add((x + 2 + bounds.xMin, y + bounds.yMin, "create"));
            }
        }
    }
}
