/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class CombatCameraMover
{
    float mousePanMultiplier = 0.1f;
    float mouseRotationMultiplier = 0.2f;
    float mouseZoomMultiplier = 5.0f;

    float minZoomDistance = 1.0f;
    float maxZoomDistance = 30.0f;
    float smoothingFactor = 0.1f;
    float goToSpeed = 0.1f;

    GameObject followTarget;
    Vector3 cameraTarget;
    float currentCameraDistance;
    Vector3 lastMousePos = Vector3.zero;
    Vector3 lastPanSpeed = Vector3.zero;
    Vector3 goingToCameraTarget = Vector3.zero;
    bool doingAutoMovement = false;

    public CombatCameraMover()
    {
        currentCameraDistance = minZoomDistance + ((maxZoomDistance - minZoomDistance) / 2.0f);
    }

    // Update is called once per frame
    public void Update()
    {
        UpdatePanning();
        UpdateRotation();
        UpdateZooming();
        UpdatePosition();
        UpdateAutoMovement();
        ConfineToPlayArea();
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Reset();
        }
        lastMousePos = Input.mousePosition;
    }

    public void GoTo(Vector3 position)
    {
        doingAutoMovement = true;
        goingToCameraTarget = position;
        followTarget = null;
    }

    public void SetFollowTarget(GameObject gameObjectToFollow)
    {
        followTarget = gameObjectToFollow;
    }

    void UpdatePanning()
    {
        Vector3 moveVector = Vector3.zero;

        //Keyboard movement
        if (Input.GetKey(KeyCode.A))
        {
            moveVector.x -= 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveVector.z -= 1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveVector.x += 1f;
        }
        if (Input.GetKey(KeyCode.W))
        {
            moveVector.z += 1f;
        }
        //Screen edge movement
        if (Input.mousePosition.x < ResourceManager.gameConstants.CameraBoarderArea)
        {
            moveVector.x -= 1f;
        }
        else if (Input.mousePosition.x > Screen.width - ResourceManager.gameConstants.CameraBoarderArea)
        {
            moveVector.x += 1f;
        }
        if (Input.mousePosition.y < ResourceManager.gameConstants.CameraBoarderArea)
        {
            moveVector.z -= 1f;
        }
        else if (Input.mousePosition.y > Screen.height - ResourceManager.gameConstants.CameraBoarderArea)
        {
            moveVector.z += 1f;
        }

        if (Input.GetMouseButton(2) && Input.GetKey(KeyCode.LeftShift))
        {
            Vector3 deltaMousePos = (Input.mousePosition - lastMousePos);
            moveVector += new Vector3(-deltaMousePos.x, 0, -deltaMousePos.y) * mousePanMultiplier;
        }
        
        if (moveVector != Vector3.zero)
        {
            followTarget = null;
            doingAutoMovement = false;
        }

        Vector3 effectivePanSpeed = moveVector;
        effectivePanSpeed = Vector3.Lerp(lastPanSpeed, moveVector, smoothingFactor);
        lastPanSpeed = effectivePanSpeed;

        float oldXRotation = Camera.main.transform.localEulerAngles.x;

        // Set the local X rotation to 0;
        Camera.main.transform.localEulerAngles = new Vector3(0, Camera.main.transform.localEulerAngles.y, Camera.main.transform.localEulerAngles.z);

        float panMultiplier = Mathf.Sqrt(currentCameraDistance);
        cameraTarget = cameraTarget + Camera.main.transform.TransformDirection(effectivePanSpeed) * ResourceManager.gameConstants.CameraSpeed * panMultiplier * Time.deltaTime;

        // Set the old x rotation.
        Camera.main.transform.localEulerAngles = new Vector3(oldXRotation, Camera.main.transform.localEulerAngles.y, Camera.main.transform.localEulerAngles.z);
    }

    void UpdateRotation()
    {
        float deltaAngleH = 0.0f;
        float deltaAngleV = 0.0f;


        //Keyboard rotation
        if (Input.GetKey(KeyCode.Q))
        {
            deltaAngleH = 1.0f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            deltaAngleH = -1.0f;
        }

        //Mouse rotation
        if (Input.GetMouseButton(2) && !Input.GetKey(KeyCode.LeftShift))
        {
            Vector3 deltaMousePos = (Input.mousePosition - lastMousePos);
            deltaAngleH += deltaMousePos.x * mouseRotationMultiplier;
            deltaAngleV -= deltaMousePos.y * mouseRotationMultiplier;
        }

        float x = Mathf.Min(88.0f, Mathf.Max(0.0f, Camera.main.transform.localEulerAngles.x + deltaAngleV * Time.deltaTime * ResourceManager.gameConstants.CameraRotateRate));
        float y = Camera.main.transform.localEulerAngles.y + deltaAngleH * Time.deltaTime * ResourceManager.gameConstants.CameraRotateRate;
        Camera.main.transform.localEulerAngles = new Vector3(x, y, Camera.main.transform.localEulerAngles.z);
    }

    void UpdateZooming()
    {
        float deltaZoom = 0.0f;

        //Keyboard zoom
        if (Input.GetKey(KeyCode.F))
        {
            deltaZoom = 1.0f;
        }
        if (Input.GetKey(KeyCode.R))
        {
            deltaZoom = -1.0f;
        }

        //Mouse zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        deltaZoom -= scroll * mouseZoomMultiplier;
        float zoomedOutRatio = (currentCameraDistance - minZoomDistance) / (maxZoomDistance - minZoomDistance);
        currentCameraDistance = Mathf.Max(minZoomDistance, Mathf.Min(maxZoomDistance, currentCameraDistance + deltaZoom * Time.deltaTime * ResourceManager.gameConstants.CameraZoomRate * (zoomedOutRatio * 2.0f + 1.0f)));
    }

    void UpdatePosition()
    {
        if (followTarget != null)
        {
            cameraTarget = Vector3.Lerp(cameraTarget, followTarget.transform.position, goToSpeed);
        }

        Camera.main.transform.position = cameraTarget;
        Camera.main.transform.Translate(Vector3.back * currentCameraDistance);
    }

    void UpdateAutoMovement()
    {
        if (doingAutoMovement)
        {
            cameraTarget = Vector3.Lerp(cameraTarget, goingToCameraTarget, goToSpeed);
            if (Vector3.Distance(goingToCameraTarget, cameraTarget) < 1.0f)
            {
                doingAutoMovement = false;
            }
        }
    }

    void ConfineToPlayArea()
    {
        Vector3 position = Camera.main.transform.position;
        if (position.z > ResourceManager.gameConstants.CameraLimitUp)
        {
            position.z = ResourceManager.gameConstants.CameraLimitUp;
        }
        else if (position.z < ResourceManager.gameConstants.CameraLimitDown)
        {
            position.z = ResourceManager.gameConstants.CameraLimitDown;
        }
        if (position.x > ResourceManager.gameConstants.CameraLimitRight)
        {
            position.x = ResourceManager.gameConstants.CameraLimitRight;
        }
        else if (position.x < ResourceManager.gameConstants.CameraLimitLeft)
        {
            position.x = ResourceManager.gameConstants.CameraLimitLeft;
        }
        Camera.main.transform.position = position;
    }

    void Reset()
    {
        currentCameraDistance = minZoomDistance + ((maxZoomDistance - minZoomDistance) / 2.0f);
        Camera.main.transform.localEulerAngles = new Vector3(60f, 0, 0);
    }
}
