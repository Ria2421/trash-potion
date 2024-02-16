using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField] GameObject effectPrefab;

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Plane")
        {
            Instantiate(effectPrefab,transform.position,Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
