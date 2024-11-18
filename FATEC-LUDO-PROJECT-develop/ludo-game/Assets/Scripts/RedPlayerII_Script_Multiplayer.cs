using UnityEngine;
using System.Collections;

public class RedPlayerII_Script_Multiplayer : MonoBehaviour
{
    // Removendo o static para torná-la uma variável instanciada.
    public  static string redPlayerII_ColName;

    void Start()
    {
        redPlayerII_ColName = "none";
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "blocks")
        {
            redPlayerII_ColName = col.gameObject.name;
            if (col.gameObject.name.Contains("Safe House"))
            {
                SoundManagerScript.safeHouseAudioSource.Play();
            }
        }
    }

  
}
