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
    public float rouletteSpeed = 0;        //ルーレットの回転速度

    // Start is called before the first frame update
    void Start()
    {
        //フレームレートを60に固定
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        //
        transform.Rotate(0, 0, rouletteSpeed);
    }
}
