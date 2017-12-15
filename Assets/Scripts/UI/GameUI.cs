using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{

    public static GameUI currentUI;

    public Text MoneyText;
    public Text WaveText;
    public Text LifePointsText;

    private TowerInformationUI _towerInfoUI;
    private Tower currentHighlight;

    public TowerInformationUI towerInformationUI
    {
        get { return _towerInfoUI; }
    }

    private void Awake()
    {
        currentUI = this;
        _towerInfoUI = gameObject.GetComponentInChildren<TowerInformationUI>();
        if (_towerInfoUI == null)
            Debug.LogError("TowerInformationUI was not found on any of the GameUI children");


    }


    

    void HighlightTowerRadius(Tower t)
    {
        if(currentHighlight != null)
        {
            
            currentHighlight.transform.GetComponentInChildren<RangeDisplay>().HideRange();
        }

        t.transform.GetComponentInChildren<RangeDisplay>().DisplayRange();
    

        currentHighlight = t;

 
    }

    public void HideHighlightedTowerRange()
    {
        if (currentHighlight == null) return;
        currentHighlight.transform.GetComponentInChildren<RangeDisplay>().HideRange();
    }

    public void OnTowerPressed(Tower tower)
    {
        _towerInfoUI.DisplayInformation(tower);
        HighlightTowerRadius(tower);
    }


}
