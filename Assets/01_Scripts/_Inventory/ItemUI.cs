using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image image;   // Asigna en el prefab
    public Button button; // Asigna en el prefab

    public void Initialize(string inventoryItemName, Item item, Action<string> removeItemAction)
    {
        if (!image || !button)
        {
            Debug.LogError("ItemUI: Asigna Image y Button en el prefab.");
            return;
        }
        if (!item)
        {
            Debug.LogWarning("ItemUI.Initialize llamado con item nulo.");
            return;
        }

        image.sprite = item.icon;
        transform.localScale = Vector3.one;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => removeItemAction?.Invoke(inventoryItemName));
    }
}
