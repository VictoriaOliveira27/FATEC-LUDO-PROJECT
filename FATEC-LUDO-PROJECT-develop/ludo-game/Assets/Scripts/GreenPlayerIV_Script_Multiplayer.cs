

using UnityEngine;
using System.Collections;

public class GreenPlayerIV_Script_Multiplayer : MonoBehaviour
{
    // Removendo o static para torná-la uma variável instanciada.
    public static string greenPlayerIV_ColName;

    void Start()
    {
        greenPlayerIV_ColName = "none";
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "blocks")
        {
            greenPlayerIV_ColName = col.gameObject.name;
            
            if (col.gameObject.name.Contains("Safe House"))
            {
                SoundManagerScript.safeHouseAudioSource.Play();
            }
        }
    }

    // RPC para sincronizar a variável greenPlayerIV_ColName entre todos os jogadores

}
