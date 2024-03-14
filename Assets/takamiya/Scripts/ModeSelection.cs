using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeSelection : MonoBehaviour
{
    public bool isClicked;      //ƒNƒŠƒbƒN‚µ‚½‚©‚Ç‚¤‚©

    // Start is called before the first frame update
    void Start()
    {
        isClicked = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isClicked)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Initiate.Fade("Game", Color.black, 1.0f);
            }

            isClicked = true;
        }
    }
}
