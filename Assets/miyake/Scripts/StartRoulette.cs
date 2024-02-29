//==============================================
//Autor:三宅歩人
//Day:2/29
//ルーレット回転処理
//==============================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRoulette : MonoBehaviour
{
    float randAngle = 0;        //ランダムで回転する角度の変数

    // Start is called before the first frame update
    void Start()
    {
        randAngle = Random.Range(-180, 180);

        transform.eulerAngles = new Vector3(0, 0, randAngle);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
