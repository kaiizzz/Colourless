using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnergy : MonoBehaviour
{
    public GameObject energyPrefab;
    private GameObject energyInstance;

    private bool isReset = false;
    // Start is called before the first frame update
    void Start()
    {
        energyInstance = Instantiate(energyPrefab, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (energyInstance == null && isReset)
        {
            energyInstance = Instantiate(energyPrefab, transform.position, Quaternion.identity);
            isReset = false;
        }
    }

    public void Notify() {
        this.isReset = true;
    }
}
