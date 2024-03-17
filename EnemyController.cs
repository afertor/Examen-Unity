using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 5f;
    public float proximityThreshold = 3f; // Se mantiene para el parpadeo basado en la proximidad.
    private Transform player;
    private PlayerController playerController;
    private Rigidbody rb; // Referencia al Rigidbody del enemigo
    public GameObject gameOverText; // Referencia al objeto de texto de "Has perdido"

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = player.GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();

        // Desactiva el texto al inicio del juego
        gameOverText.SetActive(false);
    }

    void FixedUpdate()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        
        // Siempre calcula la dirección hacia el jugador y mueve el enemigo.
        Vector3 targetDirection = player.position - transform.position;
        targetDirection.y = 0; // Mantén el movimiento en el plano horizontal
        targetDirection.Normalize();
        rb.MovePosition(rb.position + targetDirection * speed * Time.fixedDeltaTime);

        // Comienza o detiene el parpadeo basado en la proximidad al jugador.
        if (distanceToPlayer < proximityThreshold)
        {
            playerController.StartBlinkingEffect(); // Comienza el parpadeo
        }
        else
        {
            playerController.StopBlinkingEffect(); // Detiene el parpadeo
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Verifica si el enemigo ha colisionado con el jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            // Activa el objeto de texto de "Has perdido"
            gameOverText.SetActive(true);
        }
    }
}
