using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortionManager : MonoBehaviour
{
    [SerializeField] GameObject[] portionPrefabs;       //ポーションのプレハブ
    [SerializeField] bool slowFlag;                     //ポーションのフラグ判定
    int rand;                                           //ポーション生成をランダムにするための変数
    int randAngle;                                      //ポーションの角度の変数

    // Start is called before the first frame update
    void Start()
    {
        //1.5秒間隔で関数を実行
        InvokeRepeating("SlowPortion", 5.0f,0.5f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //ポーション射出
    public void SlowPortion()
    {
        rand = Random.Range(0, 5);
        randAngle = Random.Range(-180, 180);

        //ポーションを生成してコンポーネントを取得
        GameObject portion = Instantiate(portionPrefabs[rand],transform.position,Quaternion.Euler(-90 + randAngle,0,0));
        if(slowFlag)
        {
            portion.GetComponent<portion>().SlowLeft();
        }
        else
        {
            portion.GetComponent<portion>().SlowRight();
        }
    }
}
