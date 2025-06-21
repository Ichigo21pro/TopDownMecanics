using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public string enemyTag = "Enemy";
    public int damageKnife = 25;


    public GameObject bulletPrefab; // Prefab de la bala
    public Transform firePoint;     // Punto desde donde dispara
    public float bulletSpeed = 10f; // Velocidad de la bala

    private List<GameObject> enemiesInRange = new List<GameObject>();
    public Collider2D attackRangeCollider;

    [HideInInspector] public bool isAttacking; // <- Este flag será leído por PlayerAnimations
    [HideInInspector] public bool isAttackingPistol; // <- Este flag será leído por PlayerAnimations

    [SerializeField] private Chest InteractObjectScript;
    [HideInInspector] public bool pistola;
    [HideInInspector] public bool yaTienePistola;

    public int balas = 25;
    public int cantidadCargador = 25;
    public int cargadores = 3;
    [HideInInspector] public bool isReloading = false;
    private float reloadTime = 0.35f; // Tiempo de recarga en segundos

    public TMP_Text TextoTMPBalas;
    public TMP_Text TextoTMPCargadores;
    private bool MostrarBalas=true;

    [SerializeField] private Collider2D hitboxBloqueadora;


    void Update()
    {
        if(MostrarBalas && pistola)
        {
            ActualizarHUD();
            MostrarBalas = false;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (pistola)
            {
                if (ClickSobreHitboxBloqueadora()) return; // <- Detiene el disparo si se clickeó la hitbox
                if (!isReloading)
                {
                    if (balas > 0)
                    {
                        AttackPistola();
                        balas--;
                        ActualizarHUD();
                    }
                    else if (cargadores > 0)
                    {
                        StartCoroutine(Reload());
                    }
                    else
                    {
                        Debug.Log("Sin balas y sin cargadores");
                    }
                }
            }
            else
            {
                isAttacking = true; // <- Activa el flag una vez
                
            }
        }
        if (yaTienePistola && Input.GetKeyDown(KeyCode.Alpha1))
        {
            pistola = false; 
        }
        if (yaTienePistola && Input.GetKeyDown(KeyCode.Alpha2))
        {
            pistola = true;
        }
        if (pistola && Input.GetKeyDown(KeyCode.R) && cargadores > 0 && balas < cantidadCargador)
        {
            StartCoroutine(Reload());
        }
    }

    // ataque de cuchillo
    public void Attack()
    {
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer)); // Ajusta el layer si es necesario
        filter.useTriggers = true;

        Collider2D[] results = new Collider2D[10]; // Puedes ajustar el tamaño según necesidad
        int count = attackRangeCollider.OverlapCollider(filter, results);

        for (int i = 0; i < count; i++)
        {
            Collider2D col = results[i];
            if (col != null && col.CompareTag(enemyTag))
            {
                EnemyHealth enemyHealth = col.GetComponentInParent<EnemyHealth>(); // <- importante: subir al padre si es hitbox
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damageKnife);
                }
            }
        }
    }


    //ataque pistola
    void AttackPistola()
    {
        isAttackingPistol = true;

        // Obtener dirección hacia el mouse (igual que en CursorPoint)
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 shootDirection = (mousePos - firePoint.position).normalized;

        // Instanciar la bala con la rotación hacia el mouse
        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, rotation);

        // Aplicar la velocidad
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = shootDirection * bulletSpeed;
        }
    }

    // Recargar

    private IEnumerator Reload()
    {
        if (isReloading) yield break; // <- Previene múltiples llamadas simultáneas

        isReloading = true;

        yield return new WaitForSeconds(reloadTime);

        if (cargadores > 0)
        {
            cargadores--;
            balas = cantidadCargador;
            ActualizarHUD();
        }

        isReloading = false;

    }

    // htibox dodne no se puede disparar

    private bool ClickSobreHitboxBloqueadora()
    {
        

        if (hitboxBloqueadora == null) return false;

        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        return hitboxBloqueadora.OverlapPoint(mouseWorldPos);
    }


    //actualizar HUD
    void ActualizarHUD()
    {
        TextoTMPBalas.text = "" + balas;
        TextoTMPCargadores.text = "" + cargadores;

        if (balas < 11)
        {
            TextoTMPBalas.color = Color.red;
        }
        else
        {
            TextoTMPBalas.color = Color.white;
        }
    }


    // Collider si el tag es enemigo
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(enemyTag))
        {
            if (!enemiesInRange.Contains(other.gameObject))
                enemiesInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(enemyTag))
        {
            if (enemiesInRange.Contains(other.gameObject))
                enemiesInRange.Remove(other.gameObject);
        }
    }
}
