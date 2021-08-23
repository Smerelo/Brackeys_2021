using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queue : MonoBehaviour
{
    public List<GameObject> spawnPoints;
    public GameObject watingZone;
    public GameObject passengerPrefab;
    public GameObject window;
    private List<Passenger> passengers;
    private Passenger passengerInWindow;
    private float spawnTimer;
    private float spawnTime = 0.2f;
    public float restlessTimerEnd;
    private float restlessTimer = 0;
    private float checkTimer;
    private float maxCheckTimer = 10;
    private bool passengersAreRestless { get; set; }
    public bool windowIsOccupied { get;  set; }

    void Start()
    {
        passengers = new List<Passenger>();      
    }

    internal bool CheckIfAvailable()
    {
        return passengers[0].CheckIfReady();
    }

    // Update is called once per frame
    void Update()
    {
        RestlessPassengersLogic();
        SpwanerLogic();
    }

    private void SpwanerLogic()
    {
        if (passengers.Count < 10)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnTime)
            {
                spawnTimer = 0;
                int randPos = UnityEngine.Random.Range(0, 4);
                passengers.Add(Instantiate(passengerPrefab, spawnPoints[randPos].transform.position, Quaternion.identity).GetComponent<Passenger>());
                passengers[passengers.Count - 1].transform.SetParent(this.transform);
                passengers[passengers.Count - 1].positionInLine = passengers.Count -1;
                passengers[passengers.Count - 1].watingPos = watingZone.transform.position;
            }
        }
    }

   private void RestlessPassengersLogic()
    {
        restlessTimer += Time.deltaTime;
        if (restlessTimer >= restlessTimerEnd && !windowIsOccupied)
        {
            passengersAreRestless = true;
            checkTimer += Time.deltaTime;
        }
        if (checkTimer >= maxCheckTimer && !windowIsOccupied)
        {
            int i = passengersAreRestless ? UnityEngine.Random.Range(0, 2) : UnityEngine.Random.Range(0, 5);
            if (passengersAreRestless && i == 1)
            {
                windowIsOccupied = true;
                int r = UnityEngine.Random.Range(1, 5);
                passengerInWindow = passengers[r];
                passengerInWindow.GoToWindow(window);
                passengers.Remove(passengerInWindow);
                MovePassengers(r);
                restlessTimer = 0;
            }
            else if (i > 2)
            {
                windowIsOccupied = true;
                int r = UnityEngine.Random.Range(1, 5);
                passengerInWindow = passengers[r];
                passengerInWindow.GoToWindow(window);
                passengers.Remove(passengerInWindow);
                MovePassengers(r);
                restlessTimer = 0;
            }
            else
            {
                checkTimer = 0;
            }
        }
    }

    private void MovePassengers(int i)
    {
        {
            passengers[i].positionInLine = i;
            passengers[i].Move();
            i++;
        }
    }
}
