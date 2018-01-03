using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereController : MonoBehaviour
{
    public float Torque;
    public Camera Camera;
    private Rigidbody _rigidbody;
    private Vector3 _mousePos;
    private Vector3 _playerToCameraVector;
    private float _initialAngularDrag;

    // Use this for initialization
    void Start()
    {

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.maxAngularVelocity = 15;
        _playerToCameraVector = Camera.transform.position - this.transform.position;
        _mousePos = Input.mousePosition;
        _initialAngularDrag = _rigidbody.angularDrag;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        var mouseDelta = Input.mousePosition - _mousePos;

        var horizontalDelta = mouseDelta.x;

        var rot = Quaternion.Euler(0, horizontalDelta, 0) * _playerToCameraVector;
        
        Camera.transform.SetPositionAndRotation(this.transform.position + rot, Quaternion.identity);
        Camera.transform.LookAt(transform.position);

        var cameraToPlayer = (this.transform.position - Camera.transform.position).normalized;

        if (Input.GetMouseButton(0))
        {
            _rigidbody.AddTorque(cameraToPlayer.z * Torque, 0, -cameraToPlayer.x * Torque);
        }

        if (Input.GetMouseButton(1))
        {
            _rigidbody.AddTorque(-cameraToPlayer.z * Torque, 0, cameraToPlayer.x * Torque);
        }

        if (Input.GetButton("Jump"))
        {
            _rigidbody.angularDrag = 10000;
        }
        else
        {
            _rigidbody.angularDrag = _initialAngularDrag;
        }
        
        Debug.Log(_rigidbody.velocity);
    }
}
