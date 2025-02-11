using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class XRControllerInteractor : MonoBehaviour
{
    public XRNode controllerNode = XRNode.RightHand; // Change to LeftHand if needed
    private InputDevice controllerDevice;
    private bool canRestartGame = false;

    [Header("Controller State")]
    public bool isTriggerPressed;
    public bool isGripPressed;
    public bool isPrimaryButtonPressed;
    public Vector3 controllerPosition;
    public Quaternion controllerRotation;

    void Start()
    {
        GameManager.Instance.OnWinGame += OnWinGame;
        InitializeController();
    }

    private void OnWinGame()
    {
       canRestartGame = true;
    }

    private void Update()
    {
        if (!controllerDevice.isValid)
        {
            InitializeController();
        }

        UpdateControllerTransform();
        HandleInput();
    }

    private void InitializeController()
    {
        var inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(controllerNode, inputDevices);

        if (inputDevices.Count > 0)
        {
            controllerDevice = inputDevices[0];
            Debug.Log($"Controller initialized: {controllerDevice.name}");
        }
    }

    private void UpdateControllerTransform()
    {
        if (controllerDevice.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position))
        {
            controllerPosition = position;
            transform.position = position;
        }

        if (controllerDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation))
        {
            controllerRotation = rotation;
            transform.rotation = rotation;
        }
    }

    private void HandleInput()
    {
        // Detect trigger press
        if (controllerDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerPressed))
        {
            if (triggerPressed && !isTriggerPressed)
            {
                Debug.Log("Trigger Pressed");
            }
            else if (!triggerPressed && isTriggerPressed)
            {
                Debug.Log("Trigger Released");
            }
            isTriggerPressed = triggerPressed;
        }

        // Detect grip button press
        if (controllerDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool gripPressed))
        {
            if (gripPressed && !isGripPressed)
            {
                Debug.Log("Grip Pressed");
            }
            else if (!gripPressed && isGripPressed)
            {
                Debug.Log("Grip Released");
            }
            isGripPressed = gripPressed;
        }
        
        if (controllerDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool PrimaryButtonPressed))
        {
            if (PrimaryButtonPressed && !isPrimaryButtonPressed)
            {
                if(canRestartGame)
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else if (!PrimaryButtonPressed && isPrimaryButtonPressed)
            {
                Debug.Log("Grip Released");
            }
            isPrimaryButtonPressed = PrimaryButtonPressed;
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnWinGame -= OnWinGame;
    }
}