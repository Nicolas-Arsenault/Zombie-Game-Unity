using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Rifle : MonoBehaviour
{

    [Header("Mitraillette")]
    public Camera cam;
    public float degatsRifle = 10f;
    public float tirDistance = 100f;
    public float chargeTir = 15f;
    private float prochainTempsTirer = 0f;
    public PlayerMovement joueur; //rewference au joueur
    public Transform main;
    public Animator animator;


    [Header("Mitraillette balles et tir")]
    private int maxBalles = 32;
    private int ballesPresentes;
    public float tempsRecharge = 1.3f;
    private bool setRecharge = false;

    [Header("Effets Fusil")]
    public ParticleSystem muzzleSpark;
    public GameObject impacteBoisEffet;
    public GameObject effetMort;

    [Header("Sons et UI")]
    public AudioClip sonTirer;
    public AudioClip sonRecharge;
    public AudioSource audioSource;

    private void Awake()
    {
        transform.SetParent(main);
        ballesPresentes = maxBalles;
    }


    private void Update()
    {
        if (setRecharge) return;

        if (ballesPresentes <= 0 || Input.GetKey(KeyCode.R))
        {

            StartCoroutine(recharge());
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= prochainTempsTirer)
        {

            prochainTempsTirer = Time.time + 1f / chargeTir;
            tir();
        }
        else if (Input.GetButton("Fire1") && Input.GetKey(KeyCode.W))
        {

        }
        else if (Input.GetButton("Fire2"))
        {

        }
        else
        {

        }
    }

    private void tir()
    {
        //Verifier si on a plus qu'un chargeur


        ballesPresentes--;
        AmmoCount.occurence.mettreAJourTexte(ballesPresentes);

        //Update UI

        muzzleSpark.Play(); //Effet au bout du canon
        audioSource.PlayOneShot(sonTirer);
        //À partir de la cam, on va "Raycast" un ray invisible vers la direction qu'on vise
        RaycastHit impactInfo;

        //crée un raycast vers en avant et enregistre l'info de l'impacte du raycast dans ImpactInfo
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out impactInfo, tirDistance))
        {
            ObjectToHit objetCoup = impactInfo.transform.GetComponent<ObjectToHit>(); //Reference à l'objet
            Zombie1 zombie = impactInfo.transform.GetComponent<Zombie1>();

            if (objetCoup != null)
            {
                
                GameObject impacteBoisGo = Instantiate(impacteBoisEffet, impactInfo.point, Quaternion.LookRotation(impactInfo.normal));
                Destroy(impacteBoisGo, 1f);
            }
            else if (zombie != null)
            {
                zombie.zombieHitDamage(degatsRifle);
                GameObject effetMortGameObject = Instantiate(effetMort, impactInfo.point, Quaternion.LookRotation(impactInfo.normal));
                Destroy(effetMortGameObject, 1f);
            }
        }
    }

    //Enumerator => permet d'attendre
    IEnumerator recharge()
    {

        setRecharge = true;


        audioSource.PlayOneShot(sonRecharge);

        if(Input.GetKey(KeyCode.W)  || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            animator.SetBool("RechargeMarche", true);
        }
        else 
        {
            animator.SetBool("Recharge",true);
        }

        yield return new WaitForSeconds(tempsRecharge);

        ballesPresentes = maxBalles;
        setRecharge = false;
        AmmoCount.occurence.mettreAJourTexte(ballesPresentes);

        animator.SetBool("RechargeMarche", false);
        animator.SetBool("Recharge", false);
    }
}