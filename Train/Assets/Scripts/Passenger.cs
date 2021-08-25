using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passenger : MonoBehaviour
{
    [HideInInspector]
    public Vector3 queuePos{ get; set; }
    public bool MovementFinished { get; private set; }
    public bool HasToMoveToWindow { get; private set; }
    public GameObject windowTemp { get; private set; }
    public bool IsSeated { get; private set; }

    [HideInInspector]
    public Vector3 watingPos; 
    public int positionInLine;
    private int tweenId;
    private ConductorController player;
    public float maxRange;
    public float minRange;
    private float r;
    private Train train;
    private float speed = 5;

    // Start is called before the first frame update
    void Start()
    {
        train = GameObject.Find("Train").GetComponent<Train>();
        player = GameObject.Find("Conductor").GetComponent<ConductorController>();
         r =  UnityEngine.Random.Range(minRange, maxRange);
        Vector3 v = new Vector3(queuePos.x + r, queuePos.y - .5f - .4f * positionInLine);
        Vector3 v2 = new Vector3(watingPos.x + positionInLine - 5, watingPos.y - .5f + r);
        if (positionInLine< 5)
        {
           tweenId = LeanTween.moveLocal(gameObject,v, Vector2.Distance(transform.position, v) / speed).setOnComplete(setMovementCompleted).id;
            MovementFinished = false;
        }
        else
        {
            tweenId = LeanTween.move(gameObject, v2, Vector2.Distance(transform.position, v2) / speed).setOnComplete(setMovementCompleted).id;
            MovementFinished = false;
        }
    }

    public void Move()
    {
        r = UnityEngine.Random.Range(minRange, maxRange);
        Vector3 v = new Vector3(queuePos.x + r, queuePos.y - .5f - .4f * positionInLine);
        Vector3 v2 = new Vector3(watingPos.x + positionInLine - 5, watingPos.y - .5f + r);
        if (positionInLine < 5)
        {
            tweenId = LeanTween.moveLocal(gameObject, v, 1.5f).setOnComplete(setMovementCompleted).id;
            MovementFinished = false;
        }
        else
        {
            tweenId = LeanTween.move(gameObject, v2, 1.5f).setOnComplete(setMovementCompleted).id;
            MovementFinished = false;
        }
    }

    internal bool CheckIfReady()
    {
        return MovementFinished;
    }

    private void setMovementCompleted()
    {
        MovementFinished = true;
        LeanTween.cancel(tweenId);
        if (player.IsInsideDoor && !player.canCheckTicket)
        {
            player.EnableDoorInteraction();
        }
    }

    internal void GoToWindow(GameObject window)
    {
        windowTemp = window;
        Vector3 v = new Vector3(windowTemp.transform.position.x, windowTemp.transform.position.y - 1.3f);
        if (MovementFinished)
        {
            tweenId = LeanTween.move(gameObject, v, Vector2.Distance(transform.position, v) / speed).setOnComplete(ArrivedToWindow).id;
            HasToMoveToWindow = false;
            MovementFinished = false;
        }
        else
        {
            HasToMoveToWindow = true;
        }
    }

    private void ArrivedToWindow()
    {
        LeanTween.cancel(tweenId);

    }
    private void ArrivedToFinalDest()
    {
        LeanTween.cancel(tweenId);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void GoOffScreen(Vector3 position)
    {
        tweenId = LeanTween.move(gameObject, position, Vector2.Distance(transform.position, position) / speed).setOnComplete(ArrivedToFinalDest).id;
    }

    internal void MoveInsideTheTrain(Vector3 position)
    {
        tweenId = LeanTween.move(gameObject, position, Vector2.Distance(transform.position, position) / speed).setOnComplete(FindSeat).id;
    }

    private void FindSeat()
    {
        LeanTween.cancel(tweenId);
        Vector3 sitPos = train.GetSeat(transform.position, this);
        tweenId = LeanTween.move(gameObject, sitPos, Vector2.Distance(transform.position, sitPos)/ speed).setOnComplete(ArrivedToSeat).id;
    }

    private void ArrivedToSeat()
    {
        IsSeated = true;
        LeanTween.cancel(tweenId);
    }
}
