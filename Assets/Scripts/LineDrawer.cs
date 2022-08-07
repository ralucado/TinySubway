using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    public GameObject middleLineRendererPrefab;
    public GameObject endLineRendererPrefab;

    public List<GameObject> lineRendererObjects;

    public MetroLine metroLine;
    public List<Vector3> linePoints;



    private bool isSelected;
    private Color lineColor = Color.cyan;
    private Vector3 mouseStartPosition, mouseEndPosition;

    public void setLineColor(Color color)
    {
        lineColor = color;
    }

    public void addStation(GameObject station)
    {
        if(metroLine.getStationsNumber() > 0)
            if (metroLine.getStation(-1) == station)
            {
                Debug.Log("Cannot add station: " + station.name);
                return;
            }
        metroLine.addStation(station);
        Debug.Log("Added station: " + station.name);
    }

    public void selected(Vector3 stationPosition, Vector3 mousePosition)
    {
        isSelected = true;
        mouseStartPosition = stationPosition;
        mouseEndPosition = mousePosition;
    }

    public void unselected()
    {
        isSelected = false;
    }


    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        lineRendererObjects = new List<GameObject>();
        metroLine = new MetroLine();
        linePoints = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        destroyPreviousLineRendererObjects();
        removeDrawnStationLines();
        if (metroLine.getStationsNumber() > 1)
            drawLineBetweenStations();
        if (isSelected)
            drawSegmentBetweenPositions(mouseStartPosition, mouseEndPosition);
        drawLineEndings();

    }

    private void removeDrawnStationLines()
    {
    }

    private void drawLineEndings()
    {
        if (getLinePointsNumber() >= 2)
        {
            spawnLineEnding(getLinePoint(0), getLinePoint(1));
            if (!isSelected)
                spawnLineEnding(getLinePoint(-1), getLinePoint(-2));
        }
    }

    private void spawnLineEnding(Vector3 end, Vector3 inflexion)
    {

        Vector3 direction = Vector3.Normalize(end - inflexion);
        Vector3 lineEnd = end + direction;
        spawnNewLineSegment(end, lineEnd, endLineRendererPrefab);
        Vector2 perpendicular = Vector2.Perpendicular(new Vector2(direction.x, direction.y));
        spawnNewLineSegment(lineEnd, lineEnd + new Vector3(perpendicular.x, perpendicular.y, lineEnd.z)/2, endLineRendererPrefab);
        spawnNewLineSegment(lineEnd, lineEnd - new Vector3(perpendicular.x, perpendicular.y, lineEnd.z)/2, endLineRendererPrefab);

    }

    private void drawLineBetweenStations()
    {
        Vector3 lastPos = metroLine.getStation(0).transform.position + getLineOffset(metroLine.getStation(0));
        
        for (int i = 1; i < metroLine.getStationsNumber(); ++i)
        {
            Vector3 currPos = metroLine.getStation(i).transform.position + getLineOffset(metroLine.getStation(i));
            drawSegmentBetweenPositions(lastPos, currPos);
            lastPos = currPos;
        }
        addLinePoint(lastPos);
    }

    private Vector3 getLineOffset(GameObject stationObject)
    {
        Station station = stationObject.GetComponent<Station>();
        if (station.getNumberOfDrawnLines() == 0)
            return new Vector3(0, 0, 0);
        else
        {
            int isThird = station.getNumberOfDrawnLines() % 2;
            return new Vector3(0.25f-(0.5f * isThird), 0.25f - (0.5f * isThird), 0);
        }
    }

    private void drawSegmentBetweenPositions(Vector3 lastPos, Vector3 currPos)
    {
        Vector3 inflexionPoint = getInflexionPoint(lastPos, currPos);
        spawnNewLineSegment(lastPos, inflexionPoint, middleLineRendererPrefab);
        addLinePoint(lastPos);
        spawnNewLineSegment(inflexionPoint, currPos, middleLineRendererPrefab);
        addLinePoint(inflexionPoint);
    }

    private void destroyPreviousLineRendererObjects()
    {
        for (int i = 0; i < lineRendererObjects.Count; ++i)
            Destroy(lineRendererObjects[i]);
        lineRendererObjects.Clear();
        clearLinePoints();
    }

    private void spawnNewLineSegment(Vector3 A, Vector3 B, GameObject lineRendererPrefab)
    {
        //Spawn the gameobject as a parent of this object
        GameObject lineRendererGameObject = Instantiate(lineRendererPrefab, this.transform);
        LineRenderer lineRenderer = lineRendererGameObject.GetComponent<LineRenderer>();
        lineRenderer.endColor = lineRenderer.startColor = lineColor;
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

    internal Vector3 getLinePoint(int i)
    {
        if (i < linePoints.Count && linePoints.Count > 0)
        {
            if (i >= 0)
                return linePoints[i];
            else if (Math.Abs(i) <= linePoints.Count)
                return linePoints[linePoints.Count + i];
        }
        throw new ArgumentException("The required line point position is not available");
    }

    internal int getLinePointsNumber()
    {
        return linePoints.Count;
    }

    internal void addLinePoint(Vector3 point)
    {
        linePoints.Add(point);
    }

    internal void clearLinePoints()
    {
        linePoints.Clear();
    }
}