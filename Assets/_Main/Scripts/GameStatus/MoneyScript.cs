using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyScript : MonoBehaviour
{
    public static MoneyScript Instance; // Singleton
    public TMP_Text textoTMPMoney;
    public float money = 0;
    private void Awake()
    {
        // Implementar Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantener el objeto entre escenas
        }
        else
        {
            Destroy(gameObject); // Eliminar duplicados
        }
    }

    void Start()
    {
        UpdateMoneyText();
    }

    public void AddMoney(float amount)
    {
        money += amount;
        UpdateMoneyText();
    }

    public void SpendMoney(float amount)
    {
        if (money >= amount)
        {
            money -= amount;
            UpdateMoneyText();
        }
        else
        {
            Debug.Log("No hay suficiente dinero.");
        }
    }

    public void UpdateMoneyText()
    {
        if (textoTMPMoney != null)
        {
            textoTMPMoney.text = FormatMoney(money);
        }
    }

    public void AddRandomMoney()
    {
        float amount;
        float chance = Random.value; // valor entre 0.0 y 1.0

        if (chance < 0.8f) // 80% de probabilidad
        {
            amount = Random.Range(5f, 10f);
        }
        else // 20% de probabilidad
        {
            amount = Random.Range(20f, 30f);
        }

        AddMoney(amount);
    }

    private string FormatMoney(float amount)
    {
        if (amount < 100)
            return amount.ToString("F2");
        else if (amount < 1000)
            return (amount / 100f).ToString("F2") + " C";
        else if (amount < 1_000_000)
            return (amount / 1000f).ToString("F2") + " m";
        else if (amount < 1_000_000_000)
            return (amount / 1_000_000f).ToString("F2") + " M";
        else if (amount < 1_000_000_000_000)
            return (amount / 1_000_000_000f).ToString("F2") + " B";
        else
            return (amount / 1_000_000_000_000f).ToString("F2") + " T"; // T = Trillón
    }

}
