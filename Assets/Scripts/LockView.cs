using UnityEngine;

public class LockView : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform current;
    
    public void SetTargetAngle(float angle)
    {
        target.localRotation = Quaternion.Euler(0, angle, 0);
    }
    
    public void SetCurrentAngle(float angle)
    {
        current.localRotation = Quaternion.Euler(0, angle, 0);
    }
}