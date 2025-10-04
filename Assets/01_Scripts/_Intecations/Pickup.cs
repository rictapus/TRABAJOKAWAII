using UnityEngine;

public class Pickup : MonoBehaviour
{
    #region Variables
    bool isHolding = false;

    [SerializeField] float throwForce = 600f;
    [SerializeField] float maxDistance = 3f;
    [SerializeField] float distance;

    TempParent tempParent;
    Rigidbody rb;
    Vector3 objectPos;
    #endregion

    #region Awake & Start
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
