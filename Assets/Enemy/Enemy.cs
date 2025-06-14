using System;
using System.Collections.Generic;
using Items;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Enemy
{
    public abstract class Enemy : MonoBehaviour, IInteractable, IDamageable
    {
        [SerializeField] protected Animator animator;
        [SerializeField] public float speed;
        public Vector3 MoveTarget;
        [SerializeField] public float maxBoredTime = 30f;
        public float boredTimer;
        public bool updateBoredTimer = true;
        public List<ItemDropEntry> itemDrops;
        [SerializeField] protected float maxHealth = 10f;

        [SerializeField] public float attackRange = 0.5f;
        [SerializeField] protected float detectionRange = 5.0f;
        [SerializeField] protected LayerMask targetLayer;
        [SerializeField] public float attackDamage = 10.0f;
        [SerializeField] private Object droppedItemPrefab;
        protected HealthSystem _healthSystem;

        protected StateMachine StateMachine;
        public IDamageable CurrentTarget { get; private set; }


        protected void Awake()
        {
            targetLayer = LayerMask.GetMask("Player");
            _healthSystem = gameObject.GetComponent<HealthSystem>();
            _healthSystem.Initialize(maxHealth);
        }


        private void Update()
        {
            StateMachine.Tick();
        }

        public Transform Transform => transform;
        public Faction Faction => Faction.Enemy;
        public bool IsAlive => _healthSystem.Health.Value > 0;


        void IDamageable.TakeDamage(float amount)
        {
            TakeDamage(amount);
        }

        public InteractionAnimationType AnimationType => InteractionAnimationType.Attack;

        public bool CanInteract(GameObject interactor, Item itemUsed = null)
        {
            return true;
        }

        public void Interact(GameObject interactor, Item itemUsed = null)
        {
            Debug.Log($"Interact with {gameObject.name}");
            if (interactor.TryGetComponent<Player.Player>(out var player))
            {
                Debug.Log($"Player: {player.name} attacks {gameObject.name}");
                var damage = player.AttackDamage;

                TakeDamage(damage);
            }
        }

        protected void At(IState from, IState to, Func<bool> condition)
        {
            StateMachine.AddTransition(from, to, condition);
        }

        public Vector3 FindNewTarget()
        {
            var randomCircle = Random.insideUnitCircle * 10;
            var newPoint = new Vector3(randomCircle.x, 0f, randomCircle.y);
            MoveTarget = newPoint;
            return transform.position + newPoint;
        }

        public void ModifyBoredTimer(bool stop = false, bool start = false, bool reset = false)
        {
            if (reset) boredTimer = Math.Min(Random.value * maxBoredTime, maxBoredTime / 2);
            if (stop) updateBoredTimer = false;
            if (start) updateBoredTimer = true;
        }

        protected void TakeDamage(float damage)
        {
            _healthSystem.TakeDamage(damage);
        }

        protected void Die()
        {
            foreach (var dropEntry in itemDrops)
            {
                var quantity = Random.Range(dropEntry.minQuantity, dropEntry.maxQuantity);
                DropItem(dropEntry.item, transform.position, quantity);
            }

            Destroy(gameObject);
        }

        public bool IsTargetInRange()
        {
            if (CurrentTarget == null) return false;

            var distance = Vector3.Distance(transform.position, CurrentTarget.Transform.position);

            return distance <= attackRange;
        }

        public bool CheckForTarget(float multiplier = 1)
        {
            var hits = Physics.OverlapSphere(transform.position, detectionRange * multiplier, targetLayer);

            IDamageable closestTarget = null;
            var closestDistance = Mathf.Infinity;

            foreach (var hit in hits)
                if (hit.TryGetComponent<IDamageable>(out var damageable) && damageable.IsAlive &&
                    damageable.Faction != Faction)
                {
                    var distance = Vector3.Distance(transform.position, damageable.Transform.position);

                    if (damageable == CurrentTarget) return true;
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestTarget = damageable;
                    }
                }

            CurrentTarget = closestTarget;
            return closestTarget != null;
        }

        private void DropLoot()
        {
            foreach (var dropEntry in itemDrops)
            {
                var quantity = Random.Range(dropEntry.minQuantity, dropEntry.maxQuantity);
                DropItem(dropEntry.item, transform.position, quantity);
            }
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