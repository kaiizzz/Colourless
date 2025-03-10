using UnityEngine;

public class ColouAura : MonoBehaviour
{
    public Material colorAuraMaterial;  // Assign the material with the shader
    public float effectRadius = 3.0f;  // Radius of color restoration

    private void Update()
    {
        if (colorAuraMaterial != null)
        {
            // Convert 2D world position to Vector4 (needed for the shader)
            Vector3 playerPos = transform.position;
            colorAuraMaterial.SetVector("_PlayerPos", new Vector4(playerPos.x, playerPos.y, 0, 0));
            colorAuraMaterial.SetFloat("_EffectRadius", effectRadius);
        }
    }
}
