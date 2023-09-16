using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TagHolder;
using static GameParams;
using System;
using UnityEditor;

public class PlayerController : MonoBehaviour
{
    public static PlayerController SharedInstance;
    [Range(1f, 20f)] float mouseSensitivity;   // Sensitivity of the mouse for camera control
    InvertCamera invertCam;   // Selected camera inversion option
    Vector2 look;   // Vector to store camera rotation
    Vector3 input;
    Transform cam;
    Animator animator;
    AudioSource audioSource;
    bool isPlayingSound;
    CharacterController controller;
    bool gameStopped;
    [SerializeField] DoubleSO sensitivitySO;

    public bool isGameStopped()
    {
        return gameStopped;
    }

    public Transform GetCamera()
    {
        return cam;
    }

    void Awake()
    {
        SharedInstance = this;

        cam = transform.Find("MainCamera");
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        controller = GetComponent<CharacterController>();
    }
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        invertCam = InvertCamera.NORMAL;
        isPlayingSound = false;
        gameStopped = false;
        input = new Vector3();

        mouseSensitivity = (float) sensitivitySO.Value;
    }

    void Update()
    {
        if (!PlayerStats.SharedInstance.PlayerIsDead())
        {
            CursorLockUnlock();

            if (!gameStopped)
            {
                CameraControl();
                Move();
            }
            else
                StopWalkingSound();
        }
    }

    // Check if the cursor should be locked or unlocked based on player input
    void CursorLockUnlock()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                gameStopped = true;
                UI.SharedInstance.ActivateGameStoppedUI();
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                gameStopped = false;
                UI.SharedInstance.DeactivateGameStoppedUI();
            }
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
        input = Vector3.zero;
        input += transform.forward * Input.GetAxis("Vertical");
        input += transform.right * Input.GetAxis("Horizontal");

        if (input != Vector3.zero)
        {
            animator.SetBool(IS_RUNNING, true);
            
            if (!isPlayingSound)
                audioSource.Play();
            isPlayingSound = true;
        }
        
        else
        {
            animator.SetBool(IS_RUNNING, false);

            audioSource.Stop();
            isPlayingSound = false;
        }

        controller.Move(input * speed * Time.deltaTime);
    }

    public void StopWalkingSound()
    {
        audioSource.Stop();
    }
}
