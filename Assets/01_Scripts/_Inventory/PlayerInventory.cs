using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerInventory : MonoBehaviour
{
    public GameObject inventoryUI;

    public bool openInventory;

    void Update()
    {

    }

    public void OpenInventoryUI()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            openInventory = !openInventory;
            inventoryUI.SetActive(openInventory);
        }
    }
}
