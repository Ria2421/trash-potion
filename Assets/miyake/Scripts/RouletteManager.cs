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

    // Start is called before the first frame update
    void Start()
    {
        //フレームレートを60に固定
        Application.targetFrameRate = 60;
        endCountDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerText.text == "GO!!")
        {
            endCountDown = true;
        }
        if (endCountDown) 
        { 
            //ルーレットを回転
            transform.Rotate(0, 0, rouletteSpeed);

            if (Input.GetMouseButtonDown(0))
            {//左クリックされたら
                rouletteSpeed = 0;
                Judge();
            }
        }
    }

    //判定処理
    void Judge()
    {

        angle = roulette.transform.eulerAngles.z;

        //大成功

        float angleA = (154 + angle) % 360;
        float angleB = (186 + angle) % 360;
        float angleC = (110 + angle) % 360;
        float angleD = (230 + angle) % 360;

        if (angleA > angleB)
        {//360度を超えていたら
            if ((angleA <= transform.eulerAngles.z && transform.eulerAngles.z <= 360) || (0 <= transform.eulerAngles.z && transform.eulerAngles.z <= angleB))
            {
                verygood.SetActive(true);

                return;
            }
        }
        else
        {
            if (transform.eulerAngles.z >= angleA && transform.eulerAngles.z <= angleB)
            {
                verygood.SetActive(true);
                return;
            }
        }


        //成功
        if (angleC > angleD)
        {
            if ((angleC <= transform.eulerAngles.z && transform.eulerAngles.z <= 360) || (0 <= transform.eulerAngles.z && transform.eulerAngles.z <= angleD))
            {
                good.SetActive(true);
                return;
            }
        }
        else
        {
            if (transform.eulerAngles.z >= angleC && transform.eulerAngles.z <= angleD)
            {
                good.SetActive(true);
                return;
            }
        }
        
        bad.SetActive(true);
    }
}
