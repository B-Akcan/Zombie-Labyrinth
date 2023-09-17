using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameParams;
using static TagHolder;
using UnityEngine.UI;
using TMPro;

public class GameMenu : MonoBehaviour
{
    GameObject mainMenu;
    GameObject optionsMenu;
    TMP_Dropdown screenModeDropdown;
    [SerializeField] IntSO screenModeSO;
    Slider brightnessSlider;
    [SerializeField] DoubleSO brightnessSO;
    Slider sensitivitySlider;
    [SerializeField] DoubleSO sensitivitySO;
    TMP_Dropdown difficultyDropdown;
    [SerializeField] IntSO difficultySO;
    AudioSource audioSource;

    void Awake()
    {
        mainMenu = transform.Find(MAIN_MENU).gameObject;
        optionsMenu = transform.Find(OPTIONS_MENU).gameObject;

        screenModeDropdown = transform.Find(SCREEN_MODE).gameObject.GetComponent<TMP_Dropdown>();
        brightnessSlider = transform.Find(BRIGHTNESS).gameObject.GetComponent<Slider>();
        sensitivitySlider = transform.Find(SENSITIVITY).gameObject.GetComponent<Slider>();
        difficultyDropdown = transform.Find(DIFFICULTY).gameObject.GetComponent<TMP_Dropdown>();
    }

    void Start()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);

        InitializeOptions();
    }

    void Update()
    {
        AdjustScreenMode();
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Options()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void Back()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }

    void InitializeOptions()
    {
        screenModeDropdown.value = screenModeSO.Value;
        brightnessSlider.value = (float) brightnessSO.Value;
        sensitivitySlider.value = (float) sensitivitySO.Value;
        difficultyDropdown.value = difficultySO.Value;

        screenModeDropdown.onValueChanged.AddListener(delegate {screenModeSO.Value = screenModeDropdown.value; });
        brightnessSlider.onValueChanged.AddListener(delegate {brightnessSO.Value = brightnessSlider.value; });
        sensitivitySlider.onValueChanged.AddListener(delegate {sensitivitySO.Value = sensitivitySlider.value; });
        difficultyDropdown.onValueChanged.AddListener(delegate {difficultySO.Value = difficultyDropdown.value; });
    }

    void AdjustScreenMode()
    {
        switch (screenModeDropdown.value)
        {
            case 0: Screen.fullScreenMode = FullScreenMode.FullScreenWindow; break;
            case 1: Screen.fullScreenMode = FullScreenMode.Windowed; break;
        }
    }
}
