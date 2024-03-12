//==============================================
//Autor:三宅歩人
//Day:3/4
//寸止めゲームの入れるポーションの方の減少処理
//==============================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSadTestTube : MonoBehaviour
{
    [SerializeField] float speed;
    public Slider slider;
    private bool isClicked;
    private bool _endCountDown;

    public bool endCountDown
    {
        get
        {
            return _endCountDown;
        }
        set
        {
            _endCountDown = value;
        }
    }

    void Start()
    {
        Init();
    }
    public void Init()
    {
        slider.value = 94;
        isClicked = false;
        endCountDown = false;
    }

    void Update()
    {
        if (endCountDown)
        {
            if (isClicked == false)
            {
                //クリックを離したら
                if (Input.GetMouseButtonUp(0))
                {
                    isClicked = true;
                }


                //長押しされている間実行
                if (Input.GetMouseButton(0))
                {
                    slider.value -= speed;
                }
            }
        }
    }
}
