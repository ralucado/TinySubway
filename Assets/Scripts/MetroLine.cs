using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetroLine
{

    public List<GameObject> stations;

    public MetroLine()
    {
        stations = new List<GameObject>();
    }

    public int getStationsNumber()
    {
        return stations.Count;
    }

    public GameObject getStation(int i)
    {
        if (i < stations.Count && stations.Count > 0)
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

}
