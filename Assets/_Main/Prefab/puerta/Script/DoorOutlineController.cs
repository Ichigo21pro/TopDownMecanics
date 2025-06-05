using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOutlineController : MonoBehaviour
{
    public GameObject outlinePuerta;
    public GameObject outlineMango;
    public GameObject outlineMango1;

    public void SetOutlinesActive(bool active)
    {
        if (outlinePuerta != null)
            outlinePuerta.SetActive(active);

        if (outlineMango != null)
            outlineMango.SetActive(active);

        if (outlineMango1 != null)
            outlineMango1.SetActive(active);
    }
}
