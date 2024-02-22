using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float jumpForce = 5f;
    private Rigidbody rb;
    private Transform cameraTransform;
    private bool isGrounded = true;
    public Vector3 jumpScale = new Vector3(1.2f, 0.8f, 1.2f);
    private Vector3 originalScale;
    private Renderer renderer; // Componente Renderer para cambiar el color
    public Color hitColor = Color.red; // Color cuando el jugador es golpeado
    private Color originalColor; // Color original del jugador
    public float blinkDuration = 0.5f; // Duración del parpadeo
    private float blinkTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
        originalScale = transform.localScale;
        renderer = GetComponent<Renderer>(); // Obtiene el componente Renderer
        originalColor = renderer.material.color; // Guarda el color original
    }

    void Update()
    {
        // Mover y saltar al jugador
        HandleMovement();
        HandleJumping();

        // Proceso de parpadeo
        if (blinkTimer > 0)
        {
            blinkTimer -= Time.deltaTime;
            if (blinkTimer <= 0)
            {
                renderer.material.color = originalColor; // Restaura el color original
            }
        }
    }

    private void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0;
        Vector3 cameraRight = cameraTransform.right;
        Vector3 movement = (cameraForward * moveVertical + cameraRight * moveHorizontal).normalized;
        rb.velocity = new Vector3(movement.x * moveSpeed, rb.velocity.y, movement.z * moveSpeed);
    }

    private void HandleJumping()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            transform.localScale = jumpScale;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            transform.localScale = originalScale;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
        }
        else if (other.gameObject.CompareTag("Enemy")) // Detecta al enemigo
        {
            BlinkRed(); // Inicia el parpadeo
        }
    }

    void BlinkRed()
    {
        renderer.material.color = hitColor; // Cambia al color rojo
        blinkTimer = blinkDuration; // Reinicia el temporizador de parpadeo
    }
}
