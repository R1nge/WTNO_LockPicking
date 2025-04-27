using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class LockPresenter : MonoBehaviour
{
    [SerializeField] private LockView lockView;
    [SerializeField] private LockModel lockModel;
    [SerializeField] private int angleThreshold = 10;
    private Vector2 _startPosition;
    private const float UnlockStartTimer = 2f;
    private float _unlockCurrentTimer = 2f;
    private const float TimeBeforeNewTargetTimer = 1.5f;
    private float _timeBeforeNewTargetCurrentTimer = 1.5f;

    private void Awake()
    {
        lockModel.OnLockChanged += OnLockChanged;
        lockModel.OnTargetAngleChanged += TargetAngleChanged;
        lockModel.OnCurrentAngleChanged += CurrentAngleChanged;
        lockModel.TargetAngle = GetRandomTargetAngle();
        lockModel.IsLocked = true;
    }

    private void OnLockChanged(bool locked)
    {
    }

    private void TargetAngleChanged(int angle)
    {
        lockView.SetTargetAngle(angle);
    }

    private void CurrentAngleChanged(int angle)
    {
        lockView.SetCurrentAngle(angle);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            var mousePosition = Input.mousePosition;
            var delta = mousePosition - (Vector3)_startPosition;
            lockModel.SetCurrentAngle((int)-delta.x);
        }

        if (Input.GetMouseButtonUp(0))
        {
            _startPosition = Vector2.zero;
        }

        if (Math.Abs(lockModel.CurrentAngle - lockModel.TargetAngle) < angleThreshold)
        {
            lockView.SetHighlight(true);
            _unlockCurrentTimer -= Time.deltaTime;
            if (_unlockCurrentTimer <= 0)
            {
                lockModel.IsLocked = false;
                Debug.Log("Unlocked");
            }
        }
        else
        {
            lockView.SetHighlight(false);
            _unlockCurrentTimer = UnlockStartTimer;
            Debug.Log("Reset timer");
        }

        if (lockModel.IsLocked)
        {
            _timeBeforeNewTargetCurrentTimer -= Time.deltaTime;
            if (_timeBeforeNewTargetCurrentTimer <= 0)
            {
                lockModel.SetTargetAngle(GetRandomTargetAngle());
                _timeBeforeNewTargetCurrentTimer = TimeBeforeNewTargetTimer;
            }
        }
    }

    private void OnDestroy()
    {
        lockModel.OnLockChanged -= OnLockChanged;
        lockModel.OnTargetAngleChanged -= TargetAngleChanged;
        lockModel.OnCurrentAngleChanged -= CurrentAngleChanged;
    }

    private int GetRandomTargetAngle() => Random.Range(-60, 60);
}