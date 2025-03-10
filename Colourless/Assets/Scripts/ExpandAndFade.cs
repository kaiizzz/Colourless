using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandAndFade : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Expand the object
        transform.localScale += new Vector3(1f, 1f, 1f);
        // Fade the object
        Color color = GetComponent<SpriteRenderer>().color;
        color.a -= 0.2f;

        if (color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}
