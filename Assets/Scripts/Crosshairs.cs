using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Crosshairs : MonoBehaviour 
{
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private LayerMask _enemyLayerMask;

    [SerializeField] private WeaponController _gunController;
    [SerializeField] private SpriteRenderer _dot;
    [SerializeField] private Color _targetDotColour = Color.red;

    private Color _originalDotColour;

    private void Awake()
    {
        _originalDotColour = _dot.color;
    }

    private void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        UpdatePosition();
        UpdateRotation();
        UpdateDotColour();
    }

    private void UpdateDotColour()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, Mathf.Infinity, _enemyLayerMask))
        {
            _dot.color = _targetDotColour;
        }
        else
        {
            _dot.color = _originalDotColour;
        }
    }

    private void UpdatePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        Plane groundPlane = new Plane(Vector3.up, Vector3.up * _gunController.GunSpawnTransform.position.y);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            transform.position = point;
        }
    }

    private void UpdateRotation()
    {
        transform.Rotate(-40 * Time.deltaTime * Vector3.forward);
    }
}
