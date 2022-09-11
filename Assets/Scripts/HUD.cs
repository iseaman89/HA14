using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreLabel;
    [SerializeField] private Slider _healthBar;
    private static HUD s_instance;

    public static HUD S_instance { get => s_instance; }
    public Slider HealthBar { get => _healthBar; set => _healthBar = value; }

    private void Awake()
    {
        s_instance = this;
    }

    public void SetScore(string scoreValue)
    {
        _scoreLabel.text = scoreValue;
    }
}
