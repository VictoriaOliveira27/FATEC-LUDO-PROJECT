using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoreMenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    
    public void Singleplayer()
    {
        SceneManager.LoadScene("Main Menu");  
    }
    
    public void Multiplayer()
    {
        SceneManager.LoadScene("Loading");  
    }
    
}
