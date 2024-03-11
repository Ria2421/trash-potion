//==============================================
//Autor:三宅歩人
//Day:3/1
//寸止めゲーム処理
//==============================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTestTubeManager : MonoBehaviour
{
    public Text timerText;
    int timer = 0;              //カウントダウンタイマーの変数
    [SerializeField] float speed;
    public GameObject good;//goodテキスト指定
    public GameObject veryGood;//veryGoodテキストの指定
    public GameObject Bad;//Badテキスト指定
    [SerializeField] Slider slider;//スライダーの指定
    StartMiniGame tutorialMiniGame;
    TutorialBarManager tutorialBarManager;

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
    //private bool maxValue;
   
    void Start()
    {
        tutorialMiniGame = GameObject.Find("MiniGameManager").GetComponent<StartMiniGame>();
        Init();
    }
    public void Init()
    {
        slider.value = 0;//初期化

        //timer = 3;

        endCountDown = false;

        tutorialMiniGame.gameNum = StartMiniGame.GAMEMODE.OCHA_MODE;

        veryGood.SetActive(false);
        good.SetActive(false);
        Bad.SetActive(false);

        //1秒ごとに関数を実行
        //InvokeRepeating("CountDownTimer", 1.0f, 0.7f);

        //tutorialBarManager = GameObject.Find("Slider").GetComponent<TutorialBarManager>();
    }

    void Update()
    {
        if(endCountDown)
        {
            //クリックを離した瞬間の判定
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
                else if (slider.value < 68)
                {
                    Bad.SetActive(true);
                }
                tutorialMiniGame.NextButton.SetActive(true);
                tutorialMiniGame.AgainButton.SetActive(true);

                // ミニゲームの終了
                Invoke("MiniGameDestroy", 1f);
            }
            //クリックされている間実行
            if (Input.GetMouseButton(0))
            {
                slider.value += speed;

                if (slider.value >= 94)
                {
                    Bad.SetActive(true);
                }
            }
        }
        else if(!endCountDown)
        {
            slider.value = 0;
            tutorialMiniGame.NextButton.SetActive(false);
            tutorialMiniGame.AgainButton.SetActive(false);
        }

    }
    /// <summary>
    /// ミニゲームの破棄
    /// </summary>
    private void MiniGameDestroy()
    {
        // ミニゲームを終了
        //Destroy(GameObject.Find("MiniGames"));

    }

    //void CountDownTimer()
    //{
   
    //    //カウントダウンしてタイマーのテキストに秒数を設定
    //    timer--;
    //    timerText.text = timer.ToString();
    //    if (timerText.text == "2")
    //    {
    //        timerText.color = Color.yellow;
    //    }
    //    else if (timerText.text == "1")
    //    {
    //        timerText.color = Color.red;
    //    }

    //    //timerが0になったら終了
    //    if (timer == 0)
    //    {
    //        //Invokeをやめる
    //        CancelInvoke();
    //        timerText.text = "GO!!";
    //        if(tutorialMiniGame.gameNum == StartMiniGame.GAMEMODE.OCHA_MODE)
    //        {
    //            tutorialBarManager.endCountDown = true;
    //        }
            
    //        Invoke("TextDestroy", 0.7f);
    //    }
    //}
}
