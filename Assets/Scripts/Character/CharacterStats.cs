using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;

    [SerializeField]
    private GameObject
        deathChunkParticle,
        deathBloodParticle;

    private float currentHealth;

    private HealthBar_Script healthBar;

    private GameManager GM;

    private void Start()
    {
        healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar_Script>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();

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
        GM.Respawn();
        Destroy(gameObject);
    }

}
