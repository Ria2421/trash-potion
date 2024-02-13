//
// プレイヤーコントロールスクリプト
// Name:西浦晃太 Date:2/8
//
using UnityEngine;

public class UnitControlle : MonoBehaviour
{
    //プレーヤーのタイプ
    public const int TYPE_BLUE = 1;
    public const int TYPE_RED = 2;

    const float SELECT_POS_Y = 2;

    //どちらのプレイヤーか
    public int PlayerNo;
    public int Type;

    /// <summary>
    /// 選択時の動作
    /// </summary>
    /// <param name="select">選択 or 非選択</param>
    /// <returns>アニメーション秒数</returns>
    public float Select(bool select = true)
    {
        float ret = 0;
        Vector3 pos = new Vector3(transform.position.x, SELECT_POS_Y, transform.position.z);

        if (!select)
        {
            pos = new Vector3(transform.position.x, 0.6f, transform.position.z);
        }

        transform.position = pos;

        return ret;
    }
}
