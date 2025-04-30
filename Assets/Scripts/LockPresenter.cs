using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class LockPresenter : MonoBehaviour
{
    [SerializeField] private LockView lockView;
    [SerializeField] private LockModel lockModel;
    [SerializeField] private LockConfig lockConfig;
    private Vector2 _startPosition;
    private float _unlockTimerCurrent = 2f;
    private float _timeBeforeNewTargetTimerCurrent = 1.5f;
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

    private void ProcessInput()
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
    }

    private void Update()
    {
        if (lockModel.IsLocked)
        {
            ProcessInput();

            if (Math.Abs(lockModel.CurrentAngle - lockModel.TargetAngle) < lockConfig.AngleThreshold)
            {
                lockView.SetHighlight(true);

                _progressTimerCurrent -= Time.deltaTime;
                if (_progressTimerCurrent <= 0)
                {
                    lockView.AddProgress();
                    PickNewProgressTimer();
                }

                _unlockTimerCurrent -= Time.deltaTime;
                if (_unlockTimerCurrent <= 0)
                {
                    lockView.AddProgress();
                    lockModel.IsLocked = false;
                    Debug.Log("Unlocked");
                }
            }
            else
            {
                lockView.SetHighlight(false);
                _unlockTimerCurrent = lockConfig.UnlockStartTimer;
                lockView.ResetProgress();
                PickNewProgressTimer();
                Debug.Log("Reset timer");
            }

            _timeBeforeNewTargetTimerCurrent -= Time.deltaTime;
            if (_timeBeforeNewTargetTimerCurrent <= 0)
            {
                lockModel.SetTargetAngle(GetRandomTargetAngle());
                _timeBeforeNewTargetTimerCurrent = GetRandomTargetAngleSwapTimer();
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

    private void PickNewProgressTimer() => _progressTimerCurrent = lockConfig.UnlockStartTimer / lockView.ProgressMeshesCount;
}