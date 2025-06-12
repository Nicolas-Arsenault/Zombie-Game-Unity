using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCount : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TextMeshProUGUI ballesTexte;

    public static AmmoCount occurence;

    private void Awake()
    {
        occurence = this;
    }

    public void mettreAJourTexte(int nbBalles)
    {
        ballesTexte.text = "Balles: " + nbBalles.ToString();
    }
}