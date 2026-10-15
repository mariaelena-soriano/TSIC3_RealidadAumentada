
using System.Collections;
using UnityEngine;
using Vuforia;

public class move : MonoBehaviour
{
    [Header("Modelo que se movera")]
    public GameObject model;

    [Header("Marcadores de Vuforia")]
    public ObserverBehaviour[] ImageTargets;

    [Header("Seleccion aleatoria")]
    public aleatorio selectorAleatorio;

    [Header("Configuracion del movimiento")]
    public int currentTarget = -1;
    public float speed = 1.0f;

    [Header("Separacion visual")]
    public float separacionIphone = -0.15f;

    private bool isMoving = false;

    private Transform padreInicial;
    private Vector3 posicionInicial;
    private Quaternion rotacionInicial;
    private Vector3 escalaInicial;

    private Transform marcadorActual;

    void Start()
    {
        if (model == null)
        {
            Debug.LogError("No se ha asignado el iPhone.");
            return;
        }

        padreInicial = model.transform.parent;
        marcadorActual = padreInicial;

        posicionInicial = model.transform.localPosition;
        rotacionInicial = model.transform.localRotation;
        escalaInicial = model.transform.localScale;

        if (selectorAleatorio == null)
        {
            selectorAleatorio = GetComponent<aleatorio>();
        }
    }

    // BOTON MOVE
    public void moveToNexMarker()
    {
        if (model == null || isMoving)
            return;

        ObserverBehaviour destino = BuscarSiguienteMarcador();

        if (destino == null)
        {
            Debug.LogWarning(
                "No hay otro marcador detectado."
            );
            return;
        }

        StartCoroutine(MoverIphone(destino));
    }

    private IEnumerator MoverIphone(ObserverBehaviour destino)
    {
        isMoving = true;

        Debug.Log("Iniciando movimiento hacia: " + destino.name);

        if (selectorAleatorio != null)
        {
            selectorAleatorio.OcultarModelo();
        }

        Vector3 inicio = model.transform.position;

        model.transform.SetParent(null, true);

        Vector3 offsetLocal =
            posicionInicial + new Vector3(separacionIphone, 0f, 0f);

        float duracion = Mathf.Max(
            0.1f,
            1f / Mathf.Max(speed, 0.01f)
        );

        float tiempo = 0f;

        while (tiempo < duracion)
        {
            if (destino == null ||
                !MarcadorDetectado(destino))
            {
                Debug.LogWarning(
                    "Se perdio el marcador durante el movimiento."
                );

                RestaurarMarcadorActual();
                isMoving = false;
                yield break;
            }

            tiempo += Time.deltaTime;

            float progreso = Mathf.Clamp01(
                tiempo / duracion
            );

            float suavizado = Mathf.SmoothStep(
                0f, 1f, progreso
            );

            Vector3 posicionDestino =
                destino.transform.TransformPoint(offsetLocal);

            model.transform.position = Vector3.Lerp(
                inicio,
                posicionDestino,
                suavizado
            );

            yield return null;
        }

        model.transform.SetParent(
            destino.transform,
            true
        );

        // Colocar el iPhone un poco a la izquierda
        model.transform.localPosition =
            posicionInicial + new Vector3(separacionIphone, 0f, 0f);

        model.transform.localRotation = rotacionInicial;
        model.transform.localScale = escalaInicial;

        marcadorActual = destino.transform;

        Debug.Log("El iPhone llego a: " + destino.name);

        if (selectorAleatorio != null)
        {
            selectorAleatorio.MostrarAleatorio(
                destino.transform
            );
        }

        isMoving = false;
    }

    // VERIFICAR MARCADORES
    private bool MarcadorDetectado(ObserverBehaviour target)
    {
        if (target == null)
            return false;

        Status estado = target.TargetStatus.Status;

        return estado == Status.TRACKED ||
               estado == Status.EXTENDED_TRACKED;
    }

    // BUSCAR SIGUIENTE MARCADOR
    private ObserverBehaviour BuscarSiguienteMarcador()
    {
        if (ImageTargets == null ||
            ImageTargets.Length == 0)
            return null;

        for (int i = 0; i < ImageTargets.Length; i++)
        {
            int indice = (currentTarget + i + 1)
                         % ImageTargets.Length;

            ObserverBehaviour target = ImageTargets[indice];

            if (target != null &&
                target.transform != marcadorActual &&
                MarcadorDetectado(target))
            {
                currentTarget = indice;
                return target;
            }
        }

        return null;
    }

    // RESTAURAR POSICION
    private void RestaurarMarcadorActual()
    {
        if (model == null)
            return;

        Transform padre = marcadorActual != null
            ? marcadorActual
            : padreInicial;

        model.transform.SetParent(padre, false);

        // Si vuelve al marcador inicial, posicion original
        if (padre == padreInicial)
        {
            model.transform.localPosition = posicionInicial;
        }
        else
        {
            model.transform.localPosition =
                posicionInicial + new Vector3(separacionIphone, 0f, 0f);
        }

        model.transform.localRotation = rotacionInicial;
        model.transform.localScale = escalaInicial;
    }

    // BOTON RESET
    public void Resetear()
    {
        StopAllCoroutines();

        isMoving = false;
        currentTarget = -1;

        if (selectorAleatorio != null)
        {
            selectorAleatorio.ResetearAleatorio();
        }

        marcadorActual = padreInicial;

        if (model != null)
        {
            model.transform.SetParent(
                padreInicial,
                false
            );

            model.transform.localPosition = posicionInicial;
            model.transform.localRotation = rotacionInicial;
            model.transform.localScale = escalaInicial;
        }

        Debug.Log("Experiencia reiniciada.");
    }
}
