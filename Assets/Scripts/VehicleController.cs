using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VehicleController : MonoBehaviour
{
    public Image steeringWheel;
    RectTransform rectT;

    private bool wheelHeld;
    public Vector2 centerPoint;

    private float wheelAngle = 0f;
    private float wheelPrevAngle = 0f;

    private float wheelReleaseSpeed = 720;
    private float maximumSteeringAngle = 720;

    private void Start()
    {
        steeringWheel = GameObject.FindWithTag("SteeringWheel").GetComponent<Image>();
        rectT = steeringWheel.rectTransform;
        centerPoint = rectT.position;

        wheelHeld = false;
        InitEventSystem();
    }

    private void Update()
    {
        if(!wheelHeld)
        {
            float delta = wheelReleaseSpeed * Time.deltaTime;

            if (Math.Abs(delta) > Mathf.Abs(wheelAngle))
                wheelAngle = 0f;
            else if (wheelAngle > 0f)
                wheelAngle -= delta;
            else
                wheelAngle += delta;
        }

        rectT.localEulerAngles = Vector3.back * wheelAngle;

    }

    private void InitEventSystem()
    {
        EventTrigger eventTrigger = steeringWheel.GetComponent<EventTrigger>();

        if (eventTrigger == null)
            eventTrigger = eventTrigger.AddComponent<EventTrigger>();

        if(eventTrigger.triggers == null)
            eventTrigger.triggers = new List<EventTrigger.Entry>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener(PressEvent);
        eventTrigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener(ReleaseEvent);
        eventTrigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Drag;
        entry.callback.AddListener(DragEvent);
        eventTrigger.triggers.Add(entry);
    }


    private void PressEvent(BaseEventData eventData)
    {
        wheelHeld = true;

        Vector2 pointerPos = ((PointerEventData)eventData).position;
        wheelPrevAngle = Vector2.SignedAngle(Vector2.up, pointerPos - centerPoint);
    }

    private void ReleaseEvent(BaseEventData eventData)
    {
        wheelHeld = false;
        DragEvent(eventData);
    }
    private void DragEvent(BaseEventData eventData)
    {
        Vector2 pointerPos = ((PointerEventData)eventData).position;

        float newAngle = Vector2.SignedAngle(Vector2.up, pointerPos - centerPoint);

        if (Vector2.Distance(pointerPos, centerPoint) > 25f)
        {
            wheelAngle -= newAngle - wheelPrevAngle;
            wheelPrevAngle = newAngle;
        }

        wheelAngle = Mathf.Clamp(wheelAngle, -maximumSteeringAngle, maximumSteeringAngle);
    }
}