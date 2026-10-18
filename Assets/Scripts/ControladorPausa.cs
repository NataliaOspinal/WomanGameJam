using UnityEngine;
using Yarn.Unity; // Necesario para pausar el diálogo

public class ControladorPausa : MonoBehaviour
{
    public GameObject panelOpciones;
    public GameObject botonPausa;  
    public DialogueRunner dialogueRunner;

    private bool estaPausado = false;
    public void AlternarPausa()
    {
        estaPausado = !estaPausado; 

        if (panelOpciones != null)
        {
            panelOpciones.SetActive(estaPausado);
        }

        Time.timeScale = estaPausado ? 0f : 1f;
    }
}