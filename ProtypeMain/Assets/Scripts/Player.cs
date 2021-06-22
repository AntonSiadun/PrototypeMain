using System.Collections;
using Photon.Pun;
using UnityEngine;
using PlayerElements;

public class Player : MonoBehaviourPunCallbacks,IPunObservable
{
    protected  PlayerAnimator _playerAnimator;
    protected PlayerMoving _playerMoving;
    protected PlayerInput _playerInput;
    protected bool _canMove;
    protected bool _isWin;

    [SerializeField] private Collider Sword;
    public bool invincible;
    public AttackSkill currentSkill;
    public DefendSkill defendSkill;
    public AttackSkill firstSkill;
    public AttackSkill secondSkill;

    public bool IsDead;

    public virtual void Awake()
    {
        _canMove = true;
        invincible = false;
        
        _playerAnimator = GetComponent<PlayerAnimator>();
        _playerInput = GameObject.Find("InputConnect").GetComponent<PlayerInput>();
        _playerMoving = new PlayerMoving(_playerInput.joystick,
            GetComponent<CharacterController>());
        
        firstSkill = gameObject.AddComponent<PlayerDirect>();
        secondSkill = gameObject.AddComponent<PlayerSlash>();
        defendSkill = gameObject.AddComponent<DefendSkill>();
        currentSkill = null;

        //Button Events dont need to control for enemy Object
        if (!photonView.IsMine)
            return;
            
        _playerInput.AddAttackingSkill_1_Listener(FirstAttack);
        _playerInput.AddAttackingSkill_2_Listener(SecondAttack);
        _playerInput.AddDefendingSkillListener(Roll);
    }

    private void FixedUpdate()
    {
        //Control only our Object
        if (!photonView.IsMine)
            return;

        if(IsFalling())
        {
            IsDead = true;
        }
        if(PlayerHasSomeBlock())
            return;
            
        _playerMoving.MoveCharacterIfCanMove(_canMove);
        _playerAnimator.PlayMoveAnimationByInputVector(_playerMoving.GetInputVector());
        LookAtEnemy();
    }
    
    public IEnumerator BlockPlayerMovingForTime(float time)
    {
        _canMove = false;
        yield return new WaitForSeconds(time);
        _canMove = true;
    }
    
    public IEnumerator SetPlayerSkillForTime(AttackSkill skill)
    {
        SetCurrentSkill(skill);
        yield return new WaitForSeconds(skill.duration);
        SetCurrentSkill(skill);
    }

    private bool IsFalling()
    {
        return transform.position.y < -2f;
    }
    
    public bool PlayerHasSomeBlock()
    {
        return !_canMove || IsDead || _isWin;
    }
    
    private void LookAtEnemy()
    {
        if (GameRules.Enemy == null)
            return;
        
        var enemyTransform = GameRules.Enemy.transform;
        var position = enemyTransform.position;
        position = new Vector3(position.x, 0f,position.z);
        enemyTransform.position = position;
            
        transform.LookAt(enemyTransform);
    }

    public void LookAtInputDirection()
    {
        Vector3 positionOfPointInDirection = _playerMoving.GetInputVector() + transform.position;
        transform.LookAt(positionOfPointInDirection);
    }
    
    public int GetCurrentAttackSkillDamage()
    {
        Debug.Log($"Damage:{currentSkill?.GetDamage()}");
        return currentSkill == null ? 0 : currentSkill.GetDamage();
    }

    #region Skills

    private void SetCurrentSkill(AttackSkill skill)
    {
        currentSkill = skill;
    }
        
    protected void Roll()
    {
        defendSkill.Activate();
    }

    private void FirstAttack()
    {
        photonView.RPC("ActivateAttackSkill",RpcTarget.All, firstSkill.id.ToString());
    }

    private void SecondAttack()
    {
        photonView.RPC("ActivateAttackSkill", RpcTarget.All, secondSkill.id.ToString());
    }
    #endregion

    [PunRPC]
    private void ActivateAttackSkill(string skillCode)
    {
        PlayerSkill skill;
            
        switch (skillCode)
        {
            case "1":
                skill = secondSkill;
                break;
            case "2":
                skill = firstSkill;
                break;
                 
            default:
                Debug.Log("Wrong skill code");
                return;
        }
            
        skill.Activate();
    }

    #region Finite States
    
    public void Kill()
    {
        IsDead = true;
        Sword.enabled = false;
        _playerMoving.TurnOffCharacterController();
        _playerAnimator.Die();
    }
        
    public void React()
    {
        StopAllCoroutines();
        Sword.enabled = false;
        _playerAnimator.React();
    }
    
    

    [PunRPC]
    public void Win()
    {
        _isWin = true;
        _canMove = false;
        
        _playerAnimator.Win();
    }
    #endregion

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Sword.enabled);
            stream.SendNext(IsDead);
            stream.SendNext(_canMove);
        }
        else
        {
            var isActive = (bool) stream.ReceiveNext();
            IsDead = (bool) stream.ReceiveNext();
            _canMove = (bool) stream.ReceiveNext();
                
                
            Sword.enabled=isActive;
        }
    }

    public override void OnLeftRoom()
    {
        Destroy(this);
    }

    public void DisableSword()
    {
        Sword.enabled = false;
    }

    public void EnableSword()
    {
        Sword.enabled = true;
    }

    public virtual void OnInvincible()
    {
        invincible = true;
    }

    public virtual void OffInvincible()
    {
        invincible = false;
    }

}