//
// プレイヤーコントロールスクリプト
// Name:西浦晃太 Date:2/8
//
using Unity.VisualScripting;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    //プレーヤーのタイプ
    public const int TYPE_RED = 1;
    public const int TYPE_BLUE = 2;
    public const int TYPE_YELLOW = 3;
    public const int TYPE_GREEN = 4;

    const float SELECT_POS_Y = 2;

    //どちらのプレイヤーか
    public int PlayerNo;
    public int Type;
    
    void OnColliderEnable()
    {
        GetComponent<BoxCollider>().center = new Vector3(0f, -2f, 0f);
        GetComponent<BoxCollider>().enabled = true;
    }

    public void OffColliderEnable()
    {
        GetComponent<BoxCollider>().center = new Vector3(0f, 100f,0f);
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
            pos = new Vector3(transform.position.x, 0.6f, transform.position.z);
        }

        transform.position = pos;
        return ret;
    }
}
