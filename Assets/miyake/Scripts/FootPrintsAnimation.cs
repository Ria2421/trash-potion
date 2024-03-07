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
    public GameObject leftFootPrint;        //左足
    public GameObject rightFootPrint;       //右足
    bool isMove;                            //動いたかどうか判別する変数

    // Start is called before the first frame update
    void Start()
    {
        isMove = false;
        //0.7秒間隔で呼ばれ続ける
        InvokeRepeating("Animation",1.0f,0.7f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //アニメーション処理
    void Animation()
    {
        if (isMove)
        {
            leftFootPrint.transform.position = new Vector3(-0.2f, 1.2f, -1);
            rightFootPrint.transform.position = new Vector3(0.2f, 0.7f, -1);

            isMove = false;
        }
        else
        {
            leftFootPrint.transform.position = new Vector3(-0.3f, 0.9f, -1);
            rightFootPrint.transform.position = new Vector3(0.3f, 1.12f, -1);

            isMove = true;
        }
    }
}
