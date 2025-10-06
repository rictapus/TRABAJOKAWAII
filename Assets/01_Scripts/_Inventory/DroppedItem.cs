using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DroppedItem : MonoBehaviour
{
    #region Variables
    public float enablePickupDelay = 0.25f;
    private Item item;
    private bool canPickup;          
    private bool playerInZone;    
    private bool pickedUp;            
    private Inventory playerInventory;
    #endregion

    #region Update
    void Update()
    {
        if (canPickup && playerInZone && !pickedUp && Input.GetKeyDown(KeyCode.E))
        {
            TryPickup();
        }
    }
    #endregion

    public void Initialize(Item item)
    {
        this.item = item;

        if (item && item.prefab)
        {
            var visual = Instantiate(item.prefab, transform);
            visual.transform.localPosition = Vector3.zero;
            visual.transform.localRotation = Quaternion.identity;
        }

        var col = GetComponent<Collider>();
        if (col)
        {
            col.isTrigger = true;
            col.enabled = false;      
        }

        canPickup = false;
        playerInZone = false;
        pickedUp = false;
        playerInventory = null;

        StartCoroutine(EnablePickupAfterDelay(enablePickupDelay));
    }

    IEnumerator EnablePickupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        var col = GetComponent<Collider>();
        if (col) col.enabled = true;
        canPickup = true;
    }

    void TryPickup()
    {
        if (pickedUp) return;
        if (!item) return;
        if (!playerInventory)
        {
            return;
        }

        pickedUp = true;
        playerInventory.AddItem(item);
        Destroy(gameObject);
    }

    #region Triggers
    void OnTriggerEnter(Collider other)
    {
        if (!canPickup || pickedUp) return;

        if (other.CompareTag("Player") || other.GetComponentInParent<Inventory>())
        {
            playerInventory = other.GetComponentInParent<Inventory>();
            if (playerInventory)
                playerInZone = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!playerInZone) return;

        var inv = other.GetComponentInParent<Inventory>();
        if (inv && inv == playerInventory)
        {
            playerInZone = false;
            playerInventory = null;
        }
    }
    #endregion
}
