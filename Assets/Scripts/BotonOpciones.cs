using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class BotonOpciones : MonoBehaviour
{
    public string clavePreferencia = "TextoAutomatico";
    public bool valorDeEsteBoton;

    public Image imagenFondo;
    public Color colorInactivo = Color.white;
    public Color colorActivo = Color.yellow;

    public LinePresenter presentadorDeLinea;

    void Start()
    {
        bool guardado = PlayerPrefs.GetInt(clavePreferencia, 0) == 1;
        ActualizarVisual(guardado);

        // Forzamos el estado inicial al cargar
        ActualizarYarnSpinner(guardado);
    }

    public void AlHacerClic()
    {
        PlayerPrefs.SetInt(clavePreferencia, valorDeEsteBoton ? 1 : 0);
        PlayerPrefs.Save();

        // Forzamos la actualización al hacer clic
        ActualizarYarnSpinner(valorDeEsteBoton);

        ActualizarVisual(valorDeEsteBoton);

        BotonOpciones[] todosLosBotones = transform.parent.GetComponentsInChildren<BotonOpciones>();
        foreach (var boton in todosLosBotones)
        {
            if (boton != this)
            {
                boton.ActualizarVisual(valorDeEsteBoton);
            }
        }
    }

    private void ActualizarYarnSpinner(bool estado)
    {
        if (presentadorDeLinea != null)
        {
            presentadorDeLinea.autoAdvance = estado;
            Debug.Log("AutoAdvance actualizado a: " + presentadorDeLinea.autoAdvance);
        }
        else
        {
            Debug.LogWarning("LinePresenter no asignado en el Botón " + gameObject.name);
        }
    }

    public void ActualizarVisual(bool estadoGlobal)
    {
        if (estadoGlobal == valorDeEsteBoton)
        {
            imagenFondo.color = colorActivo;
        }
        else
        {
            imagenFondo.color = colorInactivo;
        }
    }
}