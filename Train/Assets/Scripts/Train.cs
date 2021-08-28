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
    public Animator doorsAnimator;
    public Animator doorsAnimator2;
    public Animator CapacityBar;
    public TextMeshProUGUI ClockText;
    public Queue queue1;
    public Queue queue2;
    public GameObject ui;

    [HideInInspector]
    public bool IsTutorialActive {get; set;}
    public bool Moving { get; private set; }
    public bool IsMoving { get; private set; }

    private float time;
    private bool inMenu = true;
    public float moveSpeed;
    private float startMoveSpeed;
    private Animator animator;
    private CameraManager cameraManager;
    private string currentState;
    private bool doorsClosed;
    private BackgroundManager bgManager;
    void Start()
    {
        bgManager = GameObject.Find("BackGroundManager").GetComponent<BackgroundManager>();
        cameraManager = GameObject.Find("CameraManager").GetComponent<CameraManager>();
        startMoveSpeed = moveSpeed;
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
        transform.position += new Vector3(horizontalMovement, 0, 0);

    }

    void Update()
    {
        if (!inMenu && !IsMoving)
        {
            if (time > 0)
            {
                time -= Time.deltaTime;
                ClockText.text = FloatToTime();
            }
            else if (!doorsClosed)
            {
                doorsClosed = true;
                CloseDoors();
            }
        }
        if (IsMoving)
        {
            if (time > 0)
            {
                time -= Time.deltaTime;
                ClockText.text = FloatToTime();
            }
        }
    }


 

    IEnumerator LerpFunction(float endValue, float duration)
    {
        float time = 0;
        float startValue = moveSpeed;

        while (time < duration)
        {
            moveSpeed = Mathf.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        ChangeZoom();
        moveSpeed = endValue;
    }

    private void ChangeZoom()
    {
        
        if (moveSpeed < 1)
        {
            ChangeAnimationState("open", doorsAnimator);
            StartCoroutine(PlayNextAnimation(doorsAnimator, "StayOpen"));
            ChangeAnimationState("open", doorsAnimator2);
            StartCoroutine(PlayNextAnimation(doorsAnimator2, "StayOpen"));
            cameraManager.ZoomOut();
            Invoke("StartGame", 5f);
            AudioManager.AudioInstance.Play("DoorClose");
        }
        else
        {
            cameraManager.SwitchToZoom();
            bgManager.StopAddingPlatforms();
            AudioManager.AudioInstance.Play("Engine");
            AudioManager.AudioInstance.Play("Theme");
            AudioManager.AudioInstance.Stop("MainTheme");
            time = 60;
            IsMoving = true;
        }

       
    }

    private void StayOpen()
    {
        ChangeAnimationState("stayOpen", doorsAnimator);
        ChangeAnimationState("stayOpen", doorsAnimator2);
    }
     private void StayClosed()
    {
        ChangeAnimationState("stayClosed", doorsAnimator);
        ChangeAnimationState("stayClosed", doorsAnimator2);
    }

    private void StartGame()
    {
        time = 180;
        inMenu = false;
        doorsClosed = false;
        queue1.StartQueue();
        queue2.StartQueue();
        AudioManager.AudioInstance.Stop("Theme");
        AudioManager.AudioInstance.Play("MainTheme");
    }

    internal void Break()
    {
        StartCoroutine(LerpFunction(0, 10));
        AudioManager.AudioInstance.Stop("Engine");
        AudioManager.AudioInstance.Play("Braking");
    }

    private void CloseDoors()
    {
        queue1.StopQueue();
        queue2.StopQueue();
        ChangeAnimationState("close", doorsAnimator);
        StartCoroutine(PlayNextAnimation(doorsAnimator, "StayClosed"));
        ChangeAnimationState("close", doorsAnimator2);
        StartCoroutine(PlayNextAnimation(doorsAnimator2, "StayClosed"));
        StartCoroutine(LerpFunction(20, 10));
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
        Debug.Log($"illegal: {illegalPassengers.Count} legal: {passengers.Count}");
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

    IEnumerator PlayNextAnimation(Animator anim, string method)
    {
        yield return new WaitForEndOfFrame();
        Invoke(method, anim.GetCurrentAnimatorStateInfo(0).length);
    }

    private void ChangeAnimationState(string newState, Animator currentAnimator)
    {
      
        currentAnimator.Play(newState);
    }

}
