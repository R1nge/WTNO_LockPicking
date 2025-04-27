using UnityEngine;

public class TargetView : MonoBehaviour
{
    [SerializeField] private Material progressMaterial;
    private Material _defaultMaterial;
    [SerializeField] private MeshRenderer[] progressMeshes;
    private int _progress = 0;

    private void Awake()
    {
        _defaultMaterial = progressMeshes[0].material;
    }

    public void AddProgress()
    {
        if (_progress >= progressMeshes.Length) return;
        _progress++;
        UpdateProgress();
    }

    public void ResetProgress()
    {
        _progress = 0;
        UpdateProgress();
    }

    private void UpdateProgress()
    {
        for (int i = 0; i < progressMeshes.Length; i++)
        {
            progressMeshes[i].material = _progress > i ? progressMaterial : _defaultMaterial;
        }
    }
}