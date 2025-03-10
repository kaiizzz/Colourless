using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    public GameObject[] tutorialText;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.position.x < -2.5)
        {
            tutorialText[0].SetActive(true);
            for (int i = 1; i < tutorialText.Length; i++)
            {
                tutorialText[i].SetActive(false);
            }
        }

        if (player.position.x >= -2.5 && player.position.x < -0.5)
        {
            tutorialText[1].SetActive(true);
            for (int i = 0; i < tutorialText.Length; i++)
            {
                if (i != 1)
                {
                    tutorialText[i].SetActive(false);
                }
            }
        }

        if (player.position.x >= -0.5 && player.position.x < 8.5)
        {
            tutorialText[2].SetActive(true);
            for (int i = 0; i < tutorialText.Length; i++)
            {
                if (i != 2)
                {
                    tutorialText[i].SetActive(false);
                }
            }
        }

        if (player.position.x >= 8.5 && player.position.x < 16.5)
        {
            tutorialText[3].SetActive(true);
            for (int i = 0; i < tutorialText.Length; i++)
            {
                if (i != 3)
                {
                    tutorialText[i].SetActive(false);
                }
            }
        }

        if (player.position.x >= 16.5 && player.position.x < 23.5)
        {
            tutorialText[4].SetActive(true);
            for (int i = 0; i < tutorialText.Length; i++)
            {
                if (i != 4)
                {
                    tutorialText[i].SetActive(false);
                }
            }
        }

        if (player.position.x >= 23.5 && player.position.x < 31.5)
        {
            tutorialText[5].SetActive(true);
            for (int i = 0; i < tutorialText.Length; i++)
            {
                if (i != 5)
                {
                    tutorialText[i].SetActive(false);
                }
            }
        }
    }
}
