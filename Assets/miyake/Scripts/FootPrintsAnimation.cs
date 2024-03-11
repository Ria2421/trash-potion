//==============================================
//Autor:三宅歩人
//Day:3/5
//UIのアニメーション処理
//==============================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootPrintsAnimation : MonoBehaviour
{
    [SerializeField] GameObject footPrint1;  //足1
    [SerializeField] GameObject footPrint2;  //足2
    bool isMove;                             //動いたかどうか判別する変数

    // Start is called before the first frame update
    void Start()
    {
        isMove = false;
        //0.7秒間隔で呼ばれ続ける
        InvokeRepeating("Animation",1.0f,0.7f);
    }

    //アニメーション処理
    void Animation()
    {
        if (isMove)
        {
            footPrint1.SetActive(true);
            footPrint2.SetActive(false);

            isMove = false;
        }
        else
        {
            footPrint1.SetActive(false);
            footPrint2.SetActive(true);

            isMove = true;
        }
    }
}
