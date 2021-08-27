using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float lengt, startpos;
    public GameObject cam;
    public float parallax;
    void Start()
    {
        startpos = transform.position.x;
        
    }

    // Update is called once per frame
    void Update()
    {
        float dist = (cam.transform.position.x * parallax);
        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);
    }
}
