using UnityEngine;
using UnityEngine.Events;

public class InteractiveUI : MonoBehaviour
{
    [Header("Settings")]
    public UnityEvent onInteractionStart;
    public UnityEvent onInteractionEnd;
    
    [Header("Visual Feedback")]
    public Material highlightMaterial;
    private Material originalMaterial;
    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if(meshRenderer) originalMaterial = meshRenderer.material;
    }

    public void StartInteraction()
    {
        onInteractionStart.Invoke();
        if(meshRenderer && highlightMaterial) 
            meshRenderer.material = highlightMaterial;
    }

    public void EndInteraction()
    {
        onInteractionEnd.Invoke();
        if(meshRenderer && originalMaterial) 
            meshRenderer.material = originalMaterial;
    }
}