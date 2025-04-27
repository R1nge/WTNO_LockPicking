using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class LockPresenter : MonoBehaviour
{
    [SerializeField] private LockView lockView;
    [SerializeField] private LockModel lockModel;
    [SerializeField] private int angleThreshold = 10;
    private Vector2 _startPosition;
    private const float StartTimer = 2f;
    private float _currentTimer = 2f;

    private void Awake()
    {
        lockModel.OnLockChanged += OnLockChanged;
        lockModel.OnTargetAngleChanged += TargetAngleChanged;
        lockModel.OnCurrentAngleChanged += CurrentAngleChanged;
        lockModel.TargetAngle = Random.Range(0, 360);
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
            lockModel.SetCurrentAngle((int)delta.x);
        }

        if (Input.GetMouseButtonUp(0))
        {
            _startPosition = Vector2.zero;
        }

        if (Math.Abs(lockModel.CurrentAngle - lockModel.TargetAngle) < angleThreshold)
        {
            _currentTimer -= Time.deltaTime;
            if (_currentTimer <= 0)
            {
                lockModel.TargetAngle = Random.Range(0, 360);
                Debug.Log("Unlocked");
            }
        }
        else
        {
            _currentTimer = StartTimer;
            Debug.Log("Reset timer");
        }
    }

    private void OnDestroy()
    {
        lockModel.OnLockChanged -= OnLockChanged;
        lockModel.OnTargetAngleChanged -= TargetAngleChanged;
        lockModel.OnCurrentAngleChanged -= CurrentAngleChanged;
    }
}