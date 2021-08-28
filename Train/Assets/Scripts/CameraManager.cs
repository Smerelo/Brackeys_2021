using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraManager : MonoBehaviour
{
    public List<CinemachineVirtualCamera> cameras;
    public Tutorial tutorial;
    public GameObject UI;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    // Vector3 v = new Vector3(-5.1f, 4.7f, 7.7f);

    public void SwitchToZoom()
    {
        Vector3 v = new Vector3(-0.9f, 4.7f, 7.7f);
        cameras[0].gameObject.SetActive(false);
        cameras[1].gameObject.SetActive(true);
        LeanTween.moveLocal(UI, v, .2f);
        Invoke("StartTutorial", 1.3f);
    }

    private void StartTutorial()
    {
        UI.SetActive(true);
        tutorial.StartTutorial();
    }

    internal void ZoomOut()
    {
        Vector3 v = new Vector3(-8.1f, 4.7f, 7.7f);
        cameras[0].gameObject.SetActive(true);
        cameras[1].gameObject.SetActive(false);
        cameras[1].gameObject.SetActive(false);
        LeanTween.moveLocal(UI, v, .2f);

    }
}

