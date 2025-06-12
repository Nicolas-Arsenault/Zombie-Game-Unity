using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [Header("ZombieSpawner Variable")]
    public GameObject zombiePrefab;
    public GameObject dangerZone;
    public Transform zombieSpawnPosition;
    public Transform joueur; // Référence au joueur
    public float rayonDespawn = 20f; // Distance à laquelle un zombie disparait


    public int nombreZombieInitiale = 5;
    public int nombreZombiePendantLaManche;

    private float delaiSpawn = 1.5f;
    private int manche = 0;
    private float tempsEntreManche = 5.0f;

    public bool tempsEntreMancheFini;
    public float compteurCooldown = 0;

    public bool mancheTerminer; // variable si la manche est fini

    public List<Zombie1> zombieEnVie;

    private AudioSource audio; // ajouter 'new'

    public TextMeshProUGUI cooldownMancheUI;
    public TextMeshProUGUI mancheTermineUI;
    public TextMeshProUGUI compteurManchesUI;

    public int vieZombie;

    public int nombreZombieMort = 0;

    private void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();
        nombreZombiePendantLaManche = nombreZombieInitiale;
        vieZombie = 0;
        SauvegarderPartie.Instance.scoreMeilleurManche = manche;
        CommencerProchaineManche();
    }

    private void CommencerProchaineManche()
    {
        audio.Play();
        mancheTerminer = false; // la manche debute
        zombieEnVie.Clear();
        manche++;
        SauvegarderPartie.Instance.scoreMeilleurManche = manche;
        compteurManchesUI.text = manche.ToString();
        StartCoroutine(SpawnManche());
    }

    private IEnumerator SpawnManche()
    {
        for (int i = 0; i < nombreZombiePendantLaManche; i++)
        {
            // créer un zombie
            //GameObject zombie = faireSpawnerUnZombie();

            Zombie1 zombie1Script = faireSpawnerUnZombie().GetComponent<Zombie1>();

            zombie1Script.changerVieZombie(vieZombie);

            zombieEnVie.Add(zombie1Script);

            yield return new WaitForSeconds(delaiSpawn);
        }
    }
    private void Update()
    {
        Debug.Log(nombreZombieMort);

        // verifier les zombies mort
        List<Zombie1> zombieAEnlever = new List<Zombie1>();


        foreach (Zombie1 z in zombieEnVie)
        {
            if (z.estMort)
            {
                zombieAEnlever.Add(z);
            }
        }

        Debug.Log("zombieEnVie" + zombieEnVie.Count);

        foreach (Zombie1 z in zombieAEnlever)
        {
            nombreZombieMort += 1;
            zombieEnVie.Remove(z);
        }

        zombieAEnlever.Clear();



        // verifie si la manche est finie
        // commencer le timer entre les manches si tous les zombies sont morts
        if (nombreZombieMort == nombreZombiePendantLaManche && tempsEntreMancheFini == false)
        {
            mancheTerminer = true;
            // commencer le timer entre les manches
            StartCoroutine(MancheCooldown());
            nombreZombieMort = 0;
        }



        if (tempsEntreMancheFini)
        {
            compteurCooldown -= Time.deltaTime;
        }
        else
        { // reset le compteur
            compteurCooldown = tempsEntreManche;
        }

        cooldownMancheUI.text = compteurCooldown.ToString("F0");
    }
    private IEnumerator MancheCooldown()
    {
        tempsEntreMancheFini = true;

        mancheTermineUI.gameObject.SetActive(true);

        yield return new WaitForSeconds(compteurCooldown);

        tempsEntreMancheFini = false;
        mancheTermineUI.gameObject.SetActive(false);

        if (manche < 3)
        {
            nombreZombiePendantLaManche += 2;
        }
        else if (manche >= 3 && manche < 10)
        {
            nombreZombiePendantLaManche += 5;
        }
        else
        {
            nombreZombiePendantLaManche += 10;
        }

        if (vieZombie <= 50)
        {
            vieZombie += 5;
        }

        CommencerProchaineManche();
    }

    // Fonction pour faire spawner un zombie
    public GameObject faireSpawnerUnZombie()
    {
        Vector3 spawnOffset = new Vector3(Random.Range(-10f, 10f) + 2, 0f, Random.Range(-10f, 10f) + 2);
        Vector3 spawnPosition = transform.position + spawnOffset;
        return Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
    }
}