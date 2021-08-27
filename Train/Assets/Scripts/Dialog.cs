using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI text;
    public string[] sentences;
    public float typingSpeed;
    private int index;
    public GameObject continueButton;
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
    }

    IEnumerator Type()
    {
        foreach (char letter in sentences[index].ToCharArray())
        {
            text.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
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
        }
    }
}
