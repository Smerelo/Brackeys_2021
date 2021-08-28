using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI text;
    public string[] sentences;
    public float typingSpeed;
    private int index;
    public GameObject continueButton;
    public UnityEvent method;

    public bool IsTyping { get; private set; }

    private void Start()
    {
        StartCoroutine(Type());
    }

    private void Update()
    {
     
        if (text.text == sentences[index])
        {
            continueButton.SetActive(true);
        }
        if (IsTyping)
        {
            continueButton.SetActive(false);
        }
    }

    IEnumerator Type()
    {
        foreach (char letter in sentences[index].ToCharArray())
        {
            IsTyping = true;
            text.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        IsTyping = false;
    }

    public void NextSentence()
    {
        continueButton.SetActive(false);
        if (index < sentences.Length - 1)
        {
            index++;
            text.text = "";
            StartCoroutine(Type()); 
        }
        else
        {
            text.text = "";
            method.Invoke();
        }
    }
}
