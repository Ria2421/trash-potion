//==============================================
//Autor:三宅歩人
//Day:3/1
//寸止めゲーム処理
//==============================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestTubeManager : MonoBehaviour
{
    public GameObject good;
    public GameObject veryGood;
    public GameObject Bad;
    public Slider slider;
    private bool maxValue;
    private bool isClicked;

    void Start()
    {
        slider.value = 0;
        isClicked = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isClicked = true;

            if (slider.value >= 94)
            {
                Bad.SetActive(true);
            }
            else if (slider.value >= 68 && slider.value < 84)
            {
                good.SetActive(true);
            }
            else if(slider.value >= 84 && slider.value < 94)
            {
                veryGood.SetActive(true);
            }
        }

        //クリックされていなければ実行
        if (!isClicked)
        {
            slider.value += 0.3f;

            if (slider.value >= 94)
            {
                isClicked = true;
                Bad.SetActive(true);
            }
        }
    }
}
