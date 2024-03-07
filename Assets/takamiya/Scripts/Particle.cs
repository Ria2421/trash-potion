//=========================================
//紙吹雪生成処理
//Author：高宮祐翔
//Date2/28
//
//=========================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;

public class Particle : MonoBehaviour
{
    [SerializeField] GameObject particle;//Prefabsの指定

    [SerializeField] GameObject CrackerRight;//右側のパーティクル

    [SerializeField] GameObject CrackerLeft;//左側のパーティクル

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("GenerateParticle",1.0f,2.0f);
        InvokeRepeating("GenerateParticle2", 1.0f, 2.0f);
    }
    // Update is called once per frame
    void Update()
    {
       
    }

    //右側の紙吹雪を生成
    void GenerateParticle()
    {
        GameObject parent = particle;
        Instantiate(parent,CrackerRight.transform);

    }
    //左側の紙吹雪を生成
    void GenerateParticle2()
    {
        GameObject parent = particle;
        Instantiate(parent, CrackerLeft.transform);
    }
}
