//
// プレイヤーコントロールスクリプト
// Name:西浦晃太 Date:2/8
// Update:03/05
//
using Unity.VisualScripting;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    /// <summary>
    /// ポーション投擲範囲用ゲームオブジェクト
    /// </summary>
    [SerializeField] GameObject colliderObj;

    /// <summary>
    /// プレーヤーのタイプ
    /// </summary>
    public const int TYPE_RED = 1;
    public const int TYPE_BLUE = 2;
    public const int TYPE_YELLOW = 3;
    public const int TYPE_GREEN = 4;

    const float SELECT_POS_Y = 2;

    /// <summary>
    /// どちらのプレイヤーか
    /// </summary>
    public int PlayerNo;
    public int Type;

    /// <summary>
    /// アニメータ
    /// </summary>
    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// 移動範囲表示関数
    /// </summary>
    void OnColliderEnable()
    {
        GetComponent<BoxCollider>().center = new Vector3(0f, -2f, 0f);
        GetComponent<BoxCollider>().enabled = true;
    }

    /// <summary>
    /// 投擲範囲表示関数
    /// </summary>
    public void OnThrowColliderEnable()
    {
        colliderObj.GetComponent<BoxCollider>().center = new Vector3(0f, 0f, 0f);
        colliderObj.GetComponent<BoxCollider>().enabled = true;
    }

    /// <summary>
    /// 移動範囲非表示関数
    /// </summary>
    public void OffColliderEnable()
    {
        GetComponent<BoxCollider>().center = new Vector3(0f, 100f,0f);
    }

    /// <summary>
    /// 投擲範囲非表示関数
    /// </summary>
    public void OffThrowColliderEnable()
    {
        colliderObj.GetComponent<BoxCollider>().center = new Vector3(0f, 100f, 0f);
    }

    /// <summary>
    /// 選択時の動作
    /// </summary>
    /// <param name="select">選択 or 非選択</param>
    /// <returns>アニメーション秒数</returns>
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
