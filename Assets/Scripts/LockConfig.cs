using UnityEngine;

[CreateAssetMenu(fileName = "Lock Config", menuName = "Configs/Lock", order = 0)]
public class LockConfig : ScriptableObject
{
    [SerializeField] private int angleThreshold = 10;
    [SerializeField] private float unlockStartTimer = 2f;
    [SerializeField] private float offTargetTimer = 0.1f;
    public int AngleThreshold => angleThreshold;
    public float UnlockStartTimer => unlockStartTimer;
    public float OffTargetTimer => offTargetTimer;
}