//
//ランダムにエモートを表示するスクリプト
//Author:MiuraYuki
//Date:2024/03/12
//Update:2024/03/12
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoteManager: MonoBehaviour
{
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();//Animatorを取得
        Random.InitState(6);//ランダムの数値を初期化
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            int nom = Random.Range(0, 6);//ランダムに出る値の範囲の指定(1～5)
            animator.SetInteger("TransitionNom", nom);//TransitionNomにnomを代入する
        }
    }
}
