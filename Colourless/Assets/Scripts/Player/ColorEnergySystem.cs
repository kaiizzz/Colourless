using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ColorEnergySystem : MonoBehaviour
{
    public float maxEnergy = 100f;
    private float currentEnergy;
    public Image energyBar;

    private float amountToUse = 0f;

    public bool outOfEnergy = false;

    private void Start()
    {
        currentEnergy = maxEnergy;
    }

    public bool HasEnoughEnergy(float amount)
    {
        return currentEnergy >= amount;
    }

    public void UseEnergy(float amount)
    {
        if (amount <= currentEnergy)
            amountToUse += amount;
    }

    public void RestoreEnergy(float amount)
    {
        currentEnergy = Mathf.Min(currentEnergy + amount, maxEnergy);
    }

    public float GetEnergyPercentage()
    {
        return currentEnergy / maxEnergy;
    }

    void Update()
    {
        energyBar.fillAmount = GetEnergyPercentage();

        if (amountToUse > 0f)
        {
            currentEnergy--;
            amountToUse--;
        }

        if (currentEnergy <= 0.01f)
        {
            currentEnergy = 0f;
            outOfEnergy = true;
        }
        else
        {
            outOfEnergy = false;
        }


    }
}
