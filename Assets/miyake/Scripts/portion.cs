using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portion : MonoBehaviour
{
     Rigidbody rb;
    //加わる力の大きさを定義
    float forceMagnitude = 12.0f;
    float rand;

    //左にポーションを射出
    public void SlowLeft()
    {
        rand = Random.Range(0.5f, 2.5f);

        //45度の角度でポーションを射出
        Vector3 forceDirection = new Vector3(-1.0f * rand, 1.0f * rand, 0f);

        // 向きと大きさからポーションに加わる力を計算する
        Vector3 force = forceMagnitude * forceDirection;

        rb = GetComponent<Rigidbody>();

        rb.AddForce(force, ForceMode.Impulse);          //ForceMode.Impulseは撃力
    }

    //右にポーションを射出
    public void SlowRight()
    {
        rand = Random.Range(0.5f, 2.5f);

        //45度の角度でポーションを射出
        Vector3 forceDirection = new Vector3(1.0f * rand, 1.0f * rand, 0f);

        // 向きと大きさからポーションに加わる力を計算する
        Vector3 force = forceMagnitude * forceDirection;

        rb = GetComponent<Rigidbody>();

        rb.AddForce(force, ForceMode.Impulse);          //ForceMode.Impulseは撃力
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Plane")
        {
            Destroy(gameObject);
        }
    }
}
