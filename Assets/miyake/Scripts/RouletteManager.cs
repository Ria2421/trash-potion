//==============================================
//Autor:三宅歩人
//Day:2/28
//ルーレット処理
//==============================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteManager : MonoBehaviour
{
    public float rouletteSpeed = 0;        //回転速度
    public GameObject verygood;
    public GameObject good;
    public GameObject bad;
    public GameObject roulette;            //ルーレット本体
    float angle = 0;                       //回転の角度の変数

    // Start is called before the first frame update
    void Start()
    {
        //フレームレートを60に固定
        Application.targetFrameRate = 60;

        angle = roulette.transform.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        //ルーレットを回転
        transform.Rotate(0, 0, rouletteSpeed);

        if (Input.GetMouseButtonDown(0))
        {//左クリックされたら
            rouletteSpeed = 0;
            Judge();
        }
    }

    //判定処理
    void Judge()
    {
        //大成功
        if(transform.eulerAngles.z >= 154 + angle && transform.eulerAngles.z  <= 186 + angle)
        {
            verygood.SetActive(true);
        }

        //成功
        else if(transform.eulerAngles.z  >= 185 + angle && transform.eulerAngles.z  <= 230 + angle ||  transform.eulerAngles.z  >= 110 + angle && transform.eulerAngles.z  <= 155 + angle)
        {
            good.SetActive(true);
        }

        else
        {
            bad.SetActive(true);
        }
    }
}
