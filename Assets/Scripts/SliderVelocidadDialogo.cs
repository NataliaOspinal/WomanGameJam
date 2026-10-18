using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderVelocidadDialogo : MonoBehaviour
{
    public const string ClavePrefs = "VelocidadTexto";
    public const float VelocidadPorDefecto = 60f;

    public Slider sliderVelocidad;
    public TextMeshProUGUI textoValor;

    void Awake()
    {
        sliderVelocidad.minValue = 10f;
        sliderVelocidad.maxValue = 120f;
    }

    void OnEnable()
    {
        float valor = PlayerPrefs.GetFloat(ClavePrefs, VelocidadPorDefecto);
        sliderVelocidad.SetValueWithoutNotify(valor);
        ActualizarTextoDescriptivo(valor);
        sliderVelocidad.onValueChanged.AddListener(AlCambiar);
    }

    void OnDisable()
    {
        sliderVelocidad.onValueChanged.RemoveListener(AlCambiar);
        PlayerPrefs.Save(); // guardar al cerrar el panel, no en cada movimiento
    }

    void AlCambiar(float valor)
    {
        // El TypewriterAjustable lee este valor al empezar cada línea.
        PlayerPrefs.SetFloat(ClavePrefs, valor);
        ActualizarTextoDescriptivo(valor);
    }

    void ActualizarTextoDescriptivo(float valor)
    {
        if (valor < 60f) textoValor.text = "Lento";
        else if (valor < 100f) textoValor.text = "Normal";
        else textoValor.text = "Rápido";
    }
}