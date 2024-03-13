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
    bool isLottery;             //抽選したか
    public GameObject slideGame;        //ミニゲーム1
    public GameObject rouletteGame;     //ミニゲーム2
    public GameObject ochaGame;         //ミニゲーム3
    public GameObject ochaGameImg;      //ミニゲーム3の画像

    public GameObject AgainButton; //もう一度ボタン
    public GameObject NextButton;  //次のボタン
    public GameObject BackTitleButton;//タイトルに戻るボタン

    TutorialBarManager tutorialBarManager;

    TutorialTestTubeManager tutorialTestTube;

    TutorialSadTestTube tutorialSadTestTube;

    TutorialRouletteManager tutorialRouletteManager;

    TutorialStartRoulette tutorialStartRoulette;

    StartMiniGame tutorialMiniGame;

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

        //MiniGameCanvasオブジェクトを取得
        GameObject parentObject = GameObject.Find("MiniGameCanvas");
        //CountDownオブジェクトを取得
        countDownObject = parentObject.transform.Find("CountDown").gameObject;

        //OchaGameオブジェクトを取得
        GameObject ochaGameObject = parentObject.transform.Find("OchaGame").gameObject;
        //Sliderオブジェクトを取得
        GameObject SliderObject = ochaGameObject.transform.Find("Slider").gameObject;

        //Sliderオブジェクトに入っているコンポーネントを取得
        tutorialTestTube = SliderObject.GetComponent<TutorialTestTubeManager>();

        //TestTube2Sliderオブジェクトを取得
        GameObject testTube2SliderObject = ochaGameObject.transform.Find("TestTube2Slider").gameObject;

        //TestTube2Sliderオブジェクトに入っているコンポーネントを取得
        tutorialSadTestTube = testTube2SliderObject.GetComponent<TutorialSadTestTube>();

        //MiniGamesオブジェクトを取得
        GameObject miniGamesObject = GameObject.Find("MiniGames");
        //RouletteGameオブジェクトを取得
        GameObject rouletteGameObject = miniGamesObject.transform.Find("RouletteGame").gameObject;
        //Rouletteオブジェクトを取得
        GameObject rouletteObject = rouletteGameObject.transform.Find("Roulette").gameObject;
        //RouletteManagerオブジェクトを取得
        GameObject rouletteManagerObject = rouletteGameObject.transform.Find("RouletteManager").gameObject;

        //RouletteManagerオブジェクトに入っているコンポーネントを取得
        tutorialRouletteManager = rouletteManagerObject.GetComponent<TutorialRouletteManager>();
        //Rouletteオブジェクトに入っているコンポーネントを取得
        tutorialStartRoulette = rouletteObject.GetComponent<TutorialStartRoulette>();
        //MiniGameManagerオブジェクトを取得
        tutorialMiniGame = GameObject.Find("MiniGameManager").GetComponent<StartMiniGame>();

        Init();
    }
    public void Init()
    {
        isLottery = false;
        timer = 3;
        
        AgainButton.SetActive(false);//もう一度ボタンを非表示にする
        NextButton.SetActive(false);//次へボタンを非表示にする
        countDownObject.SetActive(true);
        timerText.text = timer.ToString();
        timerText.color = new Color(0, 255, 253, 255);
        //1秒ごとに関数を実行
        InvokeRepeating("CountDownTimer", 1.0f, 0.7f);
    }
    /// <summary>
    /// もう一度ミニゲームを繰り返す
    /// </summary>
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
                tutorialSadTestTube.Init();
                Init();
                break;
            case GAMEMODE.ROULETTE_MODE:
                tutorialRouletteManager.Init();
                Init();
                break;

        }
        tutorialMiniGame.NextButton.SetActive(false);//次へボタンを非表示にする
        tutorialMiniGame.BackTitleButton.SetActive(false);//タイトルに戻るボタンを非表示にする
        tutorialMiniGame.AgainButton.SetActive(false);//もう一度ボタンを非表示にする
    }

    //カウントダウン処理
    void CountDownTimer()
    {
        if (!isLottery)
        {
            LotteryGame();
        }

        //カウントダウンしてタイマーのテキストに秒数を設定
        timer--;
        timerText.text = timer.ToString();
        if (timerText.text == "2")
        {
            timerText.color = Color.yellow;//テキストカラーを黄色に変える
        }
        else if (timerText.text == "1")
        {
            timerText.color = Color.red;//テキストカラーを赤に変える
        }

        //timerが0になったら終了
        if (timer == 0)
        {
            //Invokeをやめる
            CancelInvoke();
            timerText.text = "GO!!";
            if (gameNum == GAMEMODE.SLIDE_MODE)
            {
                tutorialBarManager.endCountDown = true;
            }
            else if (gameNum == GAMEMODE.OCHA_MODE)
            {
                tutorialTestTube.endCountDown = true;
                tutorialSadTestTube.endCountDown = true;
            }
            else if (gameNum == GAMEMODE.ROULETTE_MODE)
            {
                tutorialRouletteManager.endCountDown = true;
                tutorialStartRoulette.LotteryAngle();
            }
            Invoke("TextDestroy", 0.7f);
        }
    }

    void LotteryGame()
    {
        switch (gameNum)
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
    /// <summary>
    /// カウントダウンオブジェクトを非表示にする
    /// </summary>
    void TextDestroy()
    {
        countDownObject.SetActive(false);
    }

    /// <summary>
    /// 次のミニゲームに行く時の処理
    /// </summary>
    public void NextGameButton()
    {
        switch (gameNum)
        {
            case GAMEMODE.SLIDE_MODE://スライドゲームの場合
                tutorialBarManager.DestroyMiniGame();
                gameNum++;
                Init();
                break;

            case GAMEMODE.OCHA_MODE://おちゃゲームの場合
                tutorialTestTube.DestroyMiniGame();
                gameNum++;
                Init();
                break;

            case GAMEMODE.ROULETTE_MODE://ルーレットゲームの場合
                Initiate.Fade("Title", Color.black, 1.0f);
                break;

            default:
                break;
        }
        tutorialMiniGame.NextButton.SetActive(false);
        tutorialMiniGame.AgainButton.SetActive(false);
    }
}
