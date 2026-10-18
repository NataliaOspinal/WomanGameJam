using UnityEngine;
using UnityEngine.UI;
using TMPro; // Necesario para leer el texto

public class ControladorBurbujas : MonoBehaviour
{
    public TextMeshProUGUI textoNombrePersonaje;
    public Image imagenFondoBurbuja;

    public Sprite burbujaNoelia;
    public Sprite burbujaYessica;
    public Sprite burbujaPorDefecto;

    private string nombreAnterior = "";

    void Update()
    {
        if (textoNombrePersonaje.text != nombreAnterior)
        {
            nombreAnterior = textoNombrePersonaje.text;

            if (nombreAnterior == "NOELIA")
            {
                imagenFondoBurbuja.sprite = burbujaNoelia;
            }
            else if (nombreAnterior == "YESSICA")
            {
                imagenFondoBurbuja.sprite = burbujaYessica;
            }
            else
            {
                imagenFondoBurbuja.sprite = burbujaPorDefecto;
            }
        }
    }
}