using UnityEngine;
using UnityEngine.UI;

public class CrosshairManager : MonoBehaviour
{
    [Header("Appearance")]
    public Color crosshairColor =
        Color.white;

    public float size = 8f;

    public float thickness = 2f;

    public float gap = 4f;

    public bool useDot = false;

    private RectTransform top;

    private RectTransform bottom;

    private RectTransform left;

    private RectTransform right;

    private RectTransform dot;

    void Start()
    {
        CreateCrosshair();
    }

    void CreateCrosshair()
    {
        Canvas canvas =
            FindObjectOfType<Canvas>();

        if (canvas == null)
        {
            GameObject canvasObj =
                new GameObject(
                    "Canvas"
                );

            canvas =
                canvasObj.AddComponent<
                    Canvas>();

            canvas.renderMode =
                RenderMode
                .ScreenSpaceOverlay;

            canvasObj.AddComponent<
                CanvasScaler>();

            canvasObj.AddComponent<
                GraphicRaycaster>();
        }

        CreateLine(
            canvas.transform,
            out top,
            new Vector2(
                0,
                gap + size * 0.5f
            ),
            new Vector2(
                thickness,
                size
            )
        );

        CreateLine(
            canvas.transform,
            out bottom,
            new Vector2(
                0,
                -(gap + size * 0.5f)
            ),
            new Vector2(
                thickness,
                size
            )
        );

        CreateLine(
            canvas.transform,
            out left,
            new Vector2(
                -(gap + size * 0.5f),
                0
            ),
            new Vector2(
                size,
                thickness
            )
        );

        CreateLine(
            canvas.transform,
            out right,
            new Vector2(
                gap + size * 0.5f,
                0
            ),
            new Vector2(
                size,
                thickness
            )
        );

        if (useDot)
        {
            CreateLine(
                canvas.transform,
                out dot,
                Vector2.zero,
                new Vector2(
                    thickness * 2f,
                    thickness * 2f
                )
            );
        }
    }

    void CreateLine(
        Transform parent,
        out RectTransform rect,
        Vector2 position,
        Vector2 sizeDelta
    )
    {
        GameObject obj =
            new GameObject(
                "CrosshairPart"
            );

        obj.transform.SetParent(
            parent
        );

        Image image =
            obj.AddComponent<Image>();

        image.color =
            crosshairColor;

        rect =
            obj.GetComponent<
                RectTransform>();

        rect.anchorMin =
            new Vector2(
                0.5f,
                0.5f
            );

        rect.anchorMax =
            new Vector2(
                0.5f,
                0.5f
            );

        rect.pivot =
            new Vector2(
                0.5f,
                0.5f
            );

        rect.anchoredPosition =
            position;

        rect.sizeDelta =
            sizeDelta;
    }
}