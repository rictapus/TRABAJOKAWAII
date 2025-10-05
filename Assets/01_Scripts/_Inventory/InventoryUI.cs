using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("Referencias")]
    public Inventory inventory;          // Asigna en Inspector
    public Transform uiInventoryParent;  // Contenedor (p.ej. un GridLayout en el Canvas)
    public GameObject uiItemPrefab;      // Prefab con ItemUI (Image + Button)

    // clave -> GO de UI
    private readonly Dictionary<string, GameObject> inventoryUI = new Dictionary<string, GameObject>();

    public void AddUIItem(string inventoryItemName, Item item)
    {
        if (!uiItemPrefab || !uiInventoryParent)
        {
            Debug.LogError("InventoryUI: Asigna uiItemPrefab y uiInventoryParent.");
            return;
        }

        var go = Object.Instantiate(uiItemPrefab, uiInventoryParent);
        var itemUI = go.GetComponent<ItemUI>();
        if (!itemUI)
        {
            Debug.LogError("InventoryUI: uiItemPrefab no tiene ItemUI.");
            Destroy(go);
            return;
        }

        inventoryUI[inventoryItemName] = go;
        itemUI.Initialize(inventoryItemName, item, inventory.DropItem);
    }

    public void RemoveUIItem(string inventoryItemName)
    {
        if (inventoryUI.TryGetValue(inventoryItemName, out var go) && go)
        {
            Destroy(go);
            inventoryUI.Remove(inventoryItemName);
        }
    }
}
