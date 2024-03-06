//
// �Q�[���f�B���N�^�[�X�N���v�g
// Name:���Y�W�� Date:02/07
// Update:03/05
//
using System;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class GameDirector : MonoBehaviour
{
    /// <summary>
    /// �^�C���z�u�ݒ�
    /// 0:Wall 1:NormalTile 2:SpawnPoint 3:Object1 4: -
    /// </summary>
    int[,] initTileData = new int[,]
    {
        {0,0,0,0,0,0,0,0,0,0,0},//��O
        {0,2,1,1,1,1,1,1,1,2,0},
        {0,1,1,1,1,1,1,1,1,1,0},
        {0,1,1,3,3,1,3,3,1,1,0},
        {0,1,1,3,1,1,1,3,1,1,0},
        {0,1,1,1,1,1,1,1,1,1,0},
        {0,1,1,3,1,1,1,3,1,1,0},
        {0,1,1,3,3,1,3,3,1,1,0},
        {0,1,1,1,1,1,1,1,1,1,0},
        {0,2,1,1,1,1,1,1,1,2,0},
        {0,0,0,0,0,0,0,0,0,0,0},
    };

    /// <summary>
    /// �v���C���[�����z�u
    /// </summary>
    int[,] initUnitData = new int[,]
    {
        {0,0,0,0,0,0,0,0,0,0,0},//��O
        {0,3,0,0,0,0,0,0,0,1,0},
        {0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0},
        {0,4,0,0,0,0,0,0,0,2,0},
        {0,0,0,0,0,0,0,0,0,0,0},
    };

    /// <summary>
    /// �Q�[�����[�h
    /// </summary>
    public enum MODE
    {
        NONE = -1,
        WAIT_TURN_START,
        MOVE_SELECT,
        POTION_THROW,
        FIELD_UPDATE,
        WAIT_TURN_END,
        TURN_CHANGE,
    }

    ///========================================
    ///
    /// �t�B�[���h
    /// 
    ///========================================

    /// <summary>
    /// �v���C���[ �z��
    /// </summary>
    Player[] player = new Player[playerNum];

    /// <summary>
    /// �|�[�V�����̎��
    /// </summary>
    PotionType potionType = new PotionType();

    /// <summary>
    /// �v���C���[�l��
    /// </summary>
    const int playerNum = 4;

    /// <summary>
    /// �t�B�[���h��̃v���C���[���X�g
    /// </summary>
    List<GameObject>[,] unitData;

    /// <summary>
    /// �I�����j�b�g
    /// </summary>
    GameObject selectUnit;
    int oldX, oldY;

    /// <summary>
    /// �v���C���[�̈ړ�����ϐ�
    /// </summary>
    bool isMoved = false;

    /// <summary>
    /// ���S�l�����Z�ϐ�
    /// </summary>
    int deadCnt;

    /// <summary>
    /// �v���C���[�^�C�v
    /// </summary>
    int playerType = UnitController.TYPE_RED;

    /// <summary>
    /// �ЂƂO�̃v���C���[�^�C�v
    /// </summary>
    int oldType;

    /// <summary>
    /// ���S����v���C���[�^�C�v
    /// </summary>
    int[] type = { 2 };

    /// <summary>
    /// ���݂̃^�[���̃v���C���[�^�C�v
    /// </summary>
    int nowPlayerType = 0;

    /// <summary>
    /// �^�C���f�[�^�\���̂̐錾
    /// </summary>
    TileData[,] tileData;

    /// <summary>
    /// ���݂̃^�[��
    /// </summary>
    int nowTurn;

    /// <summary>
    /// �J�����ړ��X�N���v�g
    /// </summary>
    MoveCameraManager cameraManager;

    /// <summary>
    /// ���݂̃��[�h
    /// </summary>
    MODE nowMode;

    /// <summary>
    /// ���̃��[�h
    /// </summary>
    public MODE nextMode;

    /// <summary>
    /// �e�|�[�V�����̃A�C�R��
    /// </summary>
    [SerializeField] GameObject[] BoomPotion;
    [SerializeField] GameObject[] BuffPotion;
    [SerializeField] GameObject[] DebuffPotion;
    [SerializeField] GameObject[] Potion;

    /// <summary>
    /// �Q�[���I���e�L�X�g
    /// </summary>
    [SerializeField] Text GameEndTXT;

    /// <summary>
    /// (��)�|�[�V���������_�������ϐ�
    /// </summary>
    System.Random r = new System.Random();

    /// <summary>
    /// �|�[�V���������N���X
    /// </summary>
    PotionBoom potionBoom;

    /// <summary>
    /// �ړ�����̃v���p�e�B
    /// </summary>
    public bool IsMoved
    { 
        get { return isMoved; }
    }

    /// <summary>
    /// ���݃v���C���[�̃^�[���ϐ��̃v���p�e�B
    /// </summary>
    public int NowPlayerType
    {
        get { return nowPlayerType; }
    }

    ///========================================
    ///
    /// ���\�b�h
    /// 
    ///========================================

    /// <summary>
    /// ����������
    /// </summary>
    void Start()
    {
        deadCnt = 0;

        for (int i = 0; i < player.Length; i++)
        { //�z�񕪂̃v���C���[�̍\���̂𐶐�
            player[i] = new Player();      
        }

        for (int i = 0; i < player.Length; i++)
        { //�e�ϐ���������
            player[i].IsThrowed= false;
            player[i].TurnClock = 0;
        }

        for (int i = 0; i < player.Length; i++)
        { //�v���C���[�����l�ݒ�
            player[i].PlayerState = PLAYERSTATE.NORMAL_STATE;
            player[i].PlayerNo = i + 1;
        }

        // �^�C���z�u��񕪂̔z��𐶐�
        tileData = new TileData[initTileData.GetLength(0), initTileData.GetLength(1)];

        // �^�C���f�[�^�ɔz�u�ʒu����
        for (int i = 0; i < initTileData.GetLength(0); i++)
        {
            for (int j = 0; j < initTileData.GetLength(1); j++)
            {
                tileData[i, j] = new TileData(initTileData[i, j]);
            }
        }

        // �^�C���f�[�^�Ƀv���C���[������
        for (int i = 0; i < initTileData.GetLength(0); i++)
        {
            for (int j = 0; j < initTileData.GetLength(1); j++)
            {
                tileData[i, j].pNo = initUnitData[i, j];
            }
        }
        unitData = new List<GameObject>[tileData.GetLength(0), tileData.GetLength(1)];

        //�^�C��������
        for (int i = 0; i < tileData.GetLength(0); i++)
        {
            for (int j = 0; j < tileData.GetLength(1); j++)
            {
                float x = j - (tileData.GetLength(1) / 2 - 0.5f);
                float z = i - (tileData.GetLength(0) / 2 - 0.5f);

                //�^�C���z�u
                string resname = "";

                int no = tileData[i, j].tNo;
                if (4 == no || 8 == no) no = 5;

                resname = "Cube (" + no + ")";

                resourcesInstantiate(resname, new Vector3(x, 0, z), Quaternion.identity);

                //�v���C���[�z�u
                unitData[i, j] = new List<GameObject>();

                //�v���C���[���ݒ�
                Vector3 angle = new Vector3(0, 0, 0);

                if (1 == tileData[i, j].pNo)
                { //1P���j�b�g�z�u
                    resname = "Unit1";
                    playerType = UnitController.TYPE_RED;
                }
                else if (2 == tileData[i, j].pNo)
                { //2P���j�b�g�z�u
                    resname = "Unit2";
                    playerType = UnitController.TYPE_BLUE;
                    angle.y = 180;        // �I�u�W�F�N�g�̌���
                }
                else if (3 == tileData[i, j].pNo)
                { //3P���j�b�g�z�u
                    resname = "Unit3";
                    playerType = UnitController.TYPE_YELLOW;
                }
                else if (4 == tileData[i, j].pNo)
                { //4P���j�b�g�z�u
                    resname = "Unit4";
                    playerType = UnitController.TYPE_GREEN;
                    angle.y = 180;
                }
                else
                {
                    resname = "";
                }

                GameObject unit = resourcesInstantiate(resname, new Vector3(x, 0.1f, z), Quaternion.Euler(angle));

                if (null != unit)
                {
                    unit.GetComponent<UnitController>().PlayerNo = initUnitData[i, j];
                    unit.GetComponent<UnitController>().Type = playerType;
                    unitData[i, j].Add(unit);
                }
            }
        }

        nowTurn = 0;
        nextMode = MODE.MOVE_SELECT;
    }

    // Update is called once per frame
    void Update()
    {
        if (deadCnt >= 3)
        {
            GameEnd();
        }

        if (nowPlayerType >= 4)
        {
            nowPlayerType = 0;
        }

        oldType = nowPlayerType - 1;

        if (oldType == -1)
        {
            oldType = 3;
        }

        if (oldType >= 4)
        {
            oldType = 0;
        }

        if (player[oldType].TurnClock >= 5)
        {
            potionBoom.BoomPotion(type);
        }

        for (int i = 0; i < player.Length; i++)
        {
            if (player[i].TurnClock >= 6)
            {
                player[i].TurnClock = 0;
            }
        }
        Mode();

        if (MODE.NONE != nextMode) InitMode(nextMode);
    }

    /// <summary>
    /// ���C�����[�h
    /// </summary>
    /// <param name="next"></param>
    void Mode()
    {
        if (MODE.MOVE_SELECT == nowMode)
        {
            SelectMode();
        }
        else if (MODE.FIELD_UPDATE == nowMode)
        {
            FieldUpdateMode();
        }
        else if (MODE.TURN_CHANGE == nowMode)
        {
            TurnChangeMode();
        }
        else if (MODE.POTION_THROW== nowMode)
        {
            ThrowPotion();
        }
    }

    /// <summary>
    /// ���̃��[�h����
    /// </summary>
    /// <param name="next"></param>
    void InitMode(MODE next)
    {
        if (MODE.WAIT_TURN_START == next)
        {
        }
        else if (MODE.MOVE_SELECT == next)
        {
            selectUnit = null;
        }
        else if (MODE.WAIT_TURN_END == next)
        {
        }
        else if (MODE.FIELD_UPDATE == next)
        {
        }
        nowMode = next;
        nextMode = MODE.NONE;
    }

    /// <summary>
    /// ���j�b�g�ړ����[�h
    /// </summary>
    void SelectMode()
    {
        if (player[nowPlayerType].PlayerState == PLAYERSTATE.FROZEN_STATE || player[nowPlayerType].PlayerState == PLAYERSTATE.CURSED_STATE || player[nowPlayerType].IsDead == true)
        { //�����Ă����ꍇ
            Debug.Log((nowPlayerType + 1) + "P�͂������Ȃ��I");
            nextMode = MODE.FIELD_UPDATE;
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            { //�N���b�N���A���j�b�g�I��
                isMoved = false;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100))
                {
                    if (null != hit.collider.gameObject)
                    {
                        Vector3 pos = hit.collider.gameObject.transform.position;

                        int x = (int)(pos.x + (tileData.GetLength(1) / 2 - 0.5f));
                        int z = (int)(pos.z + (tileData.GetLength(0) / 2 - 0.5f));

                        if (0 < unitData[z, x].Count && player[nowTurn].PlayerNo == unitData[z, x][0].GetComponent<UnitController>().PlayerNo)
                        { //���j�b�g�I��
                            if (null != selectUnit)
                            {
                                selectUnit.GetComponent<UnitController>().Select(false);
                            }
                            selectUnit = unitData[z, x][0];
                            oldX = x;
                            oldY = z;

                            selectUnit.GetComponent<UnitController>().Select();
                        }
                        else if (null != selectUnit)
                        { //�ړ���^�C���I��
                            if (movableTile(oldX, oldY, x, z))
                            {
                                isMoved = true;
                                unitData[oldY, oldX].Clear();
                                pos.y += 0.1f;
                                selectUnit.transform.position = pos;
                                unitData[z, x].Add(selectUnit);
                                unitData[z, x][0].GetComponent<UnitController>().OffColliderEnable();
                                nextMode = MODE.FIELD_UPDATE;
                            }
                            Debug.Log("���݂̃v���C���[:" + nowPlayerType);
                        }
                    }
                }
            }
        }
    }

    void FieldUpdateMode()
    {
        nextMode = MODE.TURN_CHANGE;
    }

    /// <summary>
    /// �^�[���`�F���W
    /// </summary>
    void TurnChangeMode()
    {
        nextMode = MODE.NONE;
        nextMode = MODE.MOVE_SELECT;

        int oldTurn = nowTurn;
        nowTurn = getNextTurn();

        nextMode = MODE.MOVE_SELECT;

        if (player[nowPlayerType].IsThrowed)
        { //�����Ă���J�E���g�J�n
            for (int i = 0; i < player.Length; i++)
            {
                player[nowPlayerType].TurnClock++;
            }
        }

        nowPlayerType++;
    }

    int getNextTurn()
    { //���^�[�����擾
        int ret = nowTurn;

        ret++;
        if (3 < ret) ret = 0;

        return ret;
    }

    /// <summary>
    /// ���\�[�X���I�u�W�F�N�g�z�u�֐�
    /// </summary>
    /// <param name="name">Object's Name</param>
    /// <param name="pos">Object's Position</param>
    /// <param name="angle">Object's Angle</param>
    /// <returns></returns>
    GameObject resourcesInstantiate(string name, Vector3 pos, Quaternion angle)
    {
        GameObject prefab = (GameObject)Resources.Load(name);

        if (null == prefab)
        {
            return null;
        }

        GameObject ret = Instantiate(prefab, pos, angle);
        return ret;
    }

    public bool movableTile(int oldx, int oldz, int x, int z)
    {
        bool ret = false;

        // �������擾
        int dx = Mathf.Abs(oldx - x);
        int dz = Mathf.Abs(oldz - z);

        Debug.Log("x:" + x);
        Debug.Log("z:" + z);

        // �΂ߐi�s�s��
        if (dx + dz > 2 || dx > 1 || dz > 1)
        {
            Debug.Log("�i�s�s��");
            Debug.Log("Z:" + z + " " + "X:" + x);

            return ret = false;
        }

        // �ǈȊO
        if (1 == tileData[z, x].tNo
           || 2 == tileData[z, x].tNo
           || player[nowTurn].PlayerNo * 4 == tileData[z, x].tNo)
        {
            if (0 == unitData[z, x].Count)
            { //�N�����Ȃ��}�X
                ret = true;
            }
            else
            { //�N������}�X
                if (unitData[z, x][0].GetComponent<UnitController>().PlayerNo != player[nowTurn].PlayerNo)
                { //�G�������ꍇ
                    ret = true;
                }
            }
        }
        return ret;
    }

    /// <summary>
    /// �^�[���I���{�^��
    /// </summary>
    public void TurnEnd()
    {
        nextMode = MODE.MOVE_SELECT;
    }

    /// <summary>
    /// �|�[�V���������{�^��
    /// </summary>
    public void Brewing()
    {
        int rndPotion = r.Next(4);
        if (player[nowPlayerType].PlayerState == PLAYERSTATE.PARALYSIS_STATE || player[nowPlayerType].PlayerState == PLAYERSTATE.FROZEN_STATE || player[nowPlayerType].IsDead == true)
        { //���т�Ă����ꍇ�܂��͓����Ă����ꍇ
            Debug.Log((nowPlayerType + 1) + "P�͂��т�Ă���B�|�[�V���������Ȃ��I");
        }
        else
        {
            if (player[nowPlayerType].OwnedPotionList?.Count >= 4)
            { //�g�����܂��Ă����ꍇ
                Debug.Log((nowPlayerType + 1) + "P�̃|�[�V�����g�͖��t���I�I");
            }
            else
            {
                if (rndPotion == 0)
                { //�g1
                    if (player[nowPlayerType].OwnedPotionList.Contains(TYPE.BOMB))
                    { //���łɓ����|�[�V�������������Ă����ꍇ
                        Debug.Log((nowPlayerType + 1) + "P�̃{�����łɂ����");
                    }
                    else
                    {
                        BoomPotion[nowPlayerType].SetActive(true);
                        player[nowPlayerType].OwnedPotionList.Add(TYPE.BOMB);
                    }
                }
                else if (rndPotion == 1)
                { //�g�Q
                    if (player[nowPlayerType].OwnedPotionList.Contains(TYPE.REFRESH))
                    { //���łɓ����|�[�V�������������Ă����ꍇ
                        Debug.Log((nowPlayerType + 1) + "P�̃o�t���łɂ����");
                    }
                    else
                    {
                        BuffPotion[nowPlayerType].SetActive(true);
                        player[nowPlayerType].OwnedPotionList.Add(TYPE.REFRESH);
                    }
                }
                else if (rndPotion == 2)
                { //�g�R
                    if (player[nowPlayerType].OwnedPotionList.Contains(TYPE.CURSE))
                    { //���łɓ����|�[�V�������������Ă����ꍇ
                        Debug.Log((nowPlayerType + 1) + "P�̎􂷂łɂ����");
                    }
                    else
                    {
                        DebuffPotion[nowPlayerType].SetActive(true);
                        player[nowPlayerType].OwnedPotionList.Add(TYPE.CURSE);
                    }
                }
                else if (rndPotion == 3)
                { //�g�S
                    if (player[nowPlayerType].OwnedPotionList.Contains(TYPE.NORMAL))
                    { //���łɓ����|�[�V�������������Ă����ꍇ
                        Debug.Log((nowPlayerType + 1) + "P�̃m�[�}�����łɂ����");
                    }
                    else
                    {
                        Potion[nowPlayerType].SetActive(true);
                        player[nowPlayerType].OwnedPotionList.Add(TYPE.NORMAL);
                    }
                }
            }
        }
        //nextMode = MODE.FIELD_UPDATE;
    }

    /// <summary>
    /// �|�[�V�����g�p�{�^��
    /// </summary>
    public void UsePotion(int buttonNum)
    {
        if (player[nowPlayerType].PlayerState == PLAYERSTATE.FROZEN_STATE || player[nowPlayerType].IsDead == true)
        { //�����Ă����ꍇ
            Debug.Log((nowPlayerType + 1) + "P�͓����Ă���B�|�[�V�����͎g���Ȃ��I");
        }
        else
        {
            //�g�ʔ���
            if (buttonNum == 1)
            { //1�Ԗڂ̏ꍇ
                if (player[nowPlayerType].OwnedPotionList.Contains(TYPE.BOMB))
                {
                    ThrowPotion();
                    GameObject.Find("Unit" + (nowPlayerType + 1) + "(Clone)").GetComponent<UnitController>().animator.SetBool("isThrow", true);     //���̃|�[�V�����ɂ������A�j���[�V����������
                    BoomPotion[nowPlayerType].SetActive(false);                 //�g�p�����|�[�V�����̃A�C�R��������
                    player[nowPlayerType].OwnedPotionList.Remove(TYPE.BOMB);    //�g�p�����|�[�V���������X�g����폜����
                }
                else
                {
                    Debug.Log((nowPlayerType + 1) + "P�͂܂�1�g�ڂ̃|�[�V����������ĂȂ�!!");
                }
            }
            else if (buttonNum == 2)
            { //�Q�Ԗڂ̏ꍇ
                if (player[nowPlayerType].OwnedPotionList.Contains(TYPE.CURSE))
                {
                    ThrowPotion();
                    GameObject.Find("Unit" + (nowPlayerType + 1) + "(Clone)").GetComponent<UnitController>().animator.SetBool("isThrow", true);     //���̃|�[�V�����ɂ������A�j���[�V����������
                    DebuffPotion[nowPlayerType].SetActive(false);                //�g�p�����|�[�V�����̃A�C�R��������
                    player[nowPlayerType].OwnedPotionList.Remove(TYPE.CURSE);    //�g�p�����|�[�V���������X�g����폜����
                }
                else
                {
                    Debug.Log((nowPlayerType + 1) + "P�͂܂�2�g�ڂ̃|�[�V����������ĂȂ�!!");
                }
            }
            else if (buttonNum == 3)
            { //�R�Ԗڂ̏ꍇ
                if (player[nowPlayerType].OwnedPotionList.Contains(TYPE.REFRESH))
                {
                    ThrowPotion();
                    GameObject.Find("Unit" + (nowPlayerType + 1) + "(Clone)").GetComponent<UnitController>().animator.SetBool("isThrow", true);     //���̃|�[�V�����ɂ������A�j���[�V����������
                    BuffPotion[nowPlayerType].SetActive(false);                    //�g�p�����|�[�V�����̃A�C�R��������
                    player[nowPlayerType].OwnedPotionList.Remove(TYPE.REFRESH);    //�g�p�����|�[�V���������X�g����폜����
                }
                else
                {
                    Debug.Log((nowPlayerType + 1) + "P�͂܂�3�g�ڂ̃|�[�V����������ĂȂ�!!");
                }
            }
            else if (buttonNum == 4)
            { //�S�Ԗڂ̏ꍇ
                if (player[nowPlayerType].OwnedPotionList.Contains(TYPE.NORMAL))
                {
                    ThrowPotion();
                    GameObject.Find("Unit" + (nowPlayerType + 1) + "(Clone)").GetComponent<UnitController>().animator.SetBool("isThrow", true);     //���̃|�[�V�����ɂ������A�j���[�V����������
                    Potion[nowPlayerType].SetActive(false);                       //�g�p�����|�[�V�����̃A�C�R�������� 
                    player[nowPlayerType].OwnedPotionList.Remove(TYPE.NORMAL);    //�g�p�����|�[�V���������X�g����폜����
                }
                else
                {
                    Debug.Log((nowPlayerType + 1) + "P�͂܂��S�g�ڂ̃|�[�V����������ĂȂ�!!");
                }
            }
        }
    }

    /// <summary>
    /// �w�胆�j�b�g�j�󏈗�
    /// </summary>
    /// <param name="unitType"></param>
    public void DestroyUnit(int unitType)
    {
        GameObject Unit;
        for (int i = 0; i < unitData.GetLength(0); i++)
        {
            for (int j = 0; j < unitData.GetLength(1); j++)
            {
                if (unitData[i, j].Count > 0)
                {
                    Unit = unitData[i, j][0];

                    if (Unit.GetComponent<UnitController>().Type == unitType)
                    { //���Y���j�b�g���E��
                        Destroy(Unit);
                        player[unitType].IsDead = true;
                        deadCnt++;
                        return;
                    }
                }
            }
        }
    }

    /// <summary>
    /// �o�t����
    /// </summary>
    /// <param name="unitType"></param>
    public void BuffUnit(int unitType, TYPE buffType)
    {
        GameObject Unit;
        for (int i = 0; i < unitData.GetLength(0); i++)
        {
            for (int j = 0; j < unitData.GetLength(1); j++)
            {
                if (unitData[i, j].Count > 0)
                {
                    Unit = unitData[i, j][0];

                    if (Unit.GetComponent<UnitController>().Type == unitType)
                    {
                        switch (buffType)
                        { //�o�t�|�[�V�����ʏ���
                            case TYPE.REFRESH: //���t���b�V���|�[�V�����̏���
                                player[unitType].PlayerState = PLAYERSTATE.NORMAL_STATE;
                                break;
                            case TYPE.INVISIBLE: //���G�|�[�V�����̏���
                                player[unitType].PlayerState = PLAYERSTATE.INVICIBLE_STATE;
                                break;
                            case TYPE.MUSCLE: //�ؗ̓|�[�V�����̏���
                                player[unitType].PlayerState = PLAYERSTATE.MUSCLE_STATE;
                                break;
                        }
                        return;
                    }
                }
            }
        }
    }

    /// <summary>
    /// �f�o�t����
    /// </summary>
    /// <param name="unitType"></param>
    public void DebuffUnit(int unitType, TYPE debuffType)
    {
        GameObject Unit;
        for (int i = 0; i < unitData.GetLength(0); i++)
        {
            for (int j = 0; j < unitData.GetLength(1); j++)
            {
                if (unitData[i, j].Count > 0)
                {
                    Unit = unitData[i, j][0];

                    if (Unit.GetComponent<UnitController>().Type == unitType)
                    {
                        switch (debuffType)
                        { //�o�t�|�[�V�����ʏ���
                            case TYPE.SOUR: //���X�b�p�C�|�[�V�����̏���
                                GameObject.Find("Unit" + (nowPlayerType + 1) + "(Clone)").GetComponent<UnitController>().animator.SetBool("isParalysis", true);
                                player[unitType].PlayerState = PLAYERSTATE.PARALYSIS_STATE;
                                break;

                            case TYPE.CURSE: //�r�l�߂̎􂢂̏���
                                GameObject.Find("Unit" + (nowPlayerType + 1) + "(Clone)").GetComponent<UnitController>().animator.SetBool("isCurse", true);
                                player[unitType].PlayerState = PLAYERSTATE.CURSED_STATE;
                                break;

                            case TYPE.ICE: //�A�C�X�|�[�V�����̏���
                                GameObject.Find("Unit" + (nowPlayerType + 1) + "(Clone)").GetComponent<UnitController>().animator.SetBool("isFrost", true);
                                player[unitType].PlayerState = PLAYERSTATE.FROZEN_STATE;
                                break;
                        }
                        return;
                    }
                }
            }
        }
    }

    /// <summary>
    /// �|�[�V������������
    /// </summary>
    void ThrowPotion()
    {
        nowMode = MODE.POTION_THROW;
        string resname = "BombPotion";
        Vector3 pos = SerchUnit((nowPlayerType + 1));

        int x = (int)(pos.x + (tileData.GetLength(1) / 2 - 0.5f));
        int z = (int)(pos.z + (tileData.GetLength(0) / 2 - 0.5f));

        unitData[z, x][0].GetComponent<UnitController>().ThrowSelect();
        unitData[z, x][0].GetComponent<UnitController>().OnThrowColliderEnable();

        if (Input.GetMouseButtonDown(0))
        { //�N���b�N���A�����ʒu��ݒ�
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                if (null != hit.collider.gameObject)
                {
                    Vector3 selectPos = hit.collider.gameObject.transform.position;

                    int selectX = (int)(selectPos.x + (tileData.GetLength(1) / 2 - 0.5f));
                    int selectZ = (int)(selectPos.z + (tileData.GetLength(0) / 2 - 0.5f));

                    resourcesInstantiate(resname, selectPos, Quaternion.Euler(0,0,0));
                    unitData[z, x][0].GetComponent<UnitController>().OffThrowColliderEnable();
                    player[nowPlayerType].IsThrowed= true;
                    potionBoom = GameObject.FindWithTag("Potion").GetComponent<PotionBoom>();
                    nextMode = MODE.FIELD_UPDATE;
                }
            }
        }
    }

    /// <summary>
    /// ���j�b�g�w�菈��
    /// </summary>
    /// <param name="unitType"></param>
    public Vector3 SerchUnit(int unitType)
    {
        GameObject Unit;
        if (player[nowPlayerType].IsDead)
        {
            return new Vector3(0, 0, 0);
        }
        else
        {
            for (int i = 0; i < unitData.GetLength(0); i++)
            {
                for (int j = 0; j < unitData.GetLength(1); j++)
                {
                    if (unitData[i, j].Count > 0)
                    {
                        Unit = unitData[i, j][0];

                        if (Unit.GetComponent<UnitController>().Type == unitType)
                        {
                            Vector3 pos = Unit.transform.position;
                            return pos;
                        }
                    }
                }
            }
        }
        return new Vector3(0, 0, 0);
    }
    /// <summary>
    /// �Q�[���I���֐�
    /// </summary>
    void GameEnd()
    {
        int wonPlayer = 0;  //���҂̃v���C���[�ԍ�����p�ϐ�
        for(int i = 0; i<4; i++)
        {�@//���҂̃v���C���[�i���o�[���擾
            if (player[i].IsDead ==false)
            { //�����c�����v���C���[�i���o�[����
                wonPlayer = (i + 1);
            }
            else
            { //�S�����S���Ă����ꍇ
                wonPlayer = 0;
            }
        }

        if (wonPlayer == 0)
        { //�S�����S���Ă����ꍇ
            GameEndTXT.GetComponent<Text>().text = "��������";
        }
        else
        { //���҂̃v���C���[�i���o�[�𔽉f
            GameEndTXT.GetComponent<Text>().text = wonPlayer.ToString() + "P�̏���!!!";
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Initiate.Fade("Result", Color.white, 0.7f);
        }
    }
}
