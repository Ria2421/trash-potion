using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portion : MonoBehaviour
{
     Rigidbody rb;
    //加わる力の大きさを定義
    float forceMagnitude = 15.0f;

    public void SlowLeft()
    {
        //45度の角度でポーションを射出
        Vector3 forceDirection = new Vector3(-1.0f, 1.0f, 0f);

        // 向きと大きさからポーションに加わる力を計算する
        Vector3 force = forceMagnitude * forceDirection;

        rb = GetComponent<Rigidbody>();

        rb.AddForce(force, ForceMode.Impulse);          //ForceMode.Impulseは撃力
    }

    public void SlowRight()
    {
        //45度の角度でポーションを射出
        Vector3 forceDirection = new Vector3(1.0f, 1.0f, 0f);

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
