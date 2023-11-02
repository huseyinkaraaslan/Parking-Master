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
                freeLookCamera.m_XAxis.m_InputAxisName = "Mouse X";
                freeLookCamera.m_YAxis.m_InputAxisName = "Mouse Y";
            }
            else
            {
                freeLookCamera.m_XAxis.m_InputAxisName = "";
                freeLookCamera.m_YAxis.m_InputAxisName = "";
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
