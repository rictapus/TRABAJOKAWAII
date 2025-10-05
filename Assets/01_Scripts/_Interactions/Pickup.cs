using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Pickup : MonoBehaviour
{
    [Header("Datos del �tem")]
    public Item item;                    // Asigna tu ScriptableObject de �tem

    [Header("Sujeci�n")]
    public Transform holdParent;         // Opcional (si no se asigna, usa la c�mara principal)
    public float holdDistance = 1.2f;    // Distancia delante de la c�mara al sostener

    Rigidbody rb;
    bool isHolding;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (!GetComponent<Collider>().isTrigger)
            GetComponent<Collider>().isTrigger = false; // este objeto se agarra con rat�n, no trigger
    }

    void Update()
    {
        // Seguir la mano/c�mara al sostener
        if (isHolding)
        {
            Transform targetParent = holdParent ? holdParent : Camera.main?.transform;
            if (targetParent)
            {
                Vector3 targetPos = targetParent.position + targetParent.forward * holdDistance;
                transform.position = targetPos;
                transform.rotation = Quaternion.LookRotation(targetParent.forward, Vector3.up);
            }

            // Agregar al inventario al presionar E
            if (Input.GetKeyDown(KeyCode.E))
                TryAddToInventory();
        }
    }

    // Agarrar con mouse (puedes reemplazar esto por tu Interactor si quieres)
    void OnMouseDown() { Hold(); }
    void OnMouseUp() { Drop(); }
    // Evita soltar por OnMouseExit (provoca soltadas accidentales)

    void Hold()
    {
        isHolding = true;
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (holdParent)
            transform.SetParent(holdParent, true);
    }

    void Drop()
    {
        isHolding = false;
        if (transform.parent == holdParent) transform.SetParent(null, true);
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    void TryAddToInventory()
    {
        if (item == null)
        {
            Debug.LogWarning($"Pickup '{name}' sin Item asignado.");
            return;
        }

        var inv = FindAnyObjectByType<Inventory>();
        if (!inv)
        {
            Debug.LogWarning("No se encontr� un Inventory en la escena.");
            return;
        }

        inv.AddItem(item);
        Destroy(gameObject); // quitar del mundo al guardarlo
    }
}
