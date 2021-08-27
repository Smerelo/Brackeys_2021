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
    public bool IsMoving { get; private set; }

    private Queue queue;
    private Animator animator;
    private Slider slider;
    private SpriteRenderer SR;
    void Start()
    {
        SR = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
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
        float horizontalMovement = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
        IsMoving = false;
        if (horizontalMovement != 0)
        {
            IsMoving = true;

            if ( horizontalMovement < 0)
            {
                SR.flipX = true;
            }
            else
            {
                SR.flipX = false;
            }

        }
        transform.position += new Vector3(horizontalMovement, 0, 0);
    }

    void Update()
    {
        CheckAnimation();
        if (Input.GetKeyDown(KeyCode.Space) && canCheckTicket) 
        {
            float i = 0;
            mask.SetActive(true);
            IsBusy = true;
            canCheckTicket = false;
            promtText.gameObject.SetActive(false);
            if (transform.position.x < -1)
            {
                queue = GameObject.Find("Queue1").GetComponent<Queue>();
                 i = 1.35f;
            }
            else if (transform.position.x > 1)
            {
                queue = GameObject.Find("Queue2").GetComponent<Queue>();
                 i = -1.3f;

            }
            Vector3 t = queue.transform.position;
            transform.position = new Vector3(t.x + i, transform.position.y, transform.position.z);
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

    private void CheckAnimation()
    {
        if (IsMoving && !IsBusy && !IsMashing)
        {
            animator.Play("walk");
        }
        else if (IsMashing)
        {

            animator.Play("hit");
        }
        else if (!IsMoving)
        {
            animator.Play("Idle");
        }
    }

    public void AcceptPassenger()
    {
        queue.AcceptPassenger();
        AudioManager.AudioInstance.Play("Stamp");
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
        if (!IsBusy)
        {
            canCheckTicket = true;
            promtText.gameObject.SetActive(true);
        }
    }

    internal void DisableDoorInteraction()
    {
        canCheckTicket = false;
        promtText.gameObject.SetActive(false); 
    }

    internal void StopAction()
    {
        if (IsMashing)
        {
            IsMashing = false;
            IsBusy = false;
            promtText.gameObject.SetActive(false);
            progressBar.SetActive(false);
        }
    }
}
