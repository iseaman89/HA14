using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUIButton : MonoBehaviour
{
    private InventoryItem _itemData;

    private InventoryUsedCallback _callback;

    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _label;
    [SerializeField] private TextMeshProUGUI _count;
    [SerializeField] private List<Sprite> _sprites;

    public InventoryUsedCallback Callback { get => _callback; set => _callback = value; }
    public InventoryItem ItemData { get => _itemData; set => _itemData = value; }

    private void Start()
    {
        string spriteNameToSearch = ItemData.CrystallType.ToString().ToLower();

        _image.sprite = _sprites.Find(x => x.name.Contains(spriteNameToSearch));

        _label.text = spriteNameToSearch;
        _count.text = ItemData.Quantity.ToString();

        gameObject.GetComponent<Button>().onClick.AddListener(() => _callback(this));
    }
}
