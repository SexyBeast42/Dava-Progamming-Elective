using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI winnerText;
    public TextMeshProUGUI reloadText;
    
    private void Start()
    {
        reloadText = GameObject.Find("Reload Announcement").GetComponent<TextMeshProUGUI>();
        winnerText = GameObject.Find("Winning Team Announcement").GetComponent<TextMeshProUGUI>();
        
        winnerText.enabled = false;
        reloadText.enabled = false;
    }

    public void AnnounceWinners(string team)
    {
        winnerText.alignment = TextAlignmentOptions.Center;
        
        winnerText.text = team + " has won" + "\n\nSpace to restart!" ;

        winnerText.enabled = true;
    }

    public void AnnounceReloaded()
    {
        reloadText.alignment = TextAlignmentOptions.Flush;
        reloadText.alignment = TextAlignmentOptions.MidlineLeft;
        
        StartCoroutine(ReloadCooldown());
    }
    
    IEnumerator ReloadCooldown()
    {
        reloadText.enabled = true;
        
        yield return new WaitForSeconds(1);

        reloadText.enabled = false;
    }
}
