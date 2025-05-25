using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    public float InteractionDistance = 2f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        DrawCircle(transform.position, InteractionDistance, 32);
    }

    public abstract void Interact(GameObject interactor, ItemTool toolUsed = null);

    public bool CheckInteractionDistance(GameObject interactor)
    {
        var a = new Vector2(transform.position.x, transform.position.z);
        var b = new Vector2(interactor.transform.position.x, interactor.transform.position.z);
        var distance = Vector2.Distance(a, b);
        return distance <= InteractionDistance;
    }

    private void DrawCircle(Vector3 center, float radius, int segments)
    {
        var previousPoint = center + new Vector3(radius, 0f, 0f);
        var angleStep = 360f / segments;

        for (var i = 1; i <= segments; i++)
        {
            var angle = i * angleStep * Mathf.Deg2Rad;
            var nextPoint = center + new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius);
            Gizmos.DrawLine(previousPoint, nextPoint);
            previousPoint = nextPoint;
        }
    }
}