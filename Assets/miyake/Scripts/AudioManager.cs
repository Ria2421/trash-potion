//==============================================
//Autor:三宅歩人
//Day:3/5
//BGM処理
//==============================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource bgm;

    // Start is called before the first frame update
    void Start()
    {
        //コンポーネントを取得
        bgm = GetComponent<AudioSource>();
        Invoke("PlaySound", 4.0f);
    }

    public void PlaySound()
    {
        //再生
        bgm.Play();
    }
}
