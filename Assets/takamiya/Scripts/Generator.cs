//======================================================
//É|Å[ÉVÉáÉìÇè„Ç©ÇÁç~ÇÁÇ∑
//AuthorÅFçÇã{óS„ƒ
//Date/2/20
//
//=======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public GameObject[] falls;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % 30 == 0)
        {
            GameObject potion = Instantiate(
                falls[Random.Range(0, falls.Length)],
                new Vector3(Random.Range(-14f, 14f), transform.position.y, transform.position.z),
                Quaternion.identity
                );
            Destroy(potion, 10f);
        }
    }
}
