using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AutoBottomAlignSprite : MonoBehaviour
{
    [Tooltip("If checked, the script will run in the editor whenever a sprite is assigned or changed.")]
    public bool updateInEditor = true;


    private void Awake()
    {
        AlignSpriteToBottom();
    }

    private void OnValidate()
    {
        if (updateInEditor && Application.isEditor && !Application.isPlaying) AlignSpriteToBottom();
    }

    private void AlignSpriteToBottom()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogWarning("AutoBottomAlignSprite: No SpriteRenderer found on this GameObject. Cannot align.", this);
            return;
        }

        if (spriteRenderer.sprite == null) return;

        var localSpriteBottomY = spriteRenderer.sprite.bounds.min.y;

        transform.localPosition = new Vector3(
            transform.localPosition.x,
            -localSpriteBottomY,
            transform.localPosition.z
        );
    }
}