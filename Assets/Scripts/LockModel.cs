using System;
using UnityEngine;

//TODO: add lerp angle and compare with it, also use it to update the view

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

    public void SetTargetAngle(int angle)
    {
        var newAngle = angle % 360;

        switch (newAngle)
        {
            case > 180:
                newAngle -= 360;
                break;
            case < -180:
                newAngle += 360;
                break;
        }

        TargetAngle = newAngle;
    }

    public void SetCurrentAngle(int angle)
    {
        var newAngle = angle % 360;

        switch (newAngle)
        {
            case > 180:
                newAngle -= 360;
                break;
            case < -180:
                newAngle += 360;
                break;
        }

        CurrentAngle = newAngle;
    }
}