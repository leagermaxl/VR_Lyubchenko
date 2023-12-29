using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSystem : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private bool useEdgeScrolling = false;
    [SerializeField] private bool useDragPan = false;
    [SerializeField] private float fieldOfViewMax = 50;
    [SerializeField] private float fieldOfViewMin = 10;

    private bool dragPanMoveActive;
    private Vector2 lastMousePosition;
    private float targetFieldOfView = 50f;

    private void Update()
    {
        HandleCameraMovement();

        if (useEdgeScrolling)
        {
            HandleCameraMovementEdgeScrolling();
        }
        if (useDragPan)
        {
            HandleCameraMovementDragPen();
        }

        HandleCameraRotation();

        HandleCameraZoom();
    }

    private void HandleCameraMovement()
    {
        Vector3 inputDir = new Vector3(0, 0, 0);
            
        if (Input.GetKey(KeyCode.W)) inputDir.z = +1f;
        if (Input.GetKey(KeyCode.S)) inputDir.z = -1f;
        if (Input.GetKey(KeyCode.A)) inputDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) inputDir.x = +1f;

        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;

        float moveSpeed = 50f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private void HandleCameraMovementEdgeScrolling()
    {
        Vector3 inputDir = new Vector3(0, 0, 0);

        int edgeScrollingSize = 20;

        if (Input.mousePosition.x < edgeScrollingSize)
        {
            inputDir.x = -1f;
        }
        if (Input.mousePosition.y < edgeScrollingSize)
        {
            inputDir.z = -1f;
        }
        if (Input.mousePosition.x > Screen.width - edgeScrollingSize)
        {
            inputDir.x = +1f;
        }
        if (Input.mousePosition.y > Screen.height - edgeScrollingSize)
        {
            inputDir.z = +1f;
        }
        
        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;

        float moveSpeed = 50f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private void HandleCameraMovementDragPen() 
    {
        Vector3 inputDir = new Vector3(0, 0, 0);

        if (Input.GetMouseButtonDown(1))
        {
            dragPanMoveActive = true;
            lastMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(1))
        {
            dragPanMoveActive = false;
        }

        if (dragPanMoveActive)
        {
            Vector2 mouseMovementDelta = (Vector2)Input.mousePosition - lastMousePosition;

            float dragPanSpeed = 1f;
            inputDir.x = mouseMovementDelta.x * dragPanSpeed;
            inputDir.z = mouseMovementDelta.y * dragPanSpeed;

            lastMousePosition = Input.mousePosition;
        }

        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;

        float moveSpeed = 50f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private void HandleCameraRotation() 
    {
        float rotateDir = 0f;
        if (Input.GetKey(KeyCode.Q)) rotateDir = -1f;
        if (Input.GetKey(KeyCode.E)) rotateDir = +1f;

        float rotateSpeed = 300f;
        transform.eulerAngles += new Vector3(0, rotateDir * rotateSpeed * Time.deltaTime, 0);
    }
    private void HandleCameraZoom()
    {
        if (Input.mouseScrollDelta.y < 0)
        {
            targetFieldOfView += 5;
        }
        if (Input.mouseScrollDelta.y > 0)
        {
            targetFieldOfView -= 5;
        }

        targetFieldOfView = Mathf.Clamp(targetFieldOfView, fieldOfViewMin, fieldOfViewMax);

        float zoomSpeed = 10f;
        cinemachineVirtualCamera.m_Lens.FieldOfView = Mathf.Lerp(cinemachineVirtualCamera.m_Lens.FieldOfView, targetFieldOfView, Time.deltaTime * zoomSpeed);
    }
}
