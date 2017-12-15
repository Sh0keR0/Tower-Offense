using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControlPanelUI : MonoBehaviour {


    public static EnemyControlPanelUI current;

    public GameObject[] Panels;

    private WaveControlUI currentWaveControlUI;
    private EnemyCreationUI currentCreationUI;

    private void Awake()
    {
        current = this;

        currentWaveControlUI = transform.GetComponentInChildren<WaveControlUI>();
        currentCreationUI = transform.GetComponentInChildren<EnemyCreationUI>();

    }


    public void HidePanel()
    {
        gameObject.SetActive(false);
    }


    /// <summary>
    /// Shows the enemy control panel and the sub panel ID
    /// </summary>
    /// <param name="panelID">The ID of the sub panel that will be displayed</param>
    public void ShowPanel(int panelID)
    {

        if(EnemySpawner.current.IsWorking)
        {
            PopupMessage.current.Show("WAVE IN PROGRESS", "Please wait until the wave is over before opening the enemy control panel.", "OKAY");
            return;
        }

        gameObject.SetActive(true);

        SetActivePanel(panelID);

        if (panelID == 1)
        {
            currentWaveControlUI.DisplayWaveControlPanel();
        }else if(panelID == 2)
        {
            currentCreationUI.DisplayCreationScreen();
        }
        
    }

    private void SetActivePanel(int id)
    {
        for (int i = 0; i < Panels.Length; i++)
        {
            if (id == i)
                Panels[i].SetActive(true);
            else
            {
                Panels[i].SetActive(false);
            }
        }
    }


}
