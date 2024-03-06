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
    [SerializeField] float speed;
    public Slider slider;
    public Text timerText;
    private bool isClicked;
    bool endCountDown;

    void Start()
    {
        slider.value = 94;
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
            if (Input.GetMouseButtonUp(0))
            {
                isClicked = true;
            }

            //クリックされていなければ実行
            if (Input.GetMouseButton(0))
            {
                slider.value -= speed;
            }
        }
    }
}
