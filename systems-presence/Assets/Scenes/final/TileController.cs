using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
// using UnityEngine.UI;
// using TMPro;

public class TileController : MonoBehaviour
{
    private Tilemap tilemap;
    public Tile[] tileTypes = new Tile[5];
    public float waitTime; // time to wait between each tile draw
    private int xDir = 1; // direction 
    private int yDir = 0;
    public int eraserSize;
    public int penSize;
    public int sizeChange = 1;
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        StartCoroutine(DrawTile()); // left and right draw
        Randomizer(); // random initial direction
    }

    void Update()
    {
        PlayerInput();
        ToolSizing();
    }

    public void ToolSizing()
    {
        Debug.Log("current pen size:" + penSize + "current eraser size:" + eraserSize);

        if (Input.GetKeyDown(KeyCode.W))
        {
            eraserSize += sizeChange;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            eraserSize -= sizeChange;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            penSize -= sizeChange;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            penSize += sizeChange;
        }

        penSize = Mathf.Clamp(penSize, 1, 12);
        eraserSize = Mathf.Clamp(eraserSize, 1, 12);
    }

    public void Randomizer()
    {
        // set random direction either vertical or horizontal
        Direction dir = (Direction)Random.Range(0, 4);

        switch (dir)
        {
            case Direction.Up:
                xDir = 0;
                yDir = 1;
                break;
            case Direction.Down:
                xDir = 0;
                yDir = -1;
                break;
            case Direction.Left:
                xDir = -1;
                yDir = 0;
                break;
            case Direction.Right:
                xDir = 1;
                yDir = 0;
                break;
        }
    }

    public void PlayerInput()
    {
        if (Input.GetMouseButton(0)) // place tile with left click
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = tilemap.WorldToCell(worldPos);

            int penRadius = penSize;

            for (int x = -penRadius; x <= penRadius; x++)
            {
                for (int y = -penRadius; y <= penRadius; y++) 
                {
                    Vector3Int bigPen = new Vector3Int(cellPos.x + x, cellPos.y + y, 0);
                    Tile randomTile = tileTypes[Random.Range(0, tileTypes.Length)];
                    tilemap.SetTile(bigPen, randomTile); 
                }
            }
        }

        if (Input.GetMouseButton(1)) // remove tile with right click
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = tilemap.WorldToCell(worldPos);

            int eraserRadius = eraserSize;

            // for loops determines size of eraser
            for (int x = -eraserRadius; x <= eraserRadius; x++) // total x range
            {
                for (int y = -eraserRadius; y <= eraserRadius; y++) // total y range
                {
                    Vector3Int bigEraser = new Vector3Int(cellPos.x + x, cellPos.y + y, 0);
                    tilemap.SetTile(bigEraser, null); // place empty tile
                }
            }
        }
    }
    public IEnumerator DrawTile()
    {
        while (true)
        {
            Randomizer();

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
                        //Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
                    }
                }
            }

            foreach ((int, int, string) change in changes) // implement changes
            {
                if (change.Item3.Equals("create")) // create logic
                {
                    Tile randomTile = tileTypes[Random.Range(0, tileTypes.Length)];
                    tilemap.SetTile(new Vector3Int(change.Item1, change.Item2, 0), randomTile);
                }
                else if (change.Item3.Equals("kys")) // uncreate logic
                {
                    tilemap.SetTile(new Vector3Int(change.Item1, change.Item2, 0), null);
                }
            }

            yield return new WaitForSeconds(waitTime);
        }
    }
    // given tile at (x, y) -> check tile in a direction (xdir ydir) -> check if add or remove tile
    private void CheckRules(int x, int y, BoundsInt bounds, TileBase[] allTiles, HashSet<(int, int, string)> changes)
    {
        // convert to grid position in the world
        int worldX = x + bounds.xMin;
        int worldY = y + bounds.yMin;

        // tile next x and y pos is the world coord and placed in the direction
        int nextX = worldX + xDir;
        int nextY = worldY + yDir;

        // if the next x pos aims to cross / exceed vertical bounds, flip xdir
        if (nextX <= bounds.xMin || nextX >= bounds.xMax)
        {
            xDir *= -1;
            nextX = worldX + xDir; // define next x
        }

        // if the next y pos aims to cross / exceed horizontal bounds, flip ydir
        if (nextY <= bounds.yMin || nextY >= bounds.yMax)
        {
            yDir *= -1;
            nextY = worldY + yDir; // define next y
        }

        // logic depending on what tile is in next planned spot
        TileBase nextTile = tilemap.GetTile(new Vector3Int(nextX, nextY, 0));


        if (nextTile != null) // if a tile already resides
        {
            changes.Add((nextX, nextY, "kys")); // remove 
        }
        else
        {
            changes.Add((nextX, nextY, "create")); // add tile otherwise
        }
    }
}

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
