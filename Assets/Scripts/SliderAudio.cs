using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SliderAudio : MonoBehaviour
{
    public AudioMixer audioMixer;
    public string nombreParametroExpuesto;
    public Slider slider;

    void Start()
    {
        slider.minValue = 0.0001f;
        slider.maxValue = 1f;

        float valorGuardado = PlayerPrefs.GetFloat(nombreParametroExpuesto, 0.75f);
        slider.value = valorGuardado;

        AplicarVolumen(valorGuardado);

        slider.onValueChanged.AddListener(AplicarVolumen);
    }

    public void AplicarVolumen(float valor)
    {
        float volumenEnDecibelios = Mathf.Log10(valor) * 20f;

        audioMixer.SetFloat(nombreParametroExpuesto, volumenEnDecibelios);

        PlayerPrefs.SetFloat(nombreParametroExpuesto, valor);
        PlayerPrefs.Save();
    }
}