using UnityEngine;

public class BulletTracer : MonoBehaviour
{
    private LineRenderer line;

    void Awake()
    {
        line =
            GetComponent<LineRenderer>();
    }

    public void Setup(
        Vector3 start,
        Vector3 end
    )
    {
        line.SetPosition(
            0,
            start
        );

        line.SetPosition(
            1,
            end
        );

        Destroy(
            gameObject,
            0.05f
        );
    }
}