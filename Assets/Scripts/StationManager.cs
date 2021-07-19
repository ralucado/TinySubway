using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationManager
{

    public List<GameObject> stations;
    public List<Vector3> linePoints;

    public StationManager()
    {
        linePoints = new List<Vector3>();
        stations = new List<GameObject>();
    }

    public int getStationsNumber()
    {
        return stations.Count;
    }

    public GameObject getStation(int i)
    {
        if(i < stations.Count && stations.Count > 0)
        {
            if (i >= 0)
                return stations[i];
            else if (Math.Abs(i) <= stations.Count)
                return stations[stations.Count + i];
        }
        throw new ArgumentException("The required station position is not available");
    }

    public void addStation(GameObject station)
    {
        stations.Add(station);
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
