using UnityEngine;

public class Pickup : MonoBehaviour, IInteractable
{
    #region Variables

    #region Var Interact
    public Item item;
    public Interactor interactor;      // (no se usa aquí, se mantiene por tu lógica)
    public Transform InteractorSource; // (no se usa aquí, se mantiene por tu lógica)
    public float InteractorRange;      // (no se usa aquí, se mantiene por tu lógica)
    Inventory inventory;
    #endregion

    #region Var Pickup
    bool isHolding = false;
    [SerializeField] float throwForce = 600f;
    [SerializeField] float maxDistance = 3f;
    [SerializeField] float distance;

    TempParent tempParent;
    Rigidbody rb;
    Vector3 objectPos;
    #endregion
    #endregion

    #region Awake & Start
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inventory = GetComponent<Inventory>(); // si no hay Inventory aquí, se busca en Interact()
    }

    void Start()
    {
        tempParent = TempParent.Instance;
    }
    #endregion

    #region Update
    void Update()
    {
        if (isHolding)
        {
            Hold();

            // >>> AÑADIDO: recoger al INVENTARIO al presionar E mientras sostienes <<<
            if (Input.GetKeyDown(KeyCode.E))
            {
                Interact(); // usa tu misma lógica de Interact para agregar al inventario
            }
        }
    }
    #endregion

    #region Interact
    public void Interact()
    {
        // Asegura que hay un Inventory de jugador
        if (inventory == null)
        {
            inventory = FindFirstObjectByType<Inventory>();
        }

        if (item == null)
        {
            Debug.LogWarning("Pickup: 'item' no asignado en el objeto.");
            return;
        }

        if (inventory != null)
        {
            // Si lo estás sosteniendo, suéltalo antes de destruir (mantiene tu flujo limpio)
            if (isHolding) Drop();

            inventory.AddItem(item);
            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("No se encontro el PlayerInventory en la escena");
        }
    }
    #endregion

    #region Mouse Events
    private void OnMouseDown()
    {
        if (tempParent != null)
        {
            distance = Vector3.Distance(this.transform.position, tempParent.transform.position);

            if (distance <= maxDistance)
            {
                isHolding = true;
                rb.useGravity = false;
                rb.detectCollisions = true;

                this.transform.SetParent(tempParent.transform);
            }
        }
        else
        {
            Debug.Log("No se encontro el TempParent en la escena");
        }
    }

    private void OnMouseUp()
    {
        Drop();
    }

    private void OnMouseExit()
    {
        Drop();
    }

    private void Hold()
    {
        distance = Vector3.Distance(this.transform.position, tempParent.transform.position);

        if (distance >= maxDistance)
        {
            Drop();
        }

        // FIX: Unity no tiene linearVelocity
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Click derecho: lanzar y soltar (manteniendo tu lógica)
        if (Input.GetMouseButtonDown(1))
        {
            rb.AddForce(tempParent.transform.forward * throwForce);
            Drop();
        }
    }

    private void Drop()
    {
        if (isHolding)
        {
            isHolding = false;
            objectPos = this.transform.position;
            this.transform.position = objectPos;
            this.transform.SetParent(null);
            rb.useGravity = true;
        }
    }
    #endregion
}
