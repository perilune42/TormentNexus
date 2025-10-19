using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Assumptions:
// Main camera is at max orthographic size
// Map background bounds >= camera bounds
public class CameraController : MonoBehaviour
{
    public float MovementKeySensitivity;
    public float ScrollSensitivity;

    public Vector2 ScrollRange;

    private Vector2 playerInput;
    private Vector3 dragOrigin;
    private Vector3 lastCamPos;
    private float camMapDiffX;

    private List<Collider2D> nodeColliders = new();
    private List<Transform> nodeStartTransforms;

    [SerializeField] private SpriteRenderer backgroundSR;
    [SerializeField] private Camera leftCam;
    [SerializeField] private Camera rightCam;
    [SerializeField] private Transform mapNodeHolder;

    private void Awake()
    {
        UnitController.Instance.OnAddUnit += AddUnitCollider;
        UnitController.Instance.OnRemoveUnit += RemoveUnitCollider;
    }

    private void Start()
    {
        AddNodeColliders(Map.Instance.Nodes);


        camMapDiffX = backgroundSR.sprite.bounds.max.x - Camera.main.orthographicSize * Camera.main.aspect;
        leftCam.transform.localPosition = Vector3.right * (Camera.main.orthographicSize * Camera.main.aspect + camMapDiffX) * 2;
        rightCam.transform.localPosition = Vector3.left * (Camera.main.orthographicSize * Camera.main.aspect + camMapDiffX) * 2;
    }
    
    private void Update()
    {
        leftCam.orthographicSize = Camera.main.orthographicSize;
        rightCam.orthographicSize = Camera.main.orthographicSize;

        // WASD
        playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        transform.position += MovementKeySensitivity * Time.deltaTime * (Vector3)playerInput;
        
        // Scroll
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - Input.mouseScrollDelta.y, ScrollRange.x, ScrollRange.y);

        // Dragging
        if (Input.GetMouseButtonDown(2))
        {
            DragStart();
        }
        if (Input.GetMouseButton(2))
        {
            Vector3 dragVec = Camera.main.ScreenToViewportPoint(Input.mousePosition) - dragOrigin;
            dragVec = new Vector2(dragVec.x * Camera.main.orthographicSize * 2 * Camera.main.aspect, dragVec.y * Camera.main.orthographicSize * 2);
            transform.position = lastCamPos - dragVec;
        }

        // Wrapping
        float wrapX = backgroundSR.sprite.bounds.max.x;
        if (transform.position.x > wrapX)
        {
            transform.position = new Vector3(-wrapX, transform.position.y, transform.position.z);
            DragStart();
        }
        else if (transform.position.x < -wrapX)
        {
            transform.position = new Vector3(wrapX, transform.position.y, transform.position.z);
            DragStart();
        }

        // "Mouse Wrapping"
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mouseWorldPos.x > wrapX)
        {
            for (int i =  0; i < nodeColliders.Count; i++)
            {
                nodeColliders[i].offset = Vector3.right * wrapX * 2;
            }
        }
        else if (mouseWorldPos.x < -wrapX)
        {
            for (int i = 0; i < nodeColliders.Count; i++)
            {
                nodeColliders[i].offset = Vector3.left * wrapX * 2;
            }
        }
        else
        {
            for (int i = 0; i < nodeColliders.Count; i++)
            {
                nodeColliders[i].offset = Vector3.zero;
            }
        }

        // Clamp Y
        float maxY = backgroundSR.sprite.bounds.max.y - Camera.main.orthographicSize;
        float clampedY = Mathf.Clamp(transform.position.y, -maxY, maxY);
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, clampedY, Camera.main.transform.position.z);
    }

    private void AddNodeColliders(List<MapNode> nodes)
    {
        foreach (MapNode node in nodes)
        {
            nodeColliders.Add(node.GetComponent<Collider2D>());
        }
    }

    public void AddUnitCollider(Unit unit)
    {
        nodeColliders.Add(unit.GetComponentInChildren<Collider2D>());
    }

    public void RemoveUnitCollider(Unit unit)
    {
        nodeColliders.Remove(unit.GetComponentInChildren<Collider2D>());
    }

    private void DragStart()
    {
        dragOrigin = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        lastCamPos = transform.position;
    }
}
