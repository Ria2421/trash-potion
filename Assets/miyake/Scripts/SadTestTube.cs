//==============================================
//Autor:三宅歩人
//Day:3/4
//寸止めゲームの入れるポーションの方の減少処理
//==============================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SadTestTube : MonoBehaviour
{
    public Slider slider;
    public Text timerText;
    private bool isClicked;
    bool endCountDown;
    public Text limitTime;              //ミニゲームの制限時間
    int limit;                          //制限時間の変数
    bool isLimit;                     //制限時間を超えたかどうか

    void Start()
    {
        slider.value = 94;
        isClicked = false;
        endCountDown = false;
        limit = 5;
        limitTime.enabled = false;
        isLimit = false;
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
                if (Input.GetMouseButtonUp(0))
                {
                    isClicked = true;
                    CancelInvoke();
                }

                //クリックされていなければ実行
                if (Input.GetMouseButton(0))
                {
                    slider.value -= 0.2f;
                }
            }
        }
    }

    void CountDownTimer()
    {
        limit--;
        limitTime.text = limit.ToString();
        if (limitTime.text == "-1")
        {
            isLimit = true;
            CancelInvoke();
            Destroy(limitTime);
        }
    }
}
