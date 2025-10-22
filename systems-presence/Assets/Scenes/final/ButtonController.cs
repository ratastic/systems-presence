using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Tilemaps;

public class ButtonController : MonoBehaviour
{
    public GameObject instructionsPanel;
    public Tilemap tm;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TogglePanelView()
    {
        instructionsPanel.SetActive(!instructionsPanel.activeSelf);
    }
    
    public void WipeBoard()
    {
        tm.ClearAllTiles();
    }
}
