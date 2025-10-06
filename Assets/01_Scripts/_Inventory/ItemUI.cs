using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemUI : MonoBehaviour, IPointerEnterHandler
{
    public Image image;   // Asigna en el prefab
    public Button button; // Asigna en el prefab

    private string key;
    private Action<string> onDrop;
    private Action<string> onSelect;

    // removeItemAction = dropear (desde botón)
    // selectItemAction = seleccionar (para soltar con Q)
    public void Initialize(string inventoryItemName, Item item,
                           Action<string> removeItemAction,
                           Action<string> selectItemAction)
    {
        key = inventoryItemName;
        onDrop = removeItemAction;
        onSelect = selectItemAction;

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
        button.onClick.AddListener(() => onDrop?.Invoke(key));
    }

    // Al pasar el mouse por encima, se selecciona este ítem para soltar con Q
    public void OnPointerEnter(PointerEventData eventData)
    {
        onSelect?.Invoke(key);
    }
}
