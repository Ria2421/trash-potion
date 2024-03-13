using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoteManager : MonoBehaviour
{
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        Random.InitState(6);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            int nom = Random.Range(0, 6);
            animator.SetInteger("TransitionNom", nom);
            Debug.Log("アニメーションを再生");
        }
    }
}