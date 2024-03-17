using UnityEngine;

public class Rotator : MonoBehaviour
{
    // Valor por el cual se multiplicará la velocidad del jugador al recoger este pickup
    public float speedMultiplier = 10f;

    // Método para manejar la acción de recoger el objeto
    private void Recoger()
    {
        // Desactiva el objeto "pickup" cuando el jugador lo toca
        gameObject.SetActive(false);

        // Encuentra al jugador y llama a la función ModifyMoveSpeed con el multiplicador de velocidad
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.ModifyMoveSpeed(speedMultiplier);
            }
        }

    }

    // Método llamado cuando un objeto entra en contacto con el objeto que contiene este script
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Llama al método Recoger si el objeto que entra en contacto es el jugador
            Recoger();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the object on X, Y, and Z axes by specified amounts, adjusted for frame rate.
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
    }
}
