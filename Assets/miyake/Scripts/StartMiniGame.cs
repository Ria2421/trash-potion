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
    int randScene = 0;          //ランダムでミニゲームの抽選するための変数
    bool endCountDown;          //カウントダウンが終わったかどうか
    public GameObject slideGame;        //ミニゲーム1
    public GameObject rouletteGame;     //ミニゲーム2
    public GameObject ochaGame;         //ミニゲーム3

    void Start()
    {
        timer = 3;
        endCountDown = false;
    }

    //シーン抽選処理
    public void StartCountDown()
    {
        //1秒ごとに関数を実行
        InvokeRepeating("CountDownTimer", 1.0f, 1.0f);
    }

    //カウントダウン処理
    void CountDownTimer ()
    {
        //カウントダウンしてタイマーのテキストに秒数を設定
        timer--;
        timerText.text = timer.ToString();

        //timerが0になったら終了
        if (timer < 0)
        {
            //Invokeをやめる
            CancelInvoke();
            endCountDown=true;
            Destroy(timerText);
            LotteryGame();
        }
    }

    void LotteryGame()
    {
        if (endCountDown)
        {
            //1～3の数字をランダムで代入。以下で抽選
            randScene = Random.Range(1, 3);

            if (randScene == 1)
            {
                slideGame.SetActive(true);
            }
            else if (randScene == 2)
            {
                rouletteGame.SetActive(true);
            }
            else if (randScene == 3)
            {
                ochaGame.SetActive(true);
            }
        }
    }
}
