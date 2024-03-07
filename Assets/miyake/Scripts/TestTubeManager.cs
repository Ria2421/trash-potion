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
    [SerializeField] float speed;
    public GameObject good;
    public GameObject veryGood;
    public GameObject Bad;
    public Slider slider;
    public Text timerText;
    private bool maxValue;
    bool endCountDown;

    void Start()
    {
        slider.value = 0;
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
                if (slider.value >= 94)
                {
                    Bad.SetActive(true);
                }
                else if (slider.value >= 68 && slider.value < 84)
                {
                    good.SetActive(true);
                }
                else if (slider.value >= 84 && slider.value < 94)
                {
                    veryGood.SetActive(true);
                }
                else if(slider.value < 68)
                {
                    Bad.SetActive(true);
                }

                // ミニゲームの終了
                Invoke("MiniGameDestroy", 1f);
            }

            //クリックされていなければ実行
            if (Input.GetMouseButton(0))
            {
                slider.value += speed;

                if (slider.value >= 94)
                {
                    Bad.SetActive(true);
                }
            }
        }
    }

    /// <summary>
    /// ミニゲームの破棄
    /// </summary>
    private void MiniGameDestroy()
    {
        // ミニゲームを終了
        Destroy(GameObject.Find("MiniGames(Clone)"));
    }
}
