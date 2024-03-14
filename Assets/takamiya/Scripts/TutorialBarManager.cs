//==============================================
//Autor:三宅歩人
//Day:3/5
//スライダー（バー）ゲーム処理
//==============================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialBarManager : MonoBehaviour
{
    [SerializeField] float speed;
    public GameObject good;
    public GameObject veryGood;
    public GameObject Bad;
    public Slider slider;
    public Text timerText;
    private bool maxValue;
    private bool isClicked;
    private bool _endCountDown;
    StartMiniGame tutorialMiniGame;
    [SerializeField] AudioClip veryGoodSE;      //大成功SE
    [SerializeField] AudioClip goodSE;          //成功SE
    [SerializeField] AudioClip badSE;           //失敗SE
    [SerializeField] AudioSource audioSource;
    public bool endCountDown {
        get
        {
            return _endCountDown;
        }
        set
        {
            _endCountDown = value;
        }
    }

    void Start()
    {
        tutorialMiniGame = GameObject.Find("MiniGameManager").GetComponent<StartMiniGame>();
        Init();
    }
    public void Init()
    {
        slider.value = 0;
        maxValue = false;
        isClicked = false;
        endCountDown = false;
        veryGood.SetActive(false);
        good.SetActive(false);
        Bad.SetActive(false);

        tutorialMiniGame.gameNum = StartMiniGame.GAMEMODE.SLIDE_MODE;
    }

    void Update()
    {
        if (endCountDown)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isClicked = true;

                if (slider.value >= 85)
                {
                    //大成功SE
                    audioSource.PlayOneShot(veryGoodSE);

                    veryGood.SetActive(true);
                }
                else if (slider.value >= 50)
                {
                    //成功SE
                    audioSource.PlayOneShot(goodSE);

                    good.SetActive(true);
                }
                else if (slider.value < 50)
                {
                    //失敗SE
                    audioSource.PlayOneShot(badSE);

                    Bad.SetActive(true);
                }
                tutorialMiniGame.NextButton.SetActive(true);
                tutorialMiniGame.AgainButton.SetActive(true);
                // ミニゲームの終了
                Invoke("MiniGameDestroy", 1f);
            }

            //クリックされていなければ実行
            if (!isClicked)
            {
                //最大値に達した場合と、最小値に戻った場合のフラグ切替え
                if (slider.value == slider.maxValue)
                {
                    maxValue = true;
                }

                if (slider.value == slider.minValue)
                {
                    maxValue = false;
                }

                //フラグによるスライダー値の増減
                if (maxValue)
                {
                    slider.value -= speed;
                }
                else
                {
                    slider.value += speed;
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
        veryGood.SetActive(false);
        good.SetActive(false);
        Bad.SetActive(false);
        Destroy(GameObject.Find("Slidegame"));
    }
}
