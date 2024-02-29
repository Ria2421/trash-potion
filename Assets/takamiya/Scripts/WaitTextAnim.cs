//---------------------------------------------------------------
//
//  待機テキストアニメーション [ WaitTextAnim.cs ]
//  Author:Kenta Nakamoto
//  Data 2024/02/29
//  Update 2024/02/29
//
//---------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitTextAnim : MonoBehaviour
{
    //------------------------------------------------------------------------------
    // フィールド ----------------------------------------

    /// <summary>
    /// 繰り返す間隔
    /// </summary>
    private float _repeatSpan;

    /// <summary>
    /// 経過時間
    /// </summary>
    private float _timeElapsed;

    /// <summary>
    /// 初期化処理
    /// </summary>
    void Start()
    {
        //表示切り替え時間を指定
        _repeatSpan = 0.5f;
        _timeElapsed = 0;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        _timeElapsed += Time.deltaTime;     //時間をカウントする
        if (_timeElapsed >= _repeatSpan)
        {   // 時間経過でテキスト表示
            GetComponent<Text>().text = "モード選択中";
        }
        if (_timeElapsed >= _repeatSpan + 0.5f)
        {   // 時間経過でテキスト表示(役職)
            GetComponent<Text>().text = "モード選択中.";
        }
        if (_timeElapsed >= _repeatSpan + 1.0f)
        {   // 時間経過でテキスト表示(役職)
            GetComponent<Text>().text = "モード選択中..";
        }
        if (_timeElapsed >= _repeatSpan + 1.5f)
        {   // 時間経過でテキスト表示(役職)
            GetComponent<Text>().text = "モード選択中...";
            _timeElapsed = 0;   //経過時間をリセットする
        }
    }
}
