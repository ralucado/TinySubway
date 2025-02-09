﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject linePrefab;
    public List<GameObject> availableLines;
    public List<GameObject> usedLines;


    private GameObject selectionFromStation;
    private GameObject selectedLine = null;
    private bool usedSelectedLine = false;
    private bool selectMode = false;

    // Start is called before the first frame update
    void Start()
    {
        initVariables();
        populateAvailableLinesArray();
    }

    private void initVariables()
    {
        availableLines = new List<GameObject>();
        usedLines = new List<GameObject>();
        turnOffSelectMode();
    }

    private void populateAvailableLinesArray()
    {
        GameObject blueLine = Instantiate(this.linePrefab, this.transform);
        availableLines.Add(blueLine);

        GameObject pinkLine = Instantiate(this.linePrefab, this.transform);
        pinkLine.GetComponent<LineDrawer>().setLineColor(Color.magenta);
        availableLines.Add(pinkLine);

        GameObject yellowLine = Instantiate(this.linePrefab, this.transform);
        yellowLine.GetComponent<LineDrawer>().setLineColor(Color.yellow);
        availableLines.Add(yellowLine);
    }

    // Update is called once per frame
    void Update()
    {
        if (selectedLine)
        {
            if (selectMode)
            {
                Vector3 mousePositionWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                selectedLine.GetComponent<LineDrawer>().selected(selectionFromStation.transform.position, new Vector3(mousePositionWorld.x, mousePositionWorld.y, 0));
            }
            else
                selectedLine.GetComponent<LineDrawer>().unselected();
        }
    }

    public void stationClicked(GameObject station)
    {
        Debug.Log("GAME_MANAGER::Clicked on station: " + station.name);
        if (availableLines.Count > 0)
        {
            selectNewLine();
            turnOnSelectMode(); 
            selectionFromStation = station;
        }
    }

    public void mouseEnteredStation(GameObject station)
    {
        Debug.Log("GAME_MANAGER::Mouse entered station: " + station.name);
        if(selectMode)
        {
            if (station != selectionFromStation) {
                addStationToSelectedLine(selectionFromStation);
                addStationToSelectedLine(station);
                selectionFromStation = station;
            }
        }

    }

    private void selectNewLine()
    {
        selectedLine = availableLines[0];
        usedSelectedLine = false;
    }


    private void addStationToSelectedLine(GameObject station)
    {
        selectedLine.GetComponent<LineDrawer>().addStation(station);
        if (!usedSelectedLine)
        {
            usedLines.Add(selectedLine);
            availableLines.RemoveAt(0);
            usedSelectedLine = true;
        }
    }

    public void mouseReleased()
    {
        turnOffSelectMode();
    }

    private void turnOnSelectMode()
    {
        Debug.Log("GAME_MANAGER::Turned ON select mode");
        selectMode = true;
    }

    private void turnOffSelectMode()
    {
        Debug.Log("GAME_MANAGER::Turned OFF select mode");
        selectMode = false;
    }

    

}
