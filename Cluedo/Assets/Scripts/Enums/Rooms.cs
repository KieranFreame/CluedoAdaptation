using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum Room
{
    None,
    Hall,
    Patio,
    Kitchen,
    Spa,
    Theater,
    LivingRoom,
    Observatory,
    DiningRoom,
    GuestHouse,
    EvidenceRoom,
}
public static class Rooms
{
    private static readonly Dictionary<Room, string> roomToString = new()
    {
        { Room.LivingRoom, "Living Room"},
        { Room.DiningRoom, "Dining Room" },
        { Room.GuestHouse, "Guest House" },
        { Room.Hall, "Hall" },
        { Room.Patio, "Patio"},
        { Room.Kitchen, "Kitchen"},
        { Room.Spa, "Spa"},
        { Room.Theater, "Theater"},
        { Room.Observatory, "Observatory" },
        { Room.EvidenceRoom, "Evidence Room" }
    };

    private static readonly Dictionary<Evidence, Room> evidenceToRoom = new()
    {
        { Evidence.None, Room.None },
        { Evidence.LivingRoom, Room.LivingRoom},
        { Evidence.DiningRoom, Room.DiningRoom },
        { Evidence.GuestHouse, Room.GuestHouse },
        { Evidence.Hall, Room.Hall },
        { Evidence.Patio, Room.Patio},
        { Evidence.Kitchen, Room.Kitchen},
        { Evidence.Spa, Room.Spa},
        { Evidence.Theater, Room.Theater},
        { Evidence.Observatory, Room.Observatory },
    };

    public static string GetRoomName(Evidence room)
    {
        if (evidenceToRoom.ContainsKey(room))
            return GetRoomName(GetRoom(room));

        return null;
    }
    
    public static string GetRoomName(Room room)
    {
        if (room != Room.EvidenceRoom)
            return roomToString.GetValueOrDefault(room);
        else
            return "Evidence Room";
    }

    public static Room GetRoom(Evidence room)
    {
        return evidenceToRoom.GetValueOrDefault(room);
    }

    public static Evidence GetEvidence(Room room)
    {
        return evidenceToRoom.FirstOrDefault(x => x.Value == room).Key;
    }
}
