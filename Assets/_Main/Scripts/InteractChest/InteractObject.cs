using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public GameObject objectToShow;           // Objeto que se va a mostrar/ocultar
    public Collider2D triggerZone;            // Zona de trigger definida manualmente
    public string playerTag = "Player";       // Tag del jugador (por defecto: "Player")
    [SerializeField] private string NameOpcion = null;
    [SerializeField] PlayerAttack PlayerAttackScript;



    private bool isPlayerInside = false;
    void Start()
    {
        if (objectToShow != null)
            objectToShow.SetActive(false);
    }

    void Update()
    {
        if (triggerZone != null)
        {
            ContactFilter2D filter = new ContactFilter2D();
            filter.useTriggers = true;
            Collider2D[] results = new Collider2D[10];

            int count = triggerZone.OverlapCollider(filter, results);

            bool playerFound = false;
            for (int i = 0; i < count; i++)
            {
                if (results[i] != null && results[i].CompareTag(playerTag))
                {
                    playerFound = true;
                    break;
                }
            }

            if (playerFound != isPlayerInside)
            {
                isPlayerInside = playerFound;
                objectToShow?.SetActive(isPlayerInside);
            }

            if (isPlayerInside)
            {
                DoSomethingWhileInside(NameOpcion);
            }
        }
    }

    void DoSomethingWhileInside(string parametro)
    {
        if (!string.IsNullOrEmpty(parametro))
        {
            if (parametro == "Chest" && Input.GetKeyDown(KeyCode.E))
            {

            }
            else if (parametro == "Pistol" && Input.GetKeyDown(KeyCode.E))
            {
                PlayerAttackScript.pistola = true;
                PlayerAttackScript.yaTienePistola = true;
                Destroy(gameObject);
            }
            else if (parametro == "Cargador" && Input.GetKeyDown(KeyCode.E) && PlayerAttackScript.pistola) {
                PlayerAttackScript.cargadores = PlayerAttackScript.cargadores+1;
                PlayerAttackScript.TextoTMPCargadores.text = "" + PlayerAttackScript.cargadores;
                Destroy(gameObject);
            }
        }
    }
    }




