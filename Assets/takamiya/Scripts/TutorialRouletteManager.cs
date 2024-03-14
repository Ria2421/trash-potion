//==============================================
//Autor:三宅歩人
//Day:2/28
//ルーレット処理
//==============================================
using UnityEngine;
using UnityEngine.UI;

public class TutorialRouletteManager : MonoBehaviour
{
    public float rouletteSpeed = 0;        //回転速度
    public GameObject verygood;              //veryGoodテキストの指定
    public GameObject good;                 //goodテキスト指定
    public GameObject bad;                 //Badテキスト指定
    public GameObject roulette;           //ルーレット本体
    public Text timerText;
    public float angle = 0;              //回転の角度の変数
    public bool endCountDown;
    public Text limitTime;              //ミニゲームの制限時間
    int limit;                         //制限時間の変数
    bool isLimit;                     //制限時間を超えたかどうか
    private bool isClicked;
    [SerializeField] AudioClip veryGoodSE;      //大成功SE
    [SerializeField] AudioClip goodSE;          //成功SE
    [SerializeField] AudioClip badSE;           //失敗SE
    [SerializeField] AudioSource audioSource;

    StartMiniGame tutorialMiniGame;

    // Start is called before the first frame update
    void Start()
    {
        tutorialMiniGame = GameObject.Find("MiniGameManager").GetComponent<StartMiniGame>();
        Init();
    }
    public void Init()
    {
        //フレームレートを60に固定
        Application.targetFrameRate = 60;
        endCountDown = false;
        limit = 5;
        limitTime.enabled = true;
        isLimit = false;
        isClicked = false;
        verygood.SetActive(false);//大成功テキストを非表示にする
        good.SetActive(false);//成功テキストを非表示にする
        bad.SetActive(false);//失敗テキストを非表示にする
        //1秒ごとに関数を実行
        //InvokeRepeating("CountDownTimer", 3.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(isClicked ==true)
        {
            return;
        }
        if (!isLimit)
        {
            if (endCountDown)
            {
                //ルーレットを回転
                transform.Rotate(0, 0, rouletteSpeed);

                if (Input.GetMouseButtonDown(0))
                {//左クリックされたら
                    isClicked = true;
                    //rouletteSpeed = 0;
                    Judge();
                    CancelInvoke();
                    tutorialMiniGame.BackTitleButton.SetActive(true);//タイトルに戻るボタンを表示する
                    tutorialMiniGame.AgainButton.SetActive(true);//もう一度ボタンを表示する
                }
            }
        }
    }

    //判定処理
    void Judge()
    {
        angle = roulette.transform.eulerAngles.z;
        float angleA = (155 + angle) % 360;     //大成功の端
        float angleB = (183 + angle) % 360;     //大成功の端
        float angleC = (110 + angle) % 360;     //成功の端
        float angleD = (229 + angle) % 360;     //成功の端

        //大成功
        if (angleA > angleB)
        {//360度を超えていたら
            if ((angleA <= transform.eulerAngles.z && transform.eulerAngles.z <= 360) || (0 <= transform.eulerAngles.z && transform.eulerAngles.z <= angleB))
            {
                //大成功SE
                audioSource.PlayOneShot(veryGoodSE);

                verygood.SetActive(true);

                return;
            }
        }
        else
        {
            if (transform.eulerAngles.z >= angleA && transform.eulerAngles.z <= angleB)
            {
                //大成功SE
                audioSource.PlayOneShot(veryGoodSE);

                verygood.SetActive(true);
                return;
            }
        }

        //成功
        if (angleC > angleD)
        {
            if ((angleC <= transform.eulerAngles.z && transform.eulerAngles.z <= 360) || (0 <= transform.eulerAngles.z && transform.eulerAngles.z <= angleD))
            {
                //成功SE
                audioSource.PlayOneShot(goodSE);

                good.SetActive(true);
                return;
            }
        }
        else
        {
            if (transform.eulerAngles.z >= angleC && transform.eulerAngles.z <= angleD)
            {
                //成功SE
                audioSource.PlayOneShot(goodSE);

                good.SetActive(true);
                return;
            }
        }

        // 範囲外はすべて失敗

        //失敗SE
        audioSource.PlayOneShot(badSE);

        bad.SetActive(true);
    }

    void CountDownTimer()
    {
        limit--;
        limitTime.text = limit.ToString();
        if (limitTime.text == "-1")
        {
            bad.SetActive(true);
            isLimit = true;
            CancelInvoke();
        }
    }
}
