using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(Rigidbody))]
public class Grabbable : MonoBehaviour
{
    public XRNode leftHand = XRNode.LeftHand;
    public XRNode rightHand = XRNode.RightHand;

    private InputDevice leftController;
    private InputDevice rightController;
    private InputDevice activeController;

    private Rigidbody rb;
    private bool isGrabbed = false;
    private Transform grabbingHand;

    [Header("Grabbable Settings")]
    public bool usePhysics = true;
    public float followSpeed = 20f;
    public float rotationSpeed = 10f; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
    }

    void Update()
    {
        if (!leftController.isValid || !rightController.isValid)
        {
            InitializeControllers();
        }

        CheckGrabInput(leftController, leftHand);
        CheckGrabInput(rightController, rightHand);

        if (isGrabbed && grabbingHand != null)
        {
            FollowHand();
        }
    }

    private void InitializeControllers()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(leftHand, devices);
        if (devices.Count > 0) leftController = devices[0];

        devices.Clear();
        InputDevices.GetDevicesAtXRNode(rightHand, devices);
        if (devices.Count > 0) rightController = devices[0];
    }

    private void CheckGrabInput(InputDevice controller, XRNode hand)
    {
        if (controller.TryGetFeatureValue(CommonUsages.gripButton, out bool isPressed) && isPressed)
        {
            if (!isGrabbed)
            {
                TryGrab(hand);
            }
        }
        else if (isGrabbed && activeController == controller)
        {
            Release();
        }
    }

    private void TryGrab(XRNode hand)
    {
        activeController = (hand == leftHand) ? leftController : rightController;

        // Find closest hand object
        GameObject handObj = GameObject.Find(hand.ToString());
        if (handObj)
        {
            grabbingHand = handObj.transform;
            isGrabbed = true;

            if (usePhysics)
            {
                rb.isKinematic = true;
            }
        }
    }

    private void FollowHand()
    {
        transform.position = Vector3.Lerp(transform.position, grabbingHand.position, followSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, grabbingHand.rotation, rotationSpeed * Time.deltaTime);
    }

    private void Release()
    {
        isGrabbed = false;
        grabbingHand = null;

        if (usePhysics)
        {
            rb.isKinematic = false;
            if (activeController.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 velocity))
            {
                rb.velocity = velocity * 1.5f; 
            }
        }

        activeController = default;
    }
}
