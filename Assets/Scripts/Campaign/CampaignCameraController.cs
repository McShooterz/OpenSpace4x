/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[AddComponentMenu("Campaign Camera Controller")]
public class CampaignCameraController : MonoBehaviour
{
    [SerializeField]
    Transform m_Transform;

    #region Movement

    [SerializeField]
    float keyboardMovementSpeed = 5f; //speed with keyboard movement

    [SerializeField]
    float screenEdgeMovementSpeed = 3f; //spee with screen edge movement

    [SerializeField]
    float followingSpeed = 5f; //speed when following a target

    [SerializeField]
    float rotationSped = 3f;

    [SerializeField]
    float panningSpeed = 10f;

    [SerializeField]
    float mouseRotationSpeed = 10f;

    #endregion

    #region Height

    [SerializeField]
    bool autoHeight = true;

    [SerializeField]
    LayerMask groundMask = -1; //layermask of ground or other objects that affect height

    [SerializeField]
    float maxHeight = 10f; //maximal height

    [SerializeField]
    float minHeight = 15f; //minimnal height

    [SerializeField]
    float heightDampening = 5f;

    [SerializeField]
    float keyboardZoomingSensitivity = 2f;

    [SerializeField]
    float scrollWheelZoomingSensitivity = 25f;

    [SerializeField]
    float zoomPos = 0; //value in range (0, 1) used as t in Matf.Lerp

    #endregion

    #region MapLimits

    [SerializeField]
    bool limitMap = true;

    [SerializeField]
    float limitX = 50f; //x limit of map

    [SerializeField]
    float limitY = 50f; //z limit of map

    #endregion

    #region Input

    [SerializeField]
    bool useScreenEdgeInput = true;

    [SerializeField]
    float screenEdgeBorder = 25f;

    [SerializeField]
    bool useKeyboardInput = true;

    [SerializeField]
    string horizontalAxis = "Horizontal";

    [SerializeField]
    string verticalAxis = "Vertical";

    [SerializeField]
    bool usePanning = true;

    [SerializeField]
    KeyCode panningKey = KeyCode.Mouse2;

    [SerializeField]
    bool useScrollwheelZooming = true;

    [SerializeField]
    string zoomingAxis = "Mouse ScrollWheel";

    [SerializeField]
    bool useKeyboardRotation = true;

    [SerializeField]
    bool useMouseRotation = true;

    [SerializeField]
    KeyCode mouseRotationKey = KeyCode.Mouse1;

    Vector2 KeyboardInput
    {
        get { return useKeyboardInput ? new Vector2(Input.GetAxis(horizontalAxis), Input.GetAxis(verticalAxis)) : Vector2.zero; }
    }

    Vector2 MouseInput
    {
        get { return Input.mousePosition; }
    }

    float ScrollWheel
    {
        get { return Input.GetAxis(zoomingAxis); }
    }

    Vector2 MouseAxis
    {
        get { return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); }
    }

    #endregion

    #region Unity_Methods

    void Start()
    {
        m_Transform = transform;
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        CameraUpdate();
    }

    #endregion

    #region RTSCamera_Methods

    /// <summary>
    /// update camera movement and rotation
    /// </summary>
    void CameraUpdate()
    {
        Move();
        HeightCalculation();
        Rotation();
        LimitPosition();
    }

    /// <summary>
    /// move camera with keyboard or with screen edge
    /// </summary>
    void Move()
    {
        if (useKeyboardInput)
        {
            Vector3 desiredMove = new Vector3(KeyboardInput.x, 0, KeyboardInput.y);

            desiredMove *= keyboardMovementSpeed;
            desiredMove *= Time.deltaTime;
            desiredMove = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f)) * desiredMove;
            desiredMove = m_Transform.InverseTransformDirection(desiredMove);

            m_Transform.Translate(desiredMove, Space.Self);
        }

        if (useScreenEdgeInput)
        {
            Vector3 desiredMove = new Vector3();

            Rect leftRect = new Rect(0, 0, screenEdgeBorder, Screen.height);
            Rect rightRect = new Rect(Screen.width - screenEdgeBorder, 0, screenEdgeBorder, Screen.height);
            Rect upRect = new Rect(0, Screen.height - screenEdgeBorder, Screen.width, screenEdgeBorder);
            Rect downRect = new Rect(0, 0, Screen.width, screenEdgeBorder);

            desiredMove.x = leftRect.Contains(MouseInput) ? -1 : rightRect.Contains(MouseInput) ? 1 : 0;
            desiredMove.z = upRect.Contains(MouseInput) ? 1 : downRect.Contains(MouseInput) ? -1 : 0;

            desiredMove *= screenEdgeMovementSpeed;
            desiredMove *= Time.deltaTime;
            desiredMove = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f)) * desiredMove;
            desiredMove = m_Transform.InverseTransformDirection(desiredMove);

            m_Transform.Translate(desiredMove, Space.Self);
        }

        if (usePanning && Input.GetKey(panningKey) && MouseAxis != Vector2.zero)
        {
            Vector3 desiredMove = new Vector3(-MouseAxis.x, 0, -MouseAxis.y);

            desiredMove *= panningSpeed;
            desiredMove *= Time.deltaTime;
            desiredMove = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f)) * desiredMove;
            desiredMove = m_Transform.InverseTransformDirection(desiredMove);

            m_Transform.Translate(desiredMove, Space.Self);
        }
    }

    /// <summary>
    /// calcualte height
    /// </summary>
    void HeightCalculation()
    {
        if (useScrollwheelZooming)
            zoomPos += ScrollWheel * Time.deltaTime * scrollWheelZoomingSensitivity;

        zoomPos = Mathf.Clamp01(zoomPos);

        float targetHeight = Mathf.Lerp(minHeight, maxHeight, zoomPos);
        float difference = 0;

        if (transform.position.y != targetHeight)
            difference = targetHeight - transform.position.y;

        m_Transform.position = Vector3.Lerp(m_Transform.position,
            new Vector3(m_Transform.position.x, targetHeight + difference, m_Transform.position.z), Time.deltaTime * heightDampening);
    }

    /// <summary>
    /// rotate camera
    /// </summary>
    void Rotation()
    {
        if (useMouseRotation && Input.GetKey(mouseRotationKey))
            m_Transform.Rotate(Vector3.up, -MouseAxis.x * Time.deltaTime * mouseRotationSpeed, Space.World);
    }

    /// <summary>
    /// limit camera position
    /// </summary>
    void LimitPosition()
    {
        if (!limitMap)
            return;

        m_Transform.position = new Vector3(Mathf.Clamp(m_Transform.position.x, -limitX, limitX),
            m_Transform.position.y,
            Mathf.Clamp(m_Transform.position.z, -limitY, limitY));
    }

    #endregion
}
