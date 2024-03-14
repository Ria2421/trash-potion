//==============================================
//Autor:三宅歩人
//Day:3/1
//寸止めゲーム処理
//==============================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTestTubeManager : MonoBehaviour
{
    public Text timerText;                 //タイマーテキストの指定
    int timer = 0;                        //カウントダウンタイマーの変数
    [SerializeField] float speed;         //速度調整
    public GameObject good;              //goodテキスト指定
    public GameObject veryGood;         //veryGoodテキストの指定
    public GameObject Bad;             //Badテキスト指定
    [SerializeField] Slider slider;   //スライダーの指定
    [SerializeField] AudioClip veryGoodSE;      //大成功SE
    [SerializeField] AudioClip goodSE;          //成功SE
    [SerializeField] AudioClip badSE;           //失敗SE
    [SerializeField] AudioSource audioSource;

    StartMiniGame tutorialMiniGame;

    private bool _endCountDown;

    private bool isClicked;

    public bool endCountDown
    {
        get
        {
            return _endCountDown;
        }
        set
        {
            _endCountDown = value;
        }
    }
    //private bool maxValue;
   
    void Start()
    {
        tutorialMiniGame = GameObject.Find("MiniGameManager").GetComponent<StartMiniGame>();
        Init();
    }
    public void Init()
    {
        slider.value = 0;//初期化

        endCountDown = false;
        isClicked = false;

        tutorialMiniGame.gameNum = StartMiniGame.GAMEMODE.OCHA_MODE;

        veryGood.SetActive(false);//大成功テキストを非表示にする
        good.SetActive(false);//成功テキストを非表示にする
        Bad.SetActive(false);//失敗テキストを非表示にする
    }

    void Update()
    {
        if(endCountDown)
        {
            if(isClicked == false)
            {
                //クリックを離した瞬間の判定
                if (Input.GetMouseButtonUp(0))
                {
                    isClicked = true;

                    if (slider.value >= 94)
                    {
                        //失敗SE
                        audioSource.PlayOneShot(badSE);

                        Bad.SetActive(true);//失敗テキストを表示にする
                    }
                    else if (slider.value >= 68 && slider.value < 84)
                    {
                        //成功SE
                        audioSource.PlayOneShot(goodSE);

                        good.SetActive(true);//成功テキストを表示にする
                    }
                    else if (slider.value >= 84 && slider.value < 94)
                    {
                        //大成功SE
                        audioSource.PlayOneShot(veryGoodSE);

                        veryGood.SetActive(true);//大成功テキストを表示にする
                    }
                    else if (slider.value < 68)
                    {
                        //失敗SE
                        audioSource.PlayOneShot(badSE);

                        Bad.SetActive(true);//失敗テキストを表示にする
                    }
                    tutorialMiniGame.NextButton.SetActive(true);
                    tutorialMiniGame.AgainButton.SetActive(true);
                }
                //クリックされている間実行
                if (Input.GetMouseButton(0))
                {
                    slider.value += speed;

                    if (slider.value >= 94)
                    {
                        //失敗SE
                        audioSource.PlayOneShot(badSE);

                        Bad.SetActive(true);//失敗テキストを表示にする
                    }
                }
            }
        }
       
    }
    /// <summary>
    /// ミニゲームの破棄
    /// </summary>
    public void DestroyMiniGame()
    {
        // ミニゲームを終了
        veryGood.SetActive(false);//大成功テキストを非表示にする
        good.SetActive(false);//成功テキストを非表示にする
        Bad.SetActive(false);//失敗テキストを非表示にする
        Destroy(GameObject.Find("OchaGame"));//おちゃゲームを削除
        Destroy(GameObject.Find("OchaGameimgs"));//おちゃゲーム画像を削除

    }
}
