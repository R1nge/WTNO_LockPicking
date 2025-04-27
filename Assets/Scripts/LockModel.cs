using System;
using UnityEngine;

[Serializable]
public class LockModel
{
    [field: SerializeField] public bool IsLocked { get; set; }
    [field: SerializeField] public int TargetAngle { get; set; }
    [field: SerializeField] public int CurrentAngle { get; set; }

    public void SetCurrentAngle(int angle)
    {
        angle %= 360;
        angle = Math.Clamp(angle, 0, 360);
        CurrentAngle = angle;
    }
}