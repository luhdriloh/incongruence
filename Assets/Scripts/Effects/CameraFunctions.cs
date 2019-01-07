using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFunctions : MonoBehaviour
{
    public GameObject _target;
    public float _cameraMaxOffset;
    public float _cameraSpeed;

    private Vector3 _mousePosition;
    private Vector3 _targetPosition;
    private Vector3 _velocityReference;
    private Vector3 _offset;
    private Vector3 _oldOffset;
    private float _zStart;

    public void Recoil(Vector2 direction, float magnitude, float length)
    { 
    }

	private void Start ()
    {
        _zStart = transform.position.z;
	}
	
	private void Update ()
    {
        _mousePosition = GetMousePosition();
        _targetPosition = GetTargetPosition();

        UpdateCameraPosition();
	}

    private Vector3 GetMousePosition()
    {
        Vector2 position = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        position *= 2;
        position -= Vector2.one;
        float max = 1f;

        if (Mathf.Abs(position.x) > max)
        {
            position.x = position.x < 0 ? -1 : 1;
        }

        if (Mathf.Abs(position.y) > max)
        {
            position.y = position.y < 0 ? -1 : 1;
        }

        return position;
    }

    private Vector3 GetTargetPosition()
    {
        // new offset should change based on the camera position and old offset
        _oldOffset = _offset;
        _offset = Vector3.Lerp(_oldOffset, _mousePosition * _cameraMaxOffset, Time.deltaTime * _cameraSpeed);

        Vector3 newCameraPosition = _target.transform.position + _offset;
        newCameraPosition.z = _zStart;

        return newCameraPosition;
    }

    private void UpdateCameraPosition()
    {
        transform.position = _targetPosition;
    }
}
