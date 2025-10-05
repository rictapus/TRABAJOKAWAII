using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("UI")]
    public InventoryUI ui; // Asigna en el Inspector

    [Header("Dropear")]
    public GameObject droppedItemPrefab; // Prefab con DroppedItem + collider trigger

    // Diccionario runtime (no serializado) clave->Item
    private readonly Dictionary<string, Item> inventory = new Dictionary<string, Item>();

    void Awake()
    {
        if (!ui) ui = FindAnyObjectByType<InventoryUI>();
    }

    public void AddItem(Item item)
    {
        if (item == null) return;

        // Clave base por nombre; si existe, añade GUID para permitir duplicados
        string key = item.itemName;
        if (inventory.ContainsKey(key))
            key = $"{item.itemName}_{Guid.NewGuid()}";

        inventory[key] = item;
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
        if (ui) ui.RemoveUIItem(inventoryItemName);
    }
}
