using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86.Avx;

[DisallowMultipleComponent]

public class PlayerInventory : MonoBehaviour
{
    #region Variables

    #region Referencias
    public static PlayerInventory current;
    public GameObject inventoryUI;
    public ItemData[] item;
    public Transform UISlots;
    public GameObject slotPrefab;
    #endregion

    public int iSlots = 6;

    public bool openInventory;
    #endregion

    #region Awake & Start
    private void Awake()
    {
        if (current == null)
        {
            current = this;
        }
        else
        {
            Destroy(this);
        }
        UISlots = GameObject.Find("UIPlayerInventory").GetComponent<Transform>();
    }

    private void Start()
    {
        item = new ItemData[iSlots];
        UISlots.gameObject.gameObject.SetActive(false);
        for (int i = 0; i < iSlots; i++)
        {
            GameObject slot = Instantiate(slotPrefab, UISlots);
            slot.name = "Slot " + i;
        }
    }
    #endregion

    void Update()
    {

    }

    public void AddToInventory(ItemData itemToAdd)
    {
        for (int i = 0; i < item.Length; i++)
        {
            if (item[i] == null)
            {
                item[i] = itemToAdd;
                Debug.Log("Agregue " + itemToAdd.itemName);
                SetSlot(i, itemToAdd);
                break;
            }
        }
    }

    public void SetSlot(int i, ItemData itemData)
    {
        UISlots.GetChild(i).GetComponent<Image>().sprite = itemData.itemIcon;
        UISlots.GetComponent<Image>().color = Color.white;
    }

    public void OpenInventoryUI()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            openInventory = !openInventory;
            inventoryUI.SetActive(openInventory);
        }
    }

    public void DeleteFromInventory(int itemSlot)
    {
        Debug.LogWarning(itemSlot);
        PlayerInventory.current.item[itemSlot] = null;
        UISlots.GetChild(itemSlot).GetComponent<Image>().color = Color.white;
        UISlots.GetChild(itemSlot).GetComponentInChildren<TMP_Text>().text = "";
    }
}
