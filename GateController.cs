using UnityEngine;

public class GateController : MonoBehaviour
{
    public Animator animator;
    private bool isOpen = false;

    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOpen = !isOpen; // Cambia el estado de la compuerta
            animator.SetBool("IsOpen", false); // Cambia la animación
        }
    }
}
