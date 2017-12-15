using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatTextDisplayScript : MonoBehaviour {

    private bool fade;
    private float TotalLifeTime = 1f;
    private float currentLifeTime;
    private TextMeshPro meshText;
    private bool init = false;
	
	void Awake () {
        meshText = GetComponent<TextMeshPro>();

        currentLifeTime = TotalLifeTime;
	}
	

	
	void Update () {

        if (init == false) return;


        float lifeTimePercentage =  currentLifeTime / TotalLifeTime ;

        if(fade)
          meshText.color = new Color(meshText.color.r, meshText.color.g, meshText.color.b, 1f * lifeTimePercentage);

        currentLifeTime -= Time.deltaTime;
        if(currentLifeTime <= 0)
        {
            Destroy(gameObject);
        }
		
	}


    public void InitializeText(string text, Color color, float lifeTime, bool fadeEffect)
    {
        if (init) return;

        meshText.text = text;
        meshText.color = color;
        TotalLifeTime = lifeTime;
        fade = fadeEffect;
        init = true;

    }
}
