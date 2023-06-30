using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Player : MonoBehaviour
{
    //Cluedo Stuff
    public Suspect Suspect;
    public Room CurrRoom { get; set; }
    public List<Evidence> evidence = new();
    public bool Eliminated { get; set; } = false;

    //Unity Components
    protected NavMeshAgent _agent;
    private Renderer _renderer;

    protected virtual void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _renderer = GetComponent<Renderer>();

        switch (Suspect)
        {
            case Suspect.Scarlett:
                _renderer.material.color = Color.red; name = "Miss Scarlett"; break;
            case Suspect.Mustard:
                _renderer.material.color = Color.yellow; name = "Colonel Mustard"; break;
            case Suspect.Green:
                _renderer.material.color = Color.green; name = "Reverend Green"; break;
            case Suspect.Peacock:
                _renderer.material.color = Color.blue; name = "Mrs Peacock"; break;
            case Suspect.Plum:
                _renderer.material.color = Color.magenta; name = "Professor Plum"; break;
            case Suspect.White:
                _renderer.material.color = Color.white; name = "Ms White"; break;
            default:
                break;
        }

        CurrRoom = Room.EvidenceRoom;
    }

    public abstract IEnumerator Move();
    public abstract IEnumerator Suggest(System.Action<Solution> callback);
    public abstract IEnumerator Disprove(Solution suggestion, System.Action<Evidence> callback);
    public abstract IEnumerator Accuse(System.Action<Solution> callback);
    public abstract void ReceiveEvidence(Evidence proof, Player gifter = null);

    protected Solution CheckHand(Solution suggestion)
    {
        Solution eligibleEvidence = new();

        if (evidence.Contains(Suspects.GetEvidence(suggestion.Suspect)))
            eligibleEvidence.Suspect = suggestion.Suspect;

        if (evidence.Contains(Rooms.GetEvidence(suggestion.Room)))
            eligibleEvidence.Room = suggestion.Room;

        if (evidence.Contains(Weapons.GetEvidence(suggestion.Weapon)))
            eligibleEvidence.Weapon = suggestion.Weapon;

        return eligibleEvidence;
    }
}
