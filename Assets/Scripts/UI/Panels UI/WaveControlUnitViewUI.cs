using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WaveControlUnitViewUI : MonoBehaviour
{

    // Responsible to adjust the content size of the scrollview and display all the enemies in the game, reading from GameData.cs

    public float SpaceForEachUnit = 100f;
    public GameObject UnitPrefab;

    private RectTransform rect;
    private WaveControlUI waveUI;

    private Dictionary<EnemyData, TextMeshProUGUI> dataToTextDictionary;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        waveUI = transform.GetComponentInParent<WaveControlUI>();

        waveUI.ActionOnUnitsCountChanged += OnUnitsCountChange;
       // DisplayUnits();

    }


    public void DisplayUnits()
    {
     
        // Reszie the content for the viewscroller
        rect.sizeDelta = new Vector2(SpaceForEachUnit + (GameData.Enemies.Count * SpaceForEachUnit), rect.sizeDelta.y);

        dataToTextDictionary = new Dictionary<EnemyData, TextMeshProUGUI>();

        // Destroy any previous children
        int childIndex = 0;
        while(childIndex < transform.childCount)
        {
            Destroy(transform.GetChild(childIndex).gameObject);
            childIndex++;
        }

        int index = 0;
        foreach(EnemyData e in GameData.Enemies)
        {
            GameObject unit_GO = (GameObject)Instantiate(UnitPrefab, transform);
            unit_GO.GetComponent<RectTransform>().anchoredPosition = new Vector2((index * SpaceForEachUnit) + SpaceForEachUnit, -25f);
            unit_GO.GetComponent<Image>().sprite = GameSpritesLoader.Sprites[e.SpriteName];
            unit_GO.GetComponent<Button>().onClick.AddListener(delegate { waveUI.OnUnitClicked(e); });
            unit_GO.GetComponent<Button>().onClick.AddListener(delegate { SoundManager.current.PlaySound("ui_Soft"); });
            dataToTextDictionary.Add(e,unit_GO.GetComponentInChildren<TextMeshProUGUI>());

            index++;
        }

    
    }


    void OnUnitsCountChange()
    {
        
        // Update the texts for all of the units
        foreach(EnemyData e in waveUI.CountForEachEnemyData.Keys)
        {
            dataToTextDictionary[e].text = waveUI.CountForEachEnemyData[e].ToString();
        }
    }



	
}
