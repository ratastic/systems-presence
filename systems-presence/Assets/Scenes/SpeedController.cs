using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SpeedController : MonoBehaviour
{
    public TileController tc;
    public float changeRate = 0.1f; // how much to change waitTime per key press

    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            tc.waitTime = Mathf.Max(0f, tc.waitTime - changeRate * Time.deltaTime); // limit to 0 or higher
            Debug.Log("waitTime: " + tc.waitTime);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            tc.waitTime += changeRate * Time.deltaTime;
            Debug.Log("waitTime: " + tc.waitTime);
        }
    }
}
