using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passenger : MonoBehaviour
{
    [HideInInspector]
    public Vector3 queuePos{ get; set; }
    public bool MovementFinished { get; private set; }

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
            Debug.Log(queuePos);

           tweenId = LeanTween.moveLocal(gameObject, new Vector3(queuePos.x, queuePos.y - .4f * positionInLine), 2).setOnComplete(setMovementCompleted).id;
        }
        else
        {
            float rand = UnityEngine.Random.Range(-3, 4);
            tweenId = LeanTween.move(gameObject, new Vector3(watingPos.x + positionInLine - 5 , watingPos.y), 2).id;

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
    // Update is called once per frame
    void Update()
    {
        
    }
}
