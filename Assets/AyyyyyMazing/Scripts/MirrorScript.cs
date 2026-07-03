using UnityEngine;

public class SimpleMirror : MonoBehaviour
{
    public Transform playerCamera;

    public Camera mirrorCamera;

    void LateUpdate()
    {
        Vector3 relativePos =
            transform.InverseTransformPoint(
                playerCamera.position
            );

        relativePos.x *= -1;

        mirrorCamera.transform.position =
            transform.TransformPoint(
                relativePos
            );

        Vector3 relativeForward =
            transform.InverseTransformDirection(
                playerCamera.forward
            );

        relativeForward.x *= -1;

        mirrorCamera.transform.rotation =
            Quaternion.LookRotation(
                transform.TransformDirection(
                    relativeForward
                ),
                Vector3.up
            );
    }
}