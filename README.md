# Examen-Unity


## Personalización del tablero

En el tablero del juego he añadido unos cuantos recorridos y obstáculos para hacer el roll a ball mas completo.
Lo primero que nos encontraremos al iniciar el juego es una compuerta que se abrirá simplemente acercándonos. Al traspasar la puerta saldremos de la zona segura la cual nos separa del enemigo.

![alt text](<Imagenes/Captura de pantalla 2024-03-17 192117.png>)

También tendremos monedas repartidas por el tablero. Las primeras están cerca pero la última se encuentra al final del laberinto, mientras tanto el enemigo nos perseguirá en todo momento y si nos toca perderemos la partida. En caso de estar simplemente cerca nuestro jugador parapadeara alertando de la presencia del enemigo.


## Interaciones

### Mensaje de derrota

Empezaré hablando de la interación con el enemigo, el cual nos perseguirá todo el rato y si en algun momento nos toca nos mostrará un mensaje en la pantalla que nos indicará que hemos perdido

![alt text](<Imagenes/Captura de pantalla 2024-03-17 192416.png>)

Este mensaje lo logramos gracias al script de EnemyController el cual con la siguiente clase nos permitirá mostrar el mensaje

```C
 void OnCollisionEnter(Collision collision)
    {
        // Verifica si el enemigo ha colisionado con el jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            // Activa el objeto de texto de "Has perdido"
            gameOverText.SetActive(true);
        }
    }
```

### Parpadeo de jugador

También podremos observar que cuando tenemos cerca al enemigo nuestro jugador empezara a parpadear hasta que volvamos a estar lejos de el.


![alt text](Gifs/Grabación-2024-03-17-193520.gif)


Esto ocurre gracias a los scripts EnemyController y PlayerController

En EnemyController tenemos la siguiente clase la cual detecta si el jugador esta cerca. En esta clase tambien configuramos que el enemigo recoja la posición del jugador en todo momento para que pueda seguirlo

```C
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
```

En cuanto a la clase PlayerController tenemos las siguiente clases para que empiece el parapadeo cuando el jugador esta cerca y para que pare de parapadear cuando ya nos alejamos del enemigo y por último tenemos la clase la cual configura los colores y el tiempo que tendrá cada color para que de ese efecto de parpadeo.

```C
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
```
### Animacón compuerta

Ahora hablaremos de la animación que permite que la puerta se abra y se cierre tan solo acercándonos debido a que hay un botón invisible que si detecta algo encima abrirá la puerta. 

![alt text](Gifs/Grabación-2024-03-17-195003.gif)

Esto es gracias al animator de unity y a un script que mostraré ahora.


Esto es el esquema del animator el cual nos muestra el ciclo de acciones que ocurren. La anumación abrir y cerrar las crearemos creando un clip del movimiento de la puerta abriendose y cerrandose.

![alt text](<Imagenes/Captura de pantalla 2024-03-17 195405.png>)

Ahora mostraré el script para que cuando pasemos por encima de un objeto el objeto detecta al jugador y abre la puerta. 

```C
public class PuertaAnimator : MonoBehaviour
{

  public Animator laPuerta;

  private void OnTriggerEnter(Collider other) 
  {
 laPuerta.Play("abrir");
  }   
 private void OntriggerExit(Collider other)
  {
    laPuerta.Play("cerrar");
}
}
```

### Monedas

Por último hablaré de la recolección de monedas. Estas monedas están repartidas por el tablero y podremos recogerlas. Cuando pasemos por encima de ellas desaparecerán esto es gracias al script rotator 


![alt text](Gifs/Grabación-2024-03-17-201516.gif)


```C

  private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Llama al método Recoger si el objeto que entra en contacto es el jugador
            Recoger();
        }
    }
```
En el script PlayerController tenemos un metodo al cual llamarán para recoger las monedas

Y también tenemos una clase para que moneda rote la cual es la siguiente.

```C

   void Update()
    {
        // Rotate the object on X, Y, and Z axes by specified amounts, adjusted for frame rate.
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
    }

```

### Salto y deformación de jugador

Tenemos una función la cual permite que el jugador pueda saltar presionando el espacio y al saltar el jugador que en este caso es una esfera, se deformará y cuando toque el suelo de nuevo volverá a su forma normal.

![alt text](Gifs/Grabación-2024-03-17-202035.gif)


para poder saltar crearemos la siguiente clase en el script PlayerController para que nos permita saltar. Y para que se deforme haremos como con la puerta. Utilizaremos el animator y grabaremos un clip deformando la pelota y cuando toque el suelo haremos otro clip poniendola en su forma original.


```C
  private void HandleJumping()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            transform.localScale = jumpScale;
        }
    }
```

### Primera persona 

También implemente una cámara en primera persona el cual permite ver la camara del jugador en primera persona pero tengo un problema con los controles que se invierten pero en si la cámara funciona gracias al siguiente script.

```C
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
```

Y hasta aqui mi Roll a Ball.











