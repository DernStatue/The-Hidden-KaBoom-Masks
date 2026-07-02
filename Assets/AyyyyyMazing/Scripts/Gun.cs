using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;

    public Transform muzzlePoint;

    public LineRenderer tracer;

    public GameObject hitEffectPrefab;

    [Header("Gun")]
    public float damage = 25f;

    public float range = 100f;

    public float fireRate = 8f;

    public int maxAmmo = 12;

    public float reloadTime = 1.5f;

    private int currentAmmo;

    private bool isReloading;

    private float nextFireTime;

    void Start()
    {
        currentAmmo = maxAmmo;

        if (tracer != null)
        {
            tracer.enabled = false;
        }
    }

    void Update()
    {
        if (isReloading)
            return;

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
            Input.GetButton("Fire1") &&
            Time.time >= nextFireTime
        )
        {
            if (currentAmmo > 0)
            {
                nextFireTime =
                    Time.time +
                    1f / fireRate;

                Shoot();
            }
        }
    }

    void Shoot()
    {
        currentAmmo--;

        Vector3 hitPoint =
            playerCamera.transform.position +
            playerCamera.transform.forward *
            range;

        if (
            Physics.Raycast(
                playerCamera.transform.position,
                playerCamera.transform.forward,
                out RaycastHit hit,
                range
            )
        )
        {
            hitPoint =
                hit.point;

            if (
                hitEffectPrefab != null
            )
            {
                GameObject effect =
                    Instantiate(
                        hitEffectPrefab,
                        hit.point,
                        Quaternion.LookRotation(
                            hit.normal
                        )
                    );

                Destroy(
                    effect,
                    2f
                );
            }

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

        if (tracer != null)
        {
            StopAllCoroutines();

            StartCoroutine(
                ShowTracer(
                    muzzlePoint.position,
                    hitPoint
                )
            );
        }
    }

    System.Collections.IEnumerator ShowTracer(
        Vector3 start,
        Vector3 end
    )
    {
        tracer.enabled = true;

        tracer.SetPosition(
            0,
            start
        );

        tracer.SetPosition(
            1,
            end
        );

        yield return new WaitForSeconds(
            0.04f
        );

        tracer.enabled = false;
    }

    System.Collections.IEnumerator Reload()
    {
        isReloading = true;

        yield return new WaitForSeconds(
            reloadTime
        );

        currentAmmo = maxAmmo;

        isReloading = false;
    }
}