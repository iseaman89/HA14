using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { Play, Pause }

public class GameController : MonoBehaviour
{
    private static GameController s_instance;
    private GameState _state;
    private int _score;
    private int _dragonHitScore = 10;
    private int _dragonKillScore = 50;
    [SerializeField] private float _maxHealth;

    public static GameController S_instance { get => s_instance; }
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

    public float MaxHealth { get => _maxHealth; set => _maxHealth = value; }

    private void Awake()
    {
        s_instance = this;
        _state = GameState.Play;
    }

    private void Start()
    {
        HUD.S_instance.HealthBar.maxValue = _maxHealth;
        HUD.S_instance.HealthBar.value = _maxHealth;
        HUD.S_instance.SetScore(Score.ToString());
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
}

