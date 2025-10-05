using UnityEngine;
using System.Collections;

public class DroppedItem : MonoBehaviour
{
    #region Variables
    bool autoStart;
    public bool pickedUp = false;
    [SerializeField] float enablePickupDelay = 3.0f;
    public Item item;
    #endregion

    void Start()
    {
        if (autoStart && item != null)
        {
            Initialize(item);
        }
    }

    public void Initialize(Item item)
    {
        this.item = item;
        var droppedItem = Instantiate(item.prefab, transform);
        droppedItem.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        StartCoroutine(EnablePickup(enablePickupDelay));
    }

    IEnumerator EnablePickup(float dealy)
    {
        yield return new WaitForSeconds(dealy);
        GetComponent<Collider>().enabled = true;
    }
}
