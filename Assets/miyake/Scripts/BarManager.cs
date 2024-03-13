//==============================================
//Autor:三宅歩人
//Day:3/5
//スライダー（バー）ゲーム処理
//==============================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarManager : MonoBehaviour
{
    [SerializeField] float speed;
    public GameObject good;
    public GameObject veryGood;
    public GameObject Bad;
    public Slider slider;
    public Text timerText;
    bool maxValue;
    bool isClicked;
    bool endCountDown;
    NetworkManager networkManager;
    bool gameFlag;
    [SerializeField] AudioClip veryGoodSE;      //大成功SE
    [SerializeField] AudioClip goodSE;          //成功SE
    [SerializeField] AudioClip badSE;           //失敗SE
    [SerializeField] AudioSource audioSource;

    void Start()
    {
        slider.value = 0;
        maxValue = false;
        isClicked = false;
        endCountDown = false;
        gameFlag = false;

        // ネットワークマネージャーの取得
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }

    void Update()
    {
        if (timerText.text == "GO!!")
        {
            endCountDown = true;
        }

        if (endCountDown)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isClicked = true;

                if (!gameFlag)
                {
                    if (slider.value >= 85)
                    {   // 大成功
                        veryGood.SetActive(true);
                        //大成功SE
                        audioSource.PlayOneShot(veryGoodSE);
                        // 生成情報の送信
                        networkManager.SendPotionStatus((int)EventID.PotionComplete);
                        gameFlag = true;
                    }
                    else if (slider.value >= 50)
                    {   // 成功
                        good.SetActive(true);
                        //成功SE
                        audioSource.PlayOneShot(goodSE);
                        // 生成情報の送信
                        networkManager.SendPotionStatus((int)EventID.PotionComplete);
                        gameFlag = true;
                    }
                    else if (slider.value < 50)
                    {   // 失敗
                        Bad.SetActive(true);
                        //失敗SE
                        audioSource.PlayOneShot(badSE);
                        // 失敗情報の送信
                        networkManager.SendPotionStatus((int)EventID.PotionFailure);
                        gameFlag = true;
                    }
                }

                // ミニゲームの終了
                Invoke("MiniGameDestroy", 1.5f);
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
    private void MiniGameDestroy()
    {
        // ミニゲームを終了
        Destroy(GameObject.Find("MiniGames(Clone)"));
    }
}
