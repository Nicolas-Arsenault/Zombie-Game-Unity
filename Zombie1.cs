using UnityEngine;
using UnityEngine.AI;

public class Zombie1 : MonoBehaviour
{
    [SerializeField] ZombieHealthBar healthBar;

    [Header("Zombie Health and Damage")]
    public float vieZombie = 50f;
    private float presentHealth;
    public float giveDamage = 25f;
    public bool estMort = false;

    [Header("Zombie Things")]
    public NavMeshAgent zombieAgent;
    public Camera AttackingRaycastArea;
    //public Transform playerBody;
    public LayerMask PlayerLayer;

    GameObject corpJoueurObject;
    Transform corpsJoueur;

    private NavMeshAgent agent;
    public float tempsInactif = 0f;
    public float tempsAvantRespawn = 3f; // Temps d'inactivité avant respawn
    public bool zombieEstBloque = false;

    public float vitesseActuelleDuZombie;
    public GameObject joueurObject;
    public GameObject spawnerObject;
    public ZombieSpawner spawnerScript; // Référence au spawner

    [Header("Zombie Guarding War")]
    public float vitesseCourseZombie = 2f;

    [Header("Zombie Attacking Var")]
    public float timeBtwAttack = 1f;


    [Header("Sons de zombie")]
    public AudioClip attackSound;
    public AudioClip footstep;

    [Header("Zombie Animation")]//500
    public Animator anim;

    [Header("Zombie mood/states")]
    public float visionRadius;
    public float attackingRadius;
    public bool playerInvisionRadius;
    public bool playerInattackingRadius;
    public bool previouslyAttack;

    private void Awake()
    {
        healthBar = GetComponentInChildren<ZombieHealthBar>();
        zombieAgent = GetComponent<NavMeshAgent>();
        presentHealth = vieZombie;
        corpJoueurObject = GameObject.FindGameObjectWithTag("CorpsJoueur");
        corpsJoueur = corpJoueurObject.GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        spawnerObject = GameObject.FindGameObjectWithTag("Spawner");
        spawnerScript = spawnerObject.GetComponent<ZombieSpawner>();
    }

    private void Update()
    {
        vitesseActuelleDuZombie = zombieAgent.velocity.magnitude;

        //Debug.Log("vitesseActuelleDuZombie : " + vitesseActuelleDuZombie);

        // check si le joueur est dans le radius du zombie
        playerInvisionRadius = Physics.CheckSphere(transform.position, visionRadius, PlayerLayer);
        playerInattackingRadius = Physics.CheckSphere(transform.position, attackingRadius, PlayerLayer);


        if (playerInvisionRadius && !playerInattackingRadius) Pursueplayer();
        if (playerInvisionRadius && playerInattackingRadius) AttackPlayer();

        if (vitesseActuelleDuZombie < 0.000001f && !estMort && !playerInattackingRadius)
        {
            tempsInactif += Time.deltaTime;
            if (tempsInactif > tempsAvantRespawn)
            {
                spawnerScript.zombieEnVie.Remove(this);
                Destroy(gameObject);

                Zombie1 nouveauZombie = spawnerScript.faireSpawnerUnZombie().GetComponent<Zombie1>();
                nouveauZombie.changerVieZombie(spawnerScript.vieZombie);

                spawnerScript.zombieEnVie.Add(nouveauZombie);
                return;
            }
        }
    }

    // changer la vie du zombie en la montant d'une certaine valeur
    public void changerVieZombie(int nouvelleVieZombie)
    {
        vieZombie += nouvelleVieZombie;
    }
    private void Pursueplayer()
    {
        if (zombieAgent.isOnNavMesh)
        {
            if (zombieAgent.SetDestination(corpsJoueur.position))
            {
                // animations
                anim.SetBool("Walking", false);
                anim.SetBool("Running", true);
                anim.SetBool("Attacking", false);
                anim.SetBool("Died", false);
            }
            else
            { // sinon le zombie marche normalement
                anim.SetBool("Walking", false);
                anim.SetBool("Running", false);
                anim.SetBool("Attacking", false);
                anim.SetBool("Died", true);
            }
        }
    }

    private void AttackPlayer()
    {
        zombieAgent.SetDestination(transform.position);
        //transform.LookAt(playerBody);
        transform.LookAt(corpsJoueur);

        if (!previouslyAttack)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(AttackingRaycastArea.transform.position, AttackingRaycastArea.transform.forward, out hitInfo, attackingRadius))
            {
                PlayerHealth playerBody = hitInfo.transform.GetComponent<PlayerHealth>();

                if (playerBody != null)
                {
                    playerBody.playerHitDamage(giveDamage);
                    AudioSource.PlayClipAtPoint(attackSound, transform.position);

                }

                // animations
                anim.SetBool("Walking", false);
                anim.SetBool("Running", false);
                anim.SetBool("Attacking", true);
                anim.SetBool("Died", false);


                previouslyAttack = true;
                Invoke(nameof(ActiveAttacking), timeBtwAttack);
            }


        }

    }

    private void ActiveAttacking()
    {
        previouslyAttack = false;
    }

    public void zombieHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;

        healthBar.UpdateHealthBar(presentHealth, vieZombie);

        if (presentHealth <= 0)
        {

            anim.SetBool("Walking", false);
            anim.SetBool("Running", false);
            anim.SetBool("Attacking", false);
            anim.SetBool("Died", true);

            zombieDie();
        }
    }

    private void zombieDie()
    {
        zombieAgent.SetDestination(transform.position);

        estMort = true;

        vitesseCourseZombie = 0;
        attackingRadius = 0;
        visionRadius = 0;
        playerInattackingRadius = false;
        playerInvisionRadius = false;

        Destroy(gameObject, 3.0f); // enlever le zombie apres 3 secondes
    }

    private void OnDrawGizmos()
    {
        // pas nécessaire car le zombie va toujours détecter le joueur meme si il est loin
        // mais je le mets quand meme pour debug
        Gizmos.color = Color.red; // vision
        Gizmos.DrawWireSphere(transform.position, 15f); // 20f = le radius de la sphere
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}