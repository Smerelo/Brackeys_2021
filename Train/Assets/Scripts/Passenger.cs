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

    [HideInInspector]
    public Vector3 watingPos; 
    public int positionInLine;
    private int tweenId;
    private ConductorController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Conductor").GetComponent<ConductorController>();
        if (positionInLine< 5)
        {
           tweenId = LeanTween.moveLocal(gameObject, new Vector3(queuePos.x, queuePos.y -.5f - .4f * positionInLine), 2).setOnComplete(setMovementCompleted).id;
            MovementFinished = false;
        }
        else
        {
            float rand = UnityEngine.Random.Range(-3, 4);
            tweenId = LeanTween.move(gameObject, new Vector3(watingPos.x + positionInLine - 5 , watingPos.y - .5f), 2).setOnComplete(setMovementCompleted).id;
            MovementFinished = false;
        }
    }

    public void Move()
    {
        if (positionInLine < 5)
        {
            tweenId = LeanTween.moveLocal(gameObject, new Vector3(queuePos.x, queuePos.y - .5f - .4f * positionInLine), 1.5f).setOnComplete(setMovementCompleted).id;
            MovementFinished = false;
        }
        else
        {
            float rand = UnityEngine.Random.Range(-3, 4);
            tweenId = LeanTween.move(gameObject, new Vector3(watingPos.x + positionInLine - 5, watingPos.y - .5f), 1.5f).setOnComplete(setMovementCompleted).id;
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
        if (MovementFinished)
        {
            tweenId = LeanTween.move(gameObject, new Vector3(windowTemp.transform.position.x, windowTemp.transform.position.y - 1.3f), 2).setOnComplete(ArrivedToWindow).id;
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
