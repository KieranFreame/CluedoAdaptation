using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
    private static RoomManager inst;

    private readonly Dictionary<Room, Node> rooms = new();

    [SerializeField] private List<Node> nodeList = new();

    private void Awake()
    {
        inst = this;
    }

    private void OnEnable()
    {
        for (int i = 1; i < 11; i++)
        {
            rooms.Add((Room)i, nodeList[i-1]);
        }
    }

    public static IEnumerator SelectRoom(Player p, System.Action<Node> callback)
    {
        yield return inst.StartCoroutine(inst.RoomSelector(ConvertToNode(p.CurrRoom), destRoom =>
        {
            callback(destRoom);
        }));   
    }

    public static Room ConvertToRoom(Node dest)
    {
        return inst.rooms.FirstOrDefault(x => x.Value == dest).Key;
    }

    public static Node ConvertToNode(Room room)
    {
        return inst.rooms.GetValueOrDefault(room);
    }

    private IEnumerator RoomSelector(Node startRoom, System.Action<Node> callback)
    {
        TextLog.inst.LogText("Select a room adjacent to " + startRoom.name);

        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.gameObject.TryGetComponent(out Node destRoom))
                    {
                        //If selected room is adjacent to the start room
                        if (startRoom.adjRooms.Contains(ConvertToRoom(destRoom)))
                        {
                            //TODO: Account for Secret Passages

                            callback(destRoom);
                            break;
                        }
                        else { TextLog.inst.LogText(destRoom.name = " is not adjacent to " + startRoom.name); }
                    }
                    else { TextLog.inst.LogText("That is not a Room"); }
                }
            }

            yield return null;
        }
    }
}
