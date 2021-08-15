using UnityEngine.Tilemaps;
using UnityEngine;

public class Secret : MonoBehaviour
{
    [SerializeField] private TilemapRenderer tilemapRenderer;

    private void Start()
    {
        var tilemapRenderer = this.GetComponent<TilemapRenderer>();
    }

    //TODO make enemies to see player in secrets

    private void OnTriggerEnter2D(Component collision)
    {
        if (collision.CompareTag("Player"))
        {
            tilemapRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }
    }

    private void OnTriggerExit2D(Component collision)
    {
        if (collision.CompareTag("Player"))
        {
            tilemapRenderer.maskInteraction = SpriteMaskInteraction.None;
        }
    }
}
