using UnityEngine;

public class FirstPerson : MonoBehaviour
{
    public Transform ball; // Referencia al objeto de la pelota
    public Vector3 cameraOffset; // Offset de la cámara respecto a la pelota

    public float mouseSensitivity = 1000.0f; // Sensibilidad del movimiento del ratón aumentada
    private float yaw = 0.0f; // Rotación horizontal

    // Start is called before the first frame update
    void Start()
    {
        // Opcional: Ocultar el cursor durante el juego
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (ball != null)
        {
            // Mueve la cámara a la nueva posición, que es la posición de la pelota más el offset
            transform.position = ball.position + cameraOffset;

            // Obtiene la entrada del ratón
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

            // Aplica la rotación horizontal
            transform.rotation = Quaternion.Euler(0.0f, yaw, 0.0f);
        }

        // Opcional: Salir del modo bloqueado del cursor al presionar Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
