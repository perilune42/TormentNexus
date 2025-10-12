using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public float MovementKeySensitivity;
    public float DragSensitivity;
    public float ScrollSensitivity;

    public Vector2 ScrollRange;

    private Vector2 playerInput;
    private Vector3 dragOrigin;
    private Vector3 lastCamPos;

    private Vector2 scrollDelta;

    private void Update()
    {
        playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            lastCamPos = transform.position;
        }
        if (Input.GetMouseButton(0)) 
        {
            transform.position = lastCamPos - (Camera.main.ScreenToViewportPoint(Input.mousePosition) - dragOrigin) * 10;
        }

        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - Input.mouseScrollDelta.y, ScrollRange.x, ScrollRange.y);
    }

    private void FixedUpdate()
    {
        transform.position += (Vector3)playerInput * MovementKeySensitivity;
    }
}
