using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum Suspect
{
    None,
    Scarlett,
    Mustard,
    Green,
    Peacock,
    Plum,
    White,
}

public static class Suspects
{
    private static readonly Dictionary<Evidence, string> suspectToString = new()
    {
        { Evidence.Scarlett, "Miss Scarlett" },
        { Evidence.Mustard, "Colonel Mustard" },
        { Evidence.Green, "Reverend Green"},
        { Evidence.Plum, "Professor Plum"}, 
        { Evidence.White, "Ms White"},
        { Evidence.Peacock, "Mrs Peacock" },
        { Evidence.None, "{Suspect}" }
    };

    private static readonly Dictionary<Evidence, Color> evidenceToColor = new()
    {
        { Evidence.Scarlett, Color.red },
        { Evidence.Green, Color.green },
        { Evidence.Mustard, Color.yellow },
        { Evidence.Peacock, Color.blue },
        { Evidence.Plum, Color.magenta },
        { Evidence.White, Color.white },
    };
    
    private static readonly Dictionary<Evidence, Suspect> evidenceToSuspect = new()
    {
        { Evidence.None, Suspect.None },
        { Evidence.Scarlett, Suspect.Scarlett },
        { Evidence.Mustard, Suspect.Mustard },
        { Evidence.Green, Suspect.Green},
        { Evidence.Plum, Suspect.Plum}, 
        { Evidence.White, Suspect.White},
        { Evidence.Peacock, Suspect.Peacock }
    };

    public static string GetSuspectName(Evidence suspect)
    {
        if (evidenceToSuspect.ContainsKey(suspect))
            return suspectToString.GetValueOrDefault(suspect);

        return null;
    }
    
    public static string GetSuspectName(Suspect suspect)
    {
        return GetSuspectName(GetEvidence(suspect));
    }

    public static Suspect GetSuspect(Evidence suspect)
    {
        return evidenceToSuspect.GetValueOrDefault(suspect);
    }

    public static Evidence GetEvidence(Suspect suspect)
    {
        return evidenceToSuspect.FirstOrDefault(x => x.Value == suspect).Key;
    }

    public static Color GetSuspectColor(Suspect suspect)
    {
        return GetSuspectColor(GetEvidence(suspect));
    }

    public static Color GetSuspectColor(Evidence suspect)
    {
        return evidenceToColor.GetValueOrDefault(suspect);
    }
}
