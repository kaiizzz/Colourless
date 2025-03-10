using UnityEngine;
using UnityEngine.Tilemaps;

public class ApplyAuraMaterial : MonoBehaviour
{
    public string targetLayerName = "Restorable";  // Set layer name in Inspector
    public Material auraMaterial;

    void Start()
    {
        int layer = LayerMask.NameToLayer(targetLayerName);
        if (layer == -1)
        {
            Debug.LogError("Layer '" + targetLayerName + "' does not exist!");
            return;
        }

        // Apply to all Tilemaps
        foreach (TilemapRenderer tr in FindObjectsOfType<TilemapRenderer>())
        {
            if (tr.gameObject.layer == layer)
                tr.material = auraMaterial;
        }

        // Apply to all Sprites
        foreach (SpriteRenderer sr in FindObjectsOfType<SpriteRenderer>())
        {
            if (sr.gameObject.layer == layer)
                sr.material = auraMaterial;
        }
    }
}
