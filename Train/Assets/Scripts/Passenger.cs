using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Passenger : MonoBehaviour
{
    [HideInInspector]
    public Vector3 queuePos{ get; set; }
    public bool MovementFinished;
    public bool HasToMoveToWindow { get; private set; }
    public GameObject windowTemp { get; private set; }
    public bool IsSeated { get; private set; }
    public bool IsTryingToGetIn { get; private set; }

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
    private float windowTimer = 0;
    private float maxTimer = 12;
    private Queue queue;
    private GameObject arrow;
    public bool isIllegal = false;
    private GameObject prompt;
    private Animator pAnimator;
    private TextMeshProUGUI text;
    private string promptText;

    void Update()
    {
        if (IsTryingToGetIn)
        {
            windowTimer += Time.deltaTime;
            if (windowTimer >= maxTimer)
            {
                GetIn();
                isIllegal = true;
            }
        }
    }
    void Start()
    {
        arrow = transform.GetChild(0).gameObject;
        prompt = transform.GetChild(2).gameObject;
        text = transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
        pAnimator = prompt.GetComponent<Animator>();
        queue = transform.parent.gameObject.GetComponent<Queue>();
        train = GameObject.Find("Train").GetComponent<Train>();
        player = GameObject.Find("Conductor").GetComponent<ConductorController>();
        r =  UnityEngine.Random.Range(minRange, maxRange);
        Vector3 v = new Vector3(queuePos.x + r, queuePos.y - .5f - .4f * positionInLine);
        Vector3 v2 = new Vector3(watingPos.x + positionInLine - 5, watingPos.y - .5f + r);
        if (positionInLine< 5)
        {
           tweenId = LeanTween.moveLocal(gameObject,v, 2f).setOnComplete(setMovementCompleted).id;
            MovementFinished = false;
        }
        else
        {
            tweenId = LeanTween.move(gameObject, v2, 2f).setOnComplete(setMovementCompleted).id;
            MovementFinished = false;
        }
    }

    internal void Expel()
    {
        if (arrow.activeSelf)
        {
            tweenId = LeanTween.moveLocal(gameObject, train.GetEjectPos(transform.localPosition).localPosition, 1f).setOnComplete(RemoveFromList).id;
            Destroy(gameObject, 10);
        }
    }

    private void RemoveFromList()
    {
        LeanTween.cancel(tweenId);
        train.RemoveIllegalPassenger(this);
        transform.parent = null;
    }

    internal void Delete()
    {
        Destroy(gameObject, 10);
    }

    internal void DeleteNow()
    {
        Destroy(gameObject, 0.2f);
    }

    public void Move()
    {
        r = UnityEngine.Random.Range(minRange, maxRange);
        Vector3 v = new Vector3(queuePos.x + r, queuePos.y - .5f - .4f * positionInLine);
        Vector3 v2 = new Vector3(watingPos.x + positionInLine - 5, watingPos.y - .5f + r);
        if (positionInLine < 5)
        {
            tweenId = LeanTween.moveLocal(gameObject, v, .5f).setOnComplete(setMovementCompleted).id;
            MovementFinished = false;
        }
        else
        {
            tweenId = LeanTween.move(gameObject, v2, .5f).setOnComplete(setMovementCompleted).id;
            MovementFinished = false;
        }
    }

    internal bool CheckIfReady()
    {
        return  MovementFinished;
    }

    private void setMovementCompleted()
    {
        MovementFinished = true;
        LeanTween.cancel(tweenId);
        if (player.IsInsideDoor && !player.canCheckTicket)
        {
            player.EnableDoorInteraction("0");
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

    internal void ShowArrow()
    {
        arrow.SetActive(true);
    }

    private void ArrivedToWindow()
    {
        AudioManager.AudioInstance.Play("Scrap");
        LeanTween.cancel(tweenId);
        IsTryingToGetIn = true;
        MovementFinished = true;
    }

    internal void ShowPrompt(string dialog)
    {
        text.text = dialog;
        prompt.gameObject.SetActive(true);
        ChangeAnimationState("grow", pAnimator);
        StartCoroutine( PlayNextAnimation(pAnimator, "PlayOpen"));
    }

    private void PlayOpen()
    {
        text.gameObject.SetActive(true);
        ChangeAnimationState("open", pAnimator);
        Invoke("ClosePrompt", 1f);
    }

    private void ClosePrompt()
    {
        prompt.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
        player.OpenSpecialUI();
    }

    private void ArrivedToFinalDest()
    {
        LeanTween.cancel(tweenId);
        Destroy(gameObject);
    }

    // Update is called once per frame
    
        

    private void GetIn()
    {
        IsTryingToGetIn = false;
        if (queue == null)
        {
            queue = transform.root.gameObject.GetComponent<Queue>();
        }
        queue.PassengerInWindowGotIn();
        Vector3 v = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        tweenId = LeanTween.move(gameObject, v, Vector2.Distance(transform.position, v) / speed).setOnComplete(FindSeat).id;
        MovementFinished = false;
    }

    internal void GoOffScreen(Vector3 position)
    {
        AudioManager.AudioInstance.Stop("Scrap");
        AudioManager.AudioInstance.Play("PassengerFall");
        tweenId = LeanTween.move(gameObject, position, Vector2.Distance(transform.position, position) / speed).setOnComplete(ArrivedToFinalDest).id;
        MovementFinished = false;
    }

    internal void MoveInsideTheTrain(Vector3 position)
    {
        tweenId = LeanTween.move(gameObject, position, Vector2.Distance(transform.position, position) / speed).setOnComplete(FindSeat).id;
        MovementFinished = false;
    }

    private void FindSeat()
    {
        gameObject.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("Char");
        AudioManager.AudioInstance.Play("Sigh");
        LeanTween.cancel(tweenId);
        Vector3 sitPos = train.GetSeat(transform.position, this);
        tweenId = LeanTween.move(gameObject, sitPos, Vector2.Distance(transform.position, sitPos)/ speed).setOnComplete(ArrivedToSeat).id;
    }

    private void ArrivedToSeat()
    {
        IsSeated = true;
        MovementFinished = true;
        LeanTween.cancel(tweenId);
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
