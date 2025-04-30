using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class LockPresenter : MonoBehaviour
{
    [SerializeField] private LockView lockView;
    [SerializeField] private LockModel lockModel;
    [SerializeField] private LockConfig lockConfig;
    private Coroutine _lerpCoroutine;
    private Vector2 _startPosition;
    private float _unlockTimerCurrent = 2f;
    private float _timeBeforeNewTargetTimerCurrent = 1.5f;
    private float _progressTimerCurrent = 1f;
    private float _offTargetTimerCurrent = 1f;

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
                _offTargetTimerCurrent = lockConfig.OffTargetTimer;
                lockView.SetHighlight(true);

                _progressTimerCurrent -= Time.deltaTime;
                if (_progressTimerCurrent <= 0)
                {
                    lockView.AddProgress();
                    ResetProgressTimer();
                }

                _unlockTimerCurrent -= Time.deltaTime;
                if (_unlockTimerCurrent <= 0)
                {
                    lockView.AddProgress();
                    lockModel.IsLocked = false;

                    if (_lerpCoroutine != null)
                    {
                        StopCoroutine(_lerpCoroutine);
                    }

                    Debug.Log("Unlocked");
                }
            }
            else
            {
                if (_offTargetTimerCurrent <= 0)
                {
                    lockView.SetHighlight(false);
                    _unlockTimerCurrent = lockConfig.UnlockStartTimer;
                    _offTargetTimerCurrent = lockConfig.OffTargetTimer;
                    ResetProgressTimer();
                    lockView.ResetProgress();
                    Debug.Log("Reset timer");
                }
            }

            _offTargetTimerCurrent -= Time.deltaTime;
            _timeBeforeNewTargetTimerCurrent -= Time.deltaTime;
            if (_timeBeforeNewTargetTimerCurrent <= 0)
            {
                _timeBeforeNewTargetTimerCurrent = GetRandomTargetAngleSwapTimer();
                _lerpCoroutine = StartCoroutine(LerpTargetAngle());
            }
        }
    }

    private IEnumerator LerpTargetAngle()
    {
        var time = 0f;
        var currentAngle = lockModel.TargetAngle;
        var targetAngle = GetRandomTargetAngle();

        while (time < _timeBeforeNewTargetTimerCurrent)
        {
            var t = time / _timeBeforeNewTargetTimerCurrent;
            lockModel.SetTargetAngle((int)Mathf.Lerp(currentAngle, targetAngle, t));
            time += Time.deltaTime;
            yield return null;
        }

        lockModel.SetTargetAngle(targetAngle);
    }

    private void OnDestroy()
    {
        lockModel.OnLockChanged -= OnLockChanged;
        lockModel.OnTargetAngleChanged -= TargetAngleChanged;
        lockModel.OnCurrentAngleChanged -= CurrentAngleChanged;
    }

    private int GetRandomTargetAngle() => Random.Range(-60, 60);

    private float GetRandomTargetAngleSwapTimer() => Random.Range(0.5f, 1.5f);

    private void ResetProgressTimer() => _progressTimerCurrent = lockConfig.UnlockStartTimer / lockView.ProgressMeshesCount;
}