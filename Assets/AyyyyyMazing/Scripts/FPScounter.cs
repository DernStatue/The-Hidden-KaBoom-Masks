using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [Header("Display Settings")]
    public bool showFPS = true;
    public int targetFrameRate = 60;

    private float deltaTime = 0.0f;

    void Start()
    {
        Application.targetFrameRate = targetFrameRate;
    }

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        if (!showFPS) return;

        int width = Screen.width;
        int height = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(10, 10, width, height * 0.02f);

        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = 24;
        style.normal.textColor = Color.white;

        float fps = 1.0f / deltaTime;
        string text = $"FPS: {Mathf.Ceil(fps)}";

        GUI.Label(rect, text, style);
    }
}