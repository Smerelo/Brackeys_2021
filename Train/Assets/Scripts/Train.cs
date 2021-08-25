using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    private List<Passenger> passengers;
    private List<Passenger> illegalPassengers;
    public List<Sit> seats;
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

    internal Vector3 GetSeat(Vector3 pos, Passenger passenger)
    {

        foreach (Sit seat in seats)
        {
            Debug.Log(seat.UsedSeats);

            if (seat.UsedSeats == 0)
            {
                seat.AddPassenger(passenger);
                return seat.gameObject.transform.position;
            }
            else if (seat.UsedSeats == 1)
            {
                seat.AddPassenger(passenger);
                return new Vector3(seat.gameObject.transform.position.x + 0.5f, seat.gameObject.transform.position.y, pos.z);
            }

        }
        return pos;
    }

    internal void AddIllegalPassenger(Passenger passenger)
    {
        illegalPassengers.Add(passenger);
    }
}
