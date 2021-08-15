using UnityEngine;

public class CombatDummyController : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;
    [Header("Knockback Settings")]
    [SerializeField] private float knockbackSpeedX;
    [SerializeField] private float knockbackSpeedY;
    [SerializeField] private float knockbackDuration;
    [SerializeField] private float knockbackDeathSpeedY;
    [SerializeField] private float knockbackDeathSpeedX;
    [SerializeField] private float deathTorque;
    [Space(20)]
    [SerializeField]
    private GameObject hitParticle;

    [SerializeField]
    private bool applyKnockback;

    private float currentHealth, knockbackStart;

    private int playerFacingDirection;

    private bool playerOnLeft, knockback;

    private CharacterController2D PC;
    private GameObject aliveGO, brokenTopGO, brokenBottomGO;
    private Rigidbody2D rbAlive, rbBrokenTop, rbBrokenBottom;
    private Animator aliveAnimator;

    private void Start()
    {
        currentHealth = maxHealth;

        PC = GameObject.Find("Character").GetComponent<CharacterController2D>();

        aliveGO = transform.Find("Alive").gameObject;
        brokenTopGO = transform.Find("BrokenTop").gameObject;
        brokenBottomGO = transform.Find("BrokenBottom").gameObject;

        aliveAnimator = aliveGO.GetComponent<Animator>();

        rbAlive = aliveGO.GetComponent<Rigidbody2D>();
        rbBrokenTop = brokenTopGO.GetComponent<Rigidbody2D>();
        rbBrokenBottom = brokenBottomGO.GetComponent<Rigidbody2D>();

        aliveGO.SetActive(true);

        brokenBottomGO.SetActive(false);
        brokenTopGO.SetActive(false); 
    }

    private void Update()
    {
       CheckKnockback();
    }

    private void Damage(AttackDetails attackDetails)
    {
        currentHealth -= attackDetails.damageAmount;

        playerFacingDirection = PC.GetFacingDirection();

        //Spawning Particles
        Instantiate(hitParticle, aliveAnimator.transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));

        if (playerFacingDirection == 1)
        {
            playerOnLeft = true;
        }
        else
        {
            playerOnLeft = false;
        }

        aliveAnimator.SetBool("playerOnLeft", playerOnLeft);
        aliveAnimator.SetTrigger("damage");

        if(applyKnockback && currentHealth > 0.0f)
        {
            Knockback();
        }
        else if( currentHealth <= 0.0f)
        {
            Die();
        }
         
    }

    private void Knockback()
    {
        knockback = true;
        knockbackStart = Time.time;
        rbAlive.velocity = new Vector2(knockbackSpeedX * playerFacingDirection, knockbackSpeedY);
    }

    private void CheckKnockback()
    {
        if(Time.time >= knockbackStart + knockbackDuration && knockback)
        {
            knockback = false;
            rbAlive.velocity = new Vector2(0.0f, rbAlive.velocity.y);
        }
    }

    private void Die()
    {
        aliveGO.SetActive(false);
        brokenTopGO.SetActive(true);
        brokenBottomGO.SetActive(true);

        brokenTopGO.transform.position = aliveGO.transform.position;
        rbBrokenBottom.transform.position = aliveGO.transform.position;

        rbBrokenTop.velocity = new Vector2(knockbackSpeedX * playerFacingDirection, knockbackSpeedY);
        rbBrokenTop.velocity = new Vector2(knockbackDeathSpeedX * playerFacingDirection, knockbackDeathSpeedY);

        rbBrokenTop.AddTorque(deathTorque * -playerFacingDirection, ForceMode2D.Impulse);
    }
}
