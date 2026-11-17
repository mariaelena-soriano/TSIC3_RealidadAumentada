
using UnityEngine;
using TMPro;

public class aleatorio : MonoBehaviour
{
    [Header("Los 5 modelos 3D")]
    public GameObject[] objetos;

    [Header("Texto de la narrativa")]
    public TMP_Text textoMision;

    [Header("Posicion de aparicion")]
    public Vector3 posicionAparicion =
        new Vector3(0.15f, 0.05f, 0f);

    private int ultimoIndice = -1;
    private GameObject modeloActual;

    void Start()
    {
        MostrarMensaje(
            "Descubre tu proxima mision. " +
            "¡Presiona MOVE!"
        );
    }

    // ELEGIR Y MOSTRAR MODELO ALEATORIO
    public void MostrarAleatorio(Transform destino)
    {
        if (objetos == null || objetos.Length == 0)
        {
            Debug.LogWarning("No hay modelos asignados.");
            return;
        }

        if (destino == null)
            return;

        OcultarModelo();

        int indice;

        do
        {
            indice = Random.Range(0, objetos.Length);
        }
        while (objetos.Length > 1 &&
               indice == ultimoIndice);

        GameObject original = objetos[indice];

        if (original == null)
            return;

        modeloActual = Instantiate(
            original,
            destino,
            false
        );

        modeloActual.name = original.name + "_Aleatorio";

        modeloActual.transform.localPosition =
            posicionAparicion;

        modeloActual.transform.localRotation =
            original.transform.localRotation;

        modeloActual.transform.localScale =
            original.transform.localScale;

        modeloActual.SetActive(true);

        ultimoIndice = indice;

        // Mostrar la mision correspondiente
        MostrarMensaje(ObtenerMision(indice));

        Debug.Log(
            "Modelo seleccionado: " + original.name
        );
    }

    // NARRATIVA DE PASAPORTE AR
    private string ObtenerMision(int indice)
    {
        switch (indice)
        {
            case 0:
                return "MISION: VISITA UN MUSEO\n" +
                       "Descubre una obra de arte " +
                       "o un personaje historico.";

            case 1:
                return "MISION: VIAJA EN AVION\n" +
                       "¡Preparate para descubrir " +
                       "tu siguiente destino!";

            case 2:
                return "MISION: EXPLORA EN TREN\n" +
                       "Recorre la ciudad y conoce " +
                       "un lugar historico.";

            case 3:
                return "MISION: DESCUBRE UN MONUMENTO\n" +
                       "Investiga un dato interesante " +
                       "sobre las piramides.";

            case 4:
                return "MISION: CAPTURA UN RECUERDO\n" +
                       "Toma una fotografia del " +
                       "monumento que estas visitando.";

            default:
                return "¡Nueva mision desbloqueada!";
        }
    }

    // ACTUALIZAR TEXTO DE LA INTERFAZ
    public void MostrarMensaje(string mensaje)
    {
        if (textoMision != null)
        {
            textoMision.text = mensaje;
        }
    }

    // OCULTAR MODELO ANTERIOR
    public void OcultarModelo()
    {
        if (modeloActual != null)
        {
            Destroy(modeloActual);
            modeloActual = null;
        }
    }

    // BOTON RESET
    public void ResetearAleatorio()
    {
        OcultarModelo();

        ultimoIndice = -1;

        MostrarMensaje(
            "¡Bienvenido a Pasaporte AR!\n" +
            "Presiona MOVE para descubrir " +
            "una nueva mision."
        );
    }
}
