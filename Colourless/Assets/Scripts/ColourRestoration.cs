using UnityEngine;

public class ColorRestoration2D : MonoBehaviour
{
    public Material colorRestorationMaterial;  // Assign this in the Unity Inspector
    public float effectRadius = 3.0f;  // Radius of color restoration
    public int phase = 0;  // 0 = Greyscale, 1 = Blue, 2 = Green, 3 = Full Color

    private void Update()
    {
        if (colorRestorationMaterial != null)
        {
            // Convert 2D world position to Vector4 (needed for the shader)
            Vector3 playerPos = transform.position;
            colorRestorationMaterial.SetVector("_PlayerPos", new Vector4(playerPos.x, playerPos.y, 0, 0));
            colorRestorationMaterial.SetFloat("_EffectRadius", effectRadius);
            colorRestorationMaterial.SetFloat("_Phase", phase);
        }
    }

    // Call this method to progress through color phases
    public void NextPhase()
    {
        phase = Mathf.Min(phase + 1, 3);
    }
}
