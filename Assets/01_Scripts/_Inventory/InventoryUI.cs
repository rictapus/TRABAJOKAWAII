using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    #region Variables
    public Inventory inventory;
    public Transform uiInventoryParent;
    public GameObject uiItemPrefab;

    private readonly Dictionary<string, GameObject> inventoryUI = new Dictionary<string, GameObject>();
    #endregion

    #region Add & Remove
    public void AddUIItem(string inventoryItemName, Item item)
    {
        if (!uiItemPrefab || !uiInventoryParent)
        {
            Debug.LogError("InventoryUI: Asigna uiItemPrefab y uiInventoryParent.");
            return;
        }

        var go = Instantiate(uiItemPrefab, uiInventoryParent);
        var itemUI = go.GetComponent<ItemUI>();
        if (!itemUI)
        {
            Debug.LogError("InventoryUI: uiItemPrefab no tiene ItemUI.");
            Destroy(go);
            return;
        }

        inventoryUI[inventoryItemName] = go;

        itemUI.Initialize(
            inventoryItemName,
            item,
            inventory.DropItem,    
            inventory.SelectItem 
        );
    }

    public void RemoveUIItem(string inventoryItemName)
    {
        if (inventoryUI.TryGetValue(inventoryItemName, out var go) && go)
        {
            Destroy(go);
            inventoryUI.Remove(inventoryItemName);
        }
    }
    #endregion
}
