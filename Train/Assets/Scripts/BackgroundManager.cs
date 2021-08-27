using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public GameObject backgroundObject;
    public GameObject sceneObject;
    private int count = 2;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    internal void AddBackground()
    {
       GameObject go =  Instantiate(backgroundObject, new Vector3(transform.position.x + 25 * count, transform.position.y, transform.position.z), Quaternion.identity, transform);
        count++;
        Destroy(go, 5);
    }
}
