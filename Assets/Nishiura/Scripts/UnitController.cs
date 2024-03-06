//
// �v���C���[�R���g���[���X�N���v�g
// Name:���Y�W�� Date:2/8
// Update:03/05
//
using Unity.VisualScripting;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    /// <summary>
    /// �|�[�V���������͈͗p�Q�[���I�u�W�F�N�g
    /// </summary>
    [SerializeField] GameObject colliderObj;

    /// <summary>
    /// �v���[���[�̃^�C�v
    /// </summary>
    public const int TYPE_RED = 1;
    public const int TYPE_BLUE = 2;
    public const int TYPE_YELLOW = 3;
    public const int TYPE_GREEN = 4;

    const float SELECT_POS_Y = 2;

    /// <summary>
    /// �ǂ���̃v���C���[��
    /// </summary>
    public int PlayerNo;
    public int Type;

    /// <summary>
    /// �A�j���[�^
    /// </summary>
    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// �ړ��͈͕\���֐�
    /// </summary>
    void OnColliderEnable()
    {
        GetComponent<BoxCollider>().center = new Vector3(0f, -2f, 0f);
        GetComponent<BoxCollider>().enabled = true;
    }

    /// <summary>
    /// �����͈͕\���֐�
    /// </summary>
    public void OnThrowColliderEnable()
    {
        colliderObj.GetComponent<BoxCollider>().center = new Vector3(0f, 0f, 0f);
        colliderObj.GetComponent<BoxCollider>().enabled = true;
    }

    /// <summary>
    /// �ړ��͈͔�\���֐�
    /// </summary>
    public void OffColliderEnable()
    {
        GetComponent<BoxCollider>().center = new Vector3(0f, 100f,0f);
    }

    /// <summary>
    /// �����͈͔�\���֐�
    /// </summary>
    public void OffThrowColliderEnable()
    {
        colliderObj.GetComponent<BoxCollider>().center = new Vector3(0f, 100f, 0f);
    }

    /// <summary>
    /// �I�����̓���
    /// </summary>
    /// <param name="select">�I�� or ��I��</param>
    /// <returns>�A�j���[�V�����b��</returns>
    public float Select(bool select =true)
    {
        float ret = 0;
        Vector3 pos = new Vector3(transform.position.x, SELECT_POS_Y, transform.position.z);
        OnColliderEnable();

        if (!select)
        {
            pos = new Vector3(transform.position.x, 0.1f, transform.position.z);
        }

        transform.position = pos;
        return ret;
    }

    public float ThrowSelect(bool select =true)
    {
        float ret = 0;
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        OnThrowColliderEnable();

        if (!select)
        {
            pos = new Vector3(transform.position.x, 0.1f, transform.position.z);
        }

        transform.position = pos;
        return ret;
    }
}
