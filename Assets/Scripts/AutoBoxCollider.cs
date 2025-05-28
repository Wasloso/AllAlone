using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AutoBoxCollider : MonoBehaviour
{
    private void Reset()
    {
        FitColliderToSprite();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        FitColliderToSprite();
    }
#endif

    private void FitColliderToSprite()
    {
        Debug.Log($"[{gameObject.name}] Searching for SpriteRenderer in children...");

        var rend = GetComponentInChildren<SpriteRenderer>();
        if (rend == null)
        {
            Debug.LogWarning($"[{gameObject.name}] No SpriteRenderer found in children!");
            return;
        }

        if (rend.sprite == null)
        {
            Debug.LogWarning($"[{gameObject.name}] SpriteRenderer found but sprite is null!");
            return;
        }

        Debug.Log(
            $"[{gameObject.name}] Found SpriteRenderer on {rend.gameObject.name}, sprite size: {rend.sprite.bounds.size}");

        var col = GetComponent<BoxCollider>();
        if (col == null)
        {
            Debug.LogWarning($"[{gameObject.name}] No BoxCollider found!");
            return;
        }

        var spriteBounds = rend.sprite.bounds;
        var scaledSize = Vector3.Scale(spriteBounds.size, rend.transform.lossyScale);

        col.size = scaledSize;

        var worldCenter = rend.transform.TransformPoint(spriteBounds.center);
        col.center = transform.InverseTransformPoint(worldCenter);

    }

    public void Refresh()
    {
        FitColliderToSprite();
    }
}