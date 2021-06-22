using System.Collections;
using PlayerElements;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public int id;
    public string trigger;
    public float duration;

    protected Player Player;
    protected PlayerAnimator Animator;

    private void Awake()
    {
        Player = gameObject.GetComponent<Player>();
        Animator = Player.GetComponent<PlayerAnimator>();
    }
    
    public virtual void Activate(){}

    protected virtual void Animate()
    {
        Animator.PlayAnimationBySkill(this);
    }

    protected virtual void Move(){}
}

public interface IDealDamage
{
    int GetDamage();
}

public class BlockedSkill : PlayerSkill
{ 
    
    public override void Activate()
    {
        if(Player.PlayerHasSomeBlock())
            return;
        Animate();
        Move();
        StartCoroutine(Player.BlockPlayerMovingForTime(duration));
    }
    protected override void Animate()
    {
        Animator.PlayAnimationBySkill(this);
    }
}
    
public class AttackSkill : BlockedSkill, IDealDamage
{
    protected int Damage;
        
    public override void Activate()
    {
        if(Player.PlayerHasSomeBlock())
            return;
        
        Animate();
        Move();

        StartCoroutine(Player.BlockPlayerMovingForTime(duration));
        StartCoroutine(Player.SetPlayerSkillForTime(this));
    }
    
    public int GetDamage()
    {
        return Damage;
    }
    
}

//SubSkills
public class PlayerSlash : AttackSkill
{
    private void Start()
    {
        trigger = "SlashAttack";
        Damage = 2;
        id = 1;
        duration = 1.68f;
    }
}

public class PlayerDirect : AttackSkill
{
    private void Start()
    {
        trigger = "DirectAttack";
        Damage = 1;
        id = 2;
        duration = 1.807f;
    }
}

public class DefendSkill : BlockedSkill
{
    private void Start()
    {
        trigger = "Roll";
        duration = 1.667f;
    }

    protected override void Move()
    {
        Player.LookAtInputDirection();
    }
}
