using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class AudioYarnManager : MonoBehaviour
{
    public AudioSource reproductorSFX;
    public AudioClip[] listaSonidos;

    private readonly Dictionary<AudioSource, Coroutine> sonidosDeFondo =
        new Dictionary<AudioSource, Coroutine>();

    [YarnCommand("reproducir_sonido")]
    public IEnumerator ReproducirSonido(int indice, float duracion, bool fondo = false, float fadeSalida = 0f)
    {
        if (indice < 0 || indice >= listaSonidos.Length)
        {
            Debug.LogWarning($"Índice de sonido fuera de rango: {indice}");
            yield break;
        }

        AudioClip clip = listaSonidos[indice];

        if (fondo)
        {
            IniciarSonidoDeFondo(clip, duracion, fadeSalida);
            yield break; // no espera: el diálogo sigue
        }

        // Narrativo: se espera a que acabe
        reproductorSFX.PlayOneShot(clip);

        yield return new WaitForSeconds(duracion > 0f ? duracion : clip.length);

        reproductorSFX.Stop();
    }

    [YarnCommand("detener_sonidos")]
    public void DetenerSonidos(float fade = 0f)
    {
        var activos = new List<KeyValuePair<AudioSource, Coroutine>>(sonidosDeFondo);

        foreach (var par in activos)
        {
            if (par.Value != null) StopCoroutine(par.Value);
            StartCoroutine(DetenerSonidoDeFondo(par.Key, fade));
        }
    }

    private void IniciarSonidoDeFondo(AudioClip clip, float duracion, float fadeSalida)
    {
        AudioSource fuente = gameObject.AddComponent<AudioSource>();
        fuente.playOnAwake = false;
        fuente.clip = clip;
        fuente.volume = reproductorSFX.volume;
        fuente.spatialBlend = reproductorSFX.spatialBlend;
        fuente.outputAudioMixerGroup = reproductorSFX.outputAudioMixerGroup;
        fuente.Play();

        float vida = duracion > 0f ? duracion : clip.length;
        sonidosDeFondo[fuente] = StartCoroutine(ControlarSonidoDeFondo(fuente, vida, fadeSalida));
    }

    private IEnumerator ControlarSonidoDeFondo(AudioSource fuente, float vida, float fadeSalida)
    {
        fadeSalida = Mathf.Clamp(fadeSalida, 0f, vida);

        // Tiempo real: el sonido sigue sonando aunque el juego esté en pausa.
        yield return new WaitForSecondsRealtime(vida - fadeSalida);

        if (fadeSalida > 0f)
        {
            yield return FadeOut(fuente, fadeSalida);
        }

        LiberarFuente(fuente);
    }

    private IEnumerator DetenerSonidoDeFondo(AudioSource fuente, float fade)
    {
        if (fade > 0f)
        {
            yield return FadeOut(fuente, fade);
        }

        LiberarFuente(fuente);
    }

    private IEnumerator FadeOut(AudioSource fuente, float duracion)
    {
        float volumenInicial = fuente.volume;
        float t = 0f;

        while (t < duracion)
        {
            if (fuente == null) yield break;

            t += Time.unscaledDeltaTime;
            fuente.volume = Mathf.Lerp(volumenInicial, 0f, t / duracion);
            yield return null;
        }
    }

    private void LiberarFuente(AudioSource fuente)
    {
        sonidosDeFondo.Remove(fuente);

        if (fuente != null)
        {
            fuente.Stop();
            Destroy(fuente);
        }
    }
}