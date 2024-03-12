using System.Collections;
using UnityEngine;

public class RotateableWeapon : MonoBehaviour
{
    [SerializeField] private float PCRotationSpeed = 10.0f;
    private Camera cam;

    private void Start()
    {
        cam = CameraManager.Instance.GetMainMenuCamera();
    }

    private void OnMouseDrag()
    {
        float rotX = Input.GetAxis("Mouse X") * PCRotationSpeed;
        float rotY = Input.GetAxis("Mouse Y") * PCRotationSpeed;

        Vector3 right = Vector3.Cross(cam.transform.up, transform.position - cam.transform.position);
        Vector3 up = Vector3.Cross(transform.position - cam.transform.position, right);
        var rotation = transform.rotation;
        rotation = Quaternion.AngleAxis(-rotX, up) * rotation;
        rotation = Quaternion.AngleAxis(rotY, right) * rotation;
        transform.rotation = rotation;
    }
}