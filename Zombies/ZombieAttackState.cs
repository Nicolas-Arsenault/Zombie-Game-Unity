using UnityEngine;
using UnityEngine.AI;

public class ZombieAttackState : StateMachineBehaviour
{
    Transform joueur;
    NavMeshAgent agent;

    public float distanceArretAttaque = 3f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // initialisation
        joueur = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        RegarderJoueur();// regarder le joueur quand le zombie attaque

        // verif si le zombie arrete d'attaquer
        float distanceDuJoueur = Vector3.Distance(joueur.position, animator.transform.position);

        // check si le zombie arrete de courir
        if (distanceDuJoueur > distanceArretAttaque)
        {
            animator.SetBool("isAttacking", false);
        }
    }

    private void RegarderJoueur() {
        Vector3 direction = joueur.position - agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = agent.transform.eulerAngles.y;
        agent.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
