using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCurve : MonoBehaviour
{
    public GameObject lineRendererPrefab;

    private List<GameObject> lineRendererObjects;

    private LineRenderer line;

    public GameObject[] stations;

    private void Start()
    {

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        stations = GameObject.FindGameObjectsWithTag("station");
        lineRendererObjects = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

        destroyPreviousLineRendererObjects();

        Vector3 lastPos = stations[0].transform.position;

        for (int i = 1; i < stations.Length; ++i)
        {
            Vector3 currPos = stations[i].transform.position;
            Vector3 inflexionPoint = getInflexionPoint(lastPos, currPos);
            spawnNewLineSegment(lastPos, inflexionPoint);
            spawnNewLineSegment(inflexionPoint, currPos);
            lastPos = currPos;
        }


    }

    private void destroyPreviousLineRendererObjects()
    {
        for (int i = 0; i < lineRendererObjects.Count; ++i)
            Destroy(lineRendererObjects[i]);
    }

    private void addPointToLineRenderer(Vector3 point)
    {
        line.positionCount++;
        line.SetPosition(line.positionCount - 1, point);
    }

    private void spawnNewLineSegment(Vector3 A, Vector3 B)
    {
        //Spawn the gameobject as a parent of this object
        GameObject gameObject = Instantiate(this.lineRendererPrefab, this.transform);
        this.line = gameObject.GetComponent<LineRenderer>();
        this.lineRendererObjects.Add(gameObject);
        addPointToLineRenderer(A);
        addPointToLineRenderer(B);

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
