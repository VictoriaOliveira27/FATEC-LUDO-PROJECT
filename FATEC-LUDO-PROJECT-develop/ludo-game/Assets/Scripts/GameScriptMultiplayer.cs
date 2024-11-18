using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq.Expressions;

public class GameScriptMultiplayer : MonoBehaviourPunCallbacks
{
    private int totalRedInHouse, totalGreenInHouse;

    public GameObject redPlayerI_StartPos;
    public GameObject redPlayerII_StartPos;
     public GameObject redPlayerIII_StartPos;
      public GameObject redPlayerIV_StartPos;
    public GameObject greenPlayerI_StartPos;
    public GameObject greenPlayerII_StartPos;
    public GameObject greenPlayerIII_StartPos;
    public GameObject greenPlayerIV_StartPos;

    public GameObject frameRed, frameGreen;

    public GameObject redPlayerI_Border, redPlayerII_Border, redPlayerIII_Border, redPlayerIV_Border;
    public GameObject greenPlayerI_Border, greenPlayerII_Border, greenPlayerIII_Border, greenPlayerIV_Border;

    public Vector3 redPlayerI_Pos, redPlayerII_Pos, redPlayerIII_Pos, redPlayerIV_Pos;
    public Vector3 greenPlayerI_Pos, greenPlayerII_Pos, greenPlayerIII_Pos, greenPlayerIV_Pos;

    public Button RedPlayerI_Button, RedPlayerII_Button, RedPlayerIII_Button, RedPlayerIV_Button;
    public Button GreenPlayerI_Button, GreenPlayerII_Button, GreenPlayerIII_Button, GreenPlayerIV_Button;

    public GameObject greenScreen, redScreen;
    public Text greenRankText, redRankText;

    private string playerTurn = "RED";
    public Transform diceRoll;
    public Button DiceRollButton;

	private int selectDiceNumAnimation;

    public Transform redDiceRollPos, greenDiceRollPos;

    private string currentPlayer = "none";
    private string currentPlayerName = "none";

	public GameObject dice1_Roll_Animation;
	public GameObject dice2_Roll_Animation;
	public GameObject dice3_Roll_Animation;
	public GameObject dice4_Roll_Animation;
	public GameObject dice5_Roll_Animation;
	public GameObject dice6_Roll_Animation;

	public List<GameObject> redMovementBlocks = new List<GameObject>();
	public List<GameObject> greenMovementBlocks = new List<GameObject>();
	
    public GameObject redPlayerI, redPlayerII, redPlayerIII, redPlayerIV;
    public GameObject greenPlayerI, greenPlayerII, greenPlayerIII, greenPlayerIV;

    private int redPlayerI_Steps, redPlayerII_Steps, redPlayerIII_Steps, redPlayerIV_Steps;
    private int greenPlayerI_Steps, greenPlayerII_Steps, greenPlayerIII_Steps, greenPlayerIV_Steps;

    private System.Random randomNo;
    public GameObject confirmScreen;
    public GameObject gameCompletedScreen;


    public override void OnPlayerLeftRoom(Player otherPlayer)
{
    Debug.Log("Um jogador saiu da sala: " + otherPlayer.NickName);

    // Verifica se sobrou apenas 1 jogador na sala
    if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
    {
        Debug.Log("Apenas 1 jogador restante. Finalizando o jogo...");
        photonView.RPC("StartGameCompletedRoutine", RpcTarget.All);
    }
}

	public void yesMethod()
	{

		SoundManagerScript.buttonAudioSource.Play ();
        StartCoroutine("GameQuitCoroutine");
	}

    IEnumerator GameQuitCoroutine()
	{
        yield return new WaitForSeconds (1.0f);
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
    {
        yield return null;
    }
        SceneManager.LoadScene("CoreMenu");
	}

	public void noMethod()
	{
		SoundManagerScript.buttonAudioSource.Play ();
		confirmScreen.SetActive (false);
	}

	public void ExitMethod()
	{

		SoundManagerScript.buttonAudioSource.Play ();
		confirmScreen.SetActive (true);
	}
	// -============== GAME COMPLETED ROUTINE ==========================================================

	[PunRPC]
	void StartGameCompletedRoutine()
{
    StartCoroutine("GameCompletedRoutine");
}

	IEnumerator GameCompletedRoutine()
	{
		gameCompletedScreen.SetActive (true);
        yield return new WaitForSeconds (2.0f);
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
    {
        yield return null;
    }
        SceneManager.LoadScene("CoreMenu");
	}

    [PunRPC]
    void InitializeGame()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 30;

        randomNo = new System.Random();

        dice1_Roll_Animation.SetActive(false);
        dice2_Roll_Animation.SetActive(false);
        dice3_Roll_Animation.SetActive(false);
        dice4_Roll_Animation.SetActive(false);
        dice5_Roll_Animation.SetActive(false);
        dice6_Roll_Animation.SetActive(false);

        redPlayerI_Pos = redPlayerI.transform.position;
        redPlayerII_Pos = redPlayerII.transform.position;
        redPlayerIII_Pos = redPlayerIII.transform.position;
        redPlayerIV_Pos = redPlayerIV.transform.position;

        greenPlayerI_Pos = greenPlayerI.transform.position;
        greenPlayerII_Pos = greenPlayerII.transform.position;
        greenPlayerIII_Pos = greenPlayerIII.transform.position;
        greenPlayerIV_Pos = greenPlayerIV.transform.position;

        redPlayerI_Border.SetActive(false);
        redPlayerII_Border.SetActive(false);
        redPlayerIII_Border.SetActive(false);
        redPlayerIV_Border.SetActive(false);

        greenPlayerI_Border.SetActive(false);
        greenPlayerII_Border.SetActive(false);
        greenPlayerIII_Border.SetActive(false);
        greenPlayerIV_Border.SetActive(false);

        redScreen.SetActive(false);
        greenScreen.SetActive(false);

        playerTurn = "RED";

        photonView.RPC("SyncGameState", RpcTarget.OthersBuffered);
    }

    [PunRPC]
    void SyncGameState()
    {
        if (playerTurn == "RED")
        {
            frameRed.SetActive(true);
            frameGreen.SetActive(false);
        }
        else if (playerTurn == "GREEN")
        {
            frameRed.SetActive(false);
            frameGreen.SetActive(true);
        }
    }

    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("InitializeGame", RpcTarget.AllBuffered);
        }
        else
    	{
        Debug.Log("Conexão com o servidor perdida. Tentando reconectar... Voltando ao Menu");
		//SceneManager.LoadScene("CoreMenu");
    	}
    }


//▒█▀▀▄ ░▀░ █▀▀ █▀▀ 　 ▒█▀▀█ █▀▀█ █░░ █░░ 
//▒█░▒█ ▀█▀ █░░ █▀▀ 　 ▒█▄▄▀ █░░█ █░░ █░░ 
//▒█▄▄▀ ▀▀▀ ▀▀▀ ▀▀▀ 　 ▒█░▒█ ▀▀▀▀ ▀▀▀ ▀▀▀ 

    
    public void DiceRoll()

    {
        if (playerTurn == "RED" && PhotonNetwork.IsMasterClient || playerTurn == "GREEN" && !PhotonNetwork.IsMasterClient)
        {
           
			Debug.Log("O jogo entendeu que é seu turno e vc pode rodar");
            selectDiceNumAnimation = randomNo.Next(1, 7);
			//selectDiceNumAnimation = 6;
            photonView.RPC("SyncDiceRoll", RpcTarget.All, selectDiceNumAnimation);
            ExecutePlayersNotInitialized();
        }
        else
        {
            Debug.Log("Not your turn to roll the dice.");
        }
    }

[PunRPC]
void SyncDiceRoll(int diceValue)
{
   StartCoroutine(PlayDiceAnimation(diceValue));
   DiceRollButton.interactable = false;
}

IEnumerator PlayDiceAnimation(int diceValue)
{
    // Desativa todas as animações primeiro
    dice1_Roll_Animation.SetActive(false);
    dice2_Roll_Animation.SetActive(false);
    dice3_Roll_Animation.SetActive(false);
    dice4_Roll_Animation.SetActive(false);
    dice5_Roll_Animation.SetActive(false);
    dice6_Roll_Animation.SetActive(false);

    // Atraso antes de ativar a animação, para simular o movimento do dado
    yield return new WaitForSeconds(0.2f);

    // Ativa a animação correspondente ao valor do dado
    switch (diceValue)
    {
        case 1:
            dice1_Roll_Animation.SetActive(true);
            break;
        case 2:
            dice2_Roll_Animation.SetActive(true);
            break;
        case 3:
            dice3_Roll_Animation.SetActive(true);
            break;
        case 4:
            dice4_Roll_Animation.SetActive(true);
            break;
        case 5:
            dice5_Roll_Animation.SetActive(true);
            break;
        case 6:
            dice6_Roll_Animation.SetActive(true);
            break;
    }

    // Mantém a animação por um tempo antes de desativar
    yield return new WaitForSeconds(0.8f);

    // Desativa novamente após o tempo (opcional)
    dice1_Roll_Animation.SetActive(false);
    dice2_Roll_Animation.SetActive(false);
    dice3_Roll_Animation.SetActive(false);
    dice4_Roll_Animation.SetActive(false);
    dice5_Roll_Animation.SetActive(false);
    dice6_Roll_Animation.SetActive(false);

    

}
    
       
	   
    
//▒█▀▀█ █░░ █▀▀█ █░░█ █▀▀ █▀▀█ █▀▀ 　 █▀▀▄ █▀▀█ ▀▀█▀▀ 　 
//▒█▄▄█ █░░ █▄▄█ █▄▄█ █▀▀ █▄▄▀ ▀▀█ 　 █░░█ █░░█ ░░█░░ 　 
//▒█░░░ ▀▀▀ ▀░░▀ ▄▄▄█ ▀▀▀ ▀░▀▀ ▀▀▀ 　 ▀░░▀ ▀▀▀▀ ░░▀░░ 　 

//▀█▀ █▀▀▄ ░▀░ ▀▀█▀▀ ░▀░ █▀▀█ █░░ ░▀░ ▀▀█ █▀▀ █▀▀▄ 
//▒█░ █░░█ ▀█▀ ░░█░░ ▀█▀ █▄▄█ █░░ ▀█▀ ▄▀░ █▀▀ █░░█ 
//▄█▄ ▀░░▀ ▀▀▀ ░░▀░░ ▀▀▀ ▀░░▀ ▀▀▀ ▀▀▀ ▀▀▀ ▀▀▀ ▀▀▀░ 


    void ExecutePlayersNotInitialized()
{
    AtivarBordasJogador();

    string turncolor = playerTurn;
    if (DeveMudarTurno())
    	{
		Debug.Log("DeveMudarTurno retornou TRUE: Mudando o turno.");
        playerTurn = (playerTurn == "RED") ? "GREEN" : "RED";
    	}
		else{
			Debug.Log("DeveMudarTurno retornou FALSE: Mantendo o turno atual.");
		}
		Debug.Log("RedPlayerI_Border ativo: " + redPlayerI_Border.activeInHierarchy + " | RedPlayerI_Button interagível: " + RedPlayerI_Button.interactable);
     photonView.RPC("SyncPlayersState", RpcTarget.Others,
        playerTurn, selectDiceNumAnimation
        // ,
        // redPlayerI_Border.activeInHierarchy, RedPlayerI_Button.interactable,
        // redPlayerII_Border.activeInHierarchy, RedPlayerII_Button.interactable,
        // redPlayerIII_Border.activeInHierarchy, RedPlayerIII_Button.interactable,
        // redPlayerIV_Border.activeInHierarchy, RedPlayerIV_Button.interactable,
        // greenPlayerI_Border.activeInHierarchy, GreenPlayerI_Button.interactable,
        // greenPlayerII_Border.activeInHierarchy, GreenPlayerII_Button.interactable,
        // greenPlayerIII_Border.activeInHierarchy, GreenPlayerIII_Button.interactable,
        // greenPlayerIV_Border.activeInHierarchy, GreenPlayerIV_Button.interactable
        );
     
     if (turncolor != playerTurn){
        photonView.RPC("SyncGameState", RpcTarget.All);
        InitializeDice();
     }
     Debug.Log("Player Turn" + playerTurn);
     
}
 


	private void AtivarBordasJogador()
{
    bool isPlayerRed = playerTurn == "RED";
    int stepsThreshold = selectDiceNumAnimation;
    
    // Se for RED, ativa as bordas dos jogadores vermelhos conforme os passos
    if (isPlayerRed)
    {
        RedPlayerI_Button.interactable = AtivarBordaSePossivel(redPlayerI_Steps, redPlayerI_Border, stepsThreshold, redMovementBlocks);
        RedPlayerII_Button.interactable = AtivarBordaSePossivel(redPlayerII_Steps, redPlayerII_Border, stepsThreshold, redMovementBlocks);
        RedPlayerIII_Button.interactable = AtivarBordaSePossivel(redPlayerIII_Steps, redPlayerIII_Border, stepsThreshold, redMovementBlocks);
        RedPlayerIV_Button.interactable = AtivarBordaSePossivel(redPlayerIV_Steps, redPlayerIV_Border, stepsThreshold, redMovementBlocks);
    }
    else // Se for GREEN, ativa as bordas dos jogadores verdes conforme os passos
    {
        GreenPlayerI_Button.interactable = AtivarBordaSePossivel(greenPlayerI_Steps, greenPlayerI_Border, stepsThreshold, greenMovementBlocks);
        GreenPlayerII_Button.interactable = AtivarBordaSePossivel(greenPlayerII_Steps, greenPlayerII_Border, stepsThreshold, greenMovementBlocks);
        GreenPlayerIII_Button.interactable = AtivarBordaSePossivel(greenPlayerIII_Steps, greenPlayerIII_Border, stepsThreshold, greenMovementBlocks);
        GreenPlayerIV_Button.interactable = AtivarBordaSePossivel(greenPlayerIV_Steps, greenPlayerIV_Border, stepsThreshold, greenMovementBlocks);
    }
}

private bool AtivarBordaSePossivel(int playerSteps, GameObject playerBorder, int stepsThreshold, List<GameObject> movementBlocks)
{
    if ((movementBlocks.Count - playerSteps) >= stepsThreshold && playerSteps > 0 && (movementBlocks.Count > playerSteps))
	{
    playerBorder.SetActive(true);
    return true; // O botão do jogador pode ser interagido
	}
	else if (stepsThreshold == 6 && playerSteps == 0)
	{
    playerBorder.SetActive(true);
    return true;
	}
	else
	{
    playerBorder.SetActive(false);
    return false;
	}	

}
   



private bool DeveMudarTurno()
{
    bool isRedTurn = playerTurn == "RED";

    // Verifica se todas as bordas dos jogadores estão desativadas para o turno atual
    if (isRedTurn)
    {
        return !redPlayerI_Border.activeInHierarchy &&
               !redPlayerII_Border.activeInHierarchy &&
               !redPlayerIII_Border.activeInHierarchy &&
               !redPlayerIV_Border.activeInHierarchy;
    }
    else
    {
        return !greenPlayerI_Border.activeInHierarchy &&
               !greenPlayerII_Border.activeInHierarchy &&
               !greenPlayerIII_Border.activeInHierarchy &&
               !greenPlayerIV_Border.activeInHierarchy;
    }
} 

[PunRPC]
void SyncPlayersState(string turn, int diceValue

    // ,
    // bool redPlayerIBorderActive, bool redPlayerIButtonInteractable,
    // bool redPlayerIIBorderActive, bool redPlayerIIButtonInteractable,
    // bool redPlayerIIIBorderActive, bool redPlayerIIIButtonInteractable,
//     bool redPlayerIVBorderActive, bool redPlayerIVButtonInteractable,
//     bool greenPlayerIBorderActive, bool greenPlayerIButtonInteractable,
//     bool greenPlayerIIBorderActive, bool greenPlayerIIButtonInteractable,
//     bool greenPlayerIIIBorderActive, bool greenPlayerIIIButtonInteractable,
//     bool greenPlayerIVBorderActive, bool greenPlayerIVButtonInteractable
)
{
    playerTurn = turn;
    selectDiceNumAnimation = diceValue;
    Debug.Log("    selectDiceNumAnimation =  "  + selectDiceNumAnimation.ToString());

    // Sincronize os estados de borda e botão
    // redPlayerI_Border.SetActive(redPlayerIBorderActive);
    // RedPlayerI_Button.interactable = redPlayerIButtonInteractable;

    // redPlayerII_Border.SetActive(redPlayerIIBorderActive);
    // RedPlayerII_Button.interactable = redPlayerIIButtonInteractable;

    //  redPlayerIII_Border.SetActive(redPlayerIIIBorderActive);
    // RedPlayerIII_Button.interactable = redPlayerIIIButtonInteractable;

    // redPlayerIV_Border.SetActive(redPlayerIVBorderActive);
    // RedPlayerIV_Button.interactable = redPlayerIVButtonInteractable;

    // greenPlayerI_Border.SetActive(greenPlayerIBorderActive);
    // GreenPlayerI_Button.interactable = greenPlayerIButtonInteractable;

    // greenPlayerII_Border.SetActive(greenPlayerIIBorderActive);
    // GreenPlayerII_Button.interactable = greenPlayerIIButtonInteractable;

    // greenPlayerIII_Border.SetActive(greenPlayerIIIBorderActive);
    // GreenPlayerIII_Button.interactable = greenPlayerIIIButtonInteractable;

    // greenPlayerIV_Border.SetActive(greenPlayerIVBorderActive);
    // GreenPlayerIV_Button.interactable = greenPlayerIVButtonInteractable;

   
}





	//▀█▀ █▀▀▄ ░▀░ ▀▀█▀▀ ░▀░ █▀▀█ █░░ ░▀░ ▀▀█ █▀▀ 　 ▒█▀▀▄ ░▀░ █▀▀ █▀▀ 
	//▒█░ █░░█ ▀█▀ ░░█░░ ▀█▀ █▄▄█ █░░ ▀█▀ ▄▀░ █▀▀ 　 ▒█░▒█ ▀█▀ █░░ █▀▀ 
	//▄█▄ ▀░░▀ ▀▀▀ ░░▀░░ ▀▀▀ ▀░░▀ ▀▀▀ ▀▀▀ ▀▀▀ ▀▀▀ 　 ▒█▄▄▀ ▀▀▀ ▀▀▀ ▀▀▀ 

void InitializeDice()
{
    DiceRollButton.interactable = true;
       Debug.Log("Posições chegando em Initialize Dice: RedPlayerI - " + redPlayerI.transform.position +
                "Posições chegando em Initialize Dice: RedPlayerII - " + redPlayerII.transform.position +
                "Posições chegando em Initialize Dice: RedPlayerIII - " + redPlayerIII.transform.position +
                "Posições chegando em Initialize Dice: RedPlayerIV - " + redPlayerIV.transform.position);

	DesativarInteracaoPecas(); 

    Debug.Log("Posições Após em Initialize Dice DesativarInteracaoPecas: RedPlayerI - " + redPlayerI.transform.position +
                "Posições Após em Initialize Dice DesativarInteracaoPecas: RedPlayerII - " + redPlayerII.transform.position +
                "Posições Após em Initialize Dice DesativarInteracaoPecas: RedPlayerIII - " + redPlayerIII.transform.position +
                "Posições Após em Initialize Dice DesativarInteracaoPecas: RedPlayerIV - " + redPlayerIV.transform.position);

	VerificarUltrapassagem();

     Debug.Log("Posições Após em Initialize Dice VerificarUltrapassagem();: RedPlayerI - " + redPlayerI.transform.position +
                "Posições Após em Initialize Dice VerificarUltrapassagem();: RedPlayerII - " + redPlayerII.transform.position +
                "Posições Após em Initialize Dice VerificarUltrapassagem();: RedPlayerIII - " + redPlayerIII.transform.position +
                "Posições Após em Initialize Dice VerificarUltrapassagem();: RedPlayerIV - " + redPlayerIV.transform.position);


    Debug.Log("Posições Após em Initialize Dice ConfigurarPosicaoDado();: RedPlayerI - " + redPlayerI.transform.position +
                "Posições Após em Initialize Dice ConfigurarPosicaoDado();: RedPlayerII - " + redPlayerII.transform.position +
                "Posições Após em Initialize Dice ConfigurarPosicaoDado();: RedPlayerIII - " + redPlayerIII.transform.position +
                "Posições Após em Initialize Dice ConfigurarPosicaoDado();: RedPlayerIV - " + redPlayerIV.transform.position);

   Debug.Log("cheguei em initialize dice de novo");
    
    VerificarCondicaoVitoria();
  
     photonView.RPC("SyncDiceState", RpcTarget.Others, playerTurn, selectDiceNumAnimation,
               redPlayerI_Steps, redPlayerII_Steps, redPlayerIII_Steps, redPlayerIV_Steps,
               greenPlayerI_Steps, greenPlayerII_Steps, greenPlayerIII_Steps, greenPlayerIV_Steps,
               RedPlayerI_Script_Multiplayer.redPlayerI_ColName, RedPlayerII_Script_Multiplayer.redPlayerII_ColName,
               RedPlayerIII_Script_Multiplayer.redPlayerIII_ColName, RedPlayerIV_Script_Multiplayer.redPlayerIV_ColName,
               GreenPlayerI_Script_Multiplayer.greenPlayerI_ColName, GreenPlayerII_Script_Multiplayer.greenPlayerII_ColName,
               GreenPlayerIII_Script_Multiplayer.greenPlayerIII_ColName, GreenPlayerIV_Script_Multiplayer.greenPlayerIV_ColName,
               DiceRollButton.interactable,
               RedPlayerI_Button.interactable, RedPlayerII_Button.interactable, RedPlayerIII_Button.interactable, RedPlayerIV_Button.interactable,
               GreenPlayerI_Button.interactable, GreenPlayerII_Button.interactable, GreenPlayerIII_Button.interactable, GreenPlayerIV_Button.interactable);

    // Verifica se houve vitória antes de preparar o próximo turno
  
}


void VerificarCondicaoVitoria()
{
    if (playerTurn == "RED" && totalRedInHouse > 3)
    {
        photonView.RPC("StartGameCompletedRoutine", RpcTarget.All);
        
        
    }
    else if (playerTurn == "GREEN" && totalGreenInHouse > 3)
    {
        greenScreen.SetActive(true);
        photonView.RPC("StartGameCompletedRoutine", RpcTarget.All);
    }
    else{
        Debug.Log("Ainda não acabou :).");
        return;
    }
}


private void VerificarUltrapassagem()
{
    if (currentPlayerName.Contains("RED PLAYER"))
    {

        if (currentPlayerName == "RED PLAYER I") {
				Debug.Log ("currentPlayerName = " + currentPlayerName);
				currentPlayer = RedPlayerI_Script_Multiplayer.redPlayerI_ColName;
			}
			if (currentPlayerName == "RED PLAYER II") {
				Debug.Log ("currentPlayerName = " + currentPlayerName);
				currentPlayer = RedPlayerII_Script_Multiplayer.redPlayerII_ColName;
			}
			if (currentPlayerName == "RED PLAYER III") {
				Debug.Log ("currentPlayerName = " + currentPlayerName);
				currentPlayer = RedPlayerIII_Script_Multiplayer.redPlayerIII_ColName;
			}
			if (currentPlayerName == "RED PLAYER IV") {
				Debug.Log ("currentPlayerName = " + currentPlayerName);
				currentPlayer = RedPlayerIV_Script_Multiplayer.redPlayerIV_ColName;
			}

    }

     if (currentPlayerName.Contains("GREEN PLAYER"))

     {
			if (currentPlayerName == "GREEN PLAYER I") {
				Debug.Log ("currentPlayerName = " + currentPlayerName);
				currentPlayer = GreenPlayerI_Script_Multiplayer.greenPlayerI_ColName;
			}
			if (currentPlayerName == "GREEN PLAYER II") {
				Debug.Log ("currentPlayerName = " + currentPlayerName);
				currentPlayer = GreenPlayerII_Script_Multiplayer.greenPlayerII_ColName;
			}
			if (currentPlayerName == "GREEN PLAYER III") {
				Debug.Log ("currentPlayerName = " + currentPlayerName);
				currentPlayer = GreenPlayerIII_Script_Multiplayer.greenPlayerIII_ColName;
			}
			if (currentPlayerName == "GREEN PLAYER IV") {
				Debug.Log ("currentPlayerName = " + currentPlayerName);
				currentPlayer = GreenPlayerIV_Script_Multiplayer.greenPlayerIV_ColName;
			}
	}

    if (currentPlayerName != "none")
{
               
     
          if (currentPlayerName.Contains ("RED PLAYER")) {
                    Debug.Log ("currentPlayerName = " + currentPlayer);
					if (currentPlayer == GreenPlayerI_Script_Multiplayer.greenPlayerI_ColName && (currentPlayer != "Star" && GreenPlayerI_Script_Multiplayer.greenPlayerI_ColName != "Star")) {
						SoundManagerScript.dismissalAudioSource.Play ();
                        iTween.MoveTo (greenPlayerI, iTween.Hash ("position", greenPlayerI_StartPos.transform.position, "speed", 125,"time",2.0f, "easetype", "elastic", "looptype", "none"));
						GreenPlayerI_Script_Multiplayer.greenPlayerI_ColName = "none";
						greenPlayerI_Steps = 0;
						playerTurn = "RED";
                        Debug.Log("Deu true 1");

    
					}
					if (currentPlayer == GreenPlayerII_Script_Multiplayer.greenPlayerII_ColName && (currentPlayer != "Star" && GreenPlayerII_Script_Multiplayer.greenPlayerII_ColName != "Star")) {
						SoundManagerScript.dismissalAudioSource.Play ();
                        iTween.MoveTo (greenPlayerII, iTween.Hash ("position", greenPlayerII_StartPos.transform.position, "speed", 125,"time",2.0f, "easetype", "elastic", "looptype", "none"));
						GreenPlayerII_Script_Multiplayer.greenPlayerII_ColName = "none";
						greenPlayerII_Steps = 0;
						playerTurn = "RED";
                          Debug.Log("Deu true 2");
					}
					if (currentPlayer == GreenPlayerIII_Script_Multiplayer.greenPlayerIII_ColName && (currentPlayer != "Star" && GreenPlayerIII_Script_Multiplayer.greenPlayerIII_ColName != "Star")) {
						SoundManagerScript.dismissalAudioSource.Play ();
						iTween.MoveTo (greenPlayerIII, iTween.Hash ("position", greenPlayerIII_StartPos.transform.position, "speed", 125,"time",2.0f, "easetype", "elastic", "looptype", "none"));
						GreenPlayerIII_Script_Multiplayer.greenPlayerIII_ColName = "none";
						greenPlayerIII_Steps = 0;
						playerTurn = "RED";
                              Debug.Log("Deu true 3");
					}
					if (currentPlayer == GreenPlayerIV_Script_Multiplayer.greenPlayerIV_ColName && (currentPlayer != "Star" && GreenPlayerIV_Script_Multiplayer.greenPlayerIV_ColName != "Star")) {
						SoundManagerScript.dismissalAudioSource.Play ();
						iTween.MoveTo (greenPlayerIV, iTween.Hash ("position", greenPlayerIV_StartPos.transform.position, "speed", 125,"time",2.0f, "easetype", "elastic", "looptype", "none"));
						GreenPlayerIV_Script_Multiplayer.greenPlayerIV_ColName = "none";
						greenPlayerIV_Steps = 0;
						playerTurn = "RED";
                           Debug.Log("Deu true 4");
					}
				}
       
                
                if (currentPlayerName.Contains ("GREEN PLAYER")) {
					if (currentPlayer == RedPlayerI_Script_Multiplayer.redPlayerI_ColName && (currentPlayer != "Star" && RedPlayerI_Script_Multiplayer.redPlayerI_ColName != "Star")) {
						SoundManagerScript.dismissalAudioSource.Play ();
                        iTween.MoveTo (redPlayerI, iTween.Hash ("position", redPlayerI_StartPos.transform.position, "speed", 125,"time",2.0f, "easetype", "elastic", "looptype", "none"));
						RedPlayerI_Script_Multiplayer.redPlayerI_ColName = "none";
						redPlayerI_Steps = 0;
						playerTurn = "GREEN";
                        Debug.Log("Deu true 5");
					}
					if (currentPlayer == RedPlayerII_Script_Multiplayer.redPlayerII_ColName && (currentPlayer != "Star" && RedPlayerII_Script_Multiplayer.redPlayerII_ColName != "Star")) {
						SoundManagerScript.dismissalAudioSource.Play ();
						iTween.MoveTo (redPlayerII, iTween.Hash ("position", redPlayerII_StartPos.transform.position, "speed", 125,"time",2.0f, "easetype", "elastic", "looptype", "none"));
						RedPlayerII_Script_Multiplayer.redPlayerII_ColName = "none";
						redPlayerII_Steps = 0;
						playerTurn = "GREEN";
                        Debug.Log("Deu true 6");
					}
					if (currentPlayer == RedPlayerIII_Script_Multiplayer.redPlayerIII_ColName && (currentPlayer != "Star" && RedPlayerIII_Script_Multiplayer.redPlayerIII_ColName != "Star")) {
						SoundManagerScript.dismissalAudioSource.Play ();
						iTween.MoveTo (redPlayerIII, iTween.Hash ("position", redPlayerIII_StartPos.transform.position, "speed", 125,"time",2.0f, "easetype", "elastic", "looptype", "none"));
						RedPlayerIII_Script_Multiplayer.redPlayerIII_ColName = "none";
						redPlayerIII_Steps = 0;
						playerTurn = "GREEN";
                        Debug.Log("Deu true 7");
					}
					if (currentPlayer == RedPlayerIV_Script_Multiplayer.redPlayerIV_ColName && (currentPlayer != "Star" && RedPlayerIV_Script_Multiplayer.redPlayerIV_ColName != "Star")) {
						SoundManagerScript.dismissalAudioSource.Play ();
						iTween.MoveTo (redPlayerIV, iTween.Hash ("position", redPlayerI_StartPos.transform.position, "speed", 125,"time",2.0f, "easetype", "elastic", "looptype", "none"));
						RedPlayerIV_Script_Multiplayer.redPlayerIV_ColName = "none";
						redPlayerIV_Steps = 0;
						playerTurn = "GREEN";
                             Debug.Log("Deu true 8");
					}
				}
              
             


    

        
        Debug.Log("GreenPlayerI_Script_Multiplayer.greenPlayerI_ColName" + GreenPlayerI_Script_Multiplayer.greenPlayerI_ColName +
                  "GreenPlayerII_Script_Multiplayer.greenPlayerII_ColName"  + GreenPlayerII_Script_Multiplayer.greenPlayerII_ColName +                                                                                                
                  "GreenPlayerIII_Script_Multiplayer.greenPlayerIII_ColName" + GreenPlayerIII_Script_Multiplayer.greenPlayerIII_ColName +
                  "GreenPlayerIV_Script_Multiplayer.greenPlayerIV_ColName"    + GreenPlayerIV_Script_Multiplayer.greenPlayerIV_ColName +                                                                                                 
                  "RedPlayerI_Script_Multiplayer.redPlayerI_ColName" + RedPlayerI_Script_Multiplayer.redPlayerI_ColName +
                  "RedPlayerII_Script_Multiplayer.redPlayerII_ColName" + RedPlayerII_Script_Multiplayer.redPlayerII_ColName +
                  "RedPlayerIII_Script_Multiplayer.redPlayerIII_ColName" + RedPlayerIII_Script_Multiplayer.redPlayerIII_ColName +
                  "RedPlayerIV_Script_Multiplayer.redPlayerIV_ColName" + RedPlayerIV_Script_Multiplayer.redPlayerIV_ColName +
                  "currentPlayerName" + currentPlayerName +
                   "currentPlayer" + currentPlayer);
        
        
    }

}



[PunRPC]
void SyncDiceState(string newTurn, int diceValue, 
    int redPlayerI_Steps, int redPlayerII_Steps, int redPlayerIII_Steps, int redPlayerIV_Steps,
    int greenPlayerI_Steps, int greenPlayerII_Steps, int greenPlayerIII_Steps, int greenPlayerIV_Steps,
    string redPlayerI_ColName, string redPlayerII_ColName, string redPlayerIII_ColName, string redPlayerIV_ColName,
    string greenPlayerI_ColName, string greenPlayerII_ColName, string greenPlayerIII_ColName, string greenPlayerIV_ColName,
    bool  dicerollinteractable,
    bool  redPlayer1Interactable, bool redPlayer2Interactable, bool redPlayer3Interactable,  bool redPlayer4Interactable,
    bool greenPlayer1Interactable, bool greenPlayer2Interactable, bool greenPlayer3Interactable, bool greenPlayer4Interactable)
{
    // Atualiza o turno, valor do dado e os passos das peças
    playerTurn = newTurn;
    selectDiceNumAnimation = diceValue;

    this.redPlayerI_Steps = redPlayerI_Steps;
    this.redPlayerII_Steps = redPlayerII_Steps;
    this.redPlayerIII_Steps = redPlayerIII_Steps;
    this.redPlayerIV_Steps = redPlayerIV_Steps;

    this.greenPlayerI_Steps = greenPlayerI_Steps;
    this.greenPlayerII_Steps = greenPlayerII_Steps;
    this.greenPlayerIII_Steps = greenPlayerIII_Steps;
    this.greenPlayerIV_Steps = greenPlayerIV_Steps;


    RedPlayerI_Script_Multiplayer.redPlayerI_ColName = redPlayerI_ColName;
    RedPlayerII_Script_Multiplayer.redPlayerII_ColName = redPlayerII_ColName;
    RedPlayerIII_Script_Multiplayer.redPlayerIII_ColName = redPlayerIII_ColName;
    RedPlayerIV_Script_Multiplayer.redPlayerIV_ColName = redPlayerIV_ColName;
    GreenPlayerI_Script_Multiplayer.greenPlayerI_ColName = greenPlayerI_ColName;
    GreenPlayerII_Script_Multiplayer.greenPlayerII_ColName = greenPlayerII_ColName;
    GreenPlayerIII_Script_Multiplayer.greenPlayerIII_ColName = greenPlayerIII_ColName;
    GreenPlayerIV_Script_Multiplayer.greenPlayerIV_ColName = greenPlayerIV_ColName;



    


      Debug.Log("Posições antes de SyncDiceState: RedPlayerI - " + redPlayerI.transform.position +
                "Posições antes de SyncDiceState: RedPlayerII - " + redPlayerII.transform.position +
                "Posições antes de SyncDiceState: RedPlayerIII - " + redPlayerIII.transform.position +
                "Posições antes de SyncDiceState: RedPlayerIV - " + redPlayerIV.transform.position);

    DiceRollButton.interactable = dicerollinteractable;

    // Atualiza o estado de interatividade dos botões das peças vermelhas
    RedPlayerI_Button.interactable = redPlayer1Interactable;
    RedPlayerII_Button.interactable = redPlayer2Interactable;
    RedPlayerIII_Button.interactable = redPlayer3Interactable;
    RedPlayerIV_Button.interactable = redPlayer4Interactable;
    

      Debug.Log("Posições após SyncDiceState: RedPlayerI - " + redPlayerI.transform.position +
                "Posições após SyncDiceState: RedPlayerII - " + redPlayerII.transform.position +
                "Posições após SyncDiceState: RedPlayerIII - " + redPlayerIII.transform.position +
                "Posições após SyncDiceState: RedPlayerIV - " + redPlayerIV.transform.position);


     
    // Atualiza o estado de interatividade dos botões das peças verdes
    GreenPlayerI_Button.interactable = greenPlayer1Interactable;
    GreenPlayerII_Button.interactable = greenPlayer2Interactable;
    GreenPlayerIII_Button.interactable = greenPlayer3Interactable;
    GreenPlayerIV_Button.interactable = greenPlayer4Interactable;
    

    Debug.Log("SyncDiceState concluído com turno: " + playerTurn);



}

private void DesativarInteracaoPecas()
{
    GreenPlayerI_Button.interactable = false;
    GreenPlayerII_Button.interactable = false;
    GreenPlayerIII_Button.interactable = false;
    GreenPlayerIV_Button.interactable = false;
    greenPlayerI_Border.SetActive(false);
    greenPlayerII_Border.SetActive(false);
    greenPlayerIII_Border.SetActive(false);
    greenPlayerIV_Border.SetActive(false);

    RedPlayerI_Button.interactable = false;
    RedPlayerII_Button.interactable = false;
    RedPlayerIII_Button.interactable = false;
    RedPlayerIV_Button.interactable = false;
    redPlayerI_Border.SetActive(false);
    redPlayerII_Border.SetActive(false);
    redPlayerIII_Border.SetActive(false);
    redPlayerIV_Border.SetActive(false);
}


//▒█▀▄▀█ █▀▀█ ▀█░█▀ ░▀░ █▀▄▀█ █▀▀ █▀▀▄ ▀▀█▀▀ █▀▀█ █▀▀ 
//▒█▒█▒█ █░░█ ░█▄█░ ▀█▀ █░▀░█ █▀▀ █░░█ ░░█░░ █░░█ ▀▀█ 
//▒█░░▒█ ▀▀▀▀ ░░▀░░ ▀▀▀ ▀░░░▀ ▀▀▀ ▀░░▀ ░░▀░░ ▀▀▀▀ ▀▀▀ 

//█▀▀▄ █▀▀█ █▀▀ 　 █▀▀█ █▀▀ █▀▀ █▀▀█ █▀▀ 
//█░░█ █▄▄█ ▀▀█ 　 █░░█ █▀▀ █░░ █▄▄█ ▀▀█ 
//▀▀▀░ ▀░░▀ ▀▀▀ 　 █▀▀▀ ▀▀▀ ▀▀▀ ▀░░▀ ▀▀▀ 





public void redPlayerI_UI()
{
    Debug.Log("Tentativa de clique em RedPlayerI");

    // Verifica se é o Master Client e se é o turno vermelho
    if (!PhotonNetwork.IsMasterClient)
    {
        Debug.Log("Somente o Master Client pode mover a peça RED.");
        return;
    }

    if (playerTurn != "RED")
    {
        Debug.Log("Não é o turno do jogador RED.");
        return;
    }

    // Executa o movimento da peça
    MoveRedPlayerI();
}


public void redPlayerII_UI()
{
    Debug.Log("Tentativa de clique em RedPlayerII");

    // Verifica se é o Master Client e se é o turno vermelho
    if (!PhotonNetwork.IsMasterClient)
    {
        Debug.Log("Somente o Master Client pode mover a peça RED.");
        return;
    }

    if (playerTurn != "RED")
    {
        Debug.Log("Não é o turno do jogador RED.");
        return;
    }

    // Executa o movimento da peça
    MoveRedPlayerII();
}

public void redPlayerIII_UI()
{
    Debug.Log("Tentativa de clique em RedPlayerIII");

    // Verifica se é o Master Client e se é o turno vermelho
    if (!PhotonNetwork.IsMasterClient)
    {
        Debug.Log("Somente o Master Client pode mover a peça RED.");
        return;
    }

    if (playerTurn != "RED")
    {
        Debug.Log("Não é o turno do jogador RED.");
        return;
    }

    // Executa o movimento da peça
    MoveRedPlayerIII();
}


public void redPlayerIV_UI()
{
    Debug.Log("Tentativa de clique em RedPlayerIV");

    // Verifica se é o Master Client e se é o turno vermelho
    if (!PhotonNetwork.IsMasterClient)
    {
        Debug.Log("Somente o Master Client pode mover a peça RED.");
        return;
    }

    if (playerTurn != "RED")
    {
        Debug.Log("Não é o turno do jogador RED.");
        return;
    }

    // Executa o movimento da peça
    MoveRedPlayerIV();
}




public void greenPlayerI_UI()
{
    Debug.Log("Tentativa de clique em GreenPlayerI");

    // Verifica se é o Master Client e se é o turno vermelho
     if (PhotonNetwork.IsMasterClient)
    {
        Debug.Log("Somente o Non Master Client pode mover a peça GREEN.");
        return;
    }

    if (playerTurn != "GREEN")
    {
        Debug.Log("Não é o turno do jogador GREEN.");
        return;
    }

    // Executa o movimento da peça
    MoveGreenPlayerI();
}


public void greenPlayerII_UI()
{
    Debug.Log("Tentativa de clique em GreenPlayerII");

    // Verifica se é o Master Client e se é o turno vermelho
     if (PhotonNetwork.IsMasterClient)
    {
        Debug.Log("Somente o Non Master Client pode mover a peça GREEN.");
        return;
    }

    if (playerTurn != "GREEN")
    {
        Debug.Log("Não é o turno do jogador GREEN.");
        return;
    }

    // Executa o movimento da peça
    MoveGreenPlayerII();
}

public void greenPlayerIII_UI()
{
    Debug.Log("Tentativa de clique em GreenPlayerIII");

    // Verifica se é o Master Client e se é o turno vermelho
     if (PhotonNetwork.IsMasterClient)
    {
        Debug.Log("Somente o Non Master Client pode mover a peça GREEN.");
        return;
    }

    if (playerTurn != "GREEN")
    {
        Debug.Log("Não é o turno do jogador GREEN.");
        return;
    }

    // Executa o movimento da peça
    MoveGreenPlayerIII();
}


public void greenPlayerIV_UI()
{
    Debug.Log("Tentativa de clique em GreenPlayerIV");

    // Verifica se é o Master Client e se é o turno vermelho
    if (PhotonNetwork.IsMasterClient)
    {
        Debug.Log("Somente o Non Master Client pode mover a peça GREEN.");
        return;
    }

    if (playerTurn != "GREEN")
    {
        Debug.Log("Não é o turno do jogador GREEN.");
        return;
    }

    // Executa o movimento da peça
    MoveGreenPlayerIV();
}





void MoveRedPlayerI()
{
    currentPlayerName = "RED PLAYER I";
    if (playerTurn != "RED") return;

    // Desativa bordas e botões antes de iniciar o movimento
    DesativarInteracaoPecas();

    // Verifica se o movimento é possível para RedPlayerI
    if (VerificarMovimentoPossivel(redPlayerI_Steps, selectDiceNumAnimation, redMovementBlocks))
    {
        // Executa o movimento e atualiza os passos e turno
        MoverPeca(redPlayerI, ref redPlayerI_Steps, selectDiceNumAnimation, redMovementBlocks, RedPlayerI_Button, playerTurn, ref totalRedInHouse);
    }
    else
    {
        // Caso o movimento não seja possível, troca o turno ou finaliza a jogada
        if (redPlayerII_Steps + redPlayerIII_Steps + redPlayerIV_Steps == 0 && selectDiceNumAnimation != 6)
        {
            TrocarTurno();
            InitializeDice();
        }
        else
        {
            Debug.Log("Movimento não é possível com esta peça, turno mantido.");
            return;
        }
    }
}


void MoveRedPlayerII()
{
    currentPlayerName = "RED PLAYER II";
    if (playerTurn != "RED") return;

    // Desativa bordas e botões antes de iniciar o movimento
    DesativarInteracaoPecas();

    // Verifica se o movimento é possível para RedPlayerI
    if (VerificarMovimentoPossivel(redPlayerII_Steps, selectDiceNumAnimation, redMovementBlocks))
    {
        // Executa o movimento e atualiza os passos e turno
        MoverPeca(redPlayerII, ref redPlayerII_Steps, selectDiceNumAnimation, redMovementBlocks, RedPlayerII_Button, playerTurn, ref totalRedInHouse);
    }
    else
    {
        // Caso o movimento não seja possível, troca o turno ou finaliza a jogada
        if (redPlayerI_Steps + redPlayerIII_Steps + redPlayerIV_Steps == 0 && selectDiceNumAnimation != 6)
        {
            TrocarTurno();
            InitializeDice();
        }
        else
        {
            Debug.Log("Movimento não é possível com esta peça, turno mantido.");
            return;
        }
    }
}



void MoveRedPlayerIII()
{
    currentPlayerName = "RED PLAYER III";
    if (playerTurn != "RED") return;

    // Desativa bordas e botões antes de iniciar o movimento
    DesativarInteracaoPecas();

    // Verifica se o movimento é possível para RedPlayerI
    if (VerificarMovimentoPossivel(redPlayerIII_Steps, selectDiceNumAnimation, redMovementBlocks))
    {
        // Executa o movimento e atualiza os passos e turno
        MoverPeca(redPlayerIII, ref redPlayerIII_Steps, selectDiceNumAnimation, redMovementBlocks, RedPlayerIII_Button, playerTurn,  ref totalRedInHouse);
    }
    else
    {
        // Caso o movimento não seja possível, troca o turno ou finaliza a jogada
        if (redPlayerI_Steps + redPlayerII_Steps + redPlayerIV_Steps == 0 && selectDiceNumAnimation != 6)
        {
            TrocarTurno();
            InitializeDice();
        }
        else
        {
            Debug.Log("Movimento não é possível com esta peça, turno mantido.");
            return;
        }
    }
}




void MoveRedPlayerIV()
{
    currentPlayerName = "RED PLAYER IV";
    if (playerTurn != "RED") return;

    // Desativa bordas e botões antes de iniciar o movimento
    DesativarInteracaoPecas();

    // Verifica se o movimento é possível para RedPlayerI
    if (VerificarMovimentoPossivel(redPlayerIV_Steps, selectDiceNumAnimation, redMovementBlocks))
    {
        // Executa o movimento e atualiza os passos e turno
        MoverPeca(redPlayerIV, ref redPlayerIV_Steps, selectDiceNumAnimation, redMovementBlocks, RedPlayerIV_Button, playerTurn,  ref totalRedInHouse);
    }
    else
    {
        // Caso o movimento não seja possível, troca o turno ou finaliza a jogada
        if (redPlayerI_Steps + redPlayerII_Steps + redPlayerIII_Steps == 0 && selectDiceNumAnimation != 6)
        {
            TrocarTurno();
            InitializeDice();
        }
        else
        {
            Debug.Log("Movimento não é possível com esta peça, turno mantido.");
            return;
        }
    }
}



void MoveGreenPlayerI()
{
    currentPlayerName = "GREEN PLAYER I";
    if (playerTurn != "GREEN") return;

    // Desativa bordas e botões antes de iniciar o movimento
    DesativarInteracaoPecas();

    // Verifica se o movimento é possível para GreenPlayerI
    if (VerificarMovimentoPossivel(greenPlayerI_Steps, selectDiceNumAnimation, greenMovementBlocks))
    {
        // Executa o movimento e atualiza os passos e turno
        MoverPeca(greenPlayerI, ref greenPlayerI_Steps, selectDiceNumAnimation, greenMovementBlocks, GreenPlayerI_Button, playerTurn, ref totalGreenInHouse);
    }
    else
    {
        // Caso o movimento não seja possível, troca o turno ou finaliza a jogada
        if (greenPlayerII_Steps + greenPlayerIII_Steps + greenPlayerIV_Steps == 0 && selectDiceNumAnimation != 6)
        {
            TrocarTurno();
            InitializeDice();
        }
        else
        {
            Debug.Log("Movimento não é possível com esta peça, turno mantido.");
            return;
        }
    }
}


void MoveGreenPlayerII()
{
    currentPlayerName = "GREEN PLAYER II";
    if (playerTurn != "GREEN") return;

    // Desativa bordas e botões antes de iniciar o movimento
    DesativarInteracaoPecas();

    // Verifica se o movimento é possível para GreenPlayerI
    if (VerificarMovimentoPossivel(greenPlayerII_Steps, selectDiceNumAnimation, greenMovementBlocks))
    {
        // Executa o movimento e atualiza os passos e turno
        MoverPeca(greenPlayerII, ref greenPlayerII_Steps, selectDiceNumAnimation, greenMovementBlocks, GreenPlayerII_Button, playerTurn, ref totalGreenInHouse);
    }
    else
    {
        // Caso o movimento não seja possível, troca o turno ou finaliza a jogada
        if (greenPlayerI_Steps + greenPlayerIII_Steps + greenPlayerIV_Steps == 0 && selectDiceNumAnimation != 6)
        {
            TrocarTurno();
            InitializeDice();
        }
        else
        {
            Debug.Log("Movimento não é possível com esta peça, turno mantido.");
            return;
        }
    }
}



void MoveGreenPlayerIII()
{
    currentPlayerName = "GREEN PLAYER III";
    if (playerTurn != "GREEN") return;

    // Desativa bordas e botões antes de iniciar o movimento
    DesativarInteracaoPecas();

    // Verifica se o movimento é possível para GreenPlayerI
    if (VerificarMovimentoPossivel(greenPlayerIII_Steps, selectDiceNumAnimation, greenMovementBlocks))
    {
        // Executa o movimento e atualiza os passos e turno
        MoverPeca(greenPlayerIII, ref greenPlayerIII_Steps, selectDiceNumAnimation, greenMovementBlocks, GreenPlayerIII_Button, playerTurn, ref totalGreenInHouse);
    }
    else
    {
        // Caso o movimento não seja possível, troca o turno ou finaliza a jogada
        if (greenPlayerI_Steps + greenPlayerII_Steps + greenPlayerIV_Steps == 0 && selectDiceNumAnimation != 6)
        {
            TrocarTurno();
            InitializeDice();
        }
        else
        {
            Debug.Log("Movimento não é possível com esta peça, turno mantido.");
            return;
        }
    }
}




void MoveGreenPlayerIV()
{
    currentPlayerName = "GREEN PLAYER IV";
    if (playerTurn != "GREEN") return;

    // Desativa bordas e botões antes de iniciar o movimento
    DesativarInteracaoPecas();

    // Verifica se o movimento é possível para GreenPlayerI
    if (VerificarMovimentoPossivel(greenPlayerIV_Steps, selectDiceNumAnimation, greenMovementBlocks))
    {
        // Executa o movimento e atualiza os passos e turno
        MoverPeca(greenPlayerIV, ref greenPlayerIV_Steps, selectDiceNumAnimation, greenMovementBlocks, GreenPlayerIV_Button, playerTurn, ref totalGreenInHouse);
    }
    else
    {
        // Caso o movimento não seja possível, troca o turno ou finaliza a jogada
        if (greenPlayerI_Steps + greenPlayerII_Steps + greenPlayerIII_Steps == 0 && selectDiceNumAnimation != 6)
        {
            TrocarTurno();
            InitializeDice();
        }
        else
        {
            Debug.Log("Movimento não é possível com esta peça, turno mantido.");
            return;
        }
    }
}







void MoverPeca(GameObject player, ref int playerSteps, int diceValue, List<GameObject> movementBlocks, Button playerButton, string color, ref int totalInHouse)
{
    int stepsToMove = diceValue;
    Vector3[] Player_Path = new Vector3[stepsToMove];

    if (playerSteps > 0)
    {
        for (int i = 0; i < stepsToMove; i++)
        {
            Player_Path[i] = movementBlocks[playerSteps + i].transform.position;
        }
        playerSteps += stepsToMove;

        if ((movementBlocks.Count - playerSteps) == 0)
        {
            playerTurn = color; 
            totalInHouse += 1;
            playerButton.interactable = false;
        }
        else if (diceValue != 6)
        {
            TrocarTurno();  // Troca de turno se não tirou 6 no dado
        }

 
        if (Player_Path.Length > 1)
        {

          iTween.MoveTo (player, iTween.Hash ("path", Player_Path, "speed", 125,"time",2.0f, "easetype", "elastic", "looptype", "none", "oncomplete", "FinalizarMovimento", "oncompletetarget", this.gameObject));
         
          
        }
        else
        {
           iTween.MoveTo (player, iTween.Hash ("position", Player_Path [0], "speed", 125,"time",2.0f, "easetype", "elastic", "looptype", "none", "oncomplete", "FinalizarMovimento", "oncompletetarget", this.gameObject));
           
        }
    
    }
    else
    {
        Player_Path[0] = movementBlocks[playerSteps].transform.position;
        playerSteps += 1;
        playerTurn = color; 
        iTween.MoveTo (player, iTween.Hash ("position", Player_Path [0], "speed", 125,"time",2.0f, "easetype", "elastic", "looptype", "none", "oncomplete", "FinalizarMovimento", "oncompletetarget", this.gameObject));
       
    }

}

void FinalizarMovimento()
{
    // Chama InitializeDice, que irá sincronizar o estado com SyncDiceState
        InitializeDice();
}






bool VerificarMovimentoPossivel(int playerSteps, int diceValue, List<GameObject> movementBlocks)
{
    if (playerSteps == 0 && diceValue != 6)
    {
        return false;
    }
    // Verifica se os passos restantes permitem o movimento com base no total de blocos e posição atual
    return (movementBlocks.Count - playerSteps) >= diceValue;
}

void TrocarTurno()
{
    playerTurn = (playerTurn == "RED") ? "GREEN" : "RED";
}



 void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detecta clique esquerdo do mouse
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Raycast hit object: " + hit.collider.gameObject.name);
            }

            // Para UI (caso seja UI)
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;
            var results = new System.Collections.Generic.List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, results);

            foreach (var result in results)
            {
                Debug.Log("UI Raycast hit object: " + result.gameObject.name);
            }
        }
    }


}

