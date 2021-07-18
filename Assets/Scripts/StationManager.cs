using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationManager : MonoBehaviour
{


    public List<GameObject> stations;
    public List<Vector3> linePoints;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getStationsNumber()
    {
        return stations.Count;
    }

    public GameObject getStation(int i)
    {
        if(i < stations.Count)
        {
            if (i >= 0)
                return stations[i];
            else if (Math.Abs(i) <= stations.Count)
                return stations[stations.Count - 1];
        }
        throw new ArgumentException("The required station position is not available");
    }

    public void addStation(GameObject station)
    {
        stations.Add(station);
    }
}
