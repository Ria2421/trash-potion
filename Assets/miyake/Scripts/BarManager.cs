//==============================================
//Autor:三宅歩人
//Day:3/5
//スライダー（バー）ゲーム処理
//==============================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarManager : MonoBehaviour
{
    public GameObject good;
    public GameObject veryGood;
    public GameObject Bad;
    public Slider slider;
    public Text timerText;
    private bool maxValue;
    private bool isClicked;
    bool endCountDown;
    public Text limitTime;              //ミニゲームの制限時間
    int limit;                          //制限時間の変数
    bool isLimit;                     //制限時間を超えたかどうか

    void Start()
    {
        slider.value = 0;
        maxValue = false;
        isClicked = false;
        endCountDown = false;
        limit = 5;
        isLimit = false;
        limitTime.enabled = false;
        //1秒ごとに関数を実行
        InvokeRepeating("CountDownTimer", 3.0f, 1.0f);
    }

    void Update()
    {
        if (!isLimit)
        {
            if (timerText.text == "GO!!")
            {
                endCountDown = true;
                limitTime.enabled = true;
            }

            if (endCountDown)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    CancelInvoke();
                    isClicked = true;

                    if (slider.value >= 85)
                    {
                        veryGood.SetActive(true);
                    }
                    else if (slider.value >= 50)
                    {
                        good.SetActive(true);
                    }

                    else if (slider.value < 50)
                    {
                        Bad.SetActive(true);
                    }
                }

                //クリックされていなければ実行
                if (!isClicked)
                {
                    //最大値に達した場合と、最小値に戻った場合のフラグ切替え
                    if (slider.value == slider.maxValue)
                    {
                        maxValue = true;
                    }

                    if (slider.value == slider.minValue)
                    {
                        maxValue = false;
                    }

                    //フラグによるスライダー値の増減
                    if (maxValue)
                    {
                        slider.value -= 0.5f;
                    }
                    else
                    {
                        slider.value += 0.5f;
                    }
                }
            }
        }
    }

    void CountDownTimer()
    {
        limit--;
        limitTime.text = limit.ToString();
        if(limitTime.text == "-1")
        {
            Bad.SetActive (true);
            isLimit = true;
            CancelInvoke();
            Destroy(limitTime);
        }
    }
}
