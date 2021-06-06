using UnityEngine;

namespace NewScripts
{
    public abstract class PlayerSkill 
    {
        protected readonly Player Player;
        public float Duration;
        protected string Trigger;
        protected readonly PlayerAnimator Animator;
        public int ID;

        protected PlayerSkill(Player player,float duration)
        {
            Player = player;
            Duration = duration;
            Animator = player._playerAnimator;
        }

        public abstract void Activate();

        public abstract void Animate();

        public abstract void Move();
        
    }

    public interface IDealDamage
    {
        int DealDamage();
    }

    public class PlayerSlash : PlayerSkill, IDealDamage
    {
        private readonly int _damage;

        public PlayerSlash(Player player) : base(player, 1.85f)
        {
            _damage = 2;
            ID = 1;
        }

        public override void Activate()
        {
            Player.SetCurrentSkill(this);
            Animate();
            Move();
        }
        
        public override void Animate()
        {
            Animator.SlashAttack();
        }
        
        public override void Move(){}

        public int DealDamage()
        {
            return _damage;
        }
    }
    
    public class PlayerDirect : PlayerSkill, IDealDamage
    {
        private readonly int _damage;

        public PlayerDirect(Player player) : base(player, 2.33f)
        {
            _damage = 1;
            ID = 2;
        }

        public override void Activate()
        {
            Player.SetCurrentSkill(this);
            Animate();
            Move();
        }

        public override void Animate()
        {
            Animator.DirectAttack();
        }
        
        public override void Move(){}

        public int DealDamage()
        {
            return _damage;
        }
    }
}
