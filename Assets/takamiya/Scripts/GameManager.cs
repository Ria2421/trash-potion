using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    enum MODE
    { 
        Player1,
        Player2,
        Player3,
        Player4
    }

    [SerializeField] GameObject PlayerText;

    int nowPlayer;

    // Start is called before the first frame update
    void Start()
    {
        PlayerText = GameObject.Find("PlayerText");
    }

    // Update is called once per frame
    void Update()
    {
        //while(nowPlayer ==true)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                nowPlayer++;

                string Times = nowPlayer.ToString();
                PlayerText.GetComponent<Text>().text = Times + "P‚Ì”Ô‚Å‚·";
            }
        }
       
    }

    void TurnCalc()
    {

    }
    
    public void trunChenge()
    {
       
        
    }
}
