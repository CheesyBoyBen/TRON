using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class bikeScript : MonoBehaviour
{

    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject handle;
    public GameObject handle2;
    public GameObject VROrigin;
    public GameObject PlayerHolder;
    public GameObject BikeHolder;

    public GameObject throttle;
    bool throttleGrabbed;
    float throttleVal;

    public float maxSpeed;
    public float curSpeed;
    public float acceleration;
    public float deceleration;
    public float turnSpeed;

    [SerializeField]
    private InputActionReference trigger;
    [SerializeField]
    private InputActionReference brake;
    [SerializeField]
    private InputActionReference shake;

    public float fov;
    public float temp;

    private void Awake()
    {
        //trigger.action.started += temp;
    }

    private void OnDestroy()
    {
        //trigger.action.started -= temp;

    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
        




        Vector3 midPoint = (leftHand.transform.localPosition + rightHand.transform.localPosition) * 0.5f;
        Quaternion rot = Quaternion.LookRotation(leftHand.transform.localPosition - rightHand.transform.localPosition, Vector3.up);
        handle.transform.position = midPoint;
        handle.transform.rotation = rot;

        handle2.transform.localEulerAngles = new Vector3(0, (handle.transform.localEulerAngles.x * -1) + 90, 0);
        if (handle2.transform.localEulerAngles.y > 135)
        {
            handle2.transform.localEulerAngles = new Vector3(0, 135, 0);
        }
        else if (handle2.transform.localEulerAngles.y < 45)
        {
            handle2.transform.localEulerAngles = new Vector3(0, 45, 0);

        }
        PlayerHolder.transform.localEulerAngles = new Vector3(0, 0, (handle2.transform.localEulerAngles.y - 90) * (curSpeed / maxSpeed) * -0.5f);
        BikeHolder.transform.localEulerAngles = new Vector3(0, 0, (handle2.transform.localEulerAngles.y - 90) * (curSpeed / maxSpeed) * -0.3f);

        if (throttleGrabbed)
        {
            if (throttle.transform.localEulerAngles.z <= 60 && throttle.transform.localEulerAngles.z >= 0)
            {
                throttleVal = ((throttle.transform.localEulerAngles.z) / 60);
            }
        }
        else
        {
            throttle.transform.localEulerAngles = new Vector3(0, -90, 0);

            throttleVal = (trigger.action.ReadValue<float>());            
        }
        throttle.transform.localPosition = new Vector3(0.25f, 1.35f, 0.4f);
        VROrigin.transform.localPosition = new Vector3(0f, 0f, 0f);
        throttle.transform.localEulerAngles = new Vector3(0f, -90f, throttle.transform.localEulerAngles.z);

        if (throttleVal > 0.05f)
        {
            curSpeed = Mathf.Clamp(curSpeed + (acceleration * throttleVal), 0, maxSpeed);

        }
        else
        {
            curSpeed = Mathf.Clamp(curSpeed - deceleration - brake.action.ReadValue<float>(), 0, maxSpeed);
        }
        transform.eulerAngles += new Vector3(0, ((handle2.transform.localEulerAngles.y - 90) / 45) * (turnSpeed *curSpeed) * Time.deltaTime, 0);

        transform.position += transform.forward * curSpeed * Time.deltaTime;


    }

    public void ThrottleGrabbed()
    {
        throttleGrabbed = true;

        throttle.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;

    }

    public void ThrottleReleased()
    {
        throttleGrabbed = false;

        //throttle.transform.localEulerAngles = new Vector3(0, -90, 0);

        throttle.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }



}

