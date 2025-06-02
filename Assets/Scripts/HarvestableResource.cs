using System;
using System.Collections.Generic;
using Items;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class HarvestableResource : MonoBehaviour, IInteractable
    {
        [SerializeField] private InteractionAnimationType interactionAnimationType = InteractionAnimationType.Attack;
        public GameObject droppedItemPrefab;
        public ToolType requiredToolType = ToolType.None;

        public float currentHealth = 10f;

        // TODO: Item drops
        public List<ItemDropEntry> itemDrops;


        public InteractionAnimationType AnimationType => interactionAnimationType;

        public bool CanInteract(GameObject interactor, Item itemUsed = null)
        {
            if (requiredToolType == ToolType.None) return true;
            if (itemUsed is ItemTool tool && tool.toolType == requiredToolType) return true;
            return false;
        }

        public void Interact(GameObject interactor, Item itemUsed = null)
        {
            if (requiredToolType == ToolType.None)
            {
                Harvest();
                return;
            }

            if (!itemUsed)
            {
                Debug.Log($"Cannot harvest {gameObject.name}. Requires a {requiredToolType}, but nothing is equipped.");
                return;
            }

            if (itemUsed is ItemTool tool && tool.toolType == requiredToolType)
                TakeDamage(tool.effectiveness);
            else
                Debug.Log(
                    $"Cannot harvest {gameObject.name}. Requires a {requiredToolType}, but used {itemUsed.title} (Type: {itemUsed.itemType}).");
        }

        public event Action OnHarvest;

        public virtual void TakeDamage(float damageAmount)
        {
            currentHealth -= damageAmount;
            currentHealth = Mathf.Max(currentHealth, 0f);

            Debug.Log($"{gameObject.name} took {damageAmount} damage. Remaining health: {currentHealth}");

            if (currentHealth <= 0) Harvest();
            // Optional: Play hit animation, sound, or particle effect
        }

        protected virtual void Harvest()
        {
            Debug.Log($"{gameObject.name} harvested!");
            OnHarvest?.Invoke();

            // TODO: Item drops
            foreach (var dropEntry in itemDrops)
            {
                var quantity = Random.Range(dropEntry.minQuantity, dropEntry.maxQuantity);
                DropItem(dropEntry.item, transform.position, quantity);
            }

            Destroy(gameObject);
        }


        private void DropItem(Item item, Vector3 position, int quantity)
        {
            for (var i = 0; i < quantity; i++)
            {
                var randomOffset = Random.insideUnitCircle * 0.3f;
                var spawnPosition = position + new Vector3(randomOffset.x, 0f, randomOffset.y);
                var drop = Instantiate(droppedItemPrefab, spawnPosition, Quaternion.identity);
                var dropped = drop.GetComponent<DroppedItem>();
                if (dropped != null)
                    dropped.Initialize(item, 1);
                else
                    Debug.Log($"{gameObject.name} dropped item {item} is not a dropped item.");
            }
        }
    }
}