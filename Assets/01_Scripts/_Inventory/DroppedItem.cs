using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DroppedItem : MonoBehaviour
{
    [Header("Pickup")]
    public float enablePickupDelay = 0.25f;   // tiempo antes de permitir recoger

    private Item item;
    private bool canPickup;                   // tras el delay
    private bool playerInZone;                // jugador dentro del trigger
    private bool pickedUp;                    // evita duplicados
    private Inventory playerInventory;        // cache del inventory del jugador

    public void Initialize(Item item)
    {
        this.item = item;

        // Visual
        if (item && item.prefab)
        {
            var visual = Instantiate(item.prefab, transform);
            visual.transform.localPosition = Vector3.zero;
            visual.transform.localRotation = Quaternion.identity;
        }

        // Configurar collider
        var col = GetComponent<Collider>();
        if (col)
        {
            col.isTrigger = true;
            col.enabled = false;              // deshabilitado hasta pasar el delay
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

    void Update()
    {
        // Recoger SOLO si: puede recogerse, el jugador está en zona y presiona E
        if (canPickup && playerInZone && !pickedUp && Input.GetKeyDown(KeyCode.E))
        {
            TryPickup();
        }
    }

    void TryPickup()
    {
        if (pickedUp) return;
        if (!item) return;
        if (!playerInventory)
        {
            Debug.LogWarning("DroppedItem: no se encontró Inventory en el jugador.");
            return;
        }

        pickedUp = true;
        playerInventory.AddItem(item);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!canPickup || pickedUp) return;

        // Detecta al jugador y cachea su Inventory
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
}
