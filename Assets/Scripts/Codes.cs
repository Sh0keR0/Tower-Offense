using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Codes : MonoBehaviour {


    private Dictionary<string, int> cheatCodes = new Dictionary<string, int>();

	// Use this for initialization
	void Start () {

        cheatCodes.Add("pizza", 0);
        cheatCodes.Add("snorlaxusedrest", 0);
		
	}
	
	// Update is called once per frame
	void Update ()
    {

        List<string> buffer = new List<string>(cheatCodes.Keys);
        foreach(string c in buffer)
        {
           
            if(Input.GetKeyDown(c[cheatCodes[c]].ToString().ToLower()))
            {
                cheatCodes[c]++;
                if (cheatCodes[c] == c.Length)
                {
                    CallCheatCode(c);
                    cheatCodes[c] = 0;
                }
               
            }else if(Input.anyKeyDown)
            {
                cheatCodes[c] = 0;
            } 

        }

	}


    private void CallCheatCode(string code)
    {
        if(code == "pizza")
        {
            GameController.instance.GainMoney(999999);
        }

        if(code == "snorlaxusedrest")
        {
            GameController.instance.LifePoints = 99999;
        }

        
    }
}
