//
// ƒJƒƒ‰‘JˆÚƒXƒNƒŠƒvƒg
// Name:¼‰YW‘¾ Date:2/15
//
using UnityEngine;
using Cinemachine;
public class MoveCameraManager : MonoBehaviour
{
    public CinemachineVirtualCameraBase vcam1;
    public CinemachineVirtualCameraBase vcam2;
    public CinemachineVirtualCameraBase vcam3;
    public CinemachineVirtualCameraBase vcam4;
    public CinemachineVirtualCameraBase vcam5;
    int cameraShift = 0;
    bool upCamera =false;
    int tabCnt;

    // Start is called before the first frame update
    void Start()
    {
        vcam1.Priority = 1;
        vcam2.Priority = 0;
        vcam3.Priority = 0;
        vcam4.Priority = 0;
        vcam5.Priority = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            upCamera = false;
            cameraShift++;
            if (cameraShift > 3) cameraShift = 0;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            upCamera = false;
            cameraShift--;
            if (cameraShift < 0) cameraShift = 3;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (tabCnt == 0)
            {
                upCamera = true;
                tabCnt = 1;
            }
            else if(tabCnt == 1)
            {
                upCamera = false;
                tabCnt = 0;
            } 
            else
            {
                tabCnt = 0;
            }
        }

        if (cameraShift == 0)
        { // ƒJƒƒ‰‚Ì—Dæ“x‚ð•ÏX
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
        
        if(upCamera == true) 
        {
            vcam1.Priority = 0;
            vcam2.Priority = 0;
            vcam3.Priority = 0;
            vcam4.Priority = 0;
            vcam5.Priority = 1;
        }
    }

    void MoveButton()
    {
        upCamera = true;
    }
}