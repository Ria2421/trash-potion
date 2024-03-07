//===================================================
//
//プレイヤーの生成
//Author：高宮祐翔
//Date:3/1
//Update/3/6
//
//====================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerRank : MonoBehaviour
{
    [SerializeField] GameObject[] characterPrefabs; // 表示するキャラクターのプレハブ
    [SerializeField] GameObject rankObject;//表示するランクオブジェクト
    [SerializeField] GameObject congratulationObj;//表示するcongratulationオブジェクト
    [SerializeField] int[] PlayerID;//プレイヤーを生成するIDの指定


    // Start is called before the first frame update
    void Start()
    {
        // プレイヤーの数だけ繰り返し、キャラクターを生成
        for (int i = 0; i < PlayerID.Length; ++i)
        {
            //プレイヤーが一人の場合
            if(PlayerID.Length ==1)
            {
                // プレハブを指定の位置に生成し、180度回転させる
                Instantiate(characterPrefabs[PlayerID[i]], new Vector3(0.60f, 0f, -5f), Quaternion.Euler(0f, 180f, 0f));
            }
            //それ以外の場合
            else
            {
                // プレハブを指定の位置に生成し、180度回転させる
                Instantiate(characterPrefabs[PlayerID[i]], new Vector3(i * 1.20f, 0f, -5f), Quaternion.Euler(0f, 180f, 0f));
            }
        
        }

        //ランクオブジェクトを一定感覚で拡大、縮小する
        rankObject.transform
             .DOScale(new Vector3(1.5f, 1.5f, 0.5f), 0.5f)
             .SetLoops(-1, LoopType.Yoyo)//繰り返し設定する
             .SetEase(Ease.Linear);//一定の感覚で動かしている

       //conguratulationオブジェクトを一定感覚で拡大、縮小する
        congratulationObj.transform
             .DOScale(new Vector3(1.5f, 1.5f, 0.5f), 0.5f)
             .SetLoops(-1, LoopType.Yoyo)//繰り返し設定する
             .SetEase(Ease.Linear);//一定の感覚で動かしている

    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
