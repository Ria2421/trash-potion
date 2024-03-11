//==============================================
//Autor:三宅歩人
//Day:3/4
//ミニゲーム遷移処理
//==============================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMiniGame : MonoBehaviour
{
    public Text timerText;
    int timer = 0;              //カウントダウンタイマーの変数
    //int gameNum = 3;          //ランダムでミニゲームの抽選するための変数
    bool isLottery;             //抽選したか
    public GameObject slideGame;        //ミニゲーム1
    public GameObject rouletteGame;     //ミニゲーム2
    public GameObject ochaGame;         //ミニゲーム3
    public GameObject ochaGameImg;      //ミニゲーム3の画像

    public GameObject AgainButton; //もう一度ボタン
    public GameObject NextButton;  //次のボタン

    TutorialBarManager tutorialBarManager;

    TutorialTestTubeManager tutorialTestTube;

    GameObject countDownObject;

    public enum GAMEMODE
    {
        NONE = 0,
        SLIDE_MODE,//スライドミニゲームモード
        OCHA_MODE,//おちゃミニゲームモード
        ROULETTE_MODE,//ルーレットミニゲームモード
    }

    public GAMEMODE gameNum;

    void Start()
    {
        gameNum = GAMEMODE.SLIDE_MODE;
        GameObject parentObject = GameObject.Find("MiniGameCanvas");
        countDownObject = parentObject.transform.Find("CountDown").gameObject;

        //tutorialTestTube = GameObject.Find("OchaGame").GetComponent<TutorialTestTubeManager>();
       GameObject ochaGameObject = parentObject.transform.Find("OchaGame").gameObject;
       GameObject  SliderObject = ochaGameObject.transform.Find("Slider").gameObject;
        tutorialTestTube = SliderObject.GetComponent<TutorialTestTubeManager>();
        

        Init();
    }
    public void Init()
    {
        isLottery = false;
        timer = 3;

        AgainButton.SetActive(false);
        NextButton.SetActive(false);
        countDownObject.SetActive(true);
        timerText.text = timer.ToString();
        timerText.color = new Color(0,255,253,255);
        //1秒ごとに関数を実行
        InvokeRepeating("CountDownTimer", 1.0f, 0.7f);
    }
    public void Retry()
    {
        switch (gameNum)
        {
            case GAMEMODE.SLIDE_MODE:
                tutorialBarManager.Init();
                Init();
                break;
            case GAMEMODE.OCHA_MODE:
                tutorialTestTube.Init();
                Init();
                break;



        }
    }

    //カウントダウン処理
    void CountDownTimer()
    {
        if(!isLottery)
        {
            LotteryGame();
        }

        //カウントダウンしてタイマーのテキストに秒数を設定
        timer--;
        timerText.text = timer.ToString();
        if(timerText.text == "2")
        {
            timerText.color = Color.yellow;
        }
        else if(timerText.text == "1")
        {
            timerText.color = Color.red;
        }

        //timerが0になったら終了
        if (timer == 0)
        {
            //Invokeをやめる
            CancelInvoke();
            timerText.text = "GO!!";
            if(gameNum == GAMEMODE.SLIDE_MODE)
            {
                tutorialBarManager.endCountDown = true;
            }
            Invoke("TextDestroy", 0.7f);
        }
    }

    void LotteryGame()
    { 
        switch(gameNum)
        {
            case GAMEMODE.SLIDE_MODE://スライドゲームの場合
                slideGame.SetActive(true);
                tutorialBarManager = GameObject.Find("Slider").GetComponent<TutorialBarManager>();
                break;

            case GAMEMODE.OCHA_MODE://おちゃゲームの場合
                ochaGame.SetActive(true);
                ochaGameImg.SetActive(true);
                break;

            case GAMEMODE.ROULETTE_MODE://ルーレットゲームの場合
                rouletteGame.SetActive(true);
                break;

            default:
                break;
        }

        isLottery = true;
    }
    void TextDestroy()
    {
        //Destroy(timerText);
        countDownObject.SetActive(false);

    }

    public void NextGameButton()
    {
        switch (gameNum)
        {
            case GAMEMODE.SLIDE_MODE://スライドゲームの場合
                //slideGame.SetActive(false);
                //ochaGame.SetActive(true);
                //ochaGameImg.SetActive(true);
                //tutorialBarManager.good.SetActive(false);
                //tutorialBarManager.veryGood.SetActive(false);
                //tutorialBarManager.Bad.SetActive(false);
                //NextButton.SetActive(false);
                //AgainButton.SetActive(false);
               tutorialBarManager.DestroyMiniGame();
                //tutorialTestTube.Init();
                gameNum++;
                Init();
                break;

            case GAMEMODE.OCHA_MODE://おちゃゲームの場合
                ochaGame.SetActive(false);
                ochaGameImg.SetActive(false);
                rouletteGame.SetActive(true);
                tutorialTestTube.good.SetActive(false);
                tutorialTestTube.veryGood.SetActive(false);
                tutorialTestTube.Bad.SetActive(false);
                NextButton.SetActive(false);
                AgainButton.SetActive(false);
                break;

            case GAMEMODE.ROULETTE_MODE://ルーレットゲームの場合
                rouletteGame.SetActive(true);
                break;

            default:
                break;
        }
    }
}
