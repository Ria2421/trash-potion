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
    public Text timerText;                 //タイマーテキストの指定
    int timer = 0;                        //カウントダウンタイマーの変数
    [SerializeField] float speed;         //速度調整
    public GameObject good;              //goodテキスト指定
    public GameObject veryGood;         //veryGoodテキストの指定
    public GameObject Bad;             //Badテキスト指定
    [SerializeField] Slider slider;   //スライダーの指定

    StartMiniGame tutorialMiniGame;
    TutorialBarManager tutorialBarManager;

    private bool _endCountDown;

    private bool isClicked;

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
        isClicked = false;

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
            if(isClicked == false)
            {
                //クリックを離した瞬間の判定
                if (Input.GetMouseButtonUp(0))
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
                    //Invoke("MiniGameDestroy", 1f);

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
        }
       
    }
    /// <summary>
    /// ミニゲームの破棄
    /// </summary>
    public void DestroyMiniGame()
    {
        // ミニゲームを終了
        //Destroy(GameObject.Find("MiniGames"));
        veryGood.SetActive(false);
        good.SetActive(false);
        Bad.SetActive(false);
        Destroy(GameObject.Find("OchaGame"));
        Destroy(GameObject.Find("OchaGameimgs"));

    }
}
