// Player.cs (or PlayerController.cs, PlayerManager.cs)

using System;
using System.Collections.Generic;
using System.Linq;
using Items;
using Player.States;
using UnityEngine;

namespace Player // Keep your namespaces consistent!
{
    public class Player : MonoBehaviour, IDamageable, IConsumableReciever, IDataPersistence
    {
        [Header("Player Systems")] public PlayerMovement _playerMovement;

        [SerializeField] private HealthSystem _healthSystem;
        [SerializeField] private HungerSystem _hungerSystem;
        [SerializeField] private Animator animator;
        public PlayerInteractions playerInteractions;
        public PlayerInventory _playerInventory;
        private PlayerStat _attackStats;
        private PlayerStat _defenseStats;
        private Item _lastEquippedChestItem;
        private Item _lastEquippedHandItem;
        private Item _lastEquippedHeadItem;

        private PlayerStat _movementStats;

        private StateMachine _stateMachine;

        public float AttackDamage => _attackStats.Value;

        [Header("Player Stats")] private ResourceStat HealthStats => _healthSystem.Health;

        private ResourceStat HungerStats => _hungerSystem.Hunger;
        public bool CanBeAttacked { get; }
        public float speed => _playerMovement.moveSpeed;


        private void Awake()
        {
            InitializeComponents();
            _stateMachine = new StateMachine();

            var idle = new IdleState(this, animator);
            var walk = new WalkState(this, animator);
            var moveTo = new WalkTowardsTargetState(this, animator);
            var attack = new AttackState(this, animator);
            var interact = new InteractState(this, animator);
            var death = new DeathState(this, animator, null);
            _healthSystem.OnDied += () =>
            {
                _stateMachine.SetState(death);
                _stateMachine.ClearTransitions();
            };

            _stateMachine.AddTransition(walk, idle, () => _playerMovement.MovementInput.magnitude < 0.1f);


            _stateMachine.AddTransition(moveTo, idle,
                () => !playerInteractions.Target);

            _stateMachine.AddAnyTransition(attack, () =>
                playerInteractions.Target &&
                playerInteractions.Target.TryGetComponent<IDamageable>(out _) &&
                Vector3.Distance(transform.position, playerInteractions.Target.transform.position) <=
                playerInteractions.interactRadius * 0.9f);
            _stateMachine.AddTransition(attack, idle, () => !playerInteractions.Target);
            _stateMachine.AddAnyTransition(moveTo,
                () => playerInteractions.Target &&
                      Vector3.Distance(transform.position, playerInteractions.Target.transform.position) >=
                      playerInteractions.interactRadius && _playerMovement.MovementInput.magnitude < 0.1f
            );
            _stateMachine.AddAnyTransition(interact, () =>
                playerInteractions.Target &&
                !playerInteractions.Target.TryGetComponent<IDamageable>(out _) &&
                Vector3.Distance(transform.position, playerInteractions.Target.transform.position) <=
                playerInteractions.interactRadius);
            _stateMachine.AddTransition(interact, idle, () => !playerInteractions.Target);
            _stateMachine.AddAnyTransition(walk,
                () => _playerMovement.MovementInput.magnitude > 0.1f);
            _stateMachine.SetState(idle);
        }

        private void Update()
        {
            _stateMachine.Tick();
        }

        public void Consume(ItemConsumable consumable)
        {
            _healthSystem.Consume(consumable);
            _hungerSystem.Consume(consumable);
        }

        public Transform Transform => transform;
        public Faction Faction => Faction.Player;
        public bool IsAlive => _healthSystem.Health.Value > 0;

        public void TakeDamage(float amount)
        {
            _healthSystem?.TakeDamage(amount);
        }

        public void LoadData(GameData data)
        {
            _healthSystem.Health.SetCurrent(data.playerData.health);
            _hungerSystem.Hunger.SetCurrent(data.playerData.hunger);
            var items = Resources.LoadAll<Item>("Items");

            foreach (var item in data.playerData.items)
            {
                var itemToLoad = items.FirstOrDefault(i => i.id == item.id);
                if (itemToLoad != null)
                {
                    Debug.Log("Item: " + item.count);
                    _playerInventory.AddItem(itemToLoad, item.count);
                }
            }
        }

        public void SaveData(ref GameData data)
        {
            data.playerData = toPlayerData();
        }


        private void InitializeComponents()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            _healthSystem = GetComponent<HealthSystem>();
            _hungerSystem = GetComponent<HungerSystem>();
            _movementStats = new PlayerStat(_playerMovement.moveSpeed);
            _attackStats = new PlayerStat(2);
            _defenseStats = new PlayerStat(0);
            _playerInventory = GetComponent<PlayerInventory>();
            playerInteractions = GetComponent<PlayerInteractions>();

            _healthSystem?.Initialize(100);
            _hungerSystem?.Initialize(150);
            if (_playerInventory != null)
            {
                _playerInventory.OnHandSlotChanged += OnEquippedItemChanged;
                _playerInventory.OnChestSlotChanged += OnEquippedItemChanged;
                _playerInventory.OnHeadSlotChanged += OnEquippedItemChanged;
            }
        }


        public PlayerStat GetStat(StatType statType)
        {
            return statType switch
            {
                StatType.Health => HealthStats,
                StatType.Hunger => HungerStats,
                StatType.Attack => _attackStats,
                StatType.Defense => _defenseStats,
                StatType.Movement => _movementStats,
                _ => null
            };
        }


        private void At(IState from, IState to, Func<bool> condition)
        {
            _stateMachine.AddTransition(from, to, condition);
        }

        private void OnEquippedItemChanged(SlotTag slot, Item newItem)
        {
            // Determine previous item by slot
            var previousItem = slot switch
            {
                SlotTag.Hand => _lastEquippedHandItem,
                SlotTag.Head => _lastEquippedHeadItem,
                SlotTag.Chest => _lastEquippedChestItem,
                _ => null
            };

            // Remove modifiers from previous item
            if (previousItem != null)
                RemoveAllModifiersFromItem(previousItem);

            // Add modifiers from new item
            if (newItem is ItemWeapon weapon)
                AddModifiersFromItem(weapon);
            else if (newItem is ItemTool tool)
                AddModifiersFromItem(tool);
            else if (newItem != null)
                AddModifiersFromItem(newItem);

            // Update reference
            switch (slot)
            {
                case SlotTag.Hand: _lastEquippedHandItem = newItem; break;
                case SlotTag.Head: _lastEquippedHeadItem = newItem; break;
                case SlotTag.Chest: _lastEquippedChestItem = newItem; break;
            }
        }

        private void AddModifiersFromItem(Item item)
        {
            if (item is not IStatModProvider modProvider) return;
            foreach (var mod in modProvider.CompiledModifiers)
            {
                var stat = GetStat(mod.AffectedStat);
                stat?.AddModifier(mod);
            }
        }

        private void RemoveAllModifiersFromItem(Item item)
        {
            if (item is not IStatModProvider modProvider) return;
            foreach (var mod in modProvider.CompiledModifiers)
            {
                var stat = GetStat(mod.AffectedStat);
                stat?.RemoveAllModifiersFromSource(item); // assumes mod.Source == item
            }
        }

        public PlayerData toPlayerData()
        {
            var items = new List<ItemData>();
            foreach (var slot in _playerInventory.inventorySlots)
                if (slot?._inventoryItem?.item != null)
                    items.Add(new ItemData(slot._inventoryItem.item.id, slot._inventoryItem.count));

            return new PlayerData(_healthSystem.CurrentHealth, _hungerSystem.CurrentHunger, items);
        }

        public void DisableComponents()
        {
            _playerMovement.enabled = false;
            playerInteractions.enabled = false;
        }
    }
}