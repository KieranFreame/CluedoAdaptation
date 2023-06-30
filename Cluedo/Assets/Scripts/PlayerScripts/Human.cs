using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Human : Player
{
    public override IEnumerator Move()
    {
        Node _dest = null;

        yield return StartCoroutine(RoomManager.SelectRoom(this, dest =>
        {
            _dest = dest;
        }));

        _agent.destination = _dest.transform.position;

        while (Vector3.Distance(transform.position, _agent.destination) > 1)
            yield return null;

        CurrRoom = RoomManager.ConvertToRoom(_dest);
    }

    public override IEnumerator Suggest(System.Action<Solution> callback)
    {
        yield return StartCoroutine(SolutionHandler.CreateSolution(this, sugg => { callback(sugg); }));
    }

    public override IEnumerator Disprove(Solution suggestion, System.Action<Evidence> callback)
    {
        Evidence evidToReturn = Evidence.None;
        Solution eligibleEvid = CheckHand(suggestion);

        //if any these are true, send to DisproveHandler
        if (eligibleEvid.Suspect != Suspect.None || eligibleEvid.Room != Room.None || eligibleEvid.Weapon != Weapon.None)
        {
            List<Evidence> e = new() { Suspects.GetEvidence(eligibleEvid.Suspect), Weapons.GetEvidence(eligibleEvid.Weapon), Rooms.GetEvidence(eligibleEvid.Room) };
            e.RemoveAll(x => x == Evidence.None);

            if (e.Count == 1)
            {
                string evidence = Suspects.GetSuspectName(e[0]) ?? (Rooms.GetRoomName(e[0]) ?? Weapons.GetWeaponName(e[0]));
                TextLog.inst.LogText("Showing " + evidence);
                evidToReturn = e[0];
            }
            else if (e.Count > 1)
            {
                yield return StartCoroutine(DisproveHandler.SelectEvidence(eligibleEvid, suggestion, evidence => {
                    evidToReturn = evidence;
                }));
            }  
        }

        callback(evidToReturn);
        yield break;
    }

    public override IEnumerator Accuse(System.Action<Solution> callback)
    {
        yield return StartCoroutine(SolutionHandler.CreateSolution(this, sugg => { callback(sugg); }, true));
    }

    public override void ReceiveEvidence(Evidence proof, Player gifter = null)
    {
        string evidence = Suspects.GetSuspectName(proof) ?? Rooms.GetRoomName(proof) ?? Weapons.GetWeaponName(proof);
        string msg = gifter == null ? "Received " + evidence + " from Game" : "Shown " + evidence + " by " + gifter.name;

        TextLog.inst.LogText(msg);
        NotepadHandler.inst.CheckOffEvidence(proof);
    }
}
