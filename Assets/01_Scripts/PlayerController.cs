using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
[DisallowMultipleComponent]
public class PlayerController : MonoBehaviour
{

    [Header("Camara y movimiento")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float mouseSensitivity = 200f;
    private float horizontalRotation;
    private float verticalRotation;
    private Vector3 PlayerMovement;
    private Transform camTransform;
    private float currentSpeed;

    [Header("Salto")]
    public float jumpForce = 10;
    private Rigidbody rb;

    [Header("Suelo")]
    private bool isGrounded;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;


    private void Awake()
    {
        camTransform = GetComponentInChildren<Camera>().transform;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        #region Movimiento 
        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        float moveX = Input.GetAxis("Horizontal") * currentSpeed;
        float moveZ = Input.GetAxis("Vertical") * currentSpeed;

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        PlayerMovement = new Vector3(move.x, 0, move.z);

        transform.Translate(PlayerMovement * Time.deltaTime, Space.World);
        #endregion

        #region Camara
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        horizontalRotation += mouseX;
        verticalRotation = Mathf.Clamp(verticalRotation - mouseY, -90f, 90f);

        transform.rotation = Quaternion.Euler(0, horizontalRotation, 0);
        camTransform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        #endregion

        #region Suelo
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        #endregion

        #region Salto
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        #endregion
        }
}