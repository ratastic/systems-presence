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
    public Tile randomTile;
    public float waitTime;

    private int xDir = 1; 
    private int yDir = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        StartCoroutine(DrawTile()); // left and right draw
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator DrawTile()
    {
        while (true)
        {
            BoundsInt bounds = tilemap.cellBounds; // boundsint gives bounds of where tiles can reasonably be
            TileBase[] allTiles = tilemap.GetTilesBlock(bounds); // gets the tiles in terms of 1D array representation

            // reimagine grid as 1 "row" of "things"

            var changes = new HashSet<(int, int, string)>(); // mark all changes needed to be made

            for (int x = 0; x < bounds.size.x; x++) 
            {
                for (int y = 0; y < bounds.size.y; y++) // iterate over x y
                {
                    TileBase tile = allTiles[x + y * bounds.size.x]; // get specifc tile
                    if (tile != null) // if any tile is there, check rules
                    {
                        CheckRules(x, y, bounds, allTiles, changes); // pass in changes
                        Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
                    }
                }
            }

            foreach ((int, int, string) change in changes) // implement changes
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

    private void CheckRules(int x, int y, BoundsInt bounds, TileBase[] allTiles, HashSet<(int, int, string)> changes)
    {
        // if (x < bounds.size.x - 1) // check if can populate to the right
        // {
        //     TileBase tile = allTiles[x + 1 + y * bounds.size.x]; // "+1" get tile to the right
        //     if (tile != null)
        //     {
        //         changes.Add((x + 1 + bounds.xMin, y + bounds.yMin, "kys")); // if tile to the right, kys
        //     }
        //     else
        //     {
        //         changes.Add((x + 2 + bounds.xMin, y + bounds.yMin, "create")); // if no tile to the right, add
        //     }
        // }
        // if x = 0 die; if x = 171 die
        // if y = 91 die; if y = 0 die

        //if (testTile = )

        int worldX = x + bounds.xMin;
        int worldY = y + bounds.yMin;

        int nextX = worldX + xDir;
        int nextY = worldY + yDir;

        if (nextX <= bounds.xMin || nextX >= bounds.xMax)
        {
            xDir *= -1;
            nextX = worldX + xDir;
        }

        if (nextY <= bounds.yMin || nextY >= bounds.yMax)
        {
            yDir *= -1;
            nextY = worldY + yDir;
        }

        TileBase nextTile = tilemap.GetTile(new Vector3Int(nextX, nextY, 0));

        if (nextTile != null)
        {
            changes.Add((nextX, nextY, "kys"));
        }
        else
        {
            changes.Add((nextX, nextY, "create"));
        }
    }
}
