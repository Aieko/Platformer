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

    public HealthBar_Script healthBar;

    // private GameManager GM;

    private void Start()
    {
        currentHealth = maxHealth;

        healthBar.SetMaxHealth(currentHealth);
        // GM = GameObject.Find("GameManager").GetComponent<GameManager>();
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
       // GM.Respawn();
        Destroy(gameObject);
    }

}
