using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public float MovementKeySensitivity;
    public float ScrollSensitivity;

    public Vector2 ScrollRange;

    private Vector2 playerInput;
    private Vector3 dragOrigin;
    private Vector3 lastCamPos;

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
            Vector3 dragVec = Camera.main.ScreenToViewportPoint(Input.mousePosition) - dragOrigin;
            dragVec = new Vector2(dragVec.x * Camera.main.orthographicSize * 2 * Camera.main.aspect, dragVec.y * Camera.main.orthographicSize * 2);
            transform.position = lastCamPos - dragVec;
        }

        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - Input.mouseScrollDelta.y, ScrollRange.x, ScrollRange.y);
        transform.position += (Vector3)playerInput * MovementKeySensitivity * Time.deltaTime;
    }
}
