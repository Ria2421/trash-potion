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
    private bool isClicked;

    void Start()
    {
        slider.value = 94;
        isClicked = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isClicked = true;
        }

        //クリックされていなければ実行
        if (!isClicked)
        {
            slider.value -= 0.2f;
        }
    }
}
