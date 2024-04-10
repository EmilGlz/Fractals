using UnityEngine;

public class TouchRotation : MonoBehaviour
{
    [SerializeField] private float _selfRotateAfterSeconds;
    [SerializeField] private float _selfRotationSpeed = 10f;
    [SerializeField] private float rotateSpeed = 0.5f; 
    private Vector3 lastMousePosition;
    private float _noTouchTimer;
    private bool _isSelfRotating;
    void Start()
    {
        _noTouchTimer = 0f;
    }
    void Update()
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

            // Rotate horizontally (around Y axis)
            float horizontalRotation = deltaMousePosition.x * rotateSpeed;
            transform.Rotate(Vector3.up, horizontalRotation, Space.World);

            // Rotate vertically (around X axis)
            float verticalRotation = deltaMousePosition.y * rotateSpeed;
            transform.Rotate(Vector3.right, -verticalRotation, Space.World);

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0f);

            lastMousePosition = Input.mousePosition;
        }
        else if (Input.touchCount == 0 && !_isSelfRotating)
        {
            _noTouchTimer += Time.deltaTime;
            if (_noTouchTimer >= _selfRotateAfterSeconds)
                _isSelfRotating = true;
        }
        if (_isSelfRotating)
            transform.Rotate(Vector3.up, -_selfRotationSpeed * Time.deltaTime, Space.World);
    }
}