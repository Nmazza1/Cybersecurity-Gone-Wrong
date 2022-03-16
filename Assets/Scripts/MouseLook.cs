using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField]
    Transform plyBody;

    float xRotation = 0;

    float mouseSensitivity = 200;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        plyBody.Rotate(Vector3.up * mouseX);

        if (Input.GetKey(KeyCode.LeftControl))
        {
            transform.localPosition = new Vector3(0, 0, 0);
        }
        else
            transform.localPosition = new Vector3(0, 0.685f, 0);

    }
}
