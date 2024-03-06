//
// �|�[�V���������X�N���v�g
// Name:���Y�W�� Date:02/26
// Update:03/05
//
using UnityEngine;
using static TMPro.Examples.ObjectSpin;

public class PotionBoom : MonoBehaviour
{
    /// <summary>
    /// �����G�t�F�N�g�̃v���n�u
    /// </summary>
    public GameObject explosionPrefab; 

    /// <summary>
    /// �Q�[���f�B���N�^�[
    /// </summary>
    GameDirector gameDirector;

    /// <summary>
    /// ���S����v���C���[�^�C�v
    /// </summary>
    int[] type = {2};

    /// <summary>
    /// �|�[�V�����̎��
    /// </summary>
    PotionType potionType;

    /// <summary>
    /// �����܂ł̃J�E���g�_�E���I�I�I
    /// </summary>
    int bombClock = 5;

    /// <summary>
    /// �ЂƂO�̃v���C���[�^�C�v
    /// </summary>
    int oldType;

    /// <summary>
    /// �v���C���[�z��
    /// </summary>
    Player[] player = new Player[4];

    void Start()
    {
        oldType = 0;
        potionType = new PotionType();

        for (int i = 0; i < player.Length; i++)
        { //�z�񕪂̃v���C���[�̍\���̂𐶐�
            player[i] = new Player();
        }

        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
    }

    void Update()
    {
        //oldType = gameDirector.NowPlayerType - 1;

        //if (oldType == -1)
        //{
        //    oldType = 3;
        //}

        //if (oldType >= 4)
        //{
        //    oldType = 0;
        //}


        //if (player[oldType].TurnClock >= bombClock)
        //{
        //    BoomPotion(type);
        //}

        //for (int i = 0; i < player.Length; i++)
        //{
        //    if (player[i].TurnClock >= 6)
        //    {
        //        player[i].TurnClock = 0;
        //    }
        //}
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="unitType"></param>
    public void BoomPotion(int[] unitType)
    { 
        GameObject explosion = Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);
        explosion.transform.position += new Vector3(0f, 0.5f, 0f);

        this.gameObject.transform.position += new Vector3(0f, -50f, 0f);

        Invoke("PotionKill",0.2f);

        for (int i = 0; i < unitType.Length; i++)
        {
            gameDirector.DestroyUnit(unitType[i]);
        }

        //for (int i = 0; i < unitType.Length; i++)
        //{
        //    switch(potionType.PotionTypes)
        //    { //�|�[�V�����ʏ���
        //        case TYPE.BOMB:     //�{���̏ꍇ
        //            gameDirector.DestroyUnit(unitType[i]);
        //            break;

        //        case TYPE.CRUSTER:  //�N���X�^�[�̏ꍇ
        //            gameDirector.DestroyUnit(unitType[i]);
        //            break;

        //        case TYPE.REFRESH:   //���t���b�V���̏ꍇ
        //            gameDirector.BuffUnit(unitType[i],TYPE.REFRESH);
        //            break;

        //        case TYPE.INVISIBLE: //���G�̏ꍇ
        //            gameDirector.BuffUnit(unitType[i],TYPE.INVISIBLE);
        //            break;

        //        case TYPE.MUSCLE:   //�ؗ͂̏ꍇ
        //            gameDirector.BuffUnit(unitType[i],TYPE.MUSCLE);
        //            break;

        //        case TYPE.ICE:      //�A�C�X�̏ꍇ
        //            gameDirector.DebuffUnit(unitType[i],TYPE.ICE);
        //            break;

        //        case TYPE.CURSE:    //�􂢂̏ꍇ
        //            gameDirector.DebuffUnit(unitType[i],TYPE.CURSE);
        //            break;

        //        case TYPE.SOUR:     //�X�b�p�C�ꍇ
        //            gameDirector.DebuffUnit(unitType[i],TYPE.SOUR);
        //            break;

        //        default:
        //            break;
        //    }
        //}
    }

    /// <summary>
    /// �|�[�V�����I�u�W�F�N�g�j�󏈗�
    /// </summary>
    void PotionKill()
    {
        //�|�[�V������j��
        Destroy(this.gameObject);
    }
}