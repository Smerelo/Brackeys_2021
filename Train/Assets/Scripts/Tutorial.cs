using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour
{
    public GameObject dialogue;
    private ConductorController player;
    public TextMeshProUGUI timer;
    private float time = 1;
    private bool startTimer;
    private bool isEntering;
    private BackgroundManager bgManager;
    private bool tutorial;

    void Start()
    {
        tutorial = true;
        bgManager = GameObject.Find("BackGroundManager").GetComponent<BackgroundManager>();
        player = GameObject.Find("Conductor").GetComponent<ConductorController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (startTimer && tutorial)
        {
            if (time > 0)
            {
                time -= Time.deltaTime;
                timer.text = FloatToTime();
            }
            else if (!isEntering)
            {
                isEntering = true;
                StartEnteringFaze();
            }
        }
    }

    private void StartEnteringFaze()
    {
        tutorial = false;
        bgManager.SpawnPlatform();
    }

    private string FloatToTime()
    {
        TimeSpan t = TimeSpan.FromSeconds(time);
        return t.ToString("mm':'ss");
    }

    internal void Monologue()
    {
        dialogue.SetActive(true);
    }

    internal void StartTutorial()
    {
        player.RemoveConstraints();
        startTimer = true;
    }
}
