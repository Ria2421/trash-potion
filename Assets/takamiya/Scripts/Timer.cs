//=============================================
//タイマースクリプト
//Author：高宮祐翔
//Date:2/26
//Update:2/27
//==============================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class Timer : MonoBehaviour
{
    //Mesh型のテキストを受け取っておく
    [SerializeField] private TextMeshProUGUI TimerText;
    // 5秒以下になった時のSEを受け取っておく
    [SerializeField] private AudioClip timerse;

    private AudioSource audioSource;

    private float time = 15;

    bool ishurryup = false;

    bool iscolorchange = true;

    // Start is called before the first frame update
    void Start()
    {
        // AudioSource コンポーネントの取得
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //タイムが5秒以下になったらテキストを拡大したり、縮小させたりする
        if (ishurryup == false)
        {
            if (time <= 5)
            {
                ishurryup = true;
                iscolorchange = true;
                TimerText.transform
              .DOScale(new Vector3(1.5f, 1.5f, 0.5f), 0.5f)
              .SetLoops(-1, LoopType.Yoyo)//繰り返し設定する
              .SetEase(Ease.Linear);//一定の感覚で動かしている

                TextColorChange();

                // 引数のクリップを再生
                audioSource.PlayOneShot(timerse);
            }
        }
        TimerCountdown();
    }  
    void TimerCountdown()
    {
        //時間をカウントダウンする
        time -= Time.deltaTime;
        //タイムを0秒にする
        if (time < 0)
        {
            time = 0;
            ishurryup = false;
            iscolorchange = false;
            audioSource.Stop();
        }
        //タイムをテキストに表示
        TimerText.text = time.ToString("0");
    }
    /// <summary>
    /// テキストの色変える
    /// </summary>
    private void TextColorChange()
    {
        if (iscolorchange == true)
        {
            TimerText
          .DOColor(ChangeColor(), 0.5f)
          .OnComplete(TextColorChange);
        }
    }
    /// <summary>
    /// テキストのカラーを白から赤に変える
    /// </summary>
    /// <returns></returns>
    private Color ChangeColor()
    {
        Color nowColor = TimerText.color;
        if (nowColor == Color.white)
        {
            return Color.red;
        }
        else
        {
            return Color.white;
        }
    }
}
