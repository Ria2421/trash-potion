//
//フィールドに設置したポーションを回転させる為のスクリプト
//Author:Miura Yuki
//Date:2024/2/22
//Update:2024/2/22
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(1,1,1));
    }
}
