using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandAndFade : MonoBehaviour
{
    private Color color;
    // Start is called before the first frame update
    void Start()
    {
        color = GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        // Expand the object
        transform.localScale += new Vector3(1f, 1f, 1f);
        // Fade the object
        color.a -= 0.01f;

        if (color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}
