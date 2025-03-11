using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerColourController : MonoBehaviour
{

    public Image energyBar;
    public ParticleSystem Trail;
    public ParticleSystem Trail2;
    public SpriteRenderer EnergyBurst;
    public SpriteRenderer Mask;
    public SpriteRenderer Shield;

    public SpriteRenderer ColourChangeBlast;

    public ColorRestoration2D colorEnergySystem;

    public int phase = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        phase = colorEnergySystem.phase;

        if (phase == 0) {
            // blue phase
            energyBar.color = new Color(0.0f, 0.0f, 1.0f, 1.0f);
            Trail.startColor = new Color(0.0f, 0.0f, 1.0f, 0.2f);
            Trail2.startColor = new Color(0.0f, 0.0f, 1.0f, 0.05f);
            EnergyBurst.color = new Color(0.0f, 0.0f, 1.0f, 0.5f);
            Mask.color = new Color(0.0f, 0.0f, 1.0f, 1f);
            ColourChangeBlast.color = new Color(0.0f, 0.0f, 1.0f, 0.7f);
            Shield.color = new Color(0.0f, 0.0f, 1.0f, 0.5f);
        }
        if (phase == 1) {
            // green phase
            energyBar.color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
            Trail.startColor = new Color(0.0f, 1.0f, 0.0f, 0.2f);
            Trail2.startColor = new Color(0.0f, 1.0f, 0.0f, 0.05f);
            EnergyBurst.color = new Color(0.0f, 1.0f, 0.0f, 0.5f);
            Mask.color = new Color(0.0f, 1.0f, 0.0f, 1f);
            ColourChangeBlast.color = new Color(0.0f, 1.0f, 0.0f, 0.7f);
            Shield.color = new Color(0.0f, 1.0f, 0.0f, 0.5f);
        }
        if (phase == 2) {
            // red phase
            energyBar.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            Trail.startColor = new Color(1.0f, 0.0f, 0.0f, 0.2f);
            Trail2.startColor = new Color(1.0f, 0.0f, 0.0f, 0.05f);
            EnergyBurst.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
            Mask.color = new Color(1.0f, 0.0f, 0.0f, 1f);
            ColourChangeBlast.color = new Color(1.0f, 0.0f, 0.0f, 0.7f);
            Shield.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
        }
        if (phase == 3) {
            // full colour phase
            energyBar.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            Trail.startColor = new Color(1.0f, 1.0f, 1.0f, 0.2f);
            Trail2.startColor = new Color(1.0f, 1.0f, 1.0f, 0.05f);
            EnergyBurst.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            Mask.color = new Color(1.0f, 1.0f, 1.0f, 1f);
            ColourChangeBlast.color = new Color(1.0f, 1.0f, 1.0f, 0.7f);
            Shield.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        }

    }
}
