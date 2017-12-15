using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSpritesLoader : MonoBehaviour {


    public static Dictionary<string, Sprite> Sprites;

	void Awake ()
    {
        // If we already loaded the sprites then just return
        if (Sprites != null) return;
       
        Sprites = new Dictionary<string, Sprite>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Spritesheet");

        foreach(Sprite s in sprites)
        {
         
            Sprites.Add(s.name, s);
        }

	}
	

}
