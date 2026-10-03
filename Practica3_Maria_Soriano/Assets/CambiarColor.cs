using UnityEngine;

public class CambiarColor : MonoBehaviour
{
    public GameObject modelo;

    public void cambiarAleatorio()
    {
        Renderer[] renderers = modelo.GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
            return;

        int indice = Random.Range(0, renderers.Length);

        Color colorAleatorio = new Color(
            Random.value,
            Random.value,
            Random.value
        );

        renderers[indice].material.color = colorAleatorio;
    }
}