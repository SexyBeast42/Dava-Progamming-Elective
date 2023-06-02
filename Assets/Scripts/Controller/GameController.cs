using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public UIController uiController;
    
    public List<GameObject> playersInGame;

    private void Start()
    {
        StartCoroutine(WaitForSpawn());
        
        uiController = GameObject.Find("EventSystem").GetComponent<UIController>();
    }

    private void Update()
    {
        // Check the amount of people that is alive, then declare winner after opponents have dieded
        PlayersAlive();
        OneTeamLeftStanding();
        
        // Restart game if player presses space
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }
    }

    IEnumerator WaitForSpawn()
    {
        yield return new WaitForSeconds(2);
        
        GetAllPlayers();
    }

    private void GetAllPlayers()
    {
        GameObject[] listOfPlayers = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in listOfPlayers)
        {
            playersInGame.Add(player);
        }
    }

    private void PlayersAlive()
    {
        for (int i = 0; i < playersInGame.Count; i++)
        {
            // Remove dead players
            if (playersInGame[i] != null && !playersInGame[i].gameObject.active && playersInGame.Count > 1)
            {
                playersInGame.Remove(playersInGame[i]);
            }
        }
    }

    private void OneTeamLeftStanding()
    {
        // Need to ensure no dupes
        HashSet<int> playerTeams = new HashSet<int>();

        for (int i = 0; i < playersInGame.Count; i++)
        {
            playerTeams.Add(playersInGame[i].layer);

            if (playerTeams.Count > 1)
            {
                return;
            }
        }

        if (playerTeams.Count == 0)
        {
            return;
        }
        
        // here we want to connect with UI to let them know the game has ended
        uiController.AnnounceWinners(LayerMask.LayerToName(playerTeams.First()));
        Debug.Log(LayerMask.LayerToName(playerTeams.First()) + " has won");
    }

    private void RestartGame()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentSceneIndex);
    }
}
