using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveControlWaveOrderUI : MonoBehaviour
{

    public float SpaceForEachUnit = 100f;
    public GameObject UnitPrefab;

    private RectTransform rect;

    private int unitIndex = 0;


    private void Awake()
    {
        rect = GetComponent<RectTransform>();

    }


    public void AddUnitToDisplay(EnemyData e, int amount)
    {

        unitIndex++;

        RecalculateWidthSize();

     
        GameObject unit_GO = (GameObject)Instantiate(UnitPrefab, transform);
        unit_GO.GetComponent<RectTransform>().anchoredPosition = new Vector2((unitIndex * SpaceForEachUnit), -25f);
        unit_GO.GetComponent<Image>().sprite = GameSpritesLoader.Sprites[e.SpriteName];
        unit_GO.GetComponentInChildren<TextMeshProUGUI>().text = amount.ToString();
    

    }

    public void ResetDisplay()
    {
        int childIndex = 0;
        while (childIndex < transform.childCount)
        {
            Destroy(transform.GetChild(childIndex).gameObject);
            childIndex++;
        }

        unitIndex = 0;
        RecalculateWidthSize();

    }

    private void RecalculateWidthSize()
    {
        rect.sizeDelta = new Vector2(SpaceForEachUnit + (unitIndex * SpaceForEachUnit), rect.sizeDelta.y);
    }
}
