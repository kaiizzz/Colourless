using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNotDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Do not destroy this object when loading a new scene
        DontDestroyOnLoad(gameObject);
    }
}
