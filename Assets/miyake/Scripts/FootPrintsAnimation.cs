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

    // Start is called before the first frame update
    void Start()
    {
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
        if(leftFootPrint.activeSelf)
        {//左足が表示されているとき
            leftFootPrint.SetActive(false);
            rightFootPrint.SetActive(true);
        }
        else
        {
            leftFootPrint.SetActive(true);
            rightFootPrint.SetActive(false);
        }
    }
}
