using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class EndScreen : MonoBehaviour
{
    public string[] endings;
    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   

    internal void ChooseEnding(int workerScore, int compasionScore)
    {
        if (compasionScore > workerScore)
        {
            text.text = endings[0];
        }
        if (compasionScore < workerScore)
        {
            text.text = endings[1];
        } 
        if (compasionScore == workerScore)
        {
            text.text = endings[2];
        }
    }
}
