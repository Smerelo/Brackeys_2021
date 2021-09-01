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
    public Ticket ticket;
    public GameObject letter;
    public SpriteRenderer specialObject;
    public TextMeshProUGUI letterText   ;

    public bool canCheckTicket { get; set; }
    public bool IsInsideDoor { get; set; }
    private bool IsBusy { get; set; }
    public bool IsMashing { get; private set; }
    public bool IsMoving { get; private set; }
    public bool IsInMenu { get; private set; }
    public bool CanCheckSeats { get; internal set; }

    private Queue queue;
    private Animator animator;
    private Slider slider;
    private SpriteRenderer SR;
    private string doorNb;
    private int fakeCount;
    private Sit currentSeatChecked;
    private int ticketCount;
    private UniqueStory story;
    private int workerPoints;
    private int compasionatePoints;

    void Start()
    {
        IsInMenu = true;
        SR = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        slider = progressBar.GetComponent<Slider>();
    }

    internal void EnableSeatCheck(Sit seat)
    {
        CanCheckSeats = true;
        promtText.gameObject.SetActive(true);
        currentSeatChecked = seat;
    }

    internal void DisableSeatCheck()
    {
        CanCheckSeats = false;
        promtText.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (!IsBusy && !IsInMenu)
        {
            Move();
        }
    }
    internal void RemoveConstraints()
    {
        IsInMenu = false;
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

    internal void ResetTicketCounter()
    {
        ticketCount = 0;
    }

    private void Move()
    {
        float horizontalMovement = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
        if (horizontalMovement != 0)
        {
            if (IsMoving == false)
            {
                AudioManager.AudioInstance.Play("FootSteps");
            }
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
        else
        {
            IsMoving = false;
            AudioManager.AudioInstance.Stop("FootSteps");

        }
        transform.position += new Vector3(horizontalMovement, 0, 0);
    }

    internal int GetScore2()
    {
        return compasionatePoints;
    }

    internal int GetScore()
    {
        return workerPoints;
    }

    void Update()
    {
        CheckAnimation();
        if (CanCheckSeats && Input.GetKeyDown(KeyCode.Space))
        {
            currentSeatChecked.ExpelPassengers();
        }
        if (Input.GetKeyDown(KeyCode.Space) && canCheckTicket) 
        {
            float i = 0;
            if (doorNb == "1")
            {
                queue = GameObject.Find("Queue1").GetComponent<Queue>();
                 i = 1.35f;
            }
            else if (doorNb == "2")
            {
                queue = GameObject.Find("Queue2").GetComponent<Queue>();
                 i = -1.3f;

            }
            if (queue != null)
            {
                promtText.gameObject.SetActive(false);
                IsBusy = true;
                canCheckTicket = false;
                ticketCount++;
                if (ticketCount == 5 || ticketCount == 10)
                {
                    story = ticket.UnqiueStory();
                    queue.ShowPassengerPrompt(story);
                }
                else
                {
                    mask.SetActive(true);
                    ticket.gameObject.SetActive(true);
                    ticket.RandomizeTicket();
                }
                Vector3 t = queue.transform.position;
                transform.position = new Vector3(t.x + i, transform.position.y, transform.position.z);
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

    internal void PassengerEjected()
    {
        CanCheckSeats = false;
        IsBusy = false;
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
        else if (!IsMoving || IsBusy && !IsMashing)
        {
            animator.Play("Idle");
        }
    }

    public void AcceptPassenger( bool fake)
    {
        queue.AcceptPassenger(fake);
        if (fake)
        {
            fakeCount++;
        }
        else
        {
            fakeCount = 0;
        }
        if (story != null)
        {
            workerPoints += story.Ywp;
            compasionatePoints += story.Ycp;
            story = null;
            mask.SetActive(false);
            ticket.gameObject.SetActive(true);
            specialObject.gameObject.SetActive(false);
            letter.SetActive(false);
            letterText.gameObject.SetActive(false);
        }
        IsBusy = false;
        canCheckTicket = true;
        promtText.gameObject.SetActive(true);
    }

    internal void OpenSpecialUI()
    {
        mask.SetActive(true);
        ticket.gameObject.SetActive(false);
        specialObject.gameObject.SetActive(true);
        letter.SetActive(true);
        letterText.gameObject.SetActive(true);
        specialObject.sprite = story.image;
        letterText.text = story.text;
    }

    public void RejectPassaenger()
    {
        queue.RejectPassenger();
        IsBusy = false;
        canCheckTicket = true;
        promtText.gameObject.SetActive(true);
        if (story != null)
        {
            workerPoints += story.Nwp;
            compasionatePoints += story.Ncp;
            story = null;
            mask.SetActive(false);
            ticket.gameObject.SetActive(true);
            specialObject.gameObject.SetActive(false);
            letter.SetActive(false);
            letterText.gameObject.SetActive(false);
        }
    }

    public void HideTicketUI()
    {
        mask.SetActive(false);
        IsBusy = false;
    }

    internal void EnableDoorInteraction(string door)
    {
        if (door != "0")
        {
            doorNb = door;
        }
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
