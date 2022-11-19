using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    [SerializeField] private CrystallType _crystallType;
    [SerializeField] private float _quantity;

    public CrystallType CrystallType { get => _crystallType; set => _crystallType = value; }
    public float Quantity { get => _quantity; set => _quantity = value; }
}
