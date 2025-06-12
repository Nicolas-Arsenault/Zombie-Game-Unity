using UnityEngine;
using UnityEngine.AI;

public class ZombieCourseState : StateMachineBehaviour
{
    NavMeshAgent agent;
    Transform joueur;

    public float vitesseCourse = 6f;
    public float distanceAttaque = 3f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // initialisation
        joueur = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();

        agent.speed = vitesseCourse;
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(joueur.position);
        animator.transform.LookAt(joueur);

        float distanceDuJoueur = Vector3.Distance(joueur.position, animator.transform.position);

        // check si le zombie doit attacker
        if (distanceDuJoueur < distanceAttaque) {
            animator.SetBool("isAttacking", true);
        }
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(animator.transform.position);
    }
}
