//==============================================
//Autor:�O����l
//Day:2/29
//���[���b�g��]����
//==============================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialStartRoulette : MonoBehaviour
{
    public Text timerText;
    float randAngle = 0;        //�����_���ŉ�]����p�x�̕ϐ�
    bool endCountDown;

    // Start is called before the first frame update
    void Start()
    {
        endCountDown = false;

        if (endCountDown)
        {
            randAngle = Random.Range(-180, 180);

            transform.eulerAngles = new Vector3(0, 0, randAngle);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}