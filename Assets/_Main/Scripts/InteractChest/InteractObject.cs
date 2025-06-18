using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering.Universal;

public class Chest : MonoBehaviour
{
    [Header("Referencias a Scripts")]
    public GameObject objectToShow;           // Objeto que se va a mostrar/ocultar
    public Collider2D triggerZone;            // Zona de trigger definida manualmente
    public string playerTag = "Player";       // Tag del jugador (por defecto: "Player")
    [SerializeField] private string NameOpcion = null;
    [SerializeField] PlayerAttack PlayerAttackScript;
    [SerializeField] MoneyScript MoneyScript;
    [SerializeField] private PlayerLinterna playerLinterna;

    [Header("Referencias a Objetos")]
    public GameObject moneyPrefab;
    public GameObject cargadorPrefab;
    public GameObject bateriaPrefab;

    private bool isOpened = false;
    private bool isPlayerInside = false;

    [Header("Referencias Interruptor")]
    [SerializeField] private List<Light2D> lucesAControlar;
    private bool lucesEncendidas = true; // Estado actual

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

        if (playerLinterna == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerLinterna = player.GetComponent<PlayerLinterna>();
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
            case "Bateria":
                GivePlayerBatery();
                break;
            case "Interruptor":
                GivePlayerInterruptor();
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

        // Animación
        Animator anim = GetComponent<Animator>();
        if (anim != null)
            anim.SetTrigger("Abrir");

        // Iniciar coroutine para esperar a que la animación termine
        StartCoroutine(SpawnLootAfterDelay(1f));  // Delay antes de alnzar la cosas

        // Desactivar interacción futura
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
            Vector3 offset = Vector3.right * ((i - 1) * spacing);
            float rand = Random.value;

            if (rand <= 0.6f && moneyPrefab != null)
            {
                GameObject money = Instantiate(moneyPrefab, transform.position, Quaternion.identity);
                money.transform.DOMove(spawnBase + offset, 0.5f).SetEase(Ease.OutBack);
            }
            else if (rand <= 0.8f && bateriaPrefab != null)
            {
                GameObject battery = Instantiate(bateriaPrefab, transform.position, Quaternion.identity);
                battery.transform.DOMove(spawnBase + offset, 0.5f).SetEase(Ease.OutBack);
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
        if (PlayerAttackScript.pistola)
        {
            GivePlayerAmmo();
        }
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

    //bateria 

    void GivePlayerBatery()
    {
        if (playerLinterna == null) return;

        // Verificación adicional (opcional): evitar si el texto dice "100%"
        if (playerLinterna.textoBateria != null && playerLinterna.textoBateria.text == "100%")
            return;

        if (playerLinterna.bateriaActual >= playerLinterna.bateriaMaxima)
            return;

        playerLinterna.bateriaActual = playerLinterna.bateriaMaxima;

        if (!playerLinterna.linternaActiva)
        {
            playerLinterna.linternaActiva = true;
        }

        playerLinterna.ActualizarEstadoLinternas(true);

        Destroy(gameObject);
    }

    //Interruptor

    void GivePlayerInterruptor() 
    {
        if (lucesAControlar == null || lucesAControlar.Count == 0)
            return;

        lucesEncendidas = !lucesEncendidas;

        foreach (var luz in lucesAControlar)
        {
            if (luz != null)
                luz.enabled = lucesEncendidas;
        }

        // Si solo se debe usar una vez, destruir el interruptor:
        // Destroy(gameObject);
    }
}




