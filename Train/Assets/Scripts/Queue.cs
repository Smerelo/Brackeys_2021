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
    public GameObject awayPoint;
    public GameObject insidePoint;
    public float restlessTimerEnd;

    private List<Passenger> passengers;
    private ConductorController player;
    private Passenger passengerInWindow;
    private float spawnTimer;
    private float spawnTime = 0.2f;
    private float restlessTimer = 0;
    private float checkTimer;
    private float maxCheckTimer = 5;
    private Train train;
    private bool queueIsStoped;

    private bool passengersAreRestless { get; set; }
    public bool windowIsOccupied { get;  set; }

    void Start()
    {
        queueIsStoped = true;
        player = GameObject.Find("Conductor").GetComponent<ConductorController>();
        passengers = new List<Passenger>();
        train = GameObject.Find("Train").GetComponent<Train>();
    }

    internal void StopQueue()
    {
        queueIsStoped = true;
        if (windowIsOccupied)
        {
            RemovePassengerInWindow();
        }
        RemovePassengers();
    }

    private void RemovePassengers()
    {
        foreach (Passenger p in passengers)
        {
            if (p != null)
            {
                p.gameObject.transform.parent = null;
                p.Delete();
            }
        }
        passengers.Clear();
    }

    internal bool CheckIfAvailable()
    {
        if (passengers.Count > 0)
        {
            return passengers[0].CheckIfReady();
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!queueIsStoped)
        {
            RestlessPassengersLogic();
            SpwanerLogic();
        }
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

    internal void StartQueue()
    {
        queueIsStoped = false;
    }

    private void RestlessPassengersLogic()
    {
        restlessTimer += Time.deltaTime;
        checkTimer += Time.deltaTime;
        if (restlessTimer >= restlessTimerEnd && !windowIsOccupied && train.time > 10)
        {
            AudioManager.AudioInstance.Play("AngryCrowd");
            passengersAreRestless = true;
        }
        if (checkTimer >= maxCheckTimer && !windowIsOccupied)
        {
            int i = passengersAreRestless ? UnityEngine.Random.Range(0, 2) : UnityEngine.Random.Range(0, 5);
            if (passengersAreRestless && i == 1)
            {
                SelectRandomPassenger();
            }
            else if (!passengersAreRestless && i > 3)
            {
                SelectRandomPassenger();
            }
            else
            {
                checkTimer = 0;
            }
        }
    }

    private void SelectRandomPassenger() 
    {
        windowIsOccupied = true;
        int r = UnityEngine.Random.Range(1, 5);
        passengerInWindow = passengers[r];
        passengerInWindow.GoToWindow(window);
        passengers.Remove(passengerInWindow);
        MovePassengers(r);
        restlessTimer = 0;
    }

    internal void RejectPassenger()
    {
        Passenger tempPassenger = passengers[0];
        passengers.Remove(tempPassenger);
        tempPassenger.GoOffScreen(awayPoint.transform.position);
        MovePassengers(0);
        restlessTimer = 0;
        AudioManager.AudioInstance.Stop("AngryCrowd;");
        passengersAreRestless = false;
    }

    internal void ShowPassengerPrompt(UniqueStory uniqueStory)
    {
        passengers[0].ShowPrompt(uniqueStory.dialog);
    }

    internal void AcceptPassenger(bool fake)
    {
        Passenger tempPassenger = passengers[0];
        if (fake)
        {
            tempPassenger.isIllegal = true;
            train.AddPassenger(passengers[0], 0);
        }
        else
            train.AddPassenger(passengers[0], 1);
        passengers.Remove(tempPassenger);
        tempPassenger.MoveInsideTheTrain(insidePoint.transform.position);
        MovePassengers(0);
        restlessTimer = 0;
        passengersAreRestless = false;
        AudioManager.AudioInstance.Stop("AngryCrowd;");
    }

    public void PassengerInWindowGotIn()
    {
        train.AddIllegalPassenger(passengerInWindow);
        passengerInWindow = null;
        windowIsOccupied = false;
        checkTimer = 0;
        player.StopAction();
    }

    internal void RemovePassengerInWindow()
    {
        if (passengerInWindow != null)
        {
            passengerInWindow.GoOffScreen(awayPoint.transform.position);
            passengerInWindow = null;
            windowIsOccupied = false;
            checkTimer = 0;
        }
    }

    private void MovePassengers(int i)
    {
        while(i < passengers.Count)
        {
            passengers[i].positionInLine = i;
            passengers[i].Move();
            i++;
        }
    }
}
