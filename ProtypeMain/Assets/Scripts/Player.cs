using NewScripts;
using Photon.Pun;
using UnityEngine;

public class Player : MonoBehaviourPunCallbacks,IPunObservable
{
    private PlayerInput _playerInput;
    public  PlayerAnimator playerAnimator;
    public PlayerMoving playerMoving;
    private PlayerTrigger _playerTrigger;
    
    public AttackSkill _currentSkill;
    public DefendSkill defendSkill;
    public AttackSkill firstSkill;
    public AttackSkill secondSkill;
        
    public bool canMove = true;
    public bool isWin;
    public bool IsDead { get; private set; }

    private void Awake()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
            
        _playerInput = GameObject.Find("InputConnect").GetComponent<PlayerInput>();
            
        _playerTrigger = GetComponent<PlayerTrigger>();
        _playerTrigger.SetPlayer(this);
            
        playerMoving = new PlayerMoving(_playerInput.joystick,
            GetComponent<CharacterController>());
            
            
        //Skills initialization and setup
        firstSkill = gameObject.AddComponent<PlayerDirect>();
        firstSkill.Setup(this);
        secondSkill = gameObject.AddComponent<PlayerSlash>();
        secondSkill.Setup(this);
        _currentSkill = null;
        defendSkill = gameObject.AddComponent<DefendSkill>();
        defendSkill.Setup(this);

        //Button Events dont need to control for enemy Object
        if (!photonView.IsMine)
            return;
            
        _playerInput.AddAttackingSkill_1_Listener(FirstAttack);
        _playerInput.AddAttackingSkill_2_Listener(SecondAttack);
        _playerInput.AddDefendingSkillListener(Roll);
    }

    private void FixedUpdate()
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
            
        playerMoving.MoveCharacter(canMove);
        playerAnimator.PlayMoveAnimation(playerMoving.GetInput());
            
        if (GameRules.Enemy == null)
            return;
            
        LookAtEnemy(GameRules.Enemy.transform);
    }

    private void LookAtEnemy(Transform enemyTransform)
    {
        var position = enemyTransform.position;
        position = new Vector3(position.x,0f,position.z);
            
        enemyTransform.position = position;
            
        transform.LookAt(enemyTransform);
            
    }
        
    #region Skills
        
    public void SetCurrentSkill(AttackSkill skill)
    {
        _currentSkill = skill;
        if(_currentSkill == firstSkill)
            Debug.Log("FirstSkill Was Activated");
        if(_currentSkill == null)
            Debug.Log("Current skill empty");
    }
        
    private void Roll()
    {
        defendSkill.Activate();
    }

    private void FirstAttack()
    {
        photonView.RPC("ActivateAttackSkill",RpcTarget.All,firstSkill.id.ToString());
    }

    private void SecondAttack()
    {
        photonView.RPC("ActivateAttackSkill",RpcTarget.All,secondSkill.id.ToString());
    }

    #region RCP methods

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
        Debug.Log("Skill work!");
            
        skill.Activate();
    }
        
    #endregion
        
    public int GetDamage()
    {
        Debug.Log($"Damage:{_currentSkill?.DealDamage()}");
        return _currentSkill == null ? 0 : _currentSkill.DealDamage();
    }
        
    public bool PlayerHasSomeBlock()
    {
        return !canMove || IsDead || isWin;
    }
        
        
    #endregion
        
    #region Finite States

    public void Kill()
    {
        IsDead = true;
        playerMoving.TurnOffCharacterController();
        playerAnimator.Die();
    }
        
    public void React()
    {
        playerAnimator.React();
    }

    [PunRPC]
    public void Win()
    {
        isWin = true;
        canMove = false;

        playerAnimator.Win();
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
            stream.SendNext(playerAnimator.sword.activeSelf);
            stream.SendNext(IsDead);
            stream.SendNext(canMove);
        }
        else
        {
            var isActive = (bool) stream.ReceiveNext();
            IsDead = (bool) stream.ReceiveNext();
            canMove = (bool) stream.ReceiveNext();
                
                
            playerAnimator.sword.SetActive(isActive);
        }
    }

    public override void OnLeftRoom()
    {
        Destroy(this);
    }

}