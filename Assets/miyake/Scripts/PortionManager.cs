using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortionManager : MonoBehaviour
{
    [SerializeField] GameObject[] portionPrefabs;

    [SerializeField] bool slowFlag;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SlowPortion", 1.5f,1.5f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //ポーション射出
    public void SlowPortion()
    {
        //ポーションを生成してコンポーネントを取得
        GameObject portion = Instantiate(portionPrefabs[0],transform.position,Quaternion.identity);
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
