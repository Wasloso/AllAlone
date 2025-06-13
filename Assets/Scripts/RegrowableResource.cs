using System;
using System.Collections.Generic;
using System.Collections;
using Items;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class RegrowableResource : WorldObject
    {

        [SerializeField] private float regrowTime = 30f;
        [SerializeField] private Sprite growingSprite;
        [SerializeField] private Sprite grownSprite;
        private SpriteRenderer SpriteRenderer;
        private bool isReady = true;

        public void Start()
        {
            SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            SpriteRenderer.sprite = grownSprite;
        }

        public override bool CanInteract(GameObject interactor, Item itemUsed = null)
        {
            return isReady;
        }

        public override void Interact(GameObject interactor, Item itemUsed = null)
        {
            Harvest();
        }

        public event Action OnHarvest;

        protected virtual void Harvest()
        {
            Debug.Log($"{gameObject.name} harvested!");
            OnHarvest?.Invoke();

            foreach (var dropEntry in itemDrops)
            {
                var quantity = Random.Range(dropEntry.minQuantity, dropEntry.maxQuantity);
                DropItem(dropEntry.item, transform.position, quantity);
            }
            isReady = false;
            SpriteRenderer.sprite = growingSprite;
            StartCoroutine(RegrowCoroutine());
        }

        private IEnumerator RegrowCoroutine()
        {
            yield return new WaitForSeconds(regrowTime);
            isReady = true;
            SpriteRenderer.sprite = grownSprite;
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