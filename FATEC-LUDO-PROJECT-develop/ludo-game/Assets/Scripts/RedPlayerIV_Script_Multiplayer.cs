
using UnityEngine;
using System.Collections;

public class RedPlayerIV_Script_Multiplayer : MonoBehaviour
{
    // Removendo o static para torná-la uma variável instanciada.
    public static string redPlayerIV_ColName;

    void Start()
    {
        redPlayerIV_ColName = "none";
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "blocks")
        {
            redPlayerIV_ColName = col.gameObject.name;

            if (col.gameObject.name.Contains("Safe House"))
            {
                SoundManagerScript.safeHouseAudioSource.Play();
            }
        }
    }

    // RPC para sincronizar a variável redPlayerIV_ColName entre todos os jogadores

}
