using System;
using UnityEngine;

public class Crosshairs : MonoBehaviour 
{
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private LayerMask _enemyLayerMask;

    [SerializeField] private GunController _gunController;
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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _groundLayerMask))
        {
            float newY = _gunController.GunSpawnTransform.position.y;
            transform.position = new Vector3(hitInfo.point.x, newY, hitInfo.point.z);
        }
    }

    private void UpdateRotation()
    {
        transform.Rotate(-40 * Time.deltaTime * Vector3.forward);
    }
}
