using UnityEngine;

public class CreerPinceau : MonoBehaviour
{
    public float longueur = 2.0f;
    public float largeur = 0.12f;
    public float largeurPointe = 0.03f;

    void Start()
    {
        // Allonger le cube pour former le manche
        transform.localScale = new Vector3(
            largeur,
            longueur,
            largeur
        );

        // Ajouter un matériau noir
        Renderer rendu = GetComponent<Renderer>();

        if (rendu != null)
        {
            rendu.material.color = Color.black;
        }

        // Nommer l'objet
        gameObject.name = "Pinceau";
    }
}