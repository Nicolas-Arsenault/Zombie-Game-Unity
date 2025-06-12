using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Mouvement Joueur")]
    public float vitesseJoueur = 1.9f;
    public float vitesseSprint = 5f;

// Header("Vie du joueur")]
    //private float playerHealth = 100f;
    //public float presentHealth;

    [Header("Joueur Script Caméras")]
    public Transform joueurCamera;

    [Header("Animation Joueur et gravité")]
    private CharacterController controleur;
    public float gravite = -9.81f; // Reduced gravity for smoother fall
    public Animator animator;

    [Header("Joueur sautant et vélocité")]
    public float tournerCalmeTemps = 0.1f;
    float tournerCalmeVelocite;
    public float sautRange = 5.0f;
    Vector3 velocite;
  //  public Transform surfaceCheck;
     public bool surSurface;
   // public float surfaceDistance = 0.1f;
   // public LayerMask surfaceMasque;

    private void Start()
    {
        controleur = gameObject.GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; //Rendre le curseur invisible
       // presentHealth = playerHealth;
    }

    private void Update()
    {
        // Vérifie si le joueur est au sol
        //surSurface = Physics.CheckSphere(surfaceCheck.position, surfaceDistance, surfaceMasque);

        surSurface = controleur.isGrounded;

        if (surSurface && velocite.y < 0)
        {
            velocite.y = 0f; // Slight downward push to stay grounded
        }
        else
        {
            velocite.y += gravite * Time.deltaTime;
            
            velocite.y = Mathf.Clamp(velocite.y, -2, Mathf.Infinity); // Reduced max fall speed
        }

        controleur.Move(velocite * Time.deltaTime);

        bougerJoueur();
        sauter();
    }

void bougerJoueur()
{
    bool joueurSprint = !Input.GetButton("Fire2") && !Input.GetButton("Fire1") && Input.GetKey(KeyCode.LeftShift) && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D));
    bool joueurAim = Input.GetButton("Fire2"); // Check if the player is aiming
    bool joueurFire = Input.GetButton("Fire1"); // Check if the player is firing

    float axeHorizontal = Input.GetAxisRaw("Horizontal");
    float axeVertical = Input.GetAxisRaw("Vertical");

    Vector3 direction = new Vector3(axeHorizontal, 0f, axeVertical).normalized;

    if (direction.magnitude >= 0.1f)
    {

        if (joueurAim && !joueurSprint)
        {
            // When aiming and moving, set aiming and walking animations

            // When aiming, force the player to face the camera's forward direction
            Vector3 cameraForward = joueurCamera.forward;
            cameraForward.y = 0; // Ignore vertical rotation
            cameraForward.Normalize();

            // Smoothly rotate the player to face the camera's forward direction
            float targetAngle = Mathf.Atan2(cameraForward.x, cameraForward.z) * Mathf.Rad2Deg;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref tournerCalmeVelocite, tournerCalmeTemps);
            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

            // Move the player relative to the camera's forward direction
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * direction;
            controleur.Move(moveDirection.normalized * vitesseJoueur * Time.deltaTime);
        }
        else if (joueurFire)
        {
            // When firing, prioritize the fire animation
            
            // When aiming, force the player to face the camera's forward direction
            Vector3 cameraForward = joueurCamera.forward;
            cameraForward.y = 0; // Ignore vertical rotation
            cameraForward.Normalize();

            // Smoothly rotate the player to face the camera's forward direction
            float targetAngle = Mathf.Atan2(cameraForward.x, cameraForward.z) * Mathf.Rad2Deg;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref tournerCalmeVelocite, tournerCalmeTemps);
            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

            // Move the player relative to the camera's forward direction
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * direction;
            controleur.Move(moveDirection.normalized * vitesseJoueur * Time.deltaTime);
        }
        else
        {
            // When not aiming or firing, handle movement and animation states

            float angleCible = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + joueurCamera.eulerAngles.y;
            float angleSmooth = Mathf.SmoothDampAngle(transform.eulerAngles.y, angleCible, ref tournerCalmeVelocite, tournerCalmeTemps);
            transform.rotation = Quaternion.Euler(0f, angleSmooth, 0f);

            Vector3 directionMouvement = Quaternion.Euler(0f, angleCible, 0f) * Vector3.forward;

            if (joueurSprint)
            {
                // Sprinting
                controleur.Move(directionMouvement.normalized * vitesseSprint * Time.deltaTime);

            }
            else
            {
                // Walking
                controleur.Move(directionMouvement.normalized * vitesseJoueur * Time.deltaTime);

            }
        }
    }
    else
    {
        // Idle

    }
}

    void sauter()
    {
        if (Input.GetButtonDown("Jump") && surSurface)
        {
            
           velocite.y = Mathf.Sqrt(sautRange * -2.0f * gravite);

        }
    }

    //public void playerHitDamage(float takeDamage)
    //{
    //    presentHealth -= takeDamage;

    //    if (presentHealth <= 0)
    //    {
    //        PlayerDie();
    //    }
    //}

    //private void PlayerDie()
    //{
    //    Cursor.lockState = CursorLockMode.None;
    //    Object.Destroy(gameObject, 1.0f);
    //}
}
