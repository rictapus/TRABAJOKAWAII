using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemUI : MonoBehaviour, IPointerEnterHandler
{
    #region Variables
    public Image image;  
    public Button button;

    private string key;
    private Action<string> onDrop;
    private Action<string> onSelect;
    #endregion

    public void Initialize(string inventoryItemName, Item item, Action<string> removeItemAction, Action<string> selectItemAction)
    {
        key = inventoryItemName;
        onDrop = removeItemAction;
        onSelect = selectItemAction;

        if (!image || !button)
        {
            return;
        }
        if (!item)
        {
            return;
        }

        image.sprite = item.icon;
        transform.localScale = Vector3.one;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onDrop?.Invoke(key));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onSelect?.Invoke(key);
    }
}
