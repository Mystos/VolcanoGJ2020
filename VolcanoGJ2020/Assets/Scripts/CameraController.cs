﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform mainCamera;
    public float panSpeed = 20f;
    public float rotSpeed = 20f;
    public float panBorderThickness = 10f;
    public Vector2 panLimit;
    public float scrollSpeed = 20f;
    public float minY = 20f;
    public float maxY = 80f;
    public float minRotX = 20f;
    public float maxRotX = 90f;
    public bool ActivateBorderControls = true;
    public Vector3 startPos;
    public float startRotY;

    private void Start()
    {
        transform.position = startPos;
        Quaternion rotParent = transform.rotation;
        Quaternion rotMain = mainCamera.rotation;
        rotMain.eulerAngles = new Vector3(rotMain.eulerAngles.x, startRotY, rotMain.eulerAngles.z);
        rotParent.eulerAngles = new Vector3(rotParent.eulerAngles.x, startRotY, rotParent.eulerAngles.z);
        transform.rotation = rotParent;
        mainCamera.rotation = rotMain;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        Quaternion rotParent = transform.rotation;
        Quaternion rotMain = mainCamera.rotation;


        if ( Input.GetKey(KeyCode.Z) || (ActivateBorderControls? Input.mousePosition.y >= Screen.height - panBorderThickness: false))
        {
            pos += panSpeed * Time.deltaTime * transform.forward;
        }
        if (Input.GetKey(KeyCode.S) || (ActivateBorderControls? Input.mousePosition.y <= panBorderThickness: false))
        {
            pos += panSpeed * Time.deltaTime * -transform.forward;
        }
        if (Input.GetKey(KeyCode.D) || (ActivateBorderControls? Input.mousePosition.x >= Screen.width - panBorderThickness : false))
        {
            pos += panSpeed * Time.deltaTime * transform.right;
        }
        if (Input.GetKey(KeyCode.Q) || (ActivateBorderControls? Input.mousePosition.x <= panBorderThickness: false))
        {
            pos += panSpeed * Time.deltaTime * -transform.right;
        }
        if (Input.GetKey(KeyCode.E))
        {
            rotMain.eulerAngles = new Vector3(rotMain.eulerAngles.x, rotMain.eulerAngles.y + rotSpeed * Time.deltaTime, rotMain.eulerAngles.z);
            rotParent.eulerAngles = new Vector3(rotParent.eulerAngles.x, rotParent.eulerAngles.y + rotSpeed * Time.deltaTime, rotParent.eulerAngles.z);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rotMain.eulerAngles = new Vector3(rotMain.eulerAngles.x, rotMain.eulerAngles.y - rotSpeed * Time.deltaTime, rotMain.eulerAngles.z);
            rotParent.eulerAngles = new Vector3(rotParent.eulerAngles.x, rotParent.eulerAngles.y - rotSpeed * Time.deltaTime, rotParent.eulerAngles.z);
        }


        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        float rotX = Mathf.Clamp(pos.y, minRotX, maxRotX);
        rotMain.eulerAngles = new Vector3(rotX, rotMain.eulerAngles.y, rotMain.eulerAngles.z);
        transform.position = pos;
        transform.rotation = rotParent;
        mainCamera.rotation = rotMain;
    }
}
