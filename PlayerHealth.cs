using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public Image image;
    public float playerHealth = 100f;
    public Image blood;
    public AudioSource lowHealth;
    public AudioClip lowClip;
    public AudioClip die;
    private Animator animator;
    public PlayerMovement playerMovement;
    public PlayerAnimations playerAnimations;
    public Rifle rifle;
    public Image viseurCanva;
    public Image pasViseCanva;
    public GameObject mortCanva;

    private bool isDead = false;
    private bool canTriggerLowHealthWarning = true;
    private Coroutine lowHealthCoroutine;

    private void Start()
    {
        image.fillAmount = 1;
        animator = GetComponent<Animator>();
        StartCoroutine(RegenereVie());
    }

    public void changerVie(float amount)
    {
        image.fillAmount -= 0.05f;
    }

    private IEnumerator RegenereVie()
    {
        while (!isDead)
        {
            if (playerHealth < 100f)
            {
                playerHealth += 1f;
                playerHealth = Mathf.Min(playerHealth, 100f);
                image.fillAmount = playerHealth / 100f;

                if (playerHealth >= 20f && lowHealthCoroutine == null)
                {
                    canTriggerLowHealthWarning = true;
                }
            }
            yield return new WaitForSeconds(2f);
        }
    }

    public void playerHitDamage(float takeDamage)
    {
        if (isDead) return;

        playerHealth -= takeDamage;
        image.fillAmount = playerHealth / 100f;

        if (playerHealth < 20f && canTriggerLowHealthWarning)
        {
            // Play heartbeat + show blood image at the same time
            lowHealth.PlayOneShot(lowClip);
            StartCoroutine(ShowBloodWithDelay());

            canTriggerLowHealthWarning = false;
            if (lowHealthCoroutine != null)
                StopCoroutine(lowHealthCoroutine);
            lowHealthCoroutine = StartCoroutine(ResetLowHealthWarning());
        }

        if (playerHealth <= 0)
        {
            PlayerDie();
        }
    }

    private IEnumerator ShowBloodWithDelay()
    {
        blood.enabled = true;
        yield return new WaitForSeconds(6f);
        blood.enabled = false;
    }

    private IEnumerator ResetLowHealthWarning()
    {
        yield return new WaitForSeconds(10f);
        canTriggerLowHealthWarning = true;
        lowHealthCoroutine = null;
    }

    private void PlayerDie()
    {
        if (isDead) return;
        playerAnimations.enabled = false;
        playerMovement.enabled = false;
        rifle.enabled = false;
        viseurCanva.enabled = false;
        pasViseCanva.enabled = false;
        mortCanva.SetActive(true);

        SauvegarderPartie.Instance.SauvegarderMeilleurScore();

        animator.SetBool("Marche", false);
        animator.SetBool("Course", false);
        animator.SetBool("Tirer", false);
        animator.SetBool("IdleVise", false);
        animator.SetBool("GunMarche", false);
        animator.SetBool("TirerEtMarche", false);
        animator.SetBool("Recharge", false);
        animator.SetBool("RechargeEtMarche", false);
        animator.SetTrigger("Die");
        isDead = true;
        Cursor.lockState = CursorLockMode.None;

        // Stop all ongoing low health effects
        lowHealth.Stop();
        blood.enabled = false;

        lowHealth.PlayOneShot(die);
    }
}
