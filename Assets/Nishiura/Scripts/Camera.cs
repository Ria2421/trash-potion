//
// �J�����J�ڃX�N���v�g
// Name:���Y�W�� Date:2/15
// Update:03/05
//
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
public class MoveCameraManager : MonoBehaviour
{
    /// <summary>
    /// �e�v���C���[TPS�J����
    /// </summary>
    public CinemachineVirtualCameraBase vcam1;
    public CinemachineVirtualCameraBase vcam2;
    public CinemachineVirtualCameraBase vcam3;
    public CinemachineVirtualCameraBase vcam4;
    public CinemachineVirtualCameraBase vcam5;

    /// <summary>
    /// �t���[���̃Q�[���I�u�W�F�N�g
    /// </summary>
    public GameObject Frame1;
    public GameObject Frame2;
    public GameObject Frame3;
    public GameObject Frame4;

    /// <summary>
    /// �{�^���̃Q�[���I�u�W�F�N�g
    /// </summary>
    GameObject moveButton;
    GameObject brewingButton;

    GameDirector gameDirector;

    [SerializeField] Text turnCnt;
    int cameraShift = 0;
    bool upCamera = false;
    int turnNum = 1;

    // Start is called before the first frame update
    void Start()
    {
        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();

        //�|�[�V�����t���[���̃Q�[���I�u�W�F�N�g���擾
        Frame1 = GameObject.Find("1PFrames");
        Frame2 = GameObject.Find("2PFrames");
        Frame3 = GameObject.Find("3PFrames");
        Frame4 = GameObject.Find("4PFrames"); 

        //�{�^���̃Q�[���I�u�W�F�N�g���擾
        //moveButton = GameObject.Find("MoveButton");
        brewingButton = GameObject.Find("BrewingButton");

        //�J�����D��x�̏����l��ݒ�
        vcam1.Priority = 1;
        vcam2.Priority = 0;
        vcam3.Priority = 0;
        vcam4.Priority = 0;
        vcam5.Priority = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //���݂̃��E���h����\��
        turnCnt.GetComponent<Text>().text = "���E���h" + turnNum.ToString();

        if (cameraShift > 3)
        {
            cameraShift = 0;
            turnNum++;
        }

        if (cameraShift == 0)
        { //�J�����̗D��x��ύX
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