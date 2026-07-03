using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    [Header("Audio")]
    [Range(0f, 1f)]
    public float masterVolume = 1f;

    [Range(0f, 1f)]
    public float sfxVolume = 1f;

    [Range(0f, 1f)]
    public float ambienceVolume = 1f;

    [Header("Gameplay")]
    public float mouseSensitivity = 2f;

    public float gunSwayAmount = 4f;

    [Header("Crosshair")]
    public float crosshairSize = 6f;

    public bool crosshairEnabled = true;

    void Awake()
    {
        if (
            Instance != null &&
            Instance != this
        )
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(
            gameObject
        );

        LoadSettings();
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat(
            "MasterVolume",
            masterVolume
        );

        PlayerPrefs.SetFloat(
            "SFXVolume",
            sfxVolume
        );

        PlayerPrefs.SetFloat(
            "AmbienceVolume",
            ambienceVolume
        );

        PlayerPrefs.SetFloat(
            "MouseSensitivity",
            mouseSensitivity
        );

        PlayerPrefs.SetFloat(
            "GunSwayAmount",
            gunSwayAmount
        );

        PlayerPrefs.SetFloat(
            "CrosshairSize",
            crosshairSize
        );

        PlayerPrefs.SetInt(
            "CrosshairEnabled",
            crosshairEnabled ? 1 : 0
        );

        PlayerPrefs.Save();
    }

    void LoadSettings()
    {
        masterVolume =
            PlayerPrefs.GetFloat(
                "MasterVolume",
                1f
            );

        sfxVolume =
            PlayerPrefs.GetFloat(
                "SFXVolume",
                1f
            );

        ambienceVolume =
            PlayerPrefs.GetFloat(
                "AmbienceVolume",
                1f
            );

        mouseSensitivity =
            PlayerPrefs.GetFloat(
                "MouseSensitivity",
                2f
            );

        gunSwayAmount =
            PlayerPrefs.GetFloat(
                "GunSwayAmount",
                4f
            );

        crosshairSize =
            PlayerPrefs.GetFloat(
                "CrosshairSize",
                6f
            );

        crosshairEnabled =
            PlayerPrefs.GetInt(
                "CrosshairEnabled",
                1
            ) == 1;
    }
}