using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public GameObject[] objects;
    int i=0;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the space key is pressed
        // activate the next object
        if (Input.GetKeyDown(KeyCode.Space))
        {
            i++;
        }

        if (i < objects.Length)
        {
            objects[i].SetActive(true);
        } else { 
            // go to the next scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("Level");
        }
    }
}
