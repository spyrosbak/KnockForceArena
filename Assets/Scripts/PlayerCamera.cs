using Unity.Cinemachine;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float lookSensitivity = 2.0f;
    [SerializeField] private float maxLookAngle = 80.0f;
    private CinemachineCamera cinemachinCamera;
    private Vector2 currentRotation;
    
    public bool isInitialized;

    public Vector3 forward => Quaternion.Euler(currentRotation.x, currentRotation.y, 0f) * Vector3.forward;

    private void Awake()
    {
        cinemachinCamera = GetComponent<CinemachineCamera>();
        cinemachinCamera.Priority.Value = -1;
    }

    public void Init()
    {
        isInitialized = true;
        cinemachinCamera.Priority.Value = 10;
    }

    private void LateUpdate()
    {
        if (!isInitialized)
            return;

        float mouseX = InputManager.Instance.lookAction.ReadValue<Vector2>().x * lookSensitivity;
        float mouseY = InputManager.Instance.lookAction.ReadValue<Vector2>().y * lookSensitivity;

        currentRotation.x = Mathf.Clamp(currentRotation.x - mouseY, -maxLookAngle, maxLookAngle);
        currentRotation.y += mouseX;

        transform.localRotation = Quaternion.Euler(currentRotation.x, 0f, 0f);
    }
}