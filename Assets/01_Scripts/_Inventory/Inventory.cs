using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Rendering;

public class Inventory : MonoBehaviour
{
    #region Variables
    [SerializeField] InventoryUI ui;
    [SerializeField] GameObject droppedItemPrefab;
    [SerializeField] SerializedDictionary<string, Item> inventory = new();

    #endregion

    public void AddItem(Item item)
    {
        if (inventory.ContainsKey(item.itemName))
        {
            var inventoryItemName = Guid.NewGuid().ToString();
            inventory.Add(inventoryItemName, item);
            ui.AddUIItem(inventoryItemName, item);
        }
    }

    public void DropItem(string inventoryItemName)
    {
        var droppedItem = Instantiate(droppedItemPrefab, transform.position, Quaternion.identity).GetComponent<DroppedItem>();
        var item = inventory.GetValueOrDefault(inventoryItemName);
        droppedItem.Initialize(item);
        inventory.Remove(inventoryItemName);
        ui.RemoveUIItem(inventoryItemName);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DroppedItem"))
        {
            var droppedItem = other.GetComponent<DroppedItem>();

            if (droppedItem.pickedUp)
            {
                return;
            }

            droppedItem.pickedUp = true;
            AddItem(droppedItem.item);
            Destroy(other.gameObject);
        }
    }
}
