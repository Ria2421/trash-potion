using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameObject[] TutorialImages;

    int imageNum;
    // Start is called before the first frame update
    void Start()
    {
        imageNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (imageNum == TutorialImages.Length - 1)
            {
                Initiate.Fade("Title", Color.black, 1.0f);
            }
            else
            {
                TutorialImages[imageNum].SetActive(false);

                imageNum++;

                TutorialImages[imageNum].SetActive(true);
            }
        }
        else if(Input.GetMouseButtonDown(1))
        {
            if (imageNum == 0)
            {
                Initiate.Fade("Title", Color.black, 1.0f);
            }
            else
            {
                TutorialImages[imageNum].SetActive(false);

                imageNum--;

                TutorialImages[imageNum].SetActive(true);
            }
        }
    }
        
}
