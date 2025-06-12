using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [SerializeField] private int HP = 100;
    private Animator animator;
    private NavMeshAgent agent;
    //private ZombieSpawnerController scriptSpawnerController;
    public bool estMort = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        // cherche le GameObject "ZombieSpawner" existe dans le les gameObject
        GameObject spawnerObject = GameObject.Find("ZombieSpawner");

        if (spawnerObject != null) // verif si il l'a trouver
        {
            // met le script "ZombieSpawnerController.cs" qui appartient au gameObject trouvé dans une variable
            //scriptSpawnerController = spawnerObject.GetComponent<ZombieSpawnerController>();
        }
    }
    public void TakeDamage(int d)
    {
        HP -= d;
        if (HP <= 0)
        {
            int rand = Random.Range(0, 2);
            if (rand == 0) {
                animator.SetTrigger("MORT1");
            }
            else
            {
                animator.SetTrigger("MORT2");
            }
            estMort = true;
        }
        else
        {
            animator.SetTrigger("TOUCHER");
            animator.SetBool("isRunning", true); // refait courir le zombie vers le joueur
        }
    }

    public void FixedUpdate()
    {
        // si le zombie est mort, on le fait arrêter d'avancer et on le destroy
        if (estMort)
        {
            Destroy(gameObject, 5); // enlever le zombie apres 5 secondes
            agent.velocity = Vector3.zero;
        }
    }

    private void OnDrawGizmos()
    {
        // pas nécessaire car le zombie va toujours détecter le joueur meme si il est loin
        // mais je le mets quand meme pour debug
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 20f); // 20f = le radius de la sphere
    }
}
