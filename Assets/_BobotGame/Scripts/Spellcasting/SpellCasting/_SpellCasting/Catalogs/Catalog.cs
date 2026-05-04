using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpellCasting
{
    public abstract class Catalog<TS, T1, T2> : Singleton<TS> where TS : Singleton<TS>
    {
        //jam I think I should have written a generic catalog class at some point. oh well copy paste here we goo
        protected static Dictionary<T1, T2> catalogIndexMap = new Dictionary<T1, T2>();

        [SerializeField]
        private T2[] catalogItems;

        protected override void InitOnce()
        {
            base.InitOnce();

            for (int i = 0; i < catalogItems.Length; i++)
            {
                catalogIndexMap.Add(GetIndexFromItem(catalogItems[i]), catalogItems[i]);
            }
        }

        protected abstract T1 GetIndexFromItem(T2 item);

        public static T2 GetCatalogItem(T1 itemIndex)
        {
            return catalogIndexMap[itemIndex];
        }
    }
}