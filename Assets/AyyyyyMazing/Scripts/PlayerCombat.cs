using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Mask State")]
    public bool hasBoomMask = false;

    [Header("Objects")]
    public GameObject boomMaskObject;

    public GameObject gunObject;

    void Start()
    {
        if (boomMaskObject != null)
        {
            boomMaskObject.SetActive(false);
        }

        if (gunObject != null)
        {
            gunObject.SetActive(false);
        }
    }

    public void UnlockBoomMask()
    {
        hasBoomMask = true;

        if (boomMaskObject != null)
        {
            boomMaskObject.SetActive(true);
        }

        if (gunObject != null)
        {
            gunObject.SetActive(true);
        }

        Debug.Log(
            "Boom Mask unlocked!"
        );
    }
}