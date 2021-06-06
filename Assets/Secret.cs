using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class Secret : MonoBehaviour
{
    private TilemapRenderer tileRenderer;

    private int trigger = 1;

  
    // Start is called before the first frame update
    void Start()
    {
        tileRenderer = GetComponent<TilemapRenderer>();

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        

        if (collision.CompareTag("Player"))
          tileRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        
       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            tileRenderer.maskInteraction = SpriteMaskInteraction.None;
    }


}
