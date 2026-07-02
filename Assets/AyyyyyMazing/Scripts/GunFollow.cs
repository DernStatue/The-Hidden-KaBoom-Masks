using UnityEngine;

public class GunFollow : MonoBehaviour
{
    public Transform cameraTransform;

    public float followSpeed = 12f;

    void Update()
    {
        Quaternion targetRotation =
            Quaternion.Euler(
                cameraTransform.eulerAngles.x,
                cameraTransform.eulerAngles.y,
                0f
            );

        transform.rotation =
            Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                followSpeed *
                Time.deltaTime
            );
    }
}