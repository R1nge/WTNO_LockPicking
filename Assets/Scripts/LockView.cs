using UnityEngine;

public class LockView : MonoBehaviour
{
    [SerializeField] private TargetView target;
    [SerializeField] private Transform current;
    [SerializeField] private GameObject highlight;

    public void SetTargetAngle(float angle)
    {
        target.transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    public void SetCurrentAngle(float angle)
    {
        current.localRotation = Quaternion.Euler(0, 0, angle);
    }

    public void SetHighlight(bool on)
    {
        highlight.gameObject.SetActive(on);
    }

    public void AddProgress()
    {
        target.AddProgress();
    }

    public void ResetProgress()
    {
        target.ResetProgress();
    }
}