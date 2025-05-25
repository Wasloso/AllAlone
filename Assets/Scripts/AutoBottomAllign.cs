using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public class AutoBottomAlignSprite : MonoBehaviour
{
    [Tooltip("If checked, the script will run in the editor whenever a sprite is assigned or changed.")]
    public bool updateInEditor = true;

    private void Awake()
    {
        AlignSpriteToBottom();
    }

    private void Start()
    {
        AlignSpriteToBottom(); // Safety net
    }

    private void OnEnable()
    {
        AlignSpriteToBottom(); // Ensure it runs on spawn too
    }

    private void OnValidate()
    {
        if (updateInEditor)
            AlignSpriteToBottom();
    }

    public void AlignSpriteToBottom()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null || spriteRenderer.sprite == null) return;

        var localSpriteBottomY = spriteRenderer.sprite.bounds.min.y;
        var pos = transform.localPosition;

        transform.localPosition = new Vector3(
            pos.x,
            -localSpriteBottomY,
            pos.z
        );
    }
}