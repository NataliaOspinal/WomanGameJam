using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Yarn.Unity;

/// <summary>
/// Va en una Image o RawImage hija del Canvas, colocada DESPUÉS del Line Presenter y
/// ANTES del Options Presenter en la jerarquía. Se estira sola a pantalla completa y
/// se vuelve transparente. Cada toque dispara UN solo evento: si la línea se está
/// escribiendo la acelera, y si ya terminó pasa a la siguiente.
/// Requiere que el Line Advancer tenga Input Mode = Manual.
/// </summary>
public class TocarParaAvanzar : MonoBehaviour, IPointerClickHandler
{
    [Tooltip("Si se deja vacío, se busca automáticamente en la escena.")]
    [SerializeField] private LineAdvancer lineAdvancer;

    [Tooltip("Escribe un log por cada toque (para depurar).")]
    [SerializeField] private bool mostrarLogs = true;

    void Awake()
    {
        // Cubrir toda la pantalla, sin importar cómo esté colocada en el editor.
        var rt = (RectTransform)transform;
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        rt.localScale = Vector3.one;

        // Invisible pero que sí recibe toques.
        var grafico = GetComponent<Graphic>();
        if (grafico == null)
        {
            Debug.LogWarning("TocarParaAvanzar necesita una Image o RawImage en el mismo objeto para recibir toques.", this);
            return;
        }

        grafico.raycastTarget = true;
        var color = grafico.color;
        color.a = 0f;
        grafico.color = color;
    }

    [System.Obsolete]
    void Start()
    {
        if (lineAdvancer == null)
        {
            lineAdvancer = FindFirstObjectByType<LineAdvancer>();
        }

        if (lineAdvancer == null)
        {
            Debug.LogWarning("TocarParaAvanzar: no encontré ningún LineAdvancer en la escena.", this);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Con el panel de opciones abierto el juego está en pausa: no avanzar.
        if (Time.timeScale == 0f)
        {
            if (mostrarLogs) Debug.Log("[Toque] ignorado: el juego está en pausa");
            return;
        }

        if (lineAdvancer == null) return;

        if (mostrarLogs)
        {
            Debug.Log($"[Toque] frame {Time.frameCount}");
        }

        lineAdvancer.OnInputHurryUpLines();
    }
}