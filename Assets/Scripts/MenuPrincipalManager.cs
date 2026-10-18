using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipalManager : MonoBehaviour
{
    public GameObject panelCreditos;
    public GameObject panelOpciones;
    void Start() //porsiacaso
    {
        if (panelCreditos != null)
        {
            panelCreditos.SetActive(false);
        }
        if (panelOpciones != null)
        {
            panelOpciones.SetActive(false);
        }
    }

    public void IniciarJuego()
    {
        SceneManager.LoadScene("01_Escena1");
    }

    public void MostrarCreditos()
    {
        if (panelCreditos != null)
        {
            panelCreditos.SetActive(true);
        }
    }

    public void OcultarCreditos()
    {
        if (panelCreditos != null)
        {
            panelCreditos.SetActive(false);
        }
    }

    public void MostrarOpciones()
    {
        if (panelOpciones != null)
        {
            panelOpciones.SetActive(true);
        }
    }

    public void OcultarOpciones()
    {
        if (panelOpciones != null)
        {
            panelOpciones.SetActive(false);
        }
    }
}
