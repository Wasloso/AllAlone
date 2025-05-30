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
        [SerializeField] public float maxBoredTime = 30f;
        public float boredTimer;
        public bool updateBoredTimer = true;
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
    }
}