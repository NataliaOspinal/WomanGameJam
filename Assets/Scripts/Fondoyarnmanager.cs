using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class FondoYarnManager : MonoBehaviour
{
    public RawImage fondo;

    public Texture[] fondos;

    public Color colorFundido = Color.black;

    private Image capaFundido;

    void Awake()
    {
        if (fondo == null)
        {
            Debug.LogWarning("FondoYarnManager: falta asignar el RawImage del fondo.", this);
            return;
        }

        CrearCapaFundido();
    }

    private void CrearCapaFundido()
    {
        var go = new GameObject("CapaFundido", typeof(RectTransform), typeof(Image));
        go.transform.SetParent(fondo.transform.parent, false);
        go.transform.SetSiblingIndex(fondo.transform.GetSiblingIndex() + 1);

        var rt = (RectTransform)go.transform;
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        capaFundido = go.GetComponent<Image>();
        capaFundido.color = new Color(colorFundido.r, colorFundido.g, colorFundido.b, 0f);
        capaFundido.raycastTarget = false; // no bloquea toques
    }

    // Yarn: <<cambiar_fondo NombreDelObjeto indice [duracion]>>

    [YarnCommand("cambiar_fondo")]
    public IEnumerator CambiarFondo(int indice, float duracion = 0.75f)
    {
        if (fondo == null || capaFundido == null) yield break;

        if (indice < 0 || indice >= fondos.Length)
        {
            Debug.LogWarning($"Índice de fondo fuera de rango: {indice}");
            yield break;
        }

        yield return Fundido(1f, duracion);
        fondo.texture = fondos[indice];
        yield return Fundido(0f, duracion);
    }

    /// <summary>Yarn: <<fundido_a_negro NombreDelObjeto [duracion]>></summary>
    [YarnCommand("fundido_a_negro")]
    public IEnumerator FundidoANegro(float duracion = 0.75f)
    {
        return Fundido(1f, duracion);
    }

    /// <summary>Yarn: <<fundido_desde_negro NombreDelObjeto [duracion]>></summary>
    [YarnCommand("fundido_desde_negro")]
    public IEnumerator FundidoDesdeNegro(float duracion = 0.75f)
    {
        return Fundido(0f, duracion);
    }

    private IEnumerator Fundido(float alphaFinal, float duracion)
    {
        if (capaFundido == null) yield break;

        float alphaInicial = capaFundido.color.a;

        if (duracion <= 0f)
        {
            AplicarAlpha(alphaFinal);
            yield break;
        }

        float t = 0f;
        while (t < duracion)
        {
            t += Time.deltaTime;
            AplicarAlpha(Mathf.SmoothStep(alphaInicial, alphaFinal, t / duracion));
            yield return null;
        }

        AplicarAlpha(alphaFinal);
    }

    private void AplicarAlpha(float alpha)
    {
        var c = capaFundido.color;
        c.a = alpha;
        capaFundido.color = c;
    }
}