using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public GameObject backgroundObject;
    public GameObject sceneObject;
    private int count = 2;
    private bool isAddingPlatforms;
    private Train train;

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
        

        count++;
        if (!isAddingPlatforms)
        {
            Destroy(go, 10);
        }
    }

    internal void SpawnPlatform()
    {
        isAddingPlatforms = true;
        Invoke("Break", 2F);
    }
    public void StopAddingPlatforms()
    {
        isAddingPlatforms = false;
    }
    private void Break()
    {
        train.Break();
    }
}
