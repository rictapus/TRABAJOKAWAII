using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DroppedItem : MonoBehaviour
{
    public float enablePickupDelay = 0.25f;

    Item item;
    bool pickedUp;

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
        if (col) { col.isTrigger = true; col.enabled = false; } // evita recoger al instante

        pickedUp = false;
        StartCoroutine(EnablePickup(enablePickupDelay));
    }

    IEnumerator EnablePickup(float delay)
    {
        yield return new WaitForSeconds(delay);
        var col = GetComponent<Collider>();
        if (col) col.enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (pickedUp || !item) return;

        // Recoger si el que entra tiene Inventory (p.ej. el jugador)
        var inv = other.GetComponentInParent<Inventory>();
        if (!inv) return;

        pickedUp = true;
        inv.AddItem(item);
        Destroy(gameObject);
    }
}
