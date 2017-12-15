using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeDisplay : MonoBehaviour
{
   
    // Will display the radius of the tower

    private Tower tower;
    private SpriteRenderer sr;
    private void Awake()
    {
        tower = transform.parent.GetComponent<Tower>();
        tower.ActionTowerInformationChanged += OnRangeChanged;
        if (tower == null)
            Debug.LogError("RangeDisplay couldn't find Tower component in parent!");

        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;

    }

    private void OnRangeChanged()
    {

        transform.localScale = new Vector3(tower.Range * 2, tower.Range * 2, 0);
    }

    private void OnDestroy()
    {
        tower.ActionTowerInformationChanged -= OnRangeChanged;
    }

    public void DisplayRange()
    {
        sr.enabled = true;
   
    }

    public void HideRange()
    {
        sr.enabled = false;
    }

}
