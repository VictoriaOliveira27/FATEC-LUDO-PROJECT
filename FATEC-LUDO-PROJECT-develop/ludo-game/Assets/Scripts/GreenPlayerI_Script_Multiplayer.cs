
using UnityEngine;
using System.Collections;

public class GreenPlayerI_Script_Multiplayer : MonoBehaviour
{
    // Removendo o static para torná-la uma variável instanciada.
    public static string greenPlayerI_ColName;

    void Start()
    {
        greenPlayerI_ColName = "none";
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "blocks")
        {
            greenPlayerI_ColName = col.gameObject.name;
         
            if (col.gameObject.name.Contains("Safe House"))
            {
                SoundManagerScript.safeHouseAudioSource.Play();
            }
        }
    }

    // RPC para sincronizar a variável greenPlayerI_ColName entre todos os jogadores
   
}
