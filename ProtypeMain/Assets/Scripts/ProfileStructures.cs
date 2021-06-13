using System;

[Serializable]
public class HeroStats
{
    public int hp;
    public int armor;
    public int damage;

    public HeroStats(HeroType type)
    {
        switch (type)
        {
            case HeroType.Warrior:
                SetParams(1,1,2);
                break;
            case HeroType.Master:
                SetParams(1,0,3);
                break;
            case HeroType.Priest:
                SetParams(3,1,1);
                break;
        }
    }
    
    private void SetParams(int hp,int armor,int damage)
    {
        this.hp = hp;
        this.armor = armor;
        this.damage = damage;
    }
}

public enum HeroType
{
    Warrior = 1,
    Master = 2,
    Priest = 3
}

[Serializable]
public struct HuntLevelInfo
{
    public int locationLevel1;
    public int locationLevel2;
    public int locationLevel3;
}

public struct SaveData
{
    public HeroStats HeroStats;
    public HuntLevelInfo HuntLevelInfo;
}
