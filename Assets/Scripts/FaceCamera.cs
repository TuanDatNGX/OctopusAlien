using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Camera mainCamera;

    void Update()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;  // Automatically find the main camera if not assigned
        }

        // Make the HP bar face the camera
        Vector3 direction = mainCamera.transform.position - transform.position;
        direction.y = 0; // Optional: if you don't want the HP bar to tilt up/down
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}