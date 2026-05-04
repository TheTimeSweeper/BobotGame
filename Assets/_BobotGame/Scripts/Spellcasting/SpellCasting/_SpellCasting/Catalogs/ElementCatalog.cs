using System.Collections.Generic;
using UnityEngine;

namespace SpellCasting
{
    public class ElementCatalog : Singleton<ElementCatalog>
    {
        public Dictionary<ElementTypeIndex, ElementType> ElementTypesMap = new Dictionary<ElementTypeIndex, ElementType>();
        public Dictionary<string, ElementType> NameToElementMap = new Dictionary<string, ElementType>();

        [SerializeField]
        private List<ElementType> elementTypes = new List<ElementType>();

        public static ElementType TryCombineElements(ElementType element1, ElementType element2, bool saverequired = false)
        {
            for (int i = 0; i < Instance.elementTypes.Count; i++)
            {
                ElementType combinedElement = Instance.elementTypes[i];

                if (!combinedElement.IsSecondary)
                    continue;                

                if (saverequired && !MainGame.Instance.SavedData.UnlockedElements.Contains(combinedElement))
                    continue;

                if (combinedElement.ComponentElements.Contains(element1) &&
                    combinedElement.ComponentElements.Contains(element2))
                {
                    return combinedElement;
                }
            }
            return null;
        }

        protected override void HandleAdditionalInstance()
        {

        }

        public void InitWithMainGame()
        {
            Instance = this;
            for (int i = 0; i < elementTypes.Count; i++)
            {
                ElementTypesMap.Add(elementTypes[i].Index, elementTypes[i]);
                NameToElementMap.Add(elementTypes[i].name, elementTypes[i]);
            }
        }
    }
}