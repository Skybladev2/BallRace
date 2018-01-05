using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SphereController : MonoBehaviour
{
    public float Torque;
    public float MaxAngularVelocity;
    public Camera Camera;

    public Text SpeedText;

    private Rigidbody _rigidbody;
    private Vector3 _mousePos;
    private Vector3 _playerToCameraVector;
    private float _initialAngularDrag;

    // Use this for initialization
    void Start()
    {

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.maxAngularVelocity = MaxAngularVelocity;
        _playerToCameraVector = Camera.transform.position - this.transform.position;
        _mousePos = Input.mousePosition;
        _initialAngularDrag = _rigidbody.angularDrag;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForExit();
        CheckForLevelRestart();
        RotateCamera();
        var cameraToPlayer = (this.transform.position - Camera.transform.position).normalized;
        ProcessInput(cameraToPlayer);

        UpdateUI();
    }

    private void CheckForLevelRestart()
    {
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void UpdateUI()
    {
        SpeedText.text = _rigidbody.velocity.magnitude.ToString();
    }

    private void ProcessInput(Vector3 cameraToPlayer)
    {
        ProcessKeyboardInput(cameraToPlayer);
    }

    private void ProcessKeyboardInput(Vector3 cameraToPlayer)
    {
        var vertical = Input.GetAxisRaw("Vertical");
        var horizontal = Input.GetAxisRaw("Horizontal");
        _rigidbody.AddTorque(
            (cameraToPlayer.z * vertical - cameraToPlayer.x * horizontal) * Torque, 
            0, 
            (-cameraToPlayer.x * vertical  - cameraToPlayer.z * horizontal) * Torque);

        if (Input.GetButton("Jump"))
        {
            _rigidbody.angularDrag = 10000;
        }
        else
        {
            _rigidbody.angularDrag = _initialAngularDrag;
        }
    }

    private void RotateCamera()
    {
        var mouseDelta = Input.mousePosition - _mousePos;
        var horizontalDelta = mouseDelta.x;
        var verticalDelta = mouseDelta.y;
        var rot = Quaternion.Euler(-verticalDelta * 0.5f, horizontalDelta, 0) * _playerToCameraVector;

        Camera.transform.SetPositionAndRotation(this.transform.position + rot, Quaternion.identity);
        Camera.transform.LookAt(transform.position);
    }

    private static void CheckForExit()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
