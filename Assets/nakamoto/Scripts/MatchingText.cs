//---------------------------------------------------------------
//
//  マッチングテキストアニメーション [ MatchingText.cs ]
//  Author:Kenta Nakamoto
//  Data 2024/02/27
//  Update 2024/02/27
//
//---------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchingText : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        //表示切り替え時間を指定
        _repeatSpan = 0.5f;
        _timeElapsed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _timeElapsed += Time.deltaTime;     //時間をカウントする
        if (_timeElapsed >= _repeatSpan)
        {   // 時間経過でテキスト表示
            GetComponent<Text>().text = "マッチング中";
        }
        if (_timeElapsed >= _repeatSpan + 0.5f)
        {   // 時間経過でテキスト表示(役職)
            GetComponent<Text>().text = "マッチング中.";
        }
        if (_timeElapsed >= _repeatSpan + 1.0f)
        {   // 時間経過でテキスト表示(役職)
            GetComponent<Text>().text = "マッチング中..";
        }
        if (_timeElapsed >= _repeatSpan + 1.5f)
        {   // 時間経過でテキスト表示(役職)
            GetComponent<Text>().text = "マッチング中...";
            _timeElapsed = 0;   //経過時間をリセットする
        }
    }
}
