using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queue : MonoBehaviour
{
    public List<GameObject> spawnPoints;
    public GameObject watingZone;
    public GameObject passengerPrefab;
    private List<Passenger> passengers;
    private float spawnTimer;
    private float spawnTime = 0.2f;


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
        if (passengers.Count < 10)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnTime)
            {
                spawnTimer = 0;
                int randPos = UnityEngine.Random.Range(0, 4);
                passengers.Add(Instantiate(passengerPrefab, spawnPoints[randPos].transform.position, Quaternion.identity).GetComponent<Passenger>());
                passengers[passengers.Count - 1].transform.SetParent(this.transform);
                passengers[passengers.Count - 1].positionInLine = passengers.Count;
                passengers[passengers.Count - 1].watingPos = watingZone.transform.position;
            }
        }
    }
}
