using UnityEngine;

public class movimiento : MonoBehaviour
{
    [Header("Movimiento del Jugador")]
    public float speed = 5f;   // Velocidad de movimiento
    public float jumpForce = 5f; // Fuerza de salto
    public Transform cameraTransform; // Referencia a la c�mara
    private Rigidbody rb;
    private bool isGrounded;

    [Header("Movimiento de C�mara")]
    public float sensibilidad = 2f; // Sensibilidad del mouse
    private float rotacionX = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Evita que el jugador se incline
        Cursor.lockState = CursorLockMode.Locked; // Oculta el cursor y lo bloquea en el centro
    }

    void Update()
    {
        // Movimiento en X y Z
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = (cameraTransform.forward * moveZ + cameraTransform.right * moveX).normalized;
        move.y = 0; // Evita que el jugador se incline

        rb.MovePosition(rb.position + move * speed * Time.deltaTime);

        // Salto
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        // Movimiento de la c�mara
        MoverCamara();
    }

    private void MoverCamara()
    {
        // Movimiento del mouse para rotar la c�mara y el jugador
        float mouseX = Input.GetAxis("Mouse X") * sensibilidad;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidad;

        // Rotaci�n vertical de la c�mara (limitada para no girar de m�s)
        rotacionX -= mouseY;
        rotacionX = Mathf.Clamp(rotacionX, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);

        // Rotaci�n horizontal del jugador
        transform.Rotate(Vector3.up * mouseX);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
