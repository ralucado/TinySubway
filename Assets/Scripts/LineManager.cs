using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    public GameObject linePrefab;

    private List<GameObject> availableLines;
    private List<GameObject> usedLines;
    private GameObject selectedLine;

    private bool selectMode = false;
    // Start is called before the first frame update
    void Start()
    {
        initVariables();
        populateAvailableLinesArray();
    }

    private void populateAvailableLinesArray()
    {
        GameObject defaultLineGameObject = Instantiate(this.linePrefab, this.transform);
        availableLines.Add(defaultLineGameObject);
    }

    private void initVariables()
    {
        availableLines = new List<GameObject>();
        usedLines = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (selectMode)
        {
            selectedLine.GetComponent<LineDrawer>().setMousePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    public void stationClicked(GameObject station)
    {
        Debug.Log("Clicked on station: " + station.name);
        if (availableLines.Count > 0)
        {
            selectNewLine();
            turnOnSelectMode();
            addStationToSelectedLine(station);
        }
    }

    public void mouseEnteredStation(GameObject station)
    {
        if (selectMode)
        {
            addStationToSelectedLine(station);
        }
    }

    private void selectNewLine()
    {
        selectedLine = availableLines[0];
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
        selectMode = true;
        selectedLine.GetComponent<LineDrawer>().setSelectMode(selectMode);
    }

    private void turnOffSelectMode()
    {
        selectMode = false;
        selectedLine.GetComponent<LineDrawer>().setSelectMode(selectMode);
    }

    

}
