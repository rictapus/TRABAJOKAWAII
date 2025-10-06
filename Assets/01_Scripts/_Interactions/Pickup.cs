using UnityEngine;

public class Pickup : MonoBehaviour, IInteractable
{
    #region Variables

    #region Var Interact
    public Item item;
    public Interactor interactor;      
    public Transform InteractorSource;
    public float InteractorRange;      
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
        inventory = GetComponent<Inventory>(); 
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

            if (Input.GetKeyDown(KeyCode.E))
            {
                Interact();
            }
        }
    }
    #endregion

    #region Interact
    public void Interact()
    {
        if (inventory == null)
        {
            inventory = FindFirstObjectByType<Inventory>();
        }

        if (item == null)
        {
            return;
        }

        if (inventory != null)
        {
            if (isHolding) Drop();

            inventory.AddItem(item);
            Destroy(gameObject);
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

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

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
