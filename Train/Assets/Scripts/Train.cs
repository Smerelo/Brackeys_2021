using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Train : MonoBehaviour
{
    private List<Passenger> passengers;
    private List<Passenger> illegalPassengers;
    public Transform[] ejectPos;
    public List<Sit> seats;
    public List<Animator> lives;
    public Animator doorsAnimator;
    public Animator doorsAnimator2;
    public Animator CapacityBar;
    public TextMeshProUGUI ClockText;
    public Queue queue1;
    public Queue queue2;
    public GameObject ui;
    public GameObject engineLight;

    [HideInInspector]
    public bool IsTutorialActive {get; set;}
    public bool Moving { get; private set; }
    public bool IsMoving { get; private set; }
    public bool InEndScreen { get; private set; }

    public EndScreen EndScreen;
    [HideInInspector]
    public float time;
    private bool inMenu = true;
    public float moveSpeed;
    private CameraManager cameraManager;
    private bool doorsClosed;
    private Animator a;
    private BackgroundManager bgManager;
    private ConductorController player;
    private bool isEntering;
    private int loopCount;

    void Start()
    {
        bgManager = GameObject.Find("BackGroundManager").GetComponent<BackgroundManager>();
        player = GameObject.Find("Conductor").GetComponent<ConductorController>();
        cameraManager = GameObject.Find("CameraManager").GetComponent<CameraManager>();
        time = 20;
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
        Debug.Log(time);
        if (!inMenu && !IsMoving && !InEndScreen)
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
        if (IsMoving && !InEndScreen)
        {
            if (time > 0)
            {
                time -= Time.deltaTime;
                ClockText.text = FloatToTime();
            }
            else if (!isEntering && !InEndScreen)
            {
                Debug.Log("here1");
                isEntering = true;
                StartEnteringFaze();
            }
        }
    }

    private void StartEnteringFaze()
    {
        player.ResetTicketCounter();
        bgManager.SpawnPlatform();
      
    }

    private void RemovePassengers()
    {
        foreach (Passenger p in passengers)
        {
            if (p != null)
            {
                p.gameObject.transform.parent = null;
                p.DeleteNow();
            }
        }
        passengers.Clear();
        foreach (Passenger p in illegalPassengers)
        {
            if (p != null)
            {
                p.gameObject.transform.parent = null;
                p.DeleteNow();
            }
        }
        illegalPassengers.Clear();
    }

    private void EndGame()
    {
        InEndScreen = true;
        cameraManager.ZoomOut();
        ui.SetActive(false);
        EndScreen.gameObject.SetActive(true);
        EndScreen.ChooseEnding(player.GetScore(), player.GetScore2());
    }

    internal Transform GetEjectPos(Vector3 position)
    {
        float d = 1000;
        int t = 0;
        for (int i = 0; i < ejectPos.Length; i++)
        {
            if (d > Vector2.Distance(position, ejectPos[i].localPosition))
            {
                t = i;
                d = Vector2.Distance(position, ejectPos[i].localPosition);
            }
        }
        a = ejectPos[t].gameObject.GetComponent<Animator>();
        ChangeAnimationState("open",a);
        StartCoroutine( PlayNextAnimation(a, "EjectStayOpen"));
        return ejectPos[t];
    }

    private void EjectStayOpen()
    {
        a.Play("stayOpen");
        Invoke("CloseHatch", 0.5f);
    }

    internal void RemoveIllegalPassenger(Passenger passenger)
    {
        illegalPassengers.Remove(passenger);
        int c = passengers.Count + illegalPassengers.Count;
        if (c % 5 == 0)
        {
            CapacityBar.Play((c / 5).ToString());
        }
    }

    private void CloseHatch()
    {
        ChangeAnimationState("close", a);
        StartCoroutine(PlayNextAnimation(a, "HatchToIdle"));
    }
    private void HatchToIdle()
    {
        player.PassengerEjected();
        ChangeAnimationState("idle", a);
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
            isEntering = false;
            engineLight.SetActive(false);
            ChangeAnimationState("open", doorsAnimator);
            StartCoroutine(PlayNextAnimation(doorsAnimator, "StayOpen"));
            ChangeAnimationState("open", doorsAnimator2);
            StartCoroutine(PlayNextAnimation(doorsAnimator2, "StayOpen"));
            cameraManager.ZoomOut();
            Invoke("StartGame", 5f);
            AudioManager.AudioInstance.Play("DoorClose");
            IsMoving = false;
        }
        else
        {
            loopCount++;
            if (loopCount == 4)
            {
                Invoke("EndGame", 1);
            }
            engineLight.SetActive(true);
            cameraManager.SwitchToZoom();
            foreach (Passenger p in illegalPassengers)
            {
                p.ShowArrow();
            }
            AudioManager.AudioInstance.Stop("AngryCrowd");
            AudioManager.AudioInstance.Play("Engine");
            AudioManager.AudioInstance.Play("Theme");
            AudioManager.AudioInstance.Stop("MainTheme");
            time = 30;
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
    }

    internal void Break()
    {
        Debug.Log("here3");
        StartCoroutine(LerpFunction(0, 12));
        AudioManager.AudioInstance.Stop("SpaceTravel");
        AudioManager.AudioInstance.Play("MainTheme");
        AudioManager.AudioInstance.Stop("Engine");
        AudioManager.AudioInstance.Play("Braking");
    }

    private void CloseDoors()
    {
        queue1.StopQueue();
        queue2.StopQueue();
        bgManager.Tunnel = true;
        bgManager.StopAddingPlatforms();
        ChangeAnimationState("close", doorsAnimator);
        StartCoroutine(PlayNextAnimation(doorsAnimator, "StayClosed"));
        ChangeAnimationState("close", doorsAnimator2);
        StartCoroutine(PlayNextAnimation(doorsAnimator2, "StayClosed"));
        StartCoroutine(LerpFunction(20, 8));
        AudioManager.AudioInstance.Play("EngineStart");
        player.HideTicketUI();
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
