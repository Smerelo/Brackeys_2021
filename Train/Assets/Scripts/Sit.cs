using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sit : MonoBehaviour
{
    public int UsedSeats;
    private Train train;
    private List<Passenger>  passengers {get; set;}
    private ConductorController conductor;
    void Start()
    {
        train = GameObject.Find("Train").GetComponent<Train>();
        conductor = GameObject.Find("Conductor").GetComponent<ConductorController>();
        passengers = new List<Passenger>();
        UsedSeats = 0;
    }

    public void AddPassenger(Passenger passenger)
    {
        passengers.Add(passenger);
        UsedSeats += 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && train.IsMoving && UsedSeats > 0 && CheckForIllegalPassengers())
        {
            conductor.EnableSeatCheck(this);
        }
    }

    private bool CheckForIllegalPassengers()
    {
        foreach (Passenger p in passengers)
        {
            if (p.isIllegal)
            {
                return true;
            }
        }
        return false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && train.IsMoving && UsedSeats > 0)
        {
            conductor.DisableSeatCheck();
        }
    }

    internal void ExpelPassengers()
    {
        foreach (Passenger p in passengers)
        {
            p.Expel();
        }
        passengers.Clear();
        UsedSeats = 0;
    }
}
