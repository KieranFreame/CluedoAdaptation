using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AI : Player
{
    public override IEnumerator Move()
    {
        Node currRoom = RoomManager.ConvertToNode(CurrRoom);
        Node dest = RoomManager.ConvertToNode(currRoom.adjRooms[Random.Range(0, currRoom.adjRooms.Count)]);

        _agent.destination = dest.transform.position;

        while (Vector3.Distance(transform.position, _agent.destination) > 1)
            yield return null;

        CurrRoom = RoomManager.ConvertToRoom(dest);

        yield break;
    }

    public override IEnumerator Suggest(System.Action<Solution> callback)
    {
        Solution sugg = new((Weapon)Random.Range(1, 10), (Suspect)Random.Range(1, 7), CurrRoom);
        callback(sugg);
        yield break;
    }

    public override IEnumerator Accuse(System.Action<Solution> callback)
    {
        Solution sugg = new((Weapon)Random.Range(1, 10), (Suspect)Random.Range(1, 7), CurrRoom);
        callback(sugg);
        yield break;
    }

    public override IEnumerator Disprove(Solution suggestion, System.Action<Evidence> callback)
    {
        Evidence evidToReturn = Evidence.None;
        Solution eligibleEvid = CheckHand(suggestion);

        if (eligibleEvid.Suspect != Suspect.None || eligibleEvid.Room != Room.None || eligibleEvid.Weapon != Weapon.None)
        {
            List<Evidence> e = new() { Suspects.GetEvidence(eligibleEvid.Suspect), Weapons.GetEvidence(eligibleEvid.Weapon), Rooms.GetEvidence(eligibleEvid.Room) };
            e.RemoveAll(x => x == Evidence.None);

            evidToReturn = e[Random.Range(0, e.Count)];
        }

        callback(evidToReturn);

        yield return null;
    }

    public override void ReceiveEvidence(Evidence proof, Player gifter = null)
    {
        TextLog.inst.LogText("Drat! My theory was incorrect!");
    }
}
