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
    [SerializeField] MoneyScript MoneyScript;

    public GameObject moneyPrefab;
    public GameObject cargadorPrefab;

    private bool isOpened = false;
    private bool isPlayerInside = false;

    void Start()
    {
        if (objectToShow != null)
            objectToShow.SetActive(false);
    }

    void Update()
    {
        CheckPlayerInTrigger();

        if (isPlayerInside)
        {
            HandleInteraction(NameOpcion);
        }
    }


    // Verifica si el jugador está dentro del área de interacción
    void CheckPlayerInTrigger()
    {
        if (triggerZone == null) return;

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
    }


    // Maneja la acción según el tipo de objeto
    void HandleInteraction(string tipo)
    {
        if (!Input.GetKeyDown(KeyCode.E) || string.IsNullOrEmpty(tipo)) return;

        switch (tipo)
        {
            case "Chest":
                if (!isOpened)
                    OpenChest();
                break;

            case "Pistol":
                GivePlayerPistol();
                break;

            case "Cargador":
                GivePlayerAmmo();
                break;

            case "Money":
                GivePlayerMoney();
                break;
        }
    }


    // -----------------------
    // FUNCIONES MODULARES
    // -----------------------
    void OpenChest()
    {
        isOpened = true;

        // Animación
        Animator anim = GetComponent<Animator>();
        if (anim != null)
            anim.SetTrigger("Abrir");

        // Spawn de objetos
        Vector3 spawnBase = transform.position + Vector3.up * -0.8f;

        if (moneyPrefab != null)
            Instantiate(moneyPrefab, spawnBase + Vector3.left * 0.3f, Quaternion.identity);

        if (cargadorPrefab != null)
            Instantiate(cargadorPrefab, spawnBase + Vector3.right * 0.3f, Quaternion.identity);

        // Desactivar interacción futura
        objectToShow?.SetActive(false);
        triggerZone.enabled = false;

    }
    void GivePlayerPistol()
    {
        PlayerAttackScript.pistola = true;
        PlayerAttackScript.yaTienePistola = true;
        Destroy(gameObject);
    }
    void GivePlayerAmmo()
    {
        if (!PlayerAttackScript.pistola) return;

        PlayerAttackScript.cargadores += 1;
        PlayerAttackScript.TextoTMPCargadores.text = PlayerAttackScript.cargadores.ToString();
        Destroy(gameObject);
    }
    void GivePlayerMoney()
    {
        MoneyScript.Instance.AddRandomMoney();
        Destroy(gameObject);
    }
}




