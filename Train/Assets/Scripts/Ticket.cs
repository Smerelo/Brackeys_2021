using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Ticket : MonoBehaviour
{
    private Animator animator;
    private GameObject parent;
    private bool isfake;
    private ConductorController conductor;
    private Train train;
    public GameObject stamp;
    void Start()
    {
        parent = transform.parent.gameObject;
        conductor = GameObject.Find("Conductor").GetComponent<ConductorController>();
        train = GameObject.Find("Train").GetComponent<Train>();
        animator = GetComponent<Animator>();
    }

    private void ChooseSprite()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        if (!isfake)
        {
            animator.Play("0");
        }
        else
        {
            int r = UnityEngine.Random.Range(1, 6);
            if(animator == null)
                animator = GetComponent<Animator>();
            animator.Play(r.ToString());
        }
    }

    public void RandomizeTicket()
    {
        int r = UnityEngine.Random.Range(0, 5);
        if (r == 4)
        {
            isfake = true;
        }
        else
        {
            isfake = false;
        }
        ChooseSprite();
    }

    public void CheckTicket()
    {
        AudioManager.AudioInstance.Play("Stamp");
        Invoke("AcceptPassenger", 0.5f);
    }

    private void AcceptPassenger()
    {
        stamp.SetActive(false);
        conductor.AcceptPassenger(isfake);
        parent.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
