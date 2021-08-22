using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Queue queue;
    private ConductorController conductor;
    void Start()
    {
        conductor = GameObject.Find("Conductor").GetComponent<ConductorController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Conductor")
        {
            if (queue.CheckIfAvailable())
            {
                conductor.EnableDoorInteraction();
            }
        }
    }
}
