using UnityEngine;

public class GameCamera : MonoBehaviour
{
    private Transform _cameraTarget;

    public float cameraDistance;

    private Vector3 _startDirection;

    private void Start()
    {
        _cameraTarget = GameObject.FindGameObjectWithTag(
            "Player").transform;
        _startDirection = Quaternion.Euler(
            transform.eulerAngles) * Vector3.forward;
    }

    private void LateUpdate()
    {
        Vector3 desiredCameraPosition;
        Quaternion desiredCameraRotation;

        CalculateCameraTransform(
            out desiredCameraPosition,
            out desiredCameraRotation);
        var myTransform = transform;
        myTransform.position = desiredCameraPosition;
        myTransform.rotation = desiredCameraRotation;
    }

    private void CalculateCameraTransform(
        out Vector3 cameraPosition,
        out Quaternion cameraRotation)
    {
        var position = _cameraTarget.position;
        cameraPosition = position - _startDirection * cameraDistance;
        cameraRotation = Quaternion.LookRotation(
            position - cameraPosition);
    }
}