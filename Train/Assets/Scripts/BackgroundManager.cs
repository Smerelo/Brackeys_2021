using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public GameObject backgroundObject;
    public GameObject sceneObject;
    public GameObject tunnelObject;
    private int count = 2;
    private bool isAddingPlatforms;
    private Train train;

    public bool Tunnel { get;  set; }

    void Start()
    {
        train = GameObject.Find("Train").GetComponent<Train>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    internal void AddBackground()
    {
        GameObject go =  Instantiate(backgroundObject, new Vector3(transform.position.x + 25 * count, transform.position.y, transform.position.z), Quaternion.identity, transform);
        if (isAddingPlatforms)
        {
            Instantiate(sceneObject, new Vector3(transform.position.x + 25 * count, transform.position.y, transform.position.z), Quaternion.identity, transform);
        }
        if (Tunnel)
        {
            Instantiate(tunnelObject, new Vector3(transform.position.x + 25 * count, transform.position.y, transform.position.z), Quaternion.identity, transform);
            Invoke("StopTunnel", 1f);
        }
        count++;
        if (!isAddingPlatforms)
        {
            Destroy(go, 20);
        }
    }


    private void StopTunnel()
    {
        Tunnel = false;
    }
    internal void SpawnPlatform()
    {
        Debug.Log("here2");

        Tunnel = true;
        isAddingPlatforms = true;
        Invoke("Break", 4F);
    }
    public void StopAddingPlatforms()
    {
        Invoke("StopPlat", 1f);
    }

    private void StopPlat()
    {
        isAddingPlatforms = false;
    }
    private void Break()
    {
        train.Break();
    }
}
