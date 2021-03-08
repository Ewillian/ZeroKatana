using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    [SerializeField] Player player;
    private Image healthBar;
    private float currentHealth;
    private float maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponent<Image>();
        maxHealth = player.currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = player.currentHealth;
        if (currentHealth > 0)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
        }
        else
        {
            healthBar.fillAmount = 0;
        }
    }
}
