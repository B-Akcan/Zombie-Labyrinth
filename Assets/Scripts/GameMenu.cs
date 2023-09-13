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
    Slider brightnessSlider;
    [SerializeField] DoubleSO brightnessSO;
    TMP_Dropdown difficultyDropdown;
    [SerializeField] IntSO difficultySO;

    void Awake()
    {
        mainMenu = transform.Find(MAIN_MENU).gameObject;
        optionsMenu = transform.Find(OPTIONS_MENU).gameObject;
        brightnessSlider = transform.Find(BRIGHTNESS).gameObject.GetComponent<Slider>();
        difficultyDropdown = transform.Find(DIFFICULTY).gameObject.GetComponent<TMP_Dropdown>();
    }

    void Start()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);

        InitializeOptions();
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
        brightnessSlider.value = (float) brightnessSO.Value;
        difficultyDropdown.value = difficultySO.Value;

        brightnessSlider.onValueChanged.AddListener(delegate {brightnessSO.Value = brightnessSlider.value; });
        difficultyDropdown.onValueChanged.AddListener(delegate {difficultySO.Value = difficultyDropdown.value; });
    }
}
