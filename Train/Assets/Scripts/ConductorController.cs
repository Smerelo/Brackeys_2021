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
    public GameObject progressBar;
    public GameObject mask;
    public bool canCheckTicket { get; set; }
    public bool IsInsideDoor { get; set; }
    private bool IsBusy { get; set; }
    public bool IsMashing { get; private set; }
    private Queue queue;

    private Slider slider;
    void Start()
    {
        slider = progressBar.GetComponent<Slider>();
    }

    private void FixedUpdate()
    {
        if (!IsBusy)
        {
            Move();
        }
    }

    internal void StartWindowMiniGame(Vector3 position, Queue tempQueue)
    {
        IsBusy = true;
        transform.position = new Vector3(position.x, transform.position.y, transform.position.z);
        progressBar.SetActive(true);
        promtText.gameObject.SetActive(true);
        IsMashing = true;
        InvokeRepeating("DecreaseValue", 0.7f, 0.7f);
        queue = tempQueue;
    }
    private void StopWindowMiniGame()
    {
        IsBusy = false;
        progressBar.SetActive(false);
        promtText.gameObject.SetActive(false);
        IsMashing = false;
        CancelInvoke();
        slider.value = 40;
    }

    private void DecreaseValue()
    {
        if (slider.value != 0)
        {
            slider.value -= 6;
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
            if (transform.position.x < -6)
            {
                queue = GameObject.Find("Queue1").GetComponent<Queue>();
            }
            else if (transform.position.x > 6)
            {
                queue = GameObject.Find("Queue2").GetComponent<Queue>();
            }
        }
        if (IsMashing)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                slider.value += 5;
            }
            if (slider.value == slider.maxValue)
            {
                StopWindowMiniGame();
                queue.RemovePassengerInWindow();
            }
        }
    }

    public void AcceptPassenger()
    {
        queue.AcceptPassenger();
    }

    public void RejectPassaenger()
    {
        queue.RejectPassenger();
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
