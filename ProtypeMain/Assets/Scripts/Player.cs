using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace NewScripts
{
    public class Player : MonoBehaviourPunCallbacks,IPunObservable
    {
        private PlayerInput _playerInput;
        public  PlayerAnimator _playerAnimator;
        private PlayerMoving _playerMoving;
        private PlayerTrigger _playerTrigger;
        private IDealDamage _currentSkill;

        public PlayerSkill firstSkill;
        public PlayerSkill secondSkill;
        
        public bool canMove = true;
        public bool isWin;
        public bool IsDead { get; private set; }

        private void Awake()
        {
            firstSkill=new PlayerDirect(this);
            secondSkill=new PlayerSlash(this);
            
            _playerInput = GameObject.Find("InputConnect").GetComponent<PlayerInput>();
            _playerAnimator = GetComponent<PlayerAnimator>();
            _playerTrigger = GetComponent<PlayerTrigger>();
            _playerTrigger.SetPlayer(this);
            
            _playerMoving = new PlayerMoving(_playerInput.joystick,
                GetComponent<CharacterController>());
            
            //Button Events dont need to control for enemy Object
            if (!photonView.IsMine)
                return;
            
            _playerInput.AddAttackingSkill_1_Listener(FirstAttack);
            _playerInput.AddAttackingSkill_2_Listener(SecondAttack);
            _playerInput.AddDefendingSkillListener(Roll);
        }

        private void Update()
        {
            //Contol only our Object
            if (!photonView.IsMine)
                return;

            if(IsFalling())
            {
                IsDead = true;
            }
            
            if(PlayerHasSomeBlock())
                return;
            
            _playerMoving.MoveCharacter(canMove);
            _playerAnimator.PlayMoveAnimation(_playerMoving.GetInput());
            
            if (GameRules.Enemy == null)
                return;
            transform.LookAt(GameRules.Enemy.transform);
        }

        #region Skills
        
        public void SetCurrentSkill(PlayerSkill skill)
        {
            _currentSkill = skill as IDealDamage;
        }
        
        private void Roll()
        {
            if (PlayerHasSomeBlock())
                return;
            
            _playerAnimator.Roll(_playerMoving.GetInput());
            StartCoroutine(BlockMovingForTime(2f));
        }

        private void FirstAttack()
        {
            photonView.RPC("ActivateAttackSkill",RpcTarget.All,firstSkill.ID.ToString());
        }

        private void SecondAttack()
        {
            photonView.RPC("ActivateAttackSkill",RpcTarget.All,secondSkill.ID.ToString());
        }

        #region RCP methods

        [PunRPC]
        private void ActivateAttackSkill(string skillCode)
        {
            if (PlayerHasSomeBlock())
                return;
            
            PlayerSkill skill;
            
            switch (skillCode)
            {
             case "1":
                 skill=new PlayerSlash(this);
                 break;
             case "2":
                 skill=new PlayerDirect(this);
                break;
                 
             default:
                 Debug.Log("Wrong skill code");
                 return;
            }
            Debug.Log("Skill work!");
            
            skill?.Activate();
            
            StartCoroutine(BlockMovingForTime(skill.Duration));
            StartCoroutine(ResetSkillForTime(skill.Duration));
        }
        
        #endregion
        
        public int GetDamage()
        {
            Debug.Log($"Damage:{_currentSkill?.DealDamage()}");
            return _currentSkill == null ? 0 : _currentSkill.DealDamage();
        }
        
        private bool PlayerHasSomeBlock()
        {
            return !canMove || IsDead || isWin;
        }
        
        
        #endregion

        
        #region Enumerators

        IEnumerator ResetSkillForTime(float duration)
        {
            
            yield return new WaitForSeconds(duration);
            SetCurrentSkill(null);
        }
        
        IEnumerator BlockMovingForTime(float time)
        {
            canMove = false;
            yield return new WaitForSeconds(time);
            canMove = true;
        }
        

        #endregion

        
        #region Finite States

        public void Kill()
        {
            IsDead = true;
            _playerMoving.TurnOffCharacterController();
            _playerAnimator.Die();
        }
        
        public void React()
        {
            _playerAnimator.React();
        }

        [PunRPC]
        public void Win()
        {
            isWin = true;
            canMove = false;

        _playerAnimator.Win();
        }

        private bool IsFalling()
        {
            return transform.position.y < -2f;
        }
        
        #endregion

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(_playerAnimator.sword.activeSelf);
                stream.SendNext(IsDead);
                stream.SendNext(canMove);
            }
            else
            {
                var isActive = (bool) stream.ReceiveNext();
                IsDead = (bool) stream.ReceiveNext();
                canMove = (bool) stream.ReceiveNext();
                
                
                _playerAnimator.sword.SetActive(isActive);
            }
        }

        public override void OnLeftRoom()
        {
            Destroy(this);
        }

    }
}
