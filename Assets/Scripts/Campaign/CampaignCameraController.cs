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

    [SerializeField]
    bool useFixedUpdate = false;

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

    #region Targeting

    [SerializeField]
    Transform targetFollow; //target to follow

    [SerializeField]
    Vector3 targetOffset;

    /// <summary>
    /// are we following target
    /// </summary>
    public bool FollowingTarget
    {
        get
        {
            return targetFollow != null;
        }
    }

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
    bool useKeyboardZooming = true;

    [SerializeField]
    KeyCode zoomInKey = KeyCode.E;

    [SerializeField]
    KeyCode zoomOutKey = KeyCode.Q;

    [SerializeField]
    bool useScrollwheelZooming = true;

    [SerializeField]
    string zoomingAxis = "Mouse ScrollWheel";

    [SerializeField]
    bool useKeyboardRotation = true;

    [SerializeField]
    KeyCode rotateRightKey = KeyCode.X;

    [SerializeField]
    KeyCode rotateLeftKey = KeyCode.Z;

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

    int ZoomDirection
    {
        get
        {
            bool zoomIn = Input.GetKey(zoomInKey);
            bool zoomOut = Input.GetKey(zoomOutKey);
            if (zoomIn && zoomOut)
                return 0;
            else if (!zoomIn && zoomOut)
                return 1;
            else if (zoomIn && !zoomOut)
                return -1;
            else
                return 0;
        }
    }

    int RotationDirection
    {
        get
        {
            bool rotateRight = Input.GetKey(rotateRightKey);
            bool rotateLeft = Input.GetKey(rotateLeftKey);
            if (rotateLeft && rotateRight)
                return 0;
            else if (rotateLeft && !rotateRight)
                return -1;
            else if (!rotateLeft && rotateRight)
                return 1;
            else
                return 0;
        }
    }

    #endregion

    #region Unity_Methods

    void Start()
    {
        m_Transform = transform;
    }

    void Update()
    {
        if (!useFixedUpdate)
        {
            CameraUpdate();
        }
    }

    void FixedUpdate()
    {
        if (useFixedUpdate)
        {
            CameraUpdate();
        }
    }

    #endregion

    #region RTSCamera_Methods

    /// <summary>
    /// update camera movement and rotation
    /// </summary>
    void CameraUpdate()
    {
        if (FollowingTarget)
            FollowTarget();
        else
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
        float distanceToGround = DistanceToGround();
        if (useScrollwheelZooming)
            zoomPos += ScrollWheel * Time.deltaTime * scrollWheelZoomingSensitivity;
        if (useKeyboardZooming)
            zoomPos += ZoomDirection * Time.deltaTime * keyboardZoomingSensitivity;

        zoomPos = Mathf.Clamp01(zoomPos);

        float targetHeight = Mathf.Lerp(minHeight, maxHeight, zoomPos);
        float difference = 0;

        if (distanceToGround != targetHeight)
            difference = targetHeight - distanceToGround;

        m_Transform.position = Vector3.Lerp(m_Transform.position,
            new Vector3(m_Transform.position.x, targetHeight + difference, m_Transform.position.z), Time.deltaTime * heightDampening);
    }

    /// <summary>
    /// rotate camera
    /// </summary>
    void Rotation()
    {
        if (useKeyboardRotation)
            transform.Rotate(Vector3.up, RotationDirection * Time.deltaTime * rotationSped, Space.World);

        if (useMouseRotation && Input.GetKey(mouseRotationKey))
            m_Transform.Rotate(Vector3.up, -MouseAxis.x * Time.deltaTime * mouseRotationSpeed, Space.World);
    }

    /// <summary>
    /// follow targetif target != null
    /// </summary>
    void FollowTarget()
    {
        Vector3 targetPos = new Vector3(targetFollow.position.x, m_Transform.position.y, targetFollow.position.z) + targetOffset;
        m_Transform.position = Vector3.MoveTowards(m_Transform.position, targetPos, Time.deltaTime * followingSpeed);
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

    /// <summary>
    /// set the target
    /// </summary>
    /// <param name="target"></param>
    public void SetTarget(Transform target)
    {
        targetFollow = target;
    }

    /// <summary>
    /// reset the target (target is set to null)
    /// </summary>
    public void ResetTarget()
    {
        targetFollow = null;
    }

    /// <summary>
    /// calculate distance to ground
    /// </summary>
    /// <returns></returns>
    float DistanceToGround()
    {
        Ray ray = new Ray(m_Transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, groundMask.value))
            return (hit.point - m_Transform.position).magnitude;

        return 0f;
    }

    #endregion
}
