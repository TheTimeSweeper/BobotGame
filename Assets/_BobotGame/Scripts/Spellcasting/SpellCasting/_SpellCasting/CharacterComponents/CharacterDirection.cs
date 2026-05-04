using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDirection : MonoBehaviour
{
    [SerializeField]
    private Transform baseTransform;

    [SerializeField]
    private float acceleration;

    public Vector3 DesiredDirection { get; set; }
    public Vector3? OverrideDirection { get; set; }

    private Vector3 _currentDirection;
    private Vector3 _desiredDirection;

    private float _lookTimer;

    void FixedUpdate()
    {
        _lookTimer -= Time.fixedDeltaTime;

        _desiredDirection = DesiredDirection;
        _currentDirection = Util.ExpDecayLerp(_currentDirection, DesiredDirection, acceleration, Time.fixedDeltaTime);

        //jam faster deceleration value for fuckin uhh stopping and moving opposite direction

        Vector3 finalDirection = _currentDirection;

        //jam this should loop too I suppose
        if (OverrideDirection.HasValue)
        {
            finalDirection = OverrideDirection.Value;

            if (_lookTimer < 0)
            {
                OverrideDirection = null;
            }
        }

        finalDirection.y = 0;

        if (finalDirection == Vector3.zero)
            return;

        baseTransform.rotation = Quaternion.LookRotation(finalDirection, Vector3.up);
    }

    public void OverrideLookDirection(Vector3 desiredDirection, float time = 1)
    {
        _lookTimer = time;
        OverrideDirection = desiredDirection;
    }
}
