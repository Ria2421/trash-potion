//==============================================
//Autor:三宅歩人
//Day:3/4
//ミニゲーム遷移処理
//==============================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialStartMiniGame : MonoBehaviour
{
    public Text timerText;
    int timer = 0;              //カウントダウンタイマーの変数
    int gameNum = 1;          //ランダムでミニゲームの抽選するための変数
    bool isLottery;             //抽選したか
    public GameObject slideGame;        //ミニゲーム1
    public GameObject rouletteGame;     //ミニゲーム2
    public GameObject ochaGame;         //ミニゲーム3
    public GameObject ochaGameImg;      //ミニゲーム3の画像
    [SerializeField] AudioClip countDownSE;     //カウントダウンSE
    [SerializeField] AudioSource audioSource;

    void Start()
    {
        isLottery = false;
        timer = 3;
        timerText.enabled = false;

        //カウントダウンSE
        audioSource.PlayOneShot(countDownSE);

        //+++++++++++++++++++++++++++++++++++++++++++++++
        // ボタン不使用
        timerText.enabled = true;
        //1秒ごとに関数を実行
        InvokeRepeating("CountDownTimer", 1.0f, 0.7f);
        //+++++++++++++++++++++++++++++++++++++++++++++++
    }

    //シーン抽選処理
    public void StartCountDown()
    {
        timerText.enabled = true;
        //1秒ごとに関数を実行
        InvokeRepeating("CountDownTimer", 1.0f, 0.7f);
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
            Invoke("TextDestroy", 0.7f);
        }
    }

    void LotteryGame()
    {
        //1～3の数字をランダムで代入。未満で抽選
        gameNum = Random.Range(1, 4);

        if (gameNum == 1)
        {
            slideGame.SetActive(true);
        }
        else if (gameNum == 2)
        {
            ochaGame.SetActive(true);
            ochaGameImg.SetActive(true);
        }
        else if (gameNum == 3)
        {
            rouletteGame.SetActive(true);
        }

        isLottery = true;
    }
    void TextDestroy()
    {
        Destroy(timerText);
    }
}
