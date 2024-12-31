using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Camera mainCamera;

    void LateUpdate()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;  // Automatically find the main camera if not assigned
        }

        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 180, 0);
    }
}