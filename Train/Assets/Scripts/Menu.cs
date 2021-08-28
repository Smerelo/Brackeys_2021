using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public Tutorial tutorial;
    public List<GameObject> buttons;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HideButtons()
    {
        foreach (GameObject b in buttons)
        {
            LeanTween.move(b, new Vector3(b.transform.position.x - 0.001f, b.transform.position.y, -10), 1.5f).setOnComplete(StartTutorial);
        }
    }

    private void StartTutorial()
    {
        tutorial.Monologue();
        gameObject.SetActive(false);
    }
}