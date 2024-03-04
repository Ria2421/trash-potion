//
// カメラ遷移スクリプト
// Name:西浦晃太 Date:2/15
// Update:02/29
//
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
public class MoveCameraManager : MonoBehaviour
{
    /// <summary>
    /// 各プレイヤーTPSカメラ
    /// </summary>
    public CinemachineVirtualCameraBase vcam1;
    public CinemachineVirtualCameraBase vcam2;
    public CinemachineVirtualCameraBase vcam3;
    public CinemachineVirtualCameraBase vcam4;
    public CinemachineVirtualCameraBase vcam5;

    /// <summary>
    /// フレームのゲームオブジェクト
    /// </summary>
    public GameObject Frame1;
    public GameObject Frame2;
    public GameObject Frame3;
    public GameObject Frame4;

    /// <summary>
    /// ボタンのゲームオブジェクト
    /// </summary>
    GameObject moveButton;
    GameObject brewingButton;

    GameDirector gameDirector;

    [SerializeField] Text turnCnt;
    int cameraShift = 0;
    bool upCamera = false;
    int moveCnt = 0;
    int turnNum = 1;

    // Start is called before the first frame update
    void Start()
    {
        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();

        //ポーションフレームのゲームオブジェクトを取得
        Frame1 = GameObject.Find("1PFrames");
        Frame2 = GameObject.Find("2PFrames");
        Frame3 = GameObject.Find("3PFrames");
        Frame4 = GameObject.Find("4PFrames"); 

        //ボタンのゲームオブジェクトを取得
        //moveButton = GameObject.Find("MoveButton");
        brewingButton = GameObject.Find("BrewingButton");

        //カメラ優先度の初期値を設定
        vcam1.Priority = 1;
        vcam2.Priority = 0;
        vcam3.Priority = 0;
        vcam4.Priority = 0;
        vcam5.Priority = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //現在のラウンド数を表示
        turnCnt.GetComponent<Text>().text = "ラウンド" + turnNum.ToString();

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
}