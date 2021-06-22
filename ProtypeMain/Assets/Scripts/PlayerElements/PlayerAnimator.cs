using System.Collections;
using UnityEngine;

namespace PlayerElements
{
    public class PlayerAnimator : MonoBehaviour
    {
        private Animator _animator;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void PlayMoveAnimationByInputVector(Vector3 input)
        {
            float velocityY=Vector3.Dot(input.normalized,transform.forward);
            float velocityX=Vector3.Dot(input.normalized,transform.right);
            _animator.SetFloat("Y",-velocityX,0.1f,Time.deltaTime);
            _animator.SetFloat("X",velocityY,0.1f,Time.deltaTime);
        }
    
        public void PlayAnimationBySkill(PlayerSkill skill)
        {
            _animator.SetTrigger(skill.trigger);
        }
        
        #region Finite States
        public void Die()
        {
            _animator.SetTrigger("Die");
        }

        public void React()
        {
            _animator.SetTrigger("React");
        }
        
        public void Win()
        {
            _animator.SetTrigger("Win");
        }
        #endregion
        
    }
}
