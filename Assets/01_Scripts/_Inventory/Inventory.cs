using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Variables
    public InventoryUI ui;
    public GameObject droppedItemPrefab;

    private readonly Dictionary<string, Item> inventory = new Dictionary<string, Item>();
    private readonly List<string> order = new List<string>(); 
    private string selectedKey;
    #endregion

    #region Awake & Update
    void Awake()
    {
        if (!ui) ui = FindAnyObjectByType<InventoryUI>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            DropSelectedOrLast();
    }
    #endregion

    #region Item Management
    public void AddItem(Item item)
    {
        if (item == null) return;

        string key = item.itemName;
        if (inventory.ContainsKey(key))
            key = $"{item.itemName}_{Guid.NewGuid()}";

        inventory[key] = item;
        order.Add(key);
        if (ui) ui.AddUIItem(key, item);
    }

    public void DropItem(string inventoryItemName)
    {
        if (!inventory.TryGetValue(inventoryItemName, out var item) || item == null)
            return;

        if (!droppedItemPrefab)
        {
            Debug.LogWarning("Inventory: droppedItemPrefab no asignado.");
            return;
        }

        Vector3 dropPos = transform.position + transform.forward * 0.75f;
        var go = Instantiate(droppedItemPrefab, dropPos, Quaternion.identity);
        var dropped = go.GetComponent<DroppedItem>();
        if (dropped) dropped.Initialize(item);

        inventory.Remove(inventoryItemName);
        order.Remove(inventoryItemName);
        if (selectedKey == inventoryItemName) selectedKey = null;

        if (ui) ui.RemoveUIItem(inventoryItemName);
    }

    public void SelectItem(string inventoryItemName)
    {
        if (inventory.ContainsKey(inventoryItemName))
            selectedKey = inventoryItemName;
    }

    private void DropSelectedOrLast()
    {
        string keyToDrop = null;

        if (!string.IsNullOrEmpty(selectedKey) && inventory.ContainsKey(selectedKey))
            keyToDrop = selectedKey;
        else if (order.Count > 0)
            keyToDrop = order[order.Count - 1];

        if (keyToDrop != null)
        {
            DropItem(keyToDrop);
            if (selectedKey == keyToDrop) selectedKey = null;
        }
    }
    #endregion
}
