using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

public class SnakeyTiles : MonoBehaviour
{
    private Tilemap tilemap2;
    public Tile snakeTile;

    private Vector2 pos;
    private Vector2 targetPos;

    public float speed = 2f;
    public float jitterAmount = 0.5f;
    public float change = 2f;
    private float timer = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pos = new Vector2(10, 10);
        targetPos = RandomTarget();

        tilemap2 = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > change)
        {
            targetPos = RandomTarget();
            timer = 0f;
        }

        Vector2 direction = (targetPos - pos).normalized;
        pos += direction * speed * Time.deltaTime;

        // Optional: jitter
        pos += new Vector2(Random.Range(-jitterAmount, jitterAmount), Random.Range(-jitterAmount, jitterAmount)) * Time.deltaTime;

        // Convert to tilemap cell
        Vector3Int cellPosition = Vector3Int.FloorToInt(new Vector3(pos.x, pos.y, 0));

        tilemap2.SetTile(cellPosition, snakeTile);
    }

    Vector2 RandomTarget()
    {
        float x = Random.Range(0, 171); 
        float y = Random.Range(0, 90);
        return new Vector2(x, y);
    }
}
