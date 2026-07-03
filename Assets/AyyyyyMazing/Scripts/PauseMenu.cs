using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused;

    Canvas canvas;

    GameObject panel;

    GameObject settingsPanel;

    Slider volumeSlider;

    Slider sensitivitySlider;

    void Start()
    {
        CreateCanvas();

        CreatePausePanel();

        CreateSettingsPanel();

        panel.SetActive(false);

        settingsPanel.SetActive(false);
    }

    void Update()
    {
        if (
            Input.GetKeyDown(
                KeyCode.Escape
            )
        )
        {
            if (!IsPaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

    void CreateCanvas()
    {
        GameObject obj =
            new GameObject(
                "PauseCanvas"
            );

        canvas =
            obj.AddComponent<Canvas>();

        canvas.renderMode =
            RenderMode.ScreenSpaceOverlay;

        obj.AddComponent<
            CanvasScaler>();

        obj.AddComponent<
            GraphicRaycaster>();

        if (
            FindFirstObjectByType<
                EventSystem
            >() == null
        )
        {
            GameObject eventObj =
                new GameObject(
                    "EventSystem"
                );

            eventObj.AddComponent<
                EventSystem>();

            eventObj.AddComponent<
                StandaloneInputModule>();
        }
    }

    void CreatePausePanel()
    {
        panel =
            CreatePanel(
                "PausePanel"
            );

        CreateText(
            panel.transform,
            "PAUSED",
            new Vector2(0, 220),
            52
        );

        CreateButton(
            panel.transform,
            "RESUME",
            new Vector2(0, 100),
            Resume
        );

        CreateButton(
            panel.transform,
            "SETTINGS",
            new Vector2(0, 0),
            OpenSettings
        );

        CreateButton(
            panel.transform,
            "QUIT",
            new Vector2(0, -100),
            QuitGame
        );
    }

    void CreateSettingsPanel()
    {
        settingsPanel =
            CreatePanel(
                "SettingsPanel"
            );

        CreateText(
            settingsPanel.transform,
            "SETTINGS",
            new Vector2(0, 220),
            52
        );

        CreateText(
            settingsPanel.transform,
            "MASTER VOLUME",
            new Vector2(0, 120),
            28
        );

        volumeSlider =
            CreateSlider(
                settingsPanel.transform,
                new Vector2(0, 70),
                0f,
                1f,
                AudioListener.volume
            );

        volumeSlider.onValueChanged
            .AddListener(
                SetVolume
            );

        CreateText(
            settingsPanel.transform,
            "MOUSE SENSITIVITY",
            new Vector2(0, -20),
            28
        );

        sensitivitySlider =
            CreateSlider(
                settingsPanel.transform,
                new Vector2(0, -70),
                0.5f,
                10f,
                2f
            );

        sensitivitySlider.onValueChanged
            .AddListener(
                SetSensitivity
            );

        CreateButton(
            settingsPanel.transform,
            "BACK",
            new Vector2(0, -220),
            CloseSettings
        );
    }

    GameObject CreatePanel(
        string name
    )
    {
        GameObject obj =
            new GameObject(name);

        obj.transform.SetParent(
            canvas.transform
        );

        Image img =
            obj.AddComponent<Image>();

        img.color =
            new Color(
                0,
                0,
                0,
                0.88f
            );

        RectTransform rect =
            obj.GetComponent<
                RectTransform>();

        rect.anchorMin =
            Vector2.zero;

        rect.anchorMax =
            Vector2.one;

        rect.offsetMin =
            Vector2.zero;

        rect.offsetMax =
            Vector2.zero;

        return obj;
    }

    void CreateButton(
        Transform parent,
        string text,
        Vector2 pos,
        UnityEngine.Events.UnityAction action
    )
    {
        GameObject obj =
            new GameObject(text);

        obj.transform.SetParent(
            parent
        );

        Image img =
            obj.AddComponent<Image>();

        img.color =
            Color.gray;

        Button btn =
            obj.AddComponent<Button>();

        btn.onClick.AddListener(
            action
        );

        RectTransform rect =
            obj.GetComponent<
                RectTransform>();

        rect.sizeDelta =
            new Vector2(
                300,
                80
            );

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

        rect.anchoredPosition =
            pos;

        CreateText(
            obj.transform,
            text,
            Vector2.zero,
            36
        );
    }

    void CreateText(
        Transform parent,
        string text,
        Vector2 pos,
        int size
    )
    {
        GameObject obj =
            new GameObject(text);

        obj.transform.SetParent(
            parent
        );

        TextMeshProUGUI tmp =
            obj.AddComponent<
                TextMeshProUGUI>();

        tmp.text = text;

        tmp.fontSize = size;

        tmp.color =
            Color.white;

        tmp.alignment =
            TextAlignmentOptions.Center;

        tmp.raycastTarget =
            false;

        RectTransform rect =
            tmp.rectTransform;

        rect.sizeDelta =
            new Vector2(
                700,
                100
            );

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

        rect.anchoredPosition =
            pos;
    }

    Slider CreateSlider(
        Transform parent,
        Vector2 pos,
        float min,
        float max,
        float value
    )
    {
        GameObject obj =
            new GameObject(
                "Slider"
            );

        obj.transform.SetParent(
            parent
        );

        Slider slider =
            obj.AddComponent<
                Slider>();

        RectTransform rect =
            obj.GetComponent<
                RectTransform>();

        rect.sizeDelta =
            new Vector2(
                400,
                40
            );

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

        rect.anchoredPosition =
            pos;

        GameObject bg =
            new GameObject(
                "Background"
            );

        bg.transform.SetParent(
            obj.transform
        );

        Image bgImage =
            bg.AddComponent<Image>();

        bgImage.color =
            Color.gray;

        RectTransform bgRect =
            bg.GetComponent<
                RectTransform>();

        bgRect.anchorMin =
            Vector2.zero;

        bgRect.anchorMax =
            Vector2.one;

        bgRect.offsetMin =
            Vector2.zero;

        bgRect.offsetMax =
            Vector2.zero;

        GameObject fill =
            new GameObject(
                "Fill"
            );

        fill.transform.SetParent(
            bg.transform
        );

        Image fillImage =
            fill.AddComponent<Image>();

        fillImage.color =
            Color.white;

        RectTransform fillRect =
            fill.GetComponent<
                RectTransform>();

        fillRect.anchorMin =
            Vector2.zero;

        fillRect.anchorMax =
            Vector2.one;

        fillRect.offsetMin =
            Vector2.zero;

        fillRect.offsetMax =
            Vector2.zero;

        GameObject handle =
            new GameObject(
                "Handle"
            );

        handle.transform.SetParent(
            obj.transform
        );

        Image handleImage =
            handle.AddComponent<Image>();

        handleImage.color =
            Color.black;

        RectTransform handleRect =
            handle.GetComponent<
                RectTransform>();

        handleRect.sizeDelta =
            new Vector2(
                20,
                50
            );

        slider.fillRect =
            fillRect;

        slider.handleRect =
            handleRect;

        slider.targetGraphic =
            handleImage;

        slider.minValue =
            min;

        slider.maxValue =
            max;

        slider.value =
            value;

        return slider;
    }

    void SetVolume(
        float value
    )
    {
        AudioListener.volume =
            value;
    }

    void SetSensitivity(
        float value
    )
    {
        FPSController controller =
            FindFirstObjectByType<
                FPSController>();

        if (controller != null)
        {
            controller.mouseSensitivity =
                value;
        }
    }

    public void Pause()
    {
        IsPaused = true;

        Time.timeScale = 0f;

        Cursor.lockState =
            CursorLockMode.None;

        Cursor.visible = true;

        panel.SetActive(true);
    }

    public void Resume()
    {
        IsPaused = false;

        Time.timeScale = 1f;

        Cursor.lockState =
            CursorLockMode.Locked;

        Cursor.visible = false;

        panel.SetActive(false);

        settingsPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}