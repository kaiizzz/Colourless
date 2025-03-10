using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleController : MonoBehaviour
{
    public GameObject[] Titles;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TitleAnimation());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TitleAnimation()
    {
        for (int i = 0; i < Titles.Length; i++)
        {
            Titles[i].SetActive(true);
            yield return new WaitForSeconds(1.0f);
        }
    }
}
