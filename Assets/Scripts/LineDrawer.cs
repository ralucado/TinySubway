using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    public GameObject lineRendererPrefab;

    public List<GameObject> lineRendererObjects;

    public List<GameObject> stations;

    private bool drawMouse;
    private Vector3 mouseStartPosition, mouseEndPosition;

    public void addStation(GameObject station)
    {
        if(stations.Count > 0)
            if (stations[stations.Count - 1] == station)
            {
                Debug.Log("Cannot add station: " + station.name);
                return;

            }
        stations.Add(station);
        Debug.Log("Added station: " + station.name);
    }

    public void startDrawingMouse(Vector3 stationPosition, Vector3 mousePosition)
    {
        drawMouse = true;
        mouseStartPosition = stationPosition;
        mouseEndPosition = mousePosition;
    }

    public void stopDrawingMouse()
    {
        drawMouse = false;
    }


    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        //stations = GameObject.FindGameObjectsWithTag("station");
        lineRendererObjects = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        destroyPreviousLineRendererObjects();
        if (drawMouse)
            drawSegmentBetweenPositions(mouseStartPosition, mouseEndPosition);
        if (stations.Count > 0)
            drawLineBetweenStations();

    }

    private void drawLineBetweenStations()
    {
        Vector3 lastPos = stations[0].transform.position;

        for (int i = 1; i < stations.Count; ++i)
        {
            Vector3 currPos = stations[i].transform.position;
            drawSegmentBetweenPositions(lastPos, currPos);
            lastPos = currPos;
        }
    }

    private void drawSegmentBetweenPositions(Vector3 lastPos, Vector3 currPos)
    {
        Vector3 inflexionPoint = getInflexionPoint(lastPos, currPos);
        spawnNewLineSegment(lastPos, inflexionPoint);
        spawnNewLineSegment(inflexionPoint, currPos);
    }

    private void destroyPreviousLineRendererObjects()
    {
        for (int i = 0; i < lineRendererObjects.Count; ++i)
            Destroy(lineRendererObjects[i]);
        lineRendererObjects.Clear();

    }

    private void spawnNewLineSegment(Vector3 A, Vector3 B)
    {
        //Spawn the gameobject as a parent of this object
        GameObject lineRendererGameObject = Instantiate(this.lineRendererPrefab, this.transform);
        LineRenderer lineRenderer = lineRendererGameObject.GetComponent<LineRenderer>();
        this.lineRendererObjects.Add(lineRendererGameObject);
        addPointToLineRenderer(B, lineRenderer);
        addPointToLineRenderer(A, lineRenderer);
    }

    private void addPointToLineRenderer(Vector3 point, LineRenderer lineRenderer)
    {
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, point);
    }


    private Vector3 getInflexionPoint(Vector3 lastPos, Vector3 currPos)
    {
        Vector3 delta = currPos - lastPos;
        Vector3 inflexionPoint = new Vector3(0, 0, 0);
        float slope = chooseSlope(delta);
        
        if(Mathf.Max(Mathf.Abs(delta.x), Mathf.Abs(delta.y)) == Mathf.Abs(delta.x))
        { //the longest distance to travel is alongside the X axis
            inflexionPoint.y = currPos.y;
            inflexionPoint.x = slope * inflexionPoint.y + (lastPos.x - slope * lastPos.y);
        }
        else
        { //the longest distance to travel is alongside the Y axis
            inflexionPoint.x = currPos.x;
            inflexionPoint.y = slope * inflexionPoint.x + (lastPos.y - slope * lastPos.x);
        }

        return inflexionPoint;
    }

    private float chooseSlope(Vector3 delta)
    {
        //calculate the slope of the diagonal that gets p1 closer to p2 (1 or -1)
        if ((delta.x > 0 && delta.y > 0) || (delta.x < 0 && delta.y < 0))
            return 1.0f;
        return -1.0f;
    }
}
