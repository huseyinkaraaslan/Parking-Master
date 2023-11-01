using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; set; }
    
    [Header("Camera Settings")]
    public GameObject cameraFolder;
    public CinemachineFreeLook freeLookCamera;
    
    [Header("Camera State")]
    private bool _isFreeLookCameraActive;
    
    public bool isFreeLookCameraActive
    {
        set
        {
            _isFreeLookCameraActive = value;
            if (_isFreeLookCameraActive)
            {
                freeLookCamera.GetComponent<CinemachineTouchInputMapper>().TouchXInputMapTo = "Mouse X";
                freeLookCamera.GetComponent<CinemachineTouchInputMapper>().TouchYInputMapTo = "Mouse Y";
            }
            else
            {
                freeLookCamera.GetComponent<CinemachineTouchInputMapper>().TouchXInputMapTo = "null";
                freeLookCamera.GetComponent<CinemachineTouchInputMapper>().TouchYInputMapTo = "null";
            }
        }
    }

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name != "Game")
            GetComponent<CameraController>().enabled = false;
    }
    
    private void Start()
    {
        Instance = this;
        cameraFolder.SetActive(true);
    }
}
