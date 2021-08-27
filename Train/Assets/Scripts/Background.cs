using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    // Start is called before the first frame update
    private Train train;
    private BackgroundManager bgManager;
    void Start()
    {
        train = GameObject.Find("Train").GetComponent<Train>();
        bgManager = GameObject.Find("BackGroundManager").GetComponent<BackgroundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
       
    }
   
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            bgManager.AddBackground();
        }
    }
}
