using UnityEngine;

public class movimiento : MonoBehaviour
{
    public float speed = 5f;   // Velocidad de movimiento
    public float jumpForce = 5f; // Fuerza de salto
    public Transform cameraTransform; // Referencia a la cámara
    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Evita que el jugador se incline
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
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}