using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public float Sensitivity;

    private Vector2 playerInput;
    private Vector3 dragOrigin;
    private Vector3 lastCamPos;
    private bool isDragging;

    private void Update()
    {
        playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            lastCamPos = transform.position;

            isDragging = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
        if (isDragging) transform.position = lastCamPos - (Camera.main.ScreenToViewportPoint(Input.mousePosition) - dragOrigin) * 10;
    }

    private void FixedUpdate()
    {
        transform.position += (Vector3)playerInput * Sensitivity;
    }
}
