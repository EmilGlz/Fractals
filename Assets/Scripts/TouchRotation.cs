using UnityEngine;

public class TouchRotation : Singleton<TouchRotation>
{
    [SerializeField] private float _selfRotateAfterSeconds;
    [SerializeField] private float _selfRotationSpeed = 10f;
    [SerializeField] private float rotateSpeed = 0.5f;
    private Vector3 lastMousePosition;
    private float _noTouchTimer;
    private bool _isSelfRotating;

    private float _yaw;
    private float _pitch;

    private bool _canTouchRotate;
    public bool CanTouchRotate
    {
        get => _canTouchRotate;
        set {
            _canTouchRotate = value;
            if (!_canTouchRotate)
                _isSelfRotating = true;
        }
    }
    void Start()
    {
        _noTouchTimer = 0f;
        CanTouchRotate = true;

        // Initialize from current rotation
        Vector3 angles = transform.eulerAngles;
        _yaw = angles.y;
        _pitch = angles.x;

        // Normalize to [-180, 180] so clamping works correctly
        if (_pitch > 180f) _pitch -= 360f;
        if (_yaw > 180f) _yaw -= 360f;
    }
    void Update()
    {
        if (CanTouchRotate)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isSelfRotating = false;
                lastMousePosition = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0))
            {
                _noTouchTimer = 0;
                Vector3 deltaMousePosition = Input.mousePosition - lastMousePosition;

                _yaw += deltaMousePosition.x * rotateSpeed;
                _pitch -= deltaMousePosition.y * rotateSpeed;
                _pitch = Mathf.Clamp(_pitch, -89f, 89f);

                transform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);

                lastMousePosition = Input.mousePosition;
            }
            else if (!Input.GetMouseButton(0) && !_isSelfRotating)
            {
                _noTouchTimer += Time.deltaTime;
                if (_noTouchTimer >= _selfRotateAfterSeconds)
                    _isSelfRotating = true;
            }
        }
        if (_isSelfRotating)
        {
            _yaw -= _selfRotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
        }
    }
}