using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [Header("Objet animator")]
    public Animator animator;

    private void Update()
    {
        // Gather player input for movement, aiming, shooting, etc.
        bool estEntrainMarcher = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
        bool estEntrainCourir = Input.GetKey(KeyCode.LeftShift);
        bool estEntrainSauter = Input.GetKeyDown(KeyCode.Space);
        bool estEntrainViser = Input.GetButton("Fire2");
        bool estEntrainTirer = Input.GetButton("Fire1");
        bool estEntrainRecharger = Input.GetKeyDown(KeyCode.R);

        // If the player is aiming, shooting, and walking (highest priority)
        //if(estEntrainRecharger && !estEntrainCourir && !estEntrainMarcher)
        //{
            
        //    animator.SetBool("Marche", false);
        //    animator.SetBool("Course", false);
        //    animator.SetBool("GunMarche", false);
        //    animator.SetBool("TireEtMarche", false); // Shooting while walking
        //    animator.SetBool("IdleVise", false);
        //}
        if (estEntrainViser && estEntrainTirer && estEntrainMarcher)
        {
            animator.SetBool("Marche", true);
            animator.SetBool("Course", false);
            animator.SetBool("GunMarche", true);
            animator.SetBool("TireEtMarche", false); // Shooting while walking
            animator.SetBool("IdleVise", false);
        }
        // If the player is shooting and walking (not aiming)
        else if (estEntrainTirer && estEntrainMarcher)
        {
            animator.SetBool("Marche", true);
            animator.SetBool("Course", false);
            animator.SetBool("GunMarche", false);
            animator.SetBool("TireEtMarche", true); // Shooting while walking
            animator.SetBool("IdleVise", false);
        }
        // If the player is aiming and walking (not shooting)
        else if (estEntrainViser && estEntrainMarcher)
        {
            animator.SetBool("Marche", true);
            animator.SetBool("Course", false);
            animator.SetBool("GunMarche", true); // Aiming while walking
            animator.SetBool("TireEtMarche", false);
            animator.SetBool("IdleVise", false);
        }
        // If the player is walking and sprinting (not aiming or shooting)
        else if (estEntrainMarcher && estEntrainCourir)
        {
            animator.SetBool("Marche", false);
            animator.SetBool("Course", true); // Sprinting
            animator.SetBool("GunMarche", false);
            animator.SetBool("TireEtMarche", false);
            animator.SetBool("IdleVise", false);
        }
        // If the player is just walking (not aiming or shooting)
        else if (estEntrainMarcher)
        {
            animator.SetBool("Marche", true); // Walking
            animator.SetBool("Course", false);
            animator.SetBool("GunMarche", false);
            animator.SetBool("TireEtMarche", false);
            animator.SetBool("IdleVise", false);
        }
        // If the player is aiming and not walking
        else if (estEntrainViser)
        {
            animator.SetBool("Marche", false);
            animator.SetBool("Course", false);
            animator.SetBool("GunMarche", false);
            animator.SetBool("TireEtMarche", false);
            animator.SetBool("IdleVise", true); // Aiming without movement
        }
        // If the player is shooting and not walking
        else if (estEntrainTirer)
        {
            animator.SetBool("Idle", false);
            animator.SetBool("Marche", false);
            animator.SetBool("Course", false);
            animator.SetBool("GunMarche", false);
            animator.SetBool("TireEtMarche", false);
            animator.SetBool("IdleVise", false);
            animator.SetBool("Tirer", true); // Shooting
            
        }
        // If the player is just standing still (idle)
        else
        {
            animator.SetBool("Marche", false);
            animator.SetBool("Course", false);
            animator.SetBool("GunMarche", false);
            animator.SetBool("TireEtMarche", false);
            animator.SetBool("IdleVise", false);
            animator.SetBool("Tirer", false);
            animator.SetBool("Idle", true); // Idle state
        }

        // Handling the jump animation (triggered independently)
        if (estEntrainSauter)
        {
            animator.SetTrigger("Saut");
        }
    }
}
