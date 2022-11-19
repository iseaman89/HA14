using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { Play, Pause }

public delegate void InventoryUsedCallback(InventoryUIButton item);
public delegate void UpdateHeroParamatersHandler(HeroParameters parameters);

public class GameController : MonoBehaviour
{
    #region Private_Variables

    public event UpdateHeroParamatersHandler OnUpdateHeroParameters;

    private static GameController s_instance;
    private GameState _state;
    private int _score = 0;
    private int _dragonHitScore = 10;
    private int _dragonKillScore = 50;
    [SerializeField] private List<InventoryItem> _inventory;
    private Knight _knight;

    [SerializeField] private GameObject _lastLevel;
    [SerializeField] private int dragonKillExperience;
    [SerializeField] private HeroParameters _hero;
    [SerializeField] private Audio _audioManager;

    public static GameController S_instance
    {
        get
        {
            if (s_instance == null)
            {
                GameObject gameController = Instantiate(Resources.Load("Prefabs/GameController")) as GameObject;

                s_instance = gameController.GetComponent<GameController>();
            }

            return s_instance;
        }
    }
    public int Score
    {
        get => _score;
        set
        {
            if (value != _score)
            {
                _score = value;
                HUD.S_instance.SetScore(_score.ToString());
            }
        }
    }

    public GameState State
    {
        get => _state;
        set
        {
            if (value == GameState.Play)
            {
                Time.timeScale = 1.0f;
            }
            else
            {
                Time.timeScale = 0.0f;
            }
            _state = value;
        }
    }

    public Knight Knight { get => _knight; set => _knight = value; }
    public GameObject LastLevel { get => _lastLevel; set => _lastLevel = value; }
    public Audio AudioManager { get => _audioManager; set => _audioManager = value; }
    public List<InventoryItem> Inventory { get => _inventory; set => _inventory = value; }
    public HeroParameters Hero { get => _hero; set => _hero = value; }
    #endregion

    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
        }
        else
        {
            if (s_instance != this)
            {
                Destroy(gameObject);
            }
        }

        DontDestroyOnLoad(gameObject);

        State = GameState.Play;
        Inventory = new List<InventoryItem>();

        InitializeAudioManager();
    }

    public void StartNewLevel()
    {
        HUD.S_instance.SetScore(Score.ToString());

        if (OnUpdateHeroParameters != null)
        {
            OnUpdateHeroParameters(_hero);
        }

        State = GameState.Play;
    }

    public void Hit(IDestructable victim)
    {
        if (victim.GetType() == typeof(Dragon))
        {
            if (victim.Health > 0)
            {
                Score += _dragonHitScore;
            }
            else
            {
                Score += _dragonKillScore;
            }
        }

        if (victim.GetType() == typeof(Knight))
        {
            HUD.S_instance.HealthBar.value = victim.Health;
        }
    }

    public void Killed(IDestructable victim)
    {
        if (victim.GetType() == typeof(Dragon))
        {
            Score += _dragonKillScore;

            _hero.Experience += dragonKillExperience;

            Destroy((victim as MonoBehaviour).gameObject);
        }

        if (victim.GetType() == typeof(Knight))
        {
            GameOver();
        }
    }

    public void AddNewInventoryItem(InventoryItem itemData)
    {
        InventoryUIButton newUIButton = HUD.S_instance.AddNewInventoryItem(itemData);
        InventoryUsedCallback callback = new InventoryUsedCallback(InventoryItemUsed);
        newUIButton.Callback = callback;
        Inventory.Add(itemData);
    }

    public void InventoryItemUsed(InventoryUIButton item)
    {
        _audioManager.PlaySound("Click");
        switch (item.ItemData.CrystallType)
        {
            case CrystallType.Blue:
                _hero.Speed += item.ItemData.Quantity / 10f;
                break;

            case CrystallType.Red:
                _hero.Damage += item.ItemData.Quantity / 10f;
                break;

            case CrystallType.Green:
                _hero.MaxHealth += item.ItemData.Quantity / 10f;
                break;

            default:
                Debug.LogError("Wrong crystall type!");
                break;
        }
        Inventory.Remove(item.ItemData);
        Destroy(item.gameObject);
        if (OnUpdateHeroParameters != null)
        {
            OnUpdateHeroParameters(_hero);
        }
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void PrincessFound()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("LastLevel"))
        {
            HUD.S_instance.ShowGameWonWindow();
            _lastLevel.SetActive(true);
        }
        else
        {
            HUD.S_instance.ShowLevelWonWindow();
            _audioManager.PlaySound("Easy");
        }
    }

    public void GameOver()
    {
        HUD.S_instance.ShowLevelLoseWindow();
    }

    private void InitializeAudioManager()
    {
        _audioManager.SourceSFX = gameObject.AddComponent<AudioSource>();
        _audioManager.SourceMusic = gameObject.AddComponent<AudioSource>();
        _audioManager.SourceRandomPitchSFX = gameObject.AddComponent<AudioSource>();
    }

    public void LevelUp()
    {
        if (OnUpdateHeroParameters != null)
        {
            OnUpdateHeroParameters(_hero);
        }
    }
}

