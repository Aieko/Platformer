using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTestDummy : MonoBehaviour, IDamageable
{
    private Animator anim;

    private float health = 50f;

    [SerializeField] private GameObject hitParticles;

    public void Damage(float amount)
    {
        Debug.Log(amount + "Damage taken by" + transform.name);

        Instantiate(hitParticles, transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));

        anim.SetTrigger("Damage");

        health -= amount;

        if(health <= 0)
        Destroy(gameObject);
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
}
