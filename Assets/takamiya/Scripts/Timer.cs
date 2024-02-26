//
//カウントダウンスクリプト
//Author：高宮祐翔
//Date:2/26
//
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] Text TimerText;

    private float time = 15;

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<Text>().text = ((int)time).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(time >= 0)
        {
            time -= Time.deltaTime;
            TimerText.text = time.ToString();
        }
        
    }
}
