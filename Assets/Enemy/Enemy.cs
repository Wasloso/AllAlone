using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    public abstract class Enemy : MonoBehaviour, IInteractable
    {
        [SerializeField] protected Animator animator;
        [SerializeField] public float speed;
        public Vector3 MoveTarget;
        protected StateMachine StateMachine;


        private void Awake()
        {
        }


        private void Update()
        {
            StateMachine.Tick();
        }

        public bool CanInteract(GameObject interactor)
        {
            return true;
        }

        public void Interact(GameObject interactor, ItemTool toolUsed = null)
        {
        }

        protected void At(IState from, IState to, Func<bool> condition)
        {
            StateMachine.AddTransition(from, to, condition);
        }

        protected Vector3 PickNewTarget()
        {
            var randomCircle = Random.insideUnitCircle * 10;
            var newPoint = new Vector3(randomCircle.x, 0f, randomCircle.y);
            return transform.position + newPoint;
        }
    }
}