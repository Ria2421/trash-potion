//==============================================
//Autor:三宅歩人
//Day:2/28
//ルーレット処理
//==============================================
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class RouletteManager : MonoBehaviour
{
    public float rouletteSpeed = 0;        //回転速度
    public GameObject verygood;
    public GameObject good;
    public GameObject bad;
    public GameObject roulette;            //ルーレット本体
    public Text timerText;
    float angle = 0;                       //回転の角度の変数
    bool endCountDown;
    public Text limitTime;              //ミニゲームの制限時間
    int limit;                          //制限時間の変数
    bool isLimit;                     //制限時間を超えたかどうか

    // Start is called before the first frame update
    void Start()
    {
        //フレームレートを60に固定
        Application.targetFrameRate = 60;
        endCountDown = false;
        limit = 5;
        limitTime.enabled = false;
        isLimit = false;
        //1秒ごとに関数を実行
        InvokeRepeating("CountDownTimer", 3.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLimit)
        {
            if (timerText.text == "GO!!")
            {
                endCountDown = true;
                limitTime.enabled = true;
            }
            if (endCountDown)
            {
                //ルーレットを回転
                transform.Rotate(0, 0,rouletteSpeed);

                if (Input.GetMouseButtonDown(0))
                {//左クリックされたら
                    rouletteSpeed = 0;
                    Judge();
                    CancelInvoke();
                }
            }
        }
    }

    //判定処理
    void Judge()
    {

        angle = roulette.transform.eulerAngles.y;
        float angleA = (174 + angle) % 360;     //大成功の端
        float angleB = (201 + angle) % 360;     //大成功の端
        float angleC = (133 + angle) % 360;     //成功の端
        float angleD = (244 + angle) % 360;     //成功の端


        //大成功
        if (angleA > angleB)
        {//360度を超えていたら
            if ((angleA <= transform.eulerAngles.y && transform.eulerAngles.y <= 360) || (0 <= transform.eulerAngles.y && transform.eulerAngles.y <= angleB))
            {
                verygood.SetActive(true);

                return;
            }
        }
        else
        {
            if (transform.eulerAngles.y >= angleA && transform.eulerAngles.y <= angleB)
            {
                verygood.SetActive(true);
                return;
            }
        }


        //成功
        if (angleC > angleD)
        {
            if ((angleC <= transform.eulerAngles.y && transform.eulerAngles.y <= 360) || (0 <= transform.eulerAngles.y && transform.eulerAngles.y <= angleD))
            {
                good.SetActive(true);
                return;
            }
        }
        else
        {
            if (transform.eulerAngles.y >= angleC && transform.eulerAngles.y <= angleD)
            {
                good.SetActive(true);
                return;
            }
        }
        
        bad.SetActive(true);
    }

    void CountDownTimer()
    {
        limit--;
        limitTime.text = limit.ToString();
        if (limitTime.text == "-1")
        {
            bad.SetActive(true);
            isLimit = true;
            CancelInvoke();
            Destroy(limitTime);
        }
    }
}
