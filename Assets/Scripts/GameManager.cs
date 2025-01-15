using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject linePrefab;
    private List<GameObject> availableLines;
    private List<GameObject> usedLines;
    [SerializeField] List<Station> stations;

    private GameObject selectionFromStation;
    private GameObject selectedLine = null;
    private bool usedSelectedLine = false;
    private bool selectMode = false;

    // Start is called before the first frame update
    void Start()
    {
        initVariables();
        populateAvailableLinesArray();
        registerListenerMethods();
    }

    private void registerListenerMethods() {
        for (int i = 0; i < stations.Count; ++i) {
            stations[i].a_clicked += onStationClicked;
            stations[i].a_released += onMouseReleased;
            stations[i].a_entered += onMouseEnteredStation;
        }
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

        if (selectMode){
            Vector3 mousePositionWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selectedLine.GetComponent<LineDrawer>().drawLineToCursor(selectionFromStation.transform.position, new Vector3(mousePositionWorld.x, mousePositionWorld.y, 0));
        } else
        selectedLine.GetComponent<LineDrawer>().stopDrawingOnCursor();
    }

    public void onStationClicked(GameObject station)
    {
        Debug.Log("GAME_MANAGER::Clicked on station: " + station.name);
        if (availableLines.Count > 0)
        {
            selectNewLine();
            turnOnSelectMode(); 
            selectionFromStation = station;
        }
    }

    public void onMouseEnteredStation(GameObject station)
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

    public void onMouseReleased()
    {
        turnOffSelectMode();
        //selectedLine = null;
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
