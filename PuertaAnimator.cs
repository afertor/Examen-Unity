using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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