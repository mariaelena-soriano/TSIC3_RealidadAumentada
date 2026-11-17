
using UnityEngine;

public class InicioPasaporte : MonoBehaviour
{
    public GameObject panelBienvenida;
    public GameObject controlesAR;

    void Start()
    {
        if (panelBienvenida != null)
            panelBienvenida.SetActive(true);

        if (controlesAR != null)
            controlesAR.SetActive(false);
    }

    public void Comenzar()
    {
        if (panelBienvenida != null)
            panelBienvenida.SetActive(false);

        if (controlesAR != null)
            controlesAR.SetActive(true);
    }
}
