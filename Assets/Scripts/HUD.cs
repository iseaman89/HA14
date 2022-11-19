using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreLabel;
    [SerializeField] private Slider _healthBar;
    [SerializeField] private GameObject _inventoryWindow;
    [SerializeField] private GameObject _levelWonWindow;
    [SerializeField] private GameObject _gameWonWindow;
    [SerializeField] private GameObject _levelLoseWindow;
    [SerializeField] private GameObject _inGameWindow;
    [SerializeField] private GameObject _optionWindow;
    [SerializeField] private InventoryUIButton inventoryItemPrefab;
    [SerializeField] private Transform inventoryContainer;
    [SerializeField] private TextMeshProUGUI _damageValue;
    [SerializeField] private TextMeshProUGUI _healthValue;
    [SerializeField] private TextMeshProUGUI _speedValue;

    private static HUD s_instance;

    public static HUD S_instance { get => s_instance; }
    public Slider HealthBar { get => _healthBar; set => _healthBar = value; }

    private void Awake()
    {
        s_instance = this;
    }

    private void Start()
    {
        LoadInventory();

        GameController.S_instance.OnUpdateHeroParameters += HandleOnUpdateHeroParameters;

        GameController.S_instance.StartNewLevel();
    }

    private void OnDestroy()
    {
        GameController.S_instance.OnUpdateHeroParameters -= HandleOnUpdateHeroParameters;
    }

    public void SetScore(string scoreValue)
    {
        _scoreLabel.text = scoreValue;
    }

    public void ShowWindow(GameObject window)
    {
        window.GetComponent<Animator>().SetBool("Open", true);
        GameController.S_instance.State = GameState.Pause;
        GameController.S_instance.AudioManager.PlaySound("Click");
    }

    public void HideWindow(GameObject window)
    {
        window.GetComponent<Animator>().SetBool("Open", false);
        GameController.S_instance.State = GameState.Play;
        GameController.S_instance.AudioManager.PlaySound("Click");
    }

    public InventoryUIButton AddNewInventoryItem(InventoryItem itemData)
    {
        InventoryUIButton newItem = Instantiate(inventoryItemPrefab, inventoryContainer) as InventoryUIButton;

        newItem.ItemData = itemData;

        return newItem;
    }

    public void UpdateCharacterValues(float newHealth, float newSpeed, float newDamage)
    {
        _healthValue.text = newHealth.ToString();
        _speedValue.text = newSpeed.ToString();
        _damageValue.text = newDamage.ToString();
    }

    public void ButtonNext()
    {
        GameController.S_instance.LoadNextLevel();
        GameController.S_instance.AudioManager.PlaySound("Click");
    }

    public void ButtonRestart()
    {
        GameController.S_instance.RestartLevel();
        GameController.S_instance.AudioManager.PlaySound("Click");
    }

    public void ButtonMainMenu()
    {
        GameController.S_instance.LoadMainMenu();
        GameController.S_instance.AudioManager.PlaySound("Click");
    }

    public void ShowLevelWonWindow()
    {
        ShowWindow(_levelWonWindow);
    }

    public void ShowLevelLoseWindow()
    {
        ShowWindow(_levelLoseWindow);
    }

    public void ShowGameWonWindow()
    {
        ShowWindow(_gameWonWindow);
    }

    public void LoadInventory()
    {
        InventoryUsedCallback callback = new InventoryUsedCallback(GameController.S_instance.InventoryItemUsed);

        for (int i = 0; i < GameController.S_instance.Inventory.Count; i++)
        {
            InventoryUIButton newItem = AddNewInventoryItem(GameController.S_instance.Inventory[i]);
            newItem.Callback = callback;
        }
    }

    private void HandleOnUpdateHeroParameters(HeroParameters parameters)
    {
        HealthBar.maxValue = parameters.MaxHealth;
        HealthBar.value = parameters.MaxHealth;
        UpdateCharacterValues(parameters.MaxHealth, parameters.Speed, parameters.Damage);
    }

    public void SetSoundVolume(Slider slider)
    {
        GameController.S_instance.AudioManager.SfxVolume = slider.value;
    }

    public void SetMusicVolume(Slider slider)
    {
        GameController.S_instance.AudioManager.MusicVolume = slider.value;
    }
}
