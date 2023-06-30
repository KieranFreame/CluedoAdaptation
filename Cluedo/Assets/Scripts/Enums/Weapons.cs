using System.Collections.Generic;
using System.Linq;

public enum Weapon
{
    None,
    Poison,
    Candlestick,
    Dumbbell,
    Bat,
    Knife,
    Pistol,
    Trophy,
    Rope,
    Axe,
}

public static class Weapons
{
    private static readonly Dictionary<Evidence, Weapon> evidenceToWeapon = new()
    {
        { Evidence.None, Weapon.None },
        { Evidence.Poison, Weapon.Poison },
        { Evidence.Candlestick, Weapon.Candlestick },
        { Evidence.Dumbbell, Weapon.Dumbbell },
        { Evidence.Bat, Weapon.Bat },
        { Evidence.Knife, Weapon.Knife},
        { Evidence.Pistol, Weapon.Pistol },
        { Evidence.Trophy, Weapon.Trophy },
        { Evidence.Rope, Weapon.Rope },
        { Evidence.Axe, Weapon.Axe },
    };

    private static readonly Dictionary<Evidence, string> evidenceToString = new()
    {
        { Evidence.None, "{Weapon}" },
        { Evidence.Poison, "Poison" },
        { Evidence.Candlestick, "Candlestick" },
        { Evidence.Dumbbell, "Dumbbell" },
        { Evidence.Bat, "Bat" },
        { Evidence.Knife, "Knife"},
        { Evidence.Pistol, "Pistol" },
        { Evidence.Trophy, "Trophy" },
        { Evidence.Rope, "Rope" },
        { Evidence.Axe, "Axe" },
    };

    public static Weapon GetWeapon(Evidence weapon)
    {
        return evidenceToWeapon.GetValueOrDefault(weapon);
    }
    public static Evidence GetEvidence(Weapon weapon)
    {
        return evidenceToWeapon.FirstOrDefault(x => x.Value == weapon).Key;
    }

    public static string GetWeaponName(Evidence weapon)
    {
        if (evidenceToWeapon.ContainsKey(weapon))
            return evidenceToString.GetValueOrDefault(weapon);

        return null;
    }
    public static string GetWeaponName(Weapon weapon)
    {
        return GetWeaponName(GetEvidence(weapon));
    }
}
