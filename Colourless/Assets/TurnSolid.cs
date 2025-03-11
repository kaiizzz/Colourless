using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSolid : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<SpriteRenderer>().material.color != new Color(0.0f, 0.0f, 1.0f, 1.0f))
        {
            GetComponent<BoxCollider2D>().enabled = false;
        } else {
            GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
