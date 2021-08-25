using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sit : MonoBehaviour
{
    public int UsedSeats { get; set; }
    private List<Passenger>  passengers {get; set;}
    void Start()
    {
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
}
