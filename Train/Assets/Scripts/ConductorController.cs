using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConductorController : MonoBehaviour
{
    public float moveSpeed;
    private bool canCheckTicket { get; set; }
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float horizontalMovement = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime; ;
        transform.position += new Vector3(horizontalMovement, 0, 0);
    }

    void Update()
    {
        
    }

    internal void EnableDoorInteraction()
    {
        canCheckTicket = true;
    }
}
