using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Camera")] public Camera cam;

    public float smoothSpeed = 0.125f;

    [Header("Camera Orbit Parameters")] public float height = 10f;

    public float radius = 10f;

    [Tooltip("Vertical offset from the player's pivot point for the camera to look at.")]
    public float lookAtHeightOffset = 0.5f;

    [Header("Rotation Smoothing")] public float smoothRotationSpeed = 5f;

    private float _currentRotationAngle;
    private float _targetRotationAngle;

    private void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
            if (cam == null)
            {
                Debug.LogError("CameraFollow: No camera assigned and no main camera found!");
                enabled = false;
                return;
            }
        }


        _currentRotationAngle = 0f;
        _targetRotationAngle = 0f;


        cam.transform.position = CalculateDesiredCameraPosition(_currentRotationAngle);

        cam.transform.LookAt(transform.position + Vector3.up * lookAtHeightOffset);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) RotateCamera(90);

        if (Input.GetKeyDown(KeyCode.Q)) RotateCamera(-90);
    }

    private void LateUpdate()
    {
        if (!cam) return;


        _currentRotationAngle = Mathf.LerpAngle(_currentRotationAngle, _targetRotationAngle,
            smoothRotationSpeed * Time.deltaTime);


        var desiredPosition = CalculateDesiredCameraPosition(_currentRotationAngle);


        var smoothedPosition = Vector3.Lerp(cam.transform.position, desiredPosition, smoothSpeed);
        cam.transform.position = smoothedPosition;


        cam.transform.LookAt(transform.position + Vector3.up * lookAtHeightOffset);
    }


    private void RotateCamera(int angleIncrement)
    {
        _targetRotationAngle += angleIncrement;
        _targetRotationAngle %= 360f;
        if (_targetRotationAngle < 0) _targetRotationAngle += 360f;
    }


    private Vector3 CalculateDesiredCameraPosition(float angle)
    {
        var angleRad = angle * Mathf.Deg2Rad;
        var x = radius * Mathf.Sin(angleRad);
        var z = radius * Mathf.Cos(angleRad);


        return transform.position + new Vector3(x, height, z);
    }
}