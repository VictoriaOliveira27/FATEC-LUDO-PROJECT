﻿using UnityEngine;
using System.Collections;

public class RedPlayerIII_Script_Multiplayer : MonoBehaviour
{
    // Removendo o static para torná-la uma variável instanciada.
    public static string redPlayerIII_ColName;

    void Start()
    {
        redPlayerIII_ColName = "none";
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "blocks")
        {
            redPlayerIII_ColName = col.gameObject.name;
            if (col.gameObject.name.Contains("Safe House"))
            {
                SoundManagerScript.safeHouseAudioSource.Play();
            }
        }
    }

    // RPC para sincronizar a variável redPlayerIII_ColName entre todos os jogadores
   
}
