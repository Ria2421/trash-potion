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

    void Start()
    {
        slider.value = 0;
        maxValue = false;
        isClicked = false;
        endCountDown = false;
    }

    void Update()
    {
        if (timerText.text == "GO!!")
        {
            endCountDown = true;
        }

        if (endCountDown)
        {
            if (Input.GetMouseButtonDown(0))
            {
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
