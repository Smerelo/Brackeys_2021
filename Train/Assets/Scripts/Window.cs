using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    private ConductorController conductor;
    public Queue queue;
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
        if (collision.name == "Conductor" && queue.windowIsOccupied)
        {
            conductor.StartWindowMiniGame(transform.position);
        }
    }
}
