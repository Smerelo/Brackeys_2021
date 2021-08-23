using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ConductorController : MonoBehaviour
{
    public TextMeshProUGUI promtText;
    public float moveSpeed;
    public GameObject mask;
    public bool canCheckTicket { get; set; }
    public bool IsInsideDoor { get; set; }
    private bool IsBusy { get; set; }

    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (!IsBusy)
        {
            Move();
        }
    }

    private void Move()
    {
        float horizontalMovement = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime; ;
        transform.position += new Vector3(horizontalMovement, 0, 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canCheckTicket) 
        {
            mask.SetActive(true);
            IsBusy = true;
            canCheckTicket = false;
            promtText.gameObject.SetActive(false);
        }
    }

    public void HideTicketUI()
    {
        mask.SetActive(false);
        IsBusy = false;
        canCheckTicket = true;
        promtText.gameObject.SetActive(true);
    }

    internal void EnableDoorInteraction()
    {
        canCheckTicket = true;
        promtText.gameObject.SetActive(true);
    }

    internal void DisableDoorInteraction()
    {
        canCheckTicket = false;
        promtText.gameObject.SetActive(false); 
    }
}
