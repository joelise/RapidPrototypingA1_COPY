using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLook : MonoBehaviour
{
    //public Transform cameraPivot;
    //public float sensitivity = 2f;

    public InputAction LookAction;
    public float sensitivity = 1.5f;

    private float xRotation = 0f;
    private Transform player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        player = transform.parent;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    private void OnEnable()
    {
        LookAction.Enable();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDisable()
    {
        LookAction.Disable();
        Cursor.lockState = CursorLockMode.None;
    }


    // Update is called once per frame
    void Update()
    {
        Vector2 lookInput = LookAction.ReadValue<Vector2>();

        xRotation -= lookInput.y * sensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        player.Rotate(Vector3.up * lookInput.x * sensitivity);
    }
}
