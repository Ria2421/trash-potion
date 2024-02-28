//
// カメラ遷移スクリプト
// Name:西浦晃太 Date:2/15
//
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
public class MoveCameraManager : MonoBehaviour
{
    //カメラ
    public CinemachineVirtualCameraBase vcam1;
    public CinemachineVirtualCameraBase vcam2;
    public CinemachineVirtualCameraBase vcam3;
    public CinemachineVirtualCameraBase vcam4;
    public CinemachineVirtualCameraBase vcam5;

    //アイコンのゲームオブジェクト
    GameObject Icon1P;
    GameObject Icon2P;
    GameObject Icon3P;
    GameObject Icon4P;

    //フレームのゲームオブジェクト
    public GameObject Flame1;
    public GameObject Flame2;
    public GameObject Flame3;
    public GameObject Flame4;

    //ボタンのゲームオブジェクト
    GameObject moveButton;
    GameObject brewingButton;

    GameDirector gameDirector;

    [SerializeField] Text turnCnt;
    int cameraShift = 0;
    bool upCamera = false;
    int tabCnt;
    int moveCnt = 0;
    int turnNum = 1;

    // Start is called before the first frame update
    void Start()
    {
        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();

        //アイコンのゲームオブジェクトを取得
        Icon1P = GameObject.Find("1PIcon");
        Icon2P = GameObject.Find("2PIcon");
        Icon3P = GameObject.Find("3PIcon");
        Icon4P = GameObject.Find("4PIcon");

        //ポーションフレームのゲームオブジェクトを取得
        Flame1 = GameObject.Find("1PFlames");
        Flame2 = GameObject.Find("2PFlames");
        Flame3 = GameObject.Find("3PFlames");
        Flame4 = GameObject.Find("4PFlames"); 

        //ボタンのゲームオブジェクトを取得
        //moveButton = GameObject.Find("MoveButton");
        brewingButton = GameObject.Find("BrewingButton");

        //カメラ優先度の初期値を設定
        vcam1.Priority = 1;
        vcam2.Priority = 0;
        vcam3.Priority = 0;
        vcam4.Priority = 0;
        vcam5.Priority = 0;

        //各種ゲームオブジェクトの初期設定
        //Icon1P.SetActive(false);
        //Icon2P.SetActive(false);
        //Icon3P.SetActive(false);
        //Icon4P.SetActive(false);
        //Flame1.SetActive(false);
        //Flame2.SetActive(false);
        //Flame3.SetActive(false);
        //Flame4.SetActive(false);
        //moveButton.SetActive(true);
        //brewingButton.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //現在のラウンド数を表示
        turnCnt.GetComponent<Text>().text = "ラウンド" + turnNum.ToString();

        if (Input.GetKeyDown(KeyCode.Tab))
        { //TABを押した際、カメラを俯瞰に
            if (tabCnt == 0)
            {
                upCamera = true;
                tabCnt = 1;
                //Icon1P.SetActive(true);
                //Icon2P.SetActive(true);
                //Icon3P.SetActive(true);
                //Icon4P.SetActive(true);
                //Flame1.SetActive(true);
                //Flame2.SetActive(true);
                //Flame3.SetActive(true);
                //Flame4.SetActive(true);
                //moveButton.SetActive(false);
                //brewingButton.SetActive(false);
            }
            else if (tabCnt == 1)
            {
                if (gameDirector.IsMoved == true)
                {
                    upCamera = false;
                    tabCnt = 0;
                    //Icon1P.SetActive(false);
                    //Icon2P.SetActive(false);
                    //Icon3P.SetActive(false);
                    //Icon4P.SetActive(false);
                    //Flame1.SetActive(false);
                    //Flame2.SetActive(false);
                    //Flame3.SetActive(false);
                    //Flame4.SetActive(false);
                    //moveButton.SetActive(true);
                    //brewingButton.SetActive(true);
                }
            }
            else
            {
                tabCnt = 0;
            }
        }

        if (cameraShift > 3)
        {
            cameraShift = 0;
            turnNum++;
        }

        if (cameraShift == 0)
        { //カメラの優先度を変更
            vcam1.Priority = 1;
            vcam2.Priority = 0;
            vcam3.Priority = 0;
            vcam4.Priority = 0;
            vcam5.Priority = 0;
        }
        else if (cameraShift == 1)
        {
            vcam1.Priority = 0;
            vcam2.Priority = 1;
            vcam3.Priority = 0;
            vcam4.Priority = 0;
            vcam5.Priority = 0;
        }
        else if (cameraShift == 2)
        {
            vcam1.Priority = 0;
            vcam2.Priority = 0;
            vcam3.Priority = 1;
            vcam4.Priority = 0;
            vcam5.Priority = 0;
        }
        else if (cameraShift == 3)
        {
            vcam1.Priority = 0;
            vcam2.Priority = 0;
            vcam3.Priority = 0;
            vcam4.Priority = 1;
            vcam5.Priority = 0;
        }

        if (upCamera == true)
        {
            vcam1.Priority = 0;
            vcam2.Priority = 0;
            vcam3.Priority = 0;
            vcam4.Priority = 0;
            vcam5.Priority = 1;
        }
    }

    //public void MoveButton()
    //{ //プレイヤー間のカメラ移動ボタン

    //    if (moveCnt == 0)
    //    {
    //        upCamera = true;
    //        moveCnt = 1;
    //        Icon1P.SetActive(true);
    //        Icon2P.SetActive(true);
    //        Icon3P.SetActive(true);
    //        Icon4P.SetActive(true);
    //        Flame1.SetActive(true);
    //        Flame2.SetActive(true);
    //        Flame3.SetActive(true);
    //        Flame4.SetActive(true);
    //        moveButton.SetActive(false);
    //        brewingButton.SetActive(false);
    //    }
    //    else if (moveCnt == 1)
    //    {
    //        upCamera = false;
    //        moveCnt = 0;
    //        cameraShift++;
    //        Icon1P.SetActive(false);
    //        Icon2P.SetActive(false);
    //        Icon3P.SetActive(false);
    //        Icon4P.SetActive(false);
    //        Flame1.SetActive(false);
    //        Flame2.SetActive(false);
    //        Flame3.SetActive(false);
    //        Flame4.SetActive(false);
    //        moveButton.SetActive(true);
    //        brewingButton.SetActive(true);
    //    }
    //}

    //public void CameraShift()
    //{
    //    cameraShift++;
    //}
}