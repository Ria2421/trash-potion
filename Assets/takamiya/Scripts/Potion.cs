//
//ポーションを上から落とすスクリプト
//Author：高宮祐翔
//Date:2/19
//
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    //ポーションの落下するフラグ
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.one);
        transform.Translate(Vector3.down * speed, Space.World);
    }
}
