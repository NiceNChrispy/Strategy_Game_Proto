using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Team
{
    public struct Relationship
    {
        public readonly Team A, B;
        public readonly Status Status;

        public Relationship(Team a, Team b, Status status)
        {
            A = a;
            B = b;
            Status = status;
        }

        public bool Contains(Team a, Team b)
        {
            return ((A == a && B == b) || (A == b || B == a));
        }
    }

    public enum Status { DEFAULT, FRIENDLY, HOSTILE }

    static int Count;
    static List<Relationship> s_Relationships;
    static List<Team> s_Teams;
    static int s_TeamCount;

    public int ID { get; private set; }
    public Color Color { get; private set; }

    public Team(int id)
    {
        ID = id;
    }

    public static Team CreateNewTeam()
    {
        return new Team(s_TeamCount++);
    }

    public bool IsSameTeam(Team other)
    {
        return ID == other.ID;
    }

    public static Status GetRelationship(Team a, Team b)
    {
        Relationship relationship = s_Relationships.SingleOrDefault(x => x.Contains(a, b));
        if(!relationship.Equals(default(Relationship)))
        {
            return relationship.Status;
        }
        return Status.DEFAULT;
    }
}