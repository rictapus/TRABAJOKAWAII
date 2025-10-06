using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("UI")]
    public InventoryUI ui; // Asigna en el Inspector

    [Header("Dropear")]
    public GameObject droppedItemPrefab; // Prefab con DroppedItem + collider trigger

    // Runtime
    private readonly Dictionary<string, Item> inventory = new Dictionary<string, Item>();
    private readonly List<string> order = new List<string>(); // mantiene orden de agregado
    private string selectedKey; // ítem seleccionado desde la UI (hover)

    void Awake()
    {
        if (!ui) ui = FindAnyObjectByType<InventoryUI>();
    }

    void Update()
    {
        // Soltar con Q: primero el seleccionado; si no, el último agregado
        if (Input.GetKeyDown(KeyCode.Q))
            DropSelectedOrLast();
    }

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
        // Drop llamado por UI (click) o internamente
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

    // Llamado por la UI al pasar el mouse sobre un ítem
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
}
