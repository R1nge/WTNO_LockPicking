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
        lockModel.TargetAngle = Random.Range(0, 360);
        lockView.SetTargetAngle(lockModel.TargetAngle);
        lockView.SetCurrentAngle(lockModel.CurrentAngle);
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
            lockView.SetCurrentAngle(lockModel.CurrentAngle);
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
                lockView.SetTargetAngle(lockModel.TargetAngle);
                Debug.Log("Unlocked");
            }
        }
        else
        {
            _currentTimer = StartTimer;
            Debug.Log("Reset timer");
        }
    }
}