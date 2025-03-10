using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class ApplyMaterialToLayer : MonoBehaviour
{
    [Tooltip("Name of the layer whose objects should have the material applied.")]
    public string targetLayerName = "Restorable";

    [Tooltip("Material to apply to the objects on the target layer.")]
    public Material targetMaterial;

    private int targetLayer;
    private HashSet<GameObject> processedObjects = new HashSet<GameObject>();

    void Start()
    {
        // Convert layer name to layer index.
        targetLayer = LayerMask.NameToLayer(targetLayerName);

        if (targetLayer == -1)
        {
            Debug.LogError("Layer '" + targetLayerName + "' does not exist. Please create it in the Tags & Layers settings.");
            return;
        }

        // Apply material to existing objects at start
        ApplyMaterialToAllObjects();
    }

    void Update()
    {
            ApplyMaterialToAllObjects();
    }

    void ApplyMaterialToAllObjects()
    {
        ApplyMaterialToObjects(FindObjectsOfType<TilemapRenderer>());
        ApplyMaterialToObjects(FindObjectsOfType<SpriteRenderer>());
    }

    void ApplyMaterialToNewObjects()
    {
        TilemapRenderer[] tilemaps = FindObjectsOfType<TilemapRenderer>();
        SpriteRenderer[] sprites = FindObjectsOfType<SpriteRenderer>();

        ApplyMaterialToObjects(tilemaps);
        ApplyMaterialToObjects(sprites);
    }

    void ApplyMaterialToObjects<T>(T[] objects) where T : Renderer
    {
        foreach (T obj in objects)
        {
            if (obj.gameObject.layer == targetLayer && !processedObjects.Contains(obj.gameObject))
            {
                obj.material = targetMaterial;
                processedObjects.Add(obj.gameObject);
                Debug.Log("Material applied to: " + obj.gameObject.name);
            }
        }
    }
}
