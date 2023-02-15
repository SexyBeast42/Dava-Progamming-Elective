using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public List<GameObject> playersInGame;

    private void Start()
    {
        GameObject[] listOfPlayers = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in listOfPlayers)
        {
            playersInGame.Add(player);
        }
    }

    private void Update()
    {
        PlayersAlive();
    }

    private void PlayersAlive()
    {
        for (int i = 0; i < playersInGame.Count; i++)
        {
            if (playersInGame[i] != null && !playersInGame[i].gameObject.active && playersInGame.Count > 1)
            {
                playersInGame.Remove(playersInGame[i]);
            }

            else if (playersInGame.Count <= 1)
            {
                playersInGame[i].GetComponent<AI_Logic>().IsLastManStanding();
            }
        }
        
    }
}
