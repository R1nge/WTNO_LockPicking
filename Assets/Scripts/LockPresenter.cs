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
    private float _timeBeforeNewTargetCurrentTimer = 1.5f;
    private float _progressTimerCurrent = 1f;

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

    private void TargetAngleChanged(int angle) => lockView.SetTargetAngle(angle);

    private void CurrentAngleChanged(int angle) => lockView.SetCurrentAngle(angle);

    private void Update()
    {
        if (lockModel.IsLocked)
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

                _progressTimerCurrent -= Time.deltaTime;
                if (_progressTimerCurrent <= 0)
                {
                    lockView.AddProgress();
                    PickNewProgressTimer();
                }

                _unlockCurrentTimer -= Time.deltaTime;
                if (_unlockCurrentTimer <= 0)
                {
                    lockView.AddProgress();
                    lockModel.IsLocked = false;
                    Debug.Log("Unlocked");
                }
            }
            else
            {
                lockView.SetHighlight(false);
                _unlockCurrentTimer = UnlockStartTimer;
                lockView.ResetProgress();
                PickNewProgressTimer();
                Debug.Log("Reset timer");
            }

            _timeBeforeNewTargetCurrentTimer -= Time.deltaTime;
            if (_timeBeforeNewTargetCurrentTimer <= 0)
            {
                lockModel.SetTargetAngle(GetRandomTargetAngle());
                _timeBeforeNewTargetCurrentTimer = GetRandomTargetAngleSwapTimer();
                PickNewProgressTimer();
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

    private float GetRandomTargetAngleSwapTimer() => Random.Range(0.5f, 1.5f);

    private void PickNewProgressTimer() => _progressTimerCurrent = UnlockStartTimer / 3f;
}