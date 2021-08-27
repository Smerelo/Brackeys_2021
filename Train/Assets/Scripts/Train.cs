using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Train : MonoBehaviour
{
    private List<Passenger> passengers;
    private List<Passenger> illegalPassengers;
    public List<Sit> seats;
    public List<Animator> lives;
    public Animator CapacityBar;
    public TextMeshProUGUI ClockText;
    public Queue queue1;
    public Queue queue2;
    public GameObject ui;

    [HideInInspector]
    public bool IsTutorialActive {get; set;}
    public bool Moving { get; private set; }

    private float time = 180;
    private bool inMenu = true;
    public float moveSpeed;
    private Animator animator;
    void Start()
    {
        Moving = true;
        animator = GetComponent<Animator>();
        time = 60;
        AudioManager.AudioInstance.Play("Engine");
        passengers = new List<Passenger>();
        illegalPassengers = new List<Passenger>();
    }

    private void FixedUpdate()
    {
        float horizontalMovement = moveSpeed * Time.deltaTime;
        //transform.position += new Vector3(horizontalMovement, 0, 0);

    }

    void Update()
    {
        if (!inMenu)
        {

            if (time > 0)
            {
                time -= Time.deltaTime;
                ClockText.text = FloatToTime();
            }
            else
            {
                CloseDoors();
            }
        }
        if (Moving)
        {
            animator.Play("moving");
        }
        else
        {
            animator.Play("Idle");
        }
        
    }

    private void CloseDoors()
    {
        queue1.StopQueue();
        queue2.StopQueue();
    }

    private string FloatToTime()
    {
        TimeSpan t = TimeSpan.FromSeconds(time);
        return t.ToString("mm':'ss");
    }

    internal void AddPassenger(Passenger tempPassenger, int v)
    {
        if (v == 1)
        {
            passengers.Add(tempPassenger);
        }
        else
        {
            illegalPassengers.Add(tempPassenger);
        }
        int c = passengers.Count + illegalPassengers.Count;
        if (c % 5 == 0)
        {
            CapacityBar.Play( (c/5).ToString());
        }
    }

    internal Vector3 GetSeat(Vector3 pos, Passenger passenger)
    {

        foreach (Sit seat in seats)
        {
            if (seat.UsedSeats == 0)
            {
                seat.AddPassenger(passenger);
                return seat.gameObject.transform.position;
            }
            else if (seat.UsedSeats == 1)
            {
                seat.AddPassenger(passenger);
                return new Vector3(seat.gameObject.transform.position.x + 0.5f, seat.gameObject.transform.position.y, pos.z);
            }

        }
        return pos;
    }

    internal void AddIllegalPassenger(Passenger passenger)
    {
        illegalPassengers.Add(passenger);
    }
}
