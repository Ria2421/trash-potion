using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
// <summary>
/// ‹N“®ˆ—
/// </summary>
private void Awake()
    {
        // UIƒV[ƒ“‚Ì’Ç‰Á
        SceneManager.LoadScene("UIManager", LoadSceneMode.Additive);
    }
}
