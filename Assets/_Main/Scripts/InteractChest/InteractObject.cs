using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

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

    void Awake()
    {
        if (PlayerAttackScript == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                PlayerAttackScript = player.GetComponent<PlayerAttack>();
        }

        if (MoneyScript == null)
        {
            MoneyScript = MoneyScript.Instance;
        }
    }

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


    // Verifica si el jugador est� dentro del �rea de interacci�n
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


    // Maneja la acci�n seg�n el tipo de objeto
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

    //chest
    void OpenChest()
    {
        isOpened = true;

        // Animaci�n
        Animator anim = GetComponent<Animator>();
        if (anim != null)
            anim.SetTrigger("Abrir");

        // Iniciar coroutine para esperar a que la animaci�n termine
        StartCoroutine(SpawnLootAfterDelay(1f));  // Delay antes de alnzar la cosas

        // Desactivar interacci�n futura
        objectToShow?.SetActive(false);
        triggerZone.enabled = false;
    }

    IEnumerator SpawnLootAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Vector3 spawnBase = transform.position + Vector3.up * -0.8f;

        float spacing = 0.3f;

        for (int i = 0; i < 3; i++)
        {
            Vector3 offset = Vector3.right * ((i - 1) * spacing); // Posiciones: -0.3, 0, +0.3

            float rand = Random.value; // N�mero entre 0.0 y 1.0

            if (rand <= 0.7f && moneyPrefab != null)
            {
                GameObject money = Instantiate(moneyPrefab, transform.position, Quaternion.identity);
                money.transform.DOMove(spawnBase + offset, 0.5f).SetEase(Ease.OutBack);
            }
            else if (cargadorPrefab != null)
            {
                GameObject ammo = Instantiate(cargadorPrefab, transform.position, Quaternion.identity);
                ammo.transform.DOMove(spawnBase + offset, 0.5f).SetEase(Ease.OutBack);
            }
        }
    }

    //pistol
    void GivePlayerPistol()
    {
        PlayerAttackScript.pistola = true;
        PlayerAttackScript.yaTienePistola = true;
        Destroy(gameObject);
    }

    //cargadores
    void GivePlayerAmmo()
    {
        if (!PlayerAttackScript.pistola) return;

        PlayerAttackScript.cargadores += 1;
        PlayerAttackScript.TextoTMPCargadores.text = PlayerAttackScript.cargadores.ToString();
        Destroy(gameObject);
    }


    //money
    void GivePlayerMoney()
    {
        MoneyScript.Instance.AddRandomMoney();
        Destroy(gameObject);
    }
}




