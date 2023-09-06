using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TagHolder;
using static GameParams;
using System;
using UnityEditor;

public class PlayerController : MonoBehaviour
{
    [Range(0f, 90f)][SerializeField] float yRotationLimit = 90f;   // Limit for vertical camera rotation
    [Range(1f, 10f)][SerializeField] float mouseSensitivity = 5f;   // Sensitivity of the mouse for camera control
    [SerializeField] InvertCamera invertCam;   // Selected camera inversion option
    Vector2 look;   // Vector to store camera rotation
    Vector3 movement; // Vector to store movement info
    Transform cam;
    
    void Start()
    {
        cam = transform.Find("MainCamera");
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Check if the cursor should be locked or unlocked
        CursorLockUnlock();

        // control the camera
        CameraControl();

        // Move the player
        Move();
    }

    // Check if the cursor should be locked or unlocked based on player input
    void CursorLockUnlock()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Control the camera rotation based on mouse input
    void CameraControl()
    {
        look.x += Input.GetAxis("Mouse X") * mouseSensitivity;
        look.y += Input.GetAxis("Mouse Y") * mouseSensitivity;
        look.y = Mathf.Clamp(look.y, -yRotationLimit, yRotationLimit);

        transform.localRotation = Quaternion.Euler(transform.rotation.x, look.x, transform.rotation.z);
        cam.localRotation = Quaternion.Euler(look.y * (float) invertCam, cam.rotation.y, cam.rotation.z);
    }

     // Move the player based on player input
    void Move()
    {
        movement.z = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        movement.x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        transform.Translate(movement);
    }
}
