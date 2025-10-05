using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;
using JetBrains.Annotations;

public class InventoryUI : MonoBehaviour
{
    #region Variables
    [SerializeField] GameObject uiItemPrefab;
    [SerializeField] Inventory inventory;
    [SerializeField] Transform uiInventoryParent;
    [SerializeField] SerializedDictionary<string, GameObject> inventoryUI = new();
    #endregion

    public void AddUIItem (string inventoryItemName, Item item)
    {
        var itemIU = Instantiate(uiItemPrefab).GetComponent<ItemUI>();
        itemIU.transform.SetParent(uiInventoryParent);
        inventoryUI.Add(inventoryItemName, itemIU.gameObject);
        itemIU.Initialize(inventoryItemName, item, inventory.DropItem);

    }

    public void RemoveUIItem(string inventoryItemName)
    {
        var itemUI = inventoryUI.GetValueOrDefault(inventoryItemName);
        inventoryUI.Remove(inventoryItemName);
        Destroy(itemUI);
    }
}
    