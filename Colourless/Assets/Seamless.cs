using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{
    private float width;
    private Transform cam;

    private void Start()
    {
        cam = Camera.main.transform;
        width = GetComponent<SpriteRenderer>().bounds.size.x; // Get background width
    }

    private void Update()
    {
        if (transform.position.x < cam.position.x - width)
        {
            transform.position += new Vector3(width * 2, 0, 0); // Move to the right to loop
        }
    }
}
