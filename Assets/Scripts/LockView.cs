using UnityEngine;

public class LockView : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform current;
    [SerializeField] private GameObject highlight;

    public void SetTargetAngle(float angle)
    {
        target.localRotation = Quaternion.Euler(0, 0, angle);
    }

    public void SetCurrentAngle(float angle)
    {
        current.localRotation = Quaternion.Euler(0, 0, angle);
    }

    public void SetHighlight(bool on)
    {
        highlight.gameObject.SetActive(on);
    }
}