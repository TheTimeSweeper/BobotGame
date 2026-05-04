using System;
using System.Collections.Generic;
using UnityEngine;
namespace SpellCasting
{
    public class BodyBuffPair
    {
        public CharacterBody Body;
        public BuffInfo Buff;
        public float Timer;
    }

    //public struct ActiveBuff
    //{
    //    BuffInfo buff;
    //    string id
    //}

    public class BuffManager : Singleton<BuffManager>
    {
        private List<BodyBuffPair> buffTimerList = new List<BodyBuffPair>();

        /// <summary>
        /// only to be called from characterbody. use CharacterBody.AddTimedBuff instead
        /// </summary>
        public static void AddBuffTimer(CharacterBody body, BuffInfo buff, float time)
        {
            //buff.OnApply(body);
            //if (!body.HasBuff(buff))
            //{
                Instance.buffTimerList.Add(new BodyBuffPair { Body = body, Buff = buff, Timer = time });
            //    return;
            //}

            BodyBuffPair bodyBuffPair = Instance.buffTimerList.Find((pair) => { return pair.Buff == buff && pair.Body == body; });
            if (bodyBuffPair != null)
            {
                bodyBuffPair.Timer = time;
            }
        }

        void FixedUpdate()
        {
            BodyBuffPair bodyBuffPair;
            for (int i = buffTimerList.Count - 1; i >= 0; i--)
            {
                bodyBuffPair = buffTimerList[i];
                bodyBuffPair.Timer -= Time.fixedDeltaTime;
                if (bodyBuffPair.Timer < 0)
                {
                    bodyBuffPair.Body.Removebuff(bodyBuffPair.Buff);
                    buffTimerList.RemoveAt(i);
                }
            }
        }
    }
}