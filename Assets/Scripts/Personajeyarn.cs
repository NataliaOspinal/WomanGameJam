using System.Collections;
using UnityEngine;
using Yarn.Unity;

/// <summary>
/// Va en el objeto (Image) de un personaje, hijo del Canvas.
/// Déjalo en el editor EN LA POSICIÓN donde debe verse en pantalla: al empezar el juego
/// se esconde fuera de la pantalla, a la derecha, y los comandos de Yarn lo hacen
/// entrar (de derecha a izquierda) o salir (de izquierda a derecha).
///
/// Yarn: <<personaje_entrar NombreDelObjeto [duracion] [esperar]>>
///       <<personaje_salir  NombreDelObjeto [duracion] [esperar]>>
/// </summary>
public class PersonajeYarn : MonoBehaviour
{
    private RectTransform rt;
    private Canvas canvasRaiz;
    private Vector2 posicionEnPantalla;
    private Coroutine movimiento;
    private bool visible;
    private bool inicializado;

    void Start()
    {
        Inicializar();
    }

    private void Inicializar()
    {
        if (inicializado) return;
        inicializado = true;

        rt = (RectTransform)transform;

        var canvas = GetComponentInParent<Canvas>();
        canvasRaiz = canvas != null ? canvas.rootCanvas : null;

        posicionEnPantalla = rt.anchoredPosition;

        // Empieza escondido, fuera de la pantalla por la derecha.
        rt.anchoredPosition = posicionEnPantalla + Vector2.right * (DistanciaFuera() + 2000f);
    }

    // Cuánto hay que desplazarse a la derecha para quedar totalmente fuera de pantalla.
    private float DistanciaFuera()
    {
        float anchoCanvas = canvasRaiz != null
            ? ((RectTransform)canvasRaiz.transform).rect.width
            : Screen.width;

        return anchoCanvas + rt.rect.width;
    }

    [YarnCommand("personaje_entrar")]
    public IEnumerator Entrar(float duracion = 0.3f, bool esperar = true)
    {
        Inicializar();
        if (visible) yield break;
        visible = true;

        // Parte justo en el borde derecho, fuera de pantalla.
        rt.anchoredPosition = posicionEnPantalla + Vector2.right * DistanciaFuera();

        Coroutine animacion = Iniciar(posicionEnPantalla, duracion, true);
        if (esperar) yield return animacion;
    }

    [YarnCommand("personaje_salir")]
    public IEnumerator Salir(float duracion = 0.3f, bool esperar = true)
    {
        Inicializar();
        if (!visible) yield break;
        visible = false;

        Vector2 destino = posicionEnPantalla + Vector2.right * DistanciaFuera();

        Coroutine animacion = Iniciar(destino, duracion, false);
        if (esperar) yield return animacion;
    }

    private Coroutine Iniciar(Vector2 destino, float duracion, bool entrando)
    {
        if (movimiento != null) StopCoroutine(movimiento);
        movimiento = StartCoroutine(Animar(destino, duracion, entrando));
        return movimiento;
    }

    private IEnumerator Animar(Vector2 destino, float duracion, bool entrando)
    {
        Vector2 origen = rt.anchoredPosition;

        if (duracion <= 0f)
        {
            rt.anchoredPosition = destino;
            yield break;
        }

        float t = 0f;
        while (t < duracion)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / duracion);

            float curva = entrando ? 1f - Mathf.Pow(1f - k, 3f) : k * k * k;

            rt.anchoredPosition = Vector2.LerpUnclamped(origen, destino, curva);
            yield return null;
        }

        rt.anchoredPosition = destino;
    }
}