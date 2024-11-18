

using UnityEngine;
using System.Collections;

public class GreenPlayerIII_Script_Multiplayer : MonoBehaviour
{
    // Removendo o static para torná-la uma variável instanciada.
    public static string greenPlayerIII_ColName;

    void Start()
    {
        greenPlayerIII_ColName = "none";
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "blocks")
        {
            greenPlayerIII_ColName = col.gameObject.name;
         

            if (col.gameObject.name.Contains("Safe House"))
            {
                SoundManagerScript.safeHouseAudioSource.Play();
            }
        }
    }

    // RPC para sincronizar a variável greenPlayerIII_ColName entre todos os jogadores
   
}
