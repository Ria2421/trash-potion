//
// タイルスクリプト
// Name:西浦晃太 Date:2/14
//
using UnityEngine;

public class Tile : MonoBehaviour
{
    Color color;

    /// <summary>
    /// 可動域タイル変色関数
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GetComponent<Renderer>().material.color = new Color(0f, 1f, 0f, 1f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}