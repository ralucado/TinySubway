using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    public GameObject linePrefab;

    public List<GameObject> availableLines;
    public List<GameObject> usedLines;
    private GameObject selectionFromStation;
    private GameObject selectedLine = null;

    public bool selectMode;
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
        GameObject defaultLineGameObject = Instantiate(this.linePrefab, this.transform);
        availableLines.Add(defaultLineGameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (selectedLine)
        {
            if (selectMode)
                selectedLine.GetComponent<LineDrawer>().startDrawingMouse(selectionFromStation.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            else
                selectedLine.GetComponent<LineDrawer>().stopDrawingMouse();
        }
    }

    public void stationClicked(GameObject station)
    {
        Debug.Log("Clicked on station: " + station.name);
        if(!selectedLine)
        {
            if (availableLines.Count > 0)
                selectNewLine();
            else
                return;
        }
        turnOnSelectMode();
        selectionFromStation = station;
    }

    public void mouseEnteredStation(GameObject station)
    {
        //Debug.Log("Mouse entered station: " + station.name);
        if(selectMode && station != selectionFromStation)
        {
            addStationToSelectedLine(selectionFromStation);
            addStationToSelectedLine(station);
            selectionFromStation = station;
        }

    }

    private void selectNewLine()
    {
        if(availableLines.Count > 0)
        {
            selectedLine = availableLines[0];
            usedLines.Add(selectedLine);
            availableLines.RemoveAt(0);
        }
        
    }


    private void addStationToSelectedLine(GameObject station)
    {
        selectedLine.GetComponent<LineDrawer>().addStation(station);
    }

    public void mouseReleased()
    {
        turnOffSelectMode();
    }

    private void turnOnSelectMode()
    {
        Debug.Log("Turned ON select mode");
        selectMode = true;
    }

    private void turnOffSelectMode()
    {
        Debug.Log("Turned OFF select mode");
        selectMode = false;
    }

    

}
