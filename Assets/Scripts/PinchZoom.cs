using UnityEngine;

public class PinchZoom : MonoBehaviour
{
    private const float zoomSpeed = 1f; // Adjust this value to control the zoom speed
    private const float minFOV = 5f; // Minimum field of view
    private const float maxFOV = 70; // Maximum field of view
    private Camera _camera;

    void Awake()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            _camera.fieldOfView += deltaMagnitudeDiff * zoomSpeed * Time.deltaTime;

            _camera.fieldOfView = Mathf.Clamp(_camera.fieldOfView, minFOV, maxFOV);
        }
    }
}
