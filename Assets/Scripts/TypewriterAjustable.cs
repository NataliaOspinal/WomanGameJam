#nullable enable

using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using Yarn.Markup;
using Yarn.Unity;

/// <summary>
/// Typewriter personalizado para el LinePresenter (Typewriter Style = Custom).
/// Envuelve al LetterTypewriter de Yarn Spinner, pero lee la velocidad guardada
/// en PlayerPrefs justo antes de mostrar cada línea.
/// </summary>
public class TypewriterAjustable : MonoBehaviour, IAsyncTypewriter
{
    [Tooltip("Escribe en la Consola la velocidad y duración de cada línea (para depurar).")]
    [SerializeField] private bool mostrarLogs = true;

    // Se crea aquí (y no en Awake) para que exista cuando el LinePresenter
    // le asigne el TextElement y los ActionMarkupHandlers en su propio Awake.
    private readonly LetterTypewriter interno = new LetterTypewriter();

    public TMP_Text? TextElement
    {
        get => interno.TextElement;
        set => interno.TextElement = value;
    }

    public List<IActionMarkupHandler> ActionMarkupHandlers
    {
        get => interno.ActionMarkupHandlers;
        set => interno.ActionMarkupHandlers = value;
    }

    private float LeerVelocidad()
    {
        return PlayerPrefs.GetFloat(
            SliderVelocidadDialogo.ClavePrefs,
            SliderVelocidadDialogo.VelocidadPorDefecto);
    }

    public void PrepareForContent(MarkupParseResult line)
    {
        interno.CharactersPerSecond = LeerVelocidad();
        interno.PrepareForContent(line);
    }

    public async YarnTask RunTypewriter(MarkupParseResult line, CancellationToken cancellationToken)
    {
        float velocidad = LeerVelocidad();
        interno.CharactersPerSecond = velocidad;

        int caracteres = line.Text.Length;
        float inicio = Time.realtimeSinceStartup;

        if (mostrarLogs)
        {
            Debug.Log($"[Typewriter] EMPIEZA línea: {velocidad} letras/s, {caracteres} caracteres, " +
                      $"duración esperada ≈ {caracteres / Mathf.Max(velocidad, 0.01f):0.0}s");
        }

        await interno.RunTypewriter(line, cancellationToken);

        if (mostrarLogs)
        {
            float duracion = Time.realtimeSinceStartup - inicio;
            string motivo = cancellationToken.IsCancellationRequested
                ? "ACELERADA (hurry up / toque en pantalla)"
                : "completa";
            Debug.Log($"[Typewriter] TERMINA línea: tardó {duracion:0.00}s -> {motivo}");
        }
    }

    public void ContentWillDismiss() => interno.ContentWillDismiss();

    public void ContentDidDismiss() => interno.ContentDidDismiss();
}