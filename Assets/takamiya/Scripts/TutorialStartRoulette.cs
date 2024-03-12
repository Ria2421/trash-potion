//==============================================
//Autor:三宅歩人
//Day:2/29
//ルーレット回転処理
//==============================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialStartRoulette : MonoBehaviour
{
    float randAngle = 0;        //ランダムで回転する角度の変数
   
    public void LotteryAngle()
    {
        randAngle = Random.Range(-180, 180);
        //randAngle = 180;

        transform.eulerAngles = new Vector3(0, 0, randAngle);
    }
}
