using System;
using System.Collections.Generic;

[Serializable]
public class HeroStats 
{
    private HeroType _type;

    public int hp;
    public int armor;

    public void Create(HeroType type)
    {
        switch (type)
        {
            case HeroType.Warrior:
                SetParams(1,1);
                break;
            case HeroType.Master:
                SetParams(0,5);
                break;
            case HeroType.Priest:
                SetParams(1,9);
                break;
        }
    }

    private void SetParams(int armor,int hP)
    {
        this.armor = armor;
        hp = hP;
    }
}

[Serializable]
public enum HeroType
{
    Warrior,
    Master,
    Priest
}
