using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    // Referencia al ScoreManager
    private ScoreManager scoreManager;

    public float moveSpeed = 5.0f;
    public float jumpForce = 5f;
    private Rigidbody rb;
    private Transform cameraTransform;
    private bool isGrounded = true;
    public Vector3 jumpScale = new Vector3(1.2f, 0.8f, 1.2f);
    private Vector3 originalScale;
    private Renderer playerRenderer;
    public Color blinkColor1 = Color.red;
    public Color blinkColor2 = Color.blue;
    private Color originalColor;
    private bool isBlinking = false;
    private Coroutine blinkingCoroutine;
    private float originalMoveSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
        originalScale = transform.localScale;
        playerRenderer = GetComponent<Renderer>();
        originalColor = playerRenderer.material.color;
        originalMoveSpeed = moveSpeed;

        // Busca el ScoreManager en la escena
        scoreManager = FindObjectOfType<ScoreManager>();

        if (scoreManager == null)
        {
            Debug.LogError("ScoreManager no encontrado en la escena.");
        }
    }

    void Update()
    {
        HandleMovement();
        HandleJumping();
    }

    private void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0;
        Vector3 cameraRight = cameraTransform.right;
        cameraRight.y = 0;
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            transform.localScale = originalScale;
        }

        if (collision.gameObject.CompareTag("PickUp"))
        {
            collision.gameObject.SetActive(false);
            if (scoreManager != null)
            {
                scoreManager.IncreaseScore(); // Incrementa el contador de pickups recolectados
                if (scoreManager.IsAllPickupsCollected()) // Verifica si se han recolectado todos los pickups
                {
                    Debug.Log("Has Ganado!");
                }
                // Inicia el parpadeo
                StartBlinkingEffect();
            }
        }
    }

    public void StartBlinkingEffect()
    {
        if (!isBlinking)
        {
            isBlinking = true;
            blinkingCoroutine = StartCoroutine(BlinkEffect());
        }
    }

    public void StopBlinkingEffect()
    {
        if (isBlinking)
        {
            StopCoroutine(blinkingCoroutine);
            playerRenderer.material.color = originalColor;
            isBlinking = false;
        }
    }

    IEnumerator BlinkEffect()
    {
        while (isBlinking)
        {
            playerRenderer.material.color = blinkColor1;
            yield return new WaitForSeconds(0.1f);
            playerRenderer.material.color = blinkColor2;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void ModifyMoveSpeed(float multiplier)
    {
        moveSpeed = originalMoveSpeed * multiplier;
    }
}



