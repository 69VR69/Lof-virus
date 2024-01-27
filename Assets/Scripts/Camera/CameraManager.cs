using DG.Tweening;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Camera Manager")]
    [SerializeField] private Transform _cameraCenter;
    [SerializeField] private Camera _camera;

    [Header("Camera Settings")]
    [SerializeField] [Range(10, 30)] private float _cameraRotationSpeed = 80f;
    [SerializeField] private Vector2 _limitYCameraAngle = new (-30, 10);
    [SerializeField] [Range(5, 50)] private float _minZoomDistance = 10f;
    [SerializeField] [Range(5, 50)] private float _maxZoom = 100f;

    private Vector3 _cameraCenterRotationTarget;

    // Start is called before the first frame update
    void Start()
    {
        if (_cameraCenter == null)
        {
            Debug.LogError("Target is null in Camera Manager :(");
        }

        if (_camera == null)
        {
            Debug.LogError("Camera is null in Camera Manager :(");
        }
        _cameraCenterRotationTarget = new(_cameraCenter.localEulerAngles.x, _cameraCenter.localEulerAngles.y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                MoveTargetTo(hit.point);
            }
        }

        // Move Target with arrow keys
        var moveTarget = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * 10f;
        if (moveTarget != Vector3.zero)
        {
            MoveTargetTo(_cameraCenter.position + moveTarget);
        }


        var holdMouse = Input.GetMouseButton(2);
        var holdAlt = Input.GetKey(KeyCode.LeftAlt);
        UnityEngine.Cursor.lockState = holdMouse ? CursorLockMode.Locked : UnityEngine.Cursor.lockState = CursorLockMode.None;

        if (holdMouse || holdAlt)
        {
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                var mouseMovement = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0);
                SphereRotationCamera(mouseMovement);
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            var distance = Vector3.Distance(_cameraCenter.position, _camera.transform.position);
            var scroll = Input.GetAxis("Mouse ScrollWheel");
            var zoom = distance - scroll * 10f;
            zoom = Mathf.Clamp(zoom, _minZoomDistance, _maxZoom);
            _camera.transform.position = _cameraCenter.position - _camera.transform.forward * zoom;
        }

        // Press R to zoom in, F to zoom out
        var zoomInButtonPressed = Input.GetKeyDown(KeyCode.R);
        var zoomOutButtonPressed = Input.GetKeyDown(KeyCode.F);

        if (zoomInButtonPressed || zoomOutButtonPressed)
        {
            var distance = Vector3.Distance(_cameraCenter.position, _camera.transform.position);
            var zoom = distance + (zoomInButtonPressed ? -1 : 1) * 10f;
            zoom = Mathf.Clamp(zoom, _minZoomDistance, _maxZoom);
            _camera.transform.DOMove(_cameraCenter.position - _camera.transform.forward * zoom, .5f);
        }

        if (_cameraCenter.localEulerAngles != _cameraCenterRotationTarget)
        {
            _cameraCenter.DOLocalRotate(
                _cameraCenterRotationTarget,
                .01f
            );
        }
    }

    public void MoveTargetTo(Vector3 position)
    {
        _cameraCenter.DOMove(position, .5f).SetEase(Ease.Linear);
    }

    public void SphereRotationCamera(Vector3 mouseMovement)
    {
        _cameraCenterRotationTarget += new Vector3(-mouseMovement.y, mouseMovement.x, 0) * _cameraRotationSpeed * 20f * Time.deltaTime;
        _cameraCenterRotationTarget.x = Mathf.Clamp(_cameraCenterRotationTarget.x, _limitYCameraAngle.x, _limitYCameraAngle.y);
    }
}
