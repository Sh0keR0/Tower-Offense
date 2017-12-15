using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class FloatingText : MonoBehaviour
{


    public static FloatingText current;

    [SerializeField]
    private GameObject floatingTextingPrefab;


    private void Awake()
    {
        if (current == null)
            current = this;
        else
        {
            Debug.LogError("FloatingText - there is already an instance of FloatingText in the game!");
            return;
        }

    }


    public void ShowText(string text, Color color, Vector2 worldPosition, float lifeTime, bool fadeEffect = true)
    {

        GameObject go_text = (GameObject)Instantiate(floatingTextingPrefab, worldPosition, Quaternion.identity, gameObject.transform);
        go_text.GetComponent<FloatTextDisplayScript>().InitializeText(text, color, lifeTime, fadeEffect);

    }



}
