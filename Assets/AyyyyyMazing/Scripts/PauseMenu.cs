using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private GameObject canvasObj;

    private GameObject panel;

    private bool paused;

    void Start()
    {
        CreateUI();
    }

    void Update()
    {
        if (
            Input.GetKeyDown(
                KeyCode.Escape
            )
        )
        {
            TogglePause();
        }
    }

    void CreateUI()
    {
        canvasObj =
            new GameObject("Canvas");

        Canvas canvas =
            canvasObj.AddComponent<Canvas>();

        canvas.renderMode =
            RenderMode.ScreenSpaceOverlay;

        canvasObj.AddComponent<
            CanvasScaler>();

        canvasObj.AddComponent<
            GraphicRaycaster>();

        if (
    FindObjectOfType<EventSystem>() == null
)
        {
            GameObject eventSystem =
                new GameObject("EventSystem");

            eventSystem.AddComponent<EventSystem>();

            eventSystem.AddComponent<
                StandaloneInputModule>();
        }

        panel =
            new GameObject("PausePanel");

        panel.transform.SetParent(
            canvasObj.transform,
            false
        );

        Image panelImage =
            panel.AddComponent<Image>();

        panelImage.color =
            new Color(
                0f,
                0f,
                0f,
                0.75f
            );

        RectTransform panelRect =
            panel.GetComponent<
                RectTransform>();

        panelRect.anchorMin =
            Vector2.zero;

        panelRect.anchorMax =
            Vector2.one;

        panelRect.offsetMin =
            Vector2.zero;

        panelRect.offsetMax =
            Vector2.zero;

        CreateButton(
            "Resume",
            new Vector2(0, 50),
            ResumeGame
        );

        CreateButton(
            "Quit",
            new Vector2(0, -50),
            QuitGame
        );

        panel.SetActive(false);
    }

    void CreateButton(
        string text,
        Vector2 position,
        UnityEngine.Events.UnityAction action
    )
    {
        GameObject buttonObj =
            new GameObject(text);

        buttonObj.transform.SetParent(
            panel.transform,
            false
        );

        Image image =
            buttonObj.AddComponent<Image>();

        image.color = Color.white;

        Button button =
            buttonObj.AddComponent<Button>();

        button.onClick.AddListener(
            action
        );

        RectTransform rect =
            buttonObj.GetComponent<
                RectTransform>();

        rect.sizeDelta =
            new Vector2(200, 60);

        rect.anchoredPosition =
            position;

        GameObject textObj =
            new GameObject("Text");

        textObj.transform.SetParent(
            buttonObj.transform,
            false
        );

        Text buttonText =
            textObj.AddComponent<Text>();

        buttonText.text = text;

        buttonText.font =
            Resources.GetBuiltinResource<Font>(
                "LegacyRuntime.ttf"
            );

        buttonText.color =
            Color.black;

        buttonText.alignment =
            TextAnchor.MiddleCenter;

        RectTransform textRect =
            textObj.GetComponent<
                RectTransform>();

        textRect.anchorMin =
            Vector2.zero;

        textRect.anchorMax =
            Vector2.one;

        textRect.offsetMin =
            Vector2.zero;

        textRect.offsetMax =
            Vector2.zero;
    }

    void TogglePause()
    {
        paused = !paused;

        panel.SetActive(paused);

        Time.timeScale =
            paused ? 0f : 1f;

        Cursor.lockState =
            paused ?
            CursorLockMode.None :
            CursorLockMode.Locked;

        Cursor.visible = paused;
    }

    void ResumeGame()
    {
        TogglePause();
    }

    void QuitGame()
    {
        Application.Quit();
    }
}