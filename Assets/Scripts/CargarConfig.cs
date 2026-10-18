using UnityEngine;
using Yarn.Unity;

public class CargarConfig : MonoBehaviour
{
    public LinePresenter presentadorDeLinea;

    // Aquí defines cuántos segundos esperará para pasar al siguiente diálogo
    public float segundosDeEspera = 1.5f;

    void Start()
    {
        if (presentadorDeLinea != null)
        {
            float velocidadGuardada = PlayerPrefs.GetFloat("VelocidadTexto", 60f);
            presentadorDeLinea.lettersPerSecond = Mathf.RoundToInt(velocidadGuardada);

            bool textoAutomaticoActivado = PlayerPrefs.GetInt("TextoAutomatico", 0) == 1;

            presentadorDeLinea.autoAdvance = textoAutomaticoActivado;

            presentadorDeLinea.autoAdvanceDelay = segundosDeEspera;
        }
    }
}