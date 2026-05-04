using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpellCasting
{
    public class CharacterBodyTracker : MonoBehaviour
    {
        public static Dictionary<TeamIndex, List<CharacterBodyTracker>> teamsBodies = new Dictionary<TeamIndex, List<CharacterBodyTracker>>();

        [SerializeField]
        private TeamComponent teamComponent;

        [SerializeField]
        private CharacterBody body;

        void OnEnable()
        {
            if (!teamsBodies.ContainsKey(teamComponent.TeamIndex))
            {
                teamsBodies.Add(teamComponent.TeamIndex, new List<CharacterBodyTracker>());
            }

            if (!teamsBodies[teamComponent.TeamIndex].Contains(this))
            {
                teamsBodies[teamComponent.TeamIndex].Add(this);
            }
        }
        void OnDisable()
        {
            foreach (var bodyList in teamsBodies)
            {
                if (bodyList.Value.Contains(this))
                {
                    bodyList.Value.Remove(this);
                }
            }
        }

        public static CharacterBody FindPrimaryPlayer()
        {
            if (teamsBodies.ContainsKey(TeamIndex.PLAYER))
            {
                if(teamsBodies[TeamIndex.PLAYER].Count > 0)
                {
                    return teamsBodies[TeamIndex.PLAYER][0].body;
                }
            }
            return null;
        }

        public static CharacterBody FindBodyByTeam(GameObject searchingObject, TeamIndex searchingTeamIndex, TeamTargetType teamTargeting, float maxSqrDistance)
        {
            foreach (var TeamIndex in teamsBodies.Keys)
            {
                for (int i = 0; i < teamsBodies[TeamIndex].Count; i++)
                {
                    if (Vector3.SqrMagnitude(teamsBodies[TeamIndex][i].transform.position - searchingObject.transform.position) < maxSqrDistance)
                    {
                        if (Util.ShouldTargetByTeam(searchingObject, teamsBodies[TeamIndex][i].body, searchingTeamIndex, teamTargeting))
                        {
                            return teamsBodies[TeamIndex][i].body;
                        }
                    }
                }
            }
            return null;
        }

        public static CharacterBody FindBodyBySqrDistance(TeamIndex teamIndex, Vector3 centerPosition, float sqrDistance)
        {
            for (int i = 0; i < teamsBodies[teamIndex].Count; i++)
            {
                if (Vector3.SqrMagnitude(teamsBodies[teamIndex][i].transform.position - centerPosition) < sqrDistance)
                {
                    return teamsBodies[teamIndex][i].body;
                }
            }
            return null;
        }
    }
}
