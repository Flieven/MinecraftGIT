using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Camera : MonoBehaviour
{
    #region Variables

    private Camera PlayerCamera = null;

    [SerializeField] private float RotationSpeed = 50.0f;
    [SerializeField] private bool InvertVertical = false;
    [SerializeField] private bool InvertHorizontal = false;

    [SerializeField] private float MinimumRotationAngle = -80;
    [SerializeField] private float MaximumRotationAngle = 80;

    #endregion

    private void Awake()
    {
        if(!PlayerCamera)
        {
            if(Camera.main) { PlayerCamera = Camera.main; }
            else
            {
                GameObject newObj = new GameObject();
                newObj.name = "PlayerCamera";
                newObj.transform.parent = transform;
                newObj.AddComponent<Camera>();
                PlayerCamera = newObj.GetComponent<Camera>();
            }
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PlayerCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("Player"));

    }

    private void Update()
    {
        PlayerCamera.transform.position = transform.position;
        PlayerCamera.transform.rotation = transform.rotation;
        CameraMovement();
    }

    private void CameraMovement()
    {
        if (Input.GetAxis("Mouse X") != 0) { transform.parent.Rotate(0, (Input.GetAxis("Mouse X") * RotationSpeed * Time.deltaTime) * (InverseMovement(InvertHorizontal)), 0); }

        if (Input.GetAxis("Mouse Y") != 0) { transform.Rotate((Input.GetAxis("Mouse Y") * RotationSpeed * Time.deltaTime) * InverseMovement(InvertVertical), 0, 0); }

        float z = transform.eulerAngles.z;
        transform.Rotate(0, 0, -z);
    }

    private int InverseMovement(bool inverter)
    {
        return (inverter == true ? -1 : 1);
    }
}
