using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VehicleController : MonoBehaviour
{
    [Header("Wheel Setup")]
    public Transform leftFrontWheelTransform;
    public Transform leftBackWheelTransform;
    public Transform RightFrontWheelTransform;
    public Transform RightBackWheelTransform;

    public WheelCollider leftFrontWheelCollider;
    public WheelCollider leftBackWheelCollider;
    public WheelCollider RightFrontWheelCollider;
    public WheelCollider RightBackWheelCollider;

    public float maximumWheelAngle;

    [Header("Pedals")]
    public Image gasPedal;
    public Image brakePedal;

    [Header("Steering Wheel")]
    public Image steeringWheel;
    RectTransform rectT;

    [Header("Steering Settings")]
    public Vector2 steeringWheelCenterPoint;
    public float maximumSteeringAngle;

    [Header("Motor and Brake")]
    private float motorTorqueForce = 5;
    private float motorBrakeForce = 6;

    [Header("Steering Control")]
    private bool steeringHeld;

    private float wheelAngle;
    private float wheelPrevAngle;
    private float wheelReleaseSpeed = 300f;

    private bool isGasPedalPressed;
    private bool isBrakePedalPressed;

    private void Start()
    {
        if(SceneManager.GetActiveScene().name != "Game")
            return;
        
        maximumSteeringAngle = 130f; maximumWheelAngle = 50;
        
        steeringWheel = GameObject.FindWithTag("SteeringWheel").GetComponent<Image>();
        gasPedal = GameObject.FindWithTag("GasPedal").GetComponent<Image>();
        brakePedal = GameObject.FindWithTag("BrakePedal").GetComponent<Image>();

        rectT = steeringWheel.rectTransform;
        steeringWheelCenterPoint = rectT.position;

        SteeringWheelEventSystem();
        GasEventSystem();
        BrakeEventSystem();
    }

    private void Update()
    {
        UpdateSteeringView();
        PedalsController();

        UpdateWheelView(leftBackWheelCollider, leftBackWheelTransform);
        UpdateWheelView(leftFrontWheelCollider, leftFrontWheelTransform);
        UpdateWheelView(RightBackWheelCollider, RightBackWheelTransform);
        UpdateWheelView(RightFrontWheelCollider, RightFrontWheelTransform);
    }

    private void UpdateSteeringView()
    {
        if (!steeringHeld)
        {
            CameraController.Instance.isFreeLookCameraActive = true;
            float delta = wheelReleaseSpeed * Time.deltaTime;

            if (Math.Abs(delta) > Mathf.Abs(wheelAngle))
                wheelAngle = 0f;
            else if (wheelAngle > 0f)
                wheelAngle -= delta;
            else
                wheelAngle += delta;

            leftFrontWheelCollider.steerAngle = wheelAngle;
            RightFrontWheelCollider.steerAngle = wheelAngle;
            leftFrontWheelTransform.rotation = Quaternion.Euler(new Vector3(0, wheelAngle, 0));
            RightFrontWheelTransform.rotation = Quaternion.Euler(new Vector3(0, wheelAngle, 0));
        }
        else
            CameraController.Instance.isFreeLookCameraActive = false;
            

        if (SceneManager.GetActiveScene().name == "Game")
            rectT.localEulerAngles = Vector3.back * wheelAngle;
    }

    private void PedalsController()
    {
        if (isGasPedalPressed)
        {
            CameraController.Instance.isFreeLookCameraActive = false;
            if(RightBackWheelCollider.rpm < 
                GameManager.Instance.carInformations[PlayerPrefs.GetInt("ChosenCar")].maxSpeed)
            {
                float acceleration = GameManager.Instance.carInformations[PlayerPrefs.GetInt("ChosenCar")].acceleration;
                
                leftBackWheelCollider.motorTorque = acceleration * motorTorqueForce;
                RightBackWheelCollider.motorTorque = acceleration * motorTorqueForce;
                motorTorqueForce += 0.3f;
            }         
        }
        else if (isBrakePedalPressed)
        {
            CameraController.Instance.isFreeLookCameraActive = false;
            float braking = GameManager.Instance.carInformations[PlayerPrefs.GetInt("ChosenCar")].braking;

            leftBackWheelCollider.brakeTorque = braking * motorBrakeForce;
            RightBackWheelCollider.brakeTorque = braking * motorBrakeForce;
            motorBrakeForce += 0.6f;
        }
        else if (!steeringHeld)
        {
            CameraController.Instance.isFreeLookCameraActive = true;
        }
    }

    private void UpdateWheelView(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 position;
        Quaternion rotation;
        wheelCollider.GetWorldPose(out position, out rotation);
        wheelTransform.position = position;
        wheelTransform.rotation = rotation;
    }

    private void GasEventSystem()
    {
        EventTrigger eventTrigger = gasPedal.GetComponent<EventTrigger>();

        if (eventTrigger == null)
            eventTrigger = eventTrigger.AddComponent<EventTrigger>();

        if (eventTrigger.triggers == null)
            eventTrigger.triggers = new List<EventTrigger.Entry>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener(GasPressEvent);
        eventTrigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener(GasReleaseEvent);
        eventTrigger.triggers.Add (entry);
    }

    private void GasPressEvent(BaseEventData eventData)
    {
        isGasPedalPressed = true;
    }

    private void GasReleaseEvent(BaseEventData eventData)
    {
        leftBackWheelCollider.motorTorque = 0;
        RightBackWheelCollider.motorTorque = 0;
        motorTorqueForce = 5;
        isGasPedalPressed = false;
    }

    private void BrakeEventSystem()
    {
        EventTrigger eventTrigger = brakePedal.GetComponent<EventTrigger>();

        if (eventTrigger == null)
            eventTrigger = eventTrigger.AddComponent<EventTrigger>();

        if (eventTrigger.triggers == null)
            eventTrigger.triggers = new List<EventTrigger.Entry>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener(BrakePressEvent);
        eventTrigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener(BrakeReleaseEvent);
        eventTrigger.triggers.Add(entry);
    }

    private void BrakePressEvent(BaseEventData arg0)
    {
        isBrakePedalPressed = true;
    }

    private void BrakeReleaseEvent(BaseEventData arg0)
    {
        leftBackWheelCollider.brakeTorque = 0;
        RightBackWheelCollider.brakeTorque = 0;
        motorBrakeForce = 6;
        isBrakePedalPressed = false;
    }

    private void SteeringWheelEventSystem()
    {
        EventTrigger eventTrigger = steeringWheel.GetComponent<EventTrigger>();

        if (eventTrigger == null)
            eventTrigger = eventTrigger.AddComponent<EventTrigger>();

        if(eventTrigger.triggers == null)
            eventTrigger.triggers = new List<EventTrigger.Entry>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener(SteeringWheelPressEvent);
        eventTrigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener(SteeringWheelReleaseEvent);
        eventTrigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Drag;
        entry.callback.AddListener(SteeringWheelDragEvent);
        eventTrigger.triggers.Add(entry);
    }
    private void SteeringWheelPressEvent(BaseEventData eventData)
    {
        steeringHeld = true;

        Vector2 pointerPos = ((PointerEventData)eventData).position;
        wheelPrevAngle = Vector2.SignedAngle(Vector2.up, pointerPos - steeringWheelCenterPoint);
    }
    private void SteeringWheelReleaseEvent(BaseEventData eventData)
    {
        steeringHeld = false;
        SteeringWheelDragEvent(eventData);
    }
    private void SteeringWheelDragEvent(BaseEventData eventData)
    {
        Vector2 pointerPos = ((PointerEventData)eventData).position;
        
        float newAngle = Vector2.SignedAngle(Vector2.up, pointerPos - steeringWheelCenterPoint);

        wheelAngle = Mathf.Clamp(wheelAngle, -maximumSteeringAngle, maximumSteeringAngle);

        if (wheelAngle == maximumSteeringAngle)
        {
            if(pointerPos.x - steeringWheelCenterPoint.x > 0)
            {
                wheelAngle -= newAngle - wheelPrevAngle;
                wheelPrevAngle = newAngle;

                leftFrontWheelCollider.steerAngle = maximumWheelAngle;
                RightFrontWheelCollider.steerAngle = maximumWheelAngle;

                leftFrontWheelTransform.rotation = Quaternion.Euler(new Vector3(0, maximumWheelAngle, 0));
                RightFrontWheelTransform.rotation = Quaternion.Euler(new Vector3(0, maximumWheelAngle, 0));
            }
        }
        else if(wheelAngle == -maximumSteeringAngle)
        {
            if (pointerPos.x - steeringWheelCenterPoint.x < 0)
            {
                wheelAngle -= newAngle - wheelPrevAngle;
                wheelPrevAngle = newAngle;

                leftFrontWheelCollider.steerAngle = -maximumWheelAngle;
                RightFrontWheelCollider.steerAngle = -maximumWheelAngle;

                leftFrontWheelTransform.rotation = Quaternion.Euler(new Vector3(0, -maximumWheelAngle, 0));
                RightFrontWheelTransform.rotation = Quaternion.Euler(new Vector3(0, -maximumWheelAngle, 0));
            }
        }
        else
        {
            wheelAngle -= newAngle - wheelPrevAngle;
            wheelPrevAngle = newAngle;

            leftFrontWheelCollider.steerAngle = wheelAngle / 2.5f;
            RightFrontWheelCollider.steerAngle = wheelAngle / 2.5f;

            leftFrontWheelTransform.rotation = Quaternion.Euler(new Vector3(0, wheelAngle / 2.5f, 0));
            RightFrontWheelTransform.rotation = Quaternion.Euler(new Vector3(0, wheelAngle / 2.5f, 0));
        }
    }
}