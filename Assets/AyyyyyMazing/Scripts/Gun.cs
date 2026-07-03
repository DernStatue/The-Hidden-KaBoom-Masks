using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Gun")]
    public int maxAmmo = 30;

    public int currentAmmo;

    public float fireRate = 0.07f;

    public float reloadTime = 1.3f;

    public bool isReloading;

    [Header("Shooting")]
    public Transform shootPoint;

    public GameObject bulletPrefab;

    public float bulletForce = 50f;

    public float damage = 25f;

    public float range = 200f;

    [Header("Audio")]
    public AudioClip shootSound;

    public AudioClip reloadSound;

    public AudioClip emptySound;

    [Header("Recoil")]
    public float recoilKick = 4f;

    public float recoilReturnSpeed = 8f;

    public float recoilSnappiness = 14f;

    [Header("Sway")]
    public float swayAmount = 4f;

    public float maxSwayAmount = 6f;

    public float swaySmooth = 8f;

    public float moveSwayAmount = 0.04f;

    public float moveSwaySmooth = 6f;

    float nextFireTime;

    Vector3 initialPosition;

    Quaternion initialRotation;

    Vector3 currentRotation;

    Vector3 targetRotation;

    void Start()
    {
        currentAmmo = maxAmmo;

        initialPosition =
            transform.localPosition;

        initialRotation =
            transform.localRotation;
    }

    void Update()
    {
        if (
            PauseMenu.IsPaused
        )
        {
            return;
        }

        if (isReloading)
        {
            return;
        }

        HandleSway();

        HandleRecoil();

        if (
            Input.GetKeyDown(
                KeyCode.R
            )
        )
        {
            StartCoroutine(
                Reload()
            );

            return;
        }

        if (
            Input.GetButton(
                "Fire1"
            ) &&
            Time.time >=
            nextFireTime
        )
        {
            nextFireTime =
                Time.time +
                fireRate;

            Shoot();
        }
    }

    void HandleSway()
    {
        float mouseX =
            Input.GetAxis(
                "Mouse X"
            );

        float mouseY =
            Input.GetAxis(
                "Mouse Y"
            );

        float moveX =
            Input.GetAxisRaw(
                "Horizontal"
            );

        float moveY =
            Input.GetAxisRaw(
                "Vertical"
            );

        float swayX =
            Mathf.Clamp(
                -mouseX *
                swayAmount,
                -maxSwayAmount,
                maxSwayAmount
            );

        float swayY =
            Mathf.Clamp(
                mouseY *
                swayAmount,
                -maxSwayAmount,
                maxSwayAmount
            );

        Quaternion target =
            initialRotation *
            Quaternion.Euler(
                swayY,
                swayX,
                swayX
            );

        transform.localRotation =
            Quaternion.Slerp(
                transform.localRotation,
                target *
                Quaternion.Euler(
                    currentRotation
                ),
                swaySmooth *
                Time.deltaTime
            );

        Vector3 targetPos =
            initialPosition +
            new Vector3(
                -moveX *
                moveSwayAmount,
                -moveY *
                moveSwayAmount,
                0f
            );

        transform.localPosition =
            Vector3.Lerp(
                transform.localPosition,
                targetPos,
                moveSwaySmooth *
                Time.deltaTime
            );
    }

    void HandleRecoil()
    {
        targetRotation =
            Vector3.Lerp(
                targetRotation,
                Vector3.zero,
                recoilReturnSpeed *
                Time.deltaTime
            );

        currentRotation =
            Vector3.Slerp(
                currentRotation,
                targetRotation,
                recoilSnappiness *
                Time.deltaTime
            );
    }

    void Shoot()
    {
        if (
            currentAmmo <= 0
        )
        {
            PlayGunSound(
                emptySound,
                0.7f
            );

            return;
        }

        currentAmmo--;

        targetRotation +=
            new Vector3(
                -recoilKick,
                Random.Range(
                    -1f,
                    1f
                ),
                Random.Range(
                    -0.5f,
                    0.5f
                )
            );

        PlayGunSound(
            shootSound,
            1f
        );

        RaycastHit hit;

        if (
            Physics.Raycast(
                shootPoint.position,
                shootPoint.forward,
                out hit,
                range
            )
        )
        {
            EnemyHealth enemy =
                hit.collider
                    .GetComponentInParent<
                        EnemyHealth>();

            if (enemy != null)
            {
                enemy.TakeDamage(
                    damage
                );
            }
        }

        if (
            bulletPrefab != null
        )
        {
            GameObject bullet =
                Instantiate(
                    bulletPrefab,
                    shootPoint.position,
                    shootPoint.rotation
                );

            Rigidbody rb =
                bullet.GetComponent<
                    Rigidbody>();

            if (rb != null)
            {
                rb.linearVelocity =
                    shootPoint.forward *
                    bulletForce;
            }

            Destroy(
                bullet,
                3f
            );
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;

        PlayGunSound(
            reloadSound,
            0.9f
        );

        yield return new WaitForSeconds(
            reloadTime
        );

        currentAmmo = maxAmmo;

        isReloading = false;
    }

    void PlayGunSound(
        AudioClip clip,
        float volume = 1f
    )
    {
        if (clip == null)
        {
            return;
        }

        GameObject soundObj =
            new GameObject(
                "GunSound"
            );

        soundObj.transform.position =
            transform.position;

        AudioSource source =
            soundObj.AddComponent<
                AudioSource>();

        source.clip = clip;

        source.volume = volume;

        source.pitch =
            Random.Range(
                0.96f,
                1.04f
            );

        source.spatialBlend = 0f;

        source.Play();

        Destroy(
            soundObj,
            clip.length
        );
    }
}