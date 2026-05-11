using SpellCasting;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public static class Util
{
    /// <summary>
    /// lerp but framerate independent. ty freya holmer https://www.youtube.com/watch?v=LSNQuFEDOyQ
    /// </summary>
    public static float ExpDecayLerp(float a, float b, float decay, float deltaTime)
    {
        return b + (a - b) * Mathf.Exp(-decay * deltaTime);
    }
    /// <summary>
    /// lerp but framerate independent. ty freya holmer https://www.youtube.com/watch?v=LSNQuFEDOyQ
    /// </summary>
    public static Vector3 ExpDecayLerp(Vector3 a, Vector3 b, float decay, float deltaTime)
    {
        return b + (a - b) * Mathf.Exp(-decay * deltaTime);
    }

    public static TeamIndex GetTeamIndex(IHasCommonComponents component)
    {
        if (component.CommonComponents == null)
            return TeamIndex.None;
        return component.CommonComponents.TeamComponent.TeamIndex;
    }


    public static T GetRandom<T>(this T[] stuff)
    {
        return stuff[UnityEngine.Random.Range(0, stuff.Length)];
    }

    public static int WeightedRandomIndex(this List<float> weights)
    {
        List<float> weightsList = new List<float>(weights);
        weightsList.Normalize();

        float sum = 0f;
        float rand = UnityEngine.Random.value;
        for (var i = 0; i < weightsList.Count; i++)
        {
            if (weightsList[i] == 0)
                continue;
            sum += weightsList[i];

            if (rand <= sum)
            {
                return i;
            }
        }
        return -1;
    }

    public static T WeightedRandom<T>(this List<T> items, Func<T, float> getWeight)
    {
        List<float> weightList = new List<float>();

        for (int i = 0; i < items.Count; i++)
        {
            weightList.Add(getWeight(items[i]));
        }

        Normalize(weightList);

        float sum = 0f;
        float rand = UnityEngine.Random.value;

        for (var i = 0; i < weightList.Count; i++)
        {
            if (weightList[i] == 0)
                continue;

            sum += weightList[i];

            if (rand <= sum)
            {
                return items[i];
            }
        }
        Debug.LogError($"could not get weighted random pick of {items}");
        return default;
    }

    public static void Normalize(this List<float> weights)
    {
        float sum = 0;
        for (int i = 0; i < weights.Count; i++)
        {
            sum += weights[i];
        }
        if (sum > 0)
        {
            for (int i = 0; i < weights.Count; i++)
            {
                weights[i] /= sum;
            }
        }
    }

    public static void Shuffle<T>(this List<T> list)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {

            int rnd = UnityEngine.Random.Range(0, i);
            T temp = list[i];

            list[i] = list[rnd];
            list[rnd] = temp;
        }
    }

    public static T2 TryGetValueDefault<T1, T2>(this Dictionary<T1, T2> dict, T1 key)
    {
        if (dict.ContainsKey(key))
        {
            return dict[key];
        }
        return default;
    }

    /// <summary>
    /// creates a new int entry if it doesn't exist and increments it
    /// </summary>
    public static void IncrementvValue<T1>(this Dictionary<T1, int> dict, T1 key, int value)
    {
        if (!dict.ContainsKey(key))
        {
            dict[key] = 0;
        }
        dict[key] += value;
    }
    /// <summary>
    /// creates a new float entry if it doesn't exist and increments it
    /// </summary>
    public static void IncrementValue<T1>(this Dictionary<T1, float> dict, T1 key, float value)
    {
        if (!dict.ContainsKey(key))
        {
            dict[key] = 0;
        }
        dict[key] += value;
    }

    public static bool AboutEqual(float a, float b)
    {

        if (a > 0f && b < 0f) return false;
        if (a < 0f && b > 0f) return false;

        return Math.Abs(Math.Abs(a) - Math.Abs(b)) <= float.Epsilon;
    }

    #region log
    private static void LogToDisplay(object message, string color, KeyCode keycode = default, UnityEngine.Object context = null)
    {
        if (keycode != default && !Input.GetKey(keycode))
            return;

        StaticLogDisplay.Log(message.ToString(), color);
    }

    /// <summary>
    /// logs to a debug display if it is active
    /// </summary>
    [HideInCallstack]
    public static void Log(object message, KeyCode keycode = default, UnityEngine.Object context = null)
    {
        Debug.Log(message, context);
        LogToDisplay(message, "white", keycode, context);
    }

    /// <summary>
    /// logs to a debug display if it is active
    /// </summary>
    [HideInCallstack]
    public static void LogError(object message, KeyCode keycode = default, UnityEngine.Object context = null)
    {
        Debug.LogError(message, context);;
        LogToDisplay(message, "red", keycode, context);

    }

    /// <summary>
    /// logs to a debug display if it is active
    /// </summary>
    [HideInCallstack]
    public static void LogWarning(object message, KeyCode keycode = default, UnityEngine.Object context = null)
    {
        Debug.LogWarning(message, context);
        LogToDisplay(message, "yellow", keycode, context);
    }
    #endregion

    public static bool CastHurtBox(InputBank inputbank, out RaycastHit raycastHit, float range = 100)
    {
        return Physics.Raycast(inputbank.GetBodyRay(), out raycastHit, range, LayerInfo.Hurtbox.layerMask.value);
    }

    public static bool CastHurtBox(Ray ray, out RaycastHit raycastHit, float range = 100)
    {
        return Physics.Raycast(ray, out raycastHit, range, LayerInfo.Hurtbox.layerMask.value);
    }

    public static Vector3 ScreenToCanvasPosition(Vector3 inputPosition)
    {
        //jam this probably doesnt work on other resolutions
            //get fucked ultrawiders
        inputPosition.x = inputPosition.x / Screen.width * 2560;
        inputPosition.y = inputPosition.y / Screen.height * 1440;
        return inputPosition;
    }
    public static Quaternion DirectionQuaternion (Vector3 direction)
    {
        return Quaternion.LookRotation(direction, Vector3.up);
    }

    public static float GetTestValue(this GameObject gob, int index)
    {
        return gob.GetComponent<TestValues>().floats[index];
    }

    public static void DebugDrawPoint(this Vector3 point) => DebugDrawPoint(point, Color.white, Vector3.up, 10);
    public static void DebugDrawPoint(this Vector3 point, Color color) => DebugDrawPoint(point, color, Vector3.up, 10);
    public static void DebugDrawPoint(this Vector3 point, float dist) => DebugDrawPoint(point, Color.white, Vector3.up, dist);
    public static void DebugDrawPoint(this Vector3 point, Color color, float dist) => DebugDrawPoint(point, color, Vector3.up, dist);
    public static void DebugDrawPoint(this Vector3 point, Vector3 direction, float dist) => DebugDrawPoint(point, Color.white, Vector3.up, dist);
    public static void DebugDrawPoint(this Vector3 point, Color color, Vector3 direction, float dist)
    {
        Debug.DrawLine(point, point + direction * dist, color);
    }

    public static bool ShouldTargetByTeam(GameObject thisGameObject, IHasCommonComponents targetObject, TeamIndex thisTeam, TeamTargetType teamTargeting)
    {
        bool validHit = false;
        TeamIndex targetTeamIndex = Util.GetTeamIndex(targetObject);
        switch (teamTargeting)
        {
            case var targeting when teamTargeting.HasFlag(TeamTargetType.SELF):
                if (targetObject.CommonComponents != null && targetObject.CommonComponents.gameObject == thisGameObject)
                    validHit = true;
                break;

            //JAM friendlyfiremanager?
            case var targeting when teamTargeting.HasFlag(TeamTargetType.OTHER):
                if (targetTeamIndex != thisTeam)
                    validHit = true;
                break;

            case var targeting when teamTargeting.HasFlag(TeamTargetType.ALLY):
                if (targetTeamIndex == thisTeam)
                    validHit = true;
                break;
        }

        return validHit;
    }
}
