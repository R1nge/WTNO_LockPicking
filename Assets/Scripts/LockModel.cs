using System;
using UnityEngine;

[Serializable]
public class LockModel
{
    [SerializeField] private bool isLocked;
    [SerializeField] private int targetAngle;
    [SerializeField] private int currentAngle;

    public event Action<bool> OnLockChanged;

    public bool IsLocked
    {
        get => isLocked;
        set
        {
            isLocked = value;
            OnLockChanged?.Invoke(isLocked);
        }
    }

    public event Action<int> OnTargetAngleChanged;

    public int TargetAngle
    {
        get => targetAngle;
        set
        {
            targetAngle = value;
            OnTargetAngleChanged?.Invoke(targetAngle);
        }
    }

    public event Action<int> OnCurrentAngleChanged;

    public int CurrentAngle
    {
        get => currentAngle;
        set
        {
            currentAngle = value;
            OnCurrentAngleChanged?.Invoke(currentAngle);
        }
    }

    public void SetCurrentAngle(int angle)
    {
        angle %= 360;
        angle = Math.Clamp(angle, 0, 360);
        CurrentAngle = angle;
    }
}