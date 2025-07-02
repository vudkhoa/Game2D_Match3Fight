namespace View.Player
{
    using UnityEngine;

    public class PlayerView : MonoBehaviour
    {
        private Animator _animator;

        void Awake()
        {
            this._animator = this.GetComponent<Animator>();
        }

        private void PlayAnimationShooting()
        {
            if (this._animator != null)
            {
                this._animator.Play("Shooting");
            }
        }

        public void SetState(int state)
        {
            if (state == 0)
            {
                return;
            }

            if (state == 1)
            {
                this.PlayAnimationShooting();
            }
        }
    }
}