using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HeroParameters
{
    #region Private_Variables

    [SerializeField] private float _maxHealth = 100;
    [SerializeField] private float _damage = 20;
    [SerializeField] private float _speed = 5;
    [SerializeField] private int _experience = 0;

    private int _nextExperienceLevel = 100;
    private int _previosExperienceLevel = 0;
    private int _level = 1;

    public float MaxHealth { get => _maxHealth; set => _maxHealth = value; }
    public float Damage { get => _damage; set => _damage = value; }
    public float Speed { get => _speed; set => _speed = value; }
    public int Experience
    {
        get => _experience;
        set
        {
            _experience = value;
            CheckExperienceLevel();
        } 
    }

    #endregion
    #region Function

    private void CheckExperienceLevel()
    {
        if (_experience > _nextExperienceLevel)
        {
            _level++;
            int addition = _previosExperienceLevel;
            _previosExperienceLevel = _nextExperienceLevel;
            _nextExperienceLevel += addition;

            switch (Random.Range(0, 3))
            {
                case 0: _maxHealth++;
                    break;
                case 1: _damage++;
                    break;
                case 2: _speed++;
                    break;
            }

            GameController.S_instance.LevelUp();
        }
    }

    #endregion
}
