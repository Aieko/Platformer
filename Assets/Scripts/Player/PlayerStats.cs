using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;

    [SerializeField]
    private GameObject
        deathChunkParticle,
        deathBloodParticle;

    private float currentHealth;

    private HealthBar_Script healthBar;

    private GameManager gm;



    private void Awake()
    {
        healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar_Script>();

        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void Start()
    {
        currentHealth = maxHealth;

        healthBar.SetMaxHealth(currentHealth);
    }

    public void DecreaseHealth(float amount)
    {
        currentHealth -= amount;
        Instantiate(deathBloodParticle, transform.position, deathBloodParticle.transform.rotation);
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0.0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Instantiate(deathChunkParticle, transform.position, deathChunkParticle.transform.rotation);
        Instantiate(deathBloodParticle, transform.position, deathBloodParticle.transform.rotation);
        gm.OnDeath();
        Destroy(gameObject);
    }
}
