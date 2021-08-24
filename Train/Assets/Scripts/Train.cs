using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    private List<Passenger> passengers;
    private List<Passenger> illegalPassengers;
    void Start()
    {
        passengers = new List<Passenger>();
        illegalPassengers = new List<Passenger>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void AddPassenger(Passenger tempPassenger, int v)
    {
        if (v == 1)
        {
            passengers.Add(tempPassenger);
        }
        else
        {
            illegalPassengers.Add(tempPassenger);
        }
    }
}
