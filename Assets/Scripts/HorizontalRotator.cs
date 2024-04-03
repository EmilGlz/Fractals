using System.Collections;
using UnityEngine;

public class HorizontalRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float maxRotationSpeed = 8f;
    [SerializeField] private float delay = 1f;

    private bool isRotatingItself = false;
    private bool isDragging = false;
    private Vector3 dragStartPosition;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            dragStartPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            isRotatingItself = false;
            Vector3 delta = Input.mousePosition - dragStartPosition;
            float rotationAmount = delta.x * rotationSpeed * Time.deltaTime;
            rotationAmount = Mathf.Clamp(rotationAmount, -maxRotationSpeed, maxRotationSpeed);
            transform.Rotate(Vector3.up, rotationAmount);

            dragStartPosition = Input.mousePosition;
        }
        else
        {
            if(!isRotatingItself)
                StartCoroutine(RotateItself());
        }
    }

    private IEnumerator RotateItself()
    {
        yield return new WaitForSeconds(delay);
        while(!isDragging)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            isRotatingItself = true;
        }
    }
}
