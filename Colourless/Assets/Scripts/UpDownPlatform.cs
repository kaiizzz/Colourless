using System.Collections;
using UnityEngine;

public class UpDownPlatform : MonoBehaviour
{
    public bool active = false;
    
    public Transform StartPoint;
    public Transform EndPoint;

    public ColorRestoration2D colourRestoration;

    private bool isMovingUp = true;
    private float speed = 0.2f;  // Adjust speed as needed

    private void Update()
    {
        if (active)
        {
            MovePlatform();
            UpdateColor();
        }
    }

    private void MovePlatform()
    {
        // Move the platform up/down smoothly
        if (isMovingUp)
        {
            transform.position = Vector3.Lerp(transform.position, EndPoint.position, Time.deltaTime * speed);
            if (Vector3.Distance(transform.position, EndPoint.position) < 0.1f)
            {
                StartCoroutine(SwitchDirection());
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, StartPoint.position, Time.deltaTime * speed);
            if (Vector3.Distance(transform.position, StartPoint.position) < 0.1f)
            {
                StartCoroutine(SwitchDirection());
            }
        }
    }

    private IEnumerator SwitchDirection()
    {
        yield return new WaitForSeconds(2.0f); // Pause before moving back
        isMovingUp = !isMovingUp;
    }

    private void UpdateColor()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (colourRestoration != null)
        {
            switch (colourRestoration.phase)
            {
                case 0: sr.color = Color.blue; break;
                case 1: sr.color = Color.green; break;
                case 2: sr.color = Color.red; break;
                case 3: sr.color = Color.white; break;
            }
        }
    }
}
