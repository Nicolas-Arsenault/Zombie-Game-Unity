using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZombieSpawnerController : MonoBehaviour
{
    public int nombreZombieInitiale = 5;
    public int nombreZombiePendantLaManche;

    public float delaiSpawn = 2f;

    public int manche = 0;
    public float tempsEntreManche = 10.0f;

    public bool tempsEntreMancheFini;
    public float compteurCooldown = 0;

    public bool mancheTerminer; // variable si la manche est fini

    public List<Zombie1> zombieEnVie;

    public GameObject zombiePrefab;

    public TextMeshProUGUI cooldownMancheUI;
    public TextMeshProUGUI mancheTermineUI;
    public TextMeshProUGUI compteurManchesUI;

    private void Start()
    {
        nombreZombiePendantLaManche = nombreZombieInitiale;
        CommencerProchaineManche();
    }

    private void CommencerProchaineManche() 
    {
        mancheTerminer = false; // la manche debute
        zombieEnVie.Clear();
        manche++;
        compteurManchesUI.text = manche.ToString();
        StartCoroutine(SpawnManche());
    }

    private IEnumerator SpawnManche() {
        for (int i = 0; i < nombreZombiePendantLaManche; i++) {
            Vector3 spawnOffset = new Vector3(Random.Range(-10f,10f) + 20, 0.25f, Random.Range(-10f, 10f) + 20);
            Vector3 spawnPosition = transform.position + spawnOffset;

            // creer un zombie
            GameObject zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);

            // prendre le script Zombie.cs
            Zombie1 zombieScript = zombie.GetComponent<Zombie1>();

            zombieEnVie.Add(zombieScript);

            yield return new WaitForSeconds(delaiSpawn);
        }
    }

    private void Update()
    {
        // verifier les zombies dead
        List<Zombie1> zombieAEnlever = new List<Zombie1>();
        foreach (Zombie1 z in zombieEnVie) {

            //if (z.estMort) {
              //  zombieAEnlever.Add(z);
           // }
        }

        foreach (Zombie1 z in zombieAEnlever) {
            zombieEnVie.Remove(z);
        }

        zombieAEnlever.Clear();

        // verifie si la manche est finie
        // commencer le timer entre les manches si tous les zombies sont morts
        if (zombieEnVie.Count == 0 && tempsEntreMancheFini == false) {
            mancheTerminer = true;


            // commencer le timer entre les manches
            StartCoroutine(MancheCooldown());
        }

        if (tempsEntreMancheFini)
        {
            compteurCooldown -= Time.deltaTime;
        }
        else { // reset le compteur
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

        nombreZombiePendantLaManche *= 2; // x2 pour la prochaine manche
        CommencerProchaineManche();
    }
}