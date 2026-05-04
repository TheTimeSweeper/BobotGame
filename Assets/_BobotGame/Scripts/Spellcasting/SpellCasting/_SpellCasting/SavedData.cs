using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpellCasting
{
    [System.Serializable]
    public class SavedData
    {
        [SerializeField]
        private List<string> elem = new List<string>();

        private List<ElementType> _unlockedElements;
        public List<ElementType> UnlockedElements { 
            get
            {
                if(_unlockedElements == null || _elementsDirty)
                {
                    _elementsDirty = false;
                    _unlockedElements = new List<ElementType>();
                    for (int i = 0; i < elem.Count; i++)
                    {
                        if (ElementCatalog.Instance.NameToElementMap.ContainsKey(elem[i]))
                        {
                            _unlockedElements.Add(ElementCatalog.Instance.NameToElementMap[elem[i]]);
                        }
                    }

                    if (!_unlockedElements.Contains(ElementCatalog.Instance.ElementTypesMap[ElementTypeIndex.FIRE]))
                    {
                        _unlockedElements.Add(ElementCatalog.Instance.ElementTypesMap[ElementTypeIndex.FIRE]);
                        Save();
                    }
                }
                return _unlockedElements;
            }
        }

        //jam wanna do a datafield that implicits to the type like variablenumberstat
        bool _elementsDirty = false;

        public void AddElement(ElementType element) => AddElement(element.name);
        public void AddElement(string element)
        {
            if (elem.Contains(element))
                return;

            elem.Add(element);
            _elementsDirty = true;
            Save();
        }

        public void RemoveElement(ElementType element) => RemoveElement(element.name);
        public void RemoveElement(string element)
        {
            if (!elem.Contains(element))
                return;

            elem.Remove(element);
            _elementsDirty = true;
            Save();
        }

        public void Save()
        {
            string save = JsonUtility.ToJson(this);
            PlayerPrefs.SetString("SpellCastingGame_SavedData", save);
            Debug.Log("saved " + save);
        }
        
        public static SavedData LoadOrCreate()
        {
            string savedDataString = PlayerPrefs.GetString("SpellCastingGame_SavedData", string.Empty);

            if (!string.IsNullOrEmpty(savedDataString))
            {
                Debug.Log("sucessfully loaded save: " + savedDataString);
                return JsonUtility.FromJson<SavedData>(savedDataString);
            }
            else
            {
                Debug.Log("no save found. creating new");
                SavedData savedData = new SavedData();
                return savedData;
            }
        }
    }
}