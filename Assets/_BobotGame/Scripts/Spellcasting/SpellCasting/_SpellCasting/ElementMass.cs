using ActiveStates;
using System.Collections.Generic;
using UnityEngine;

namespace SpellCasting
{
    public class ElementMass : MonoBehaviour
    {
        [SerializeField]
        private ActiveStateMachine _activeStateMachine;
        public ActiveStateMachine ActiveStateMachine => _activeStateMachine;

        //JAM originally planned to split the element into equally sized masses based on total mass but shelving for now
        [SerializeField]
        private List<ElementSubMass> subMasses;
        public List<ElementSubMass> SubMasses => subMasses;

        [SerializeField]
        private float positionLerpDecay = 3;

        public float TotalMass { get; set; }

        private Vector3 _centerPositionGlobal;
        public Vector3 CenterPosition => _centerPositionGlobal;
        public Vector3 _localCenterPosition;
        public Vector3 LocalCenterPosition => _localCenterPosition;
        public Vector3 CenterPositionRaw { get; set; }

        public bool Casted { get; set; }
        public bool Active { get; set; }

        private Vector3 _lastPosition;
        private Vector3 _currentVelocity;

        private ElementType _elementType;
        public ElementType ElementType => _elementType;

        public void Init(ElementType elementType)
        {
            _elementType = elementType;
            TotalMass = elementType.BaseMass;
        }

        public void SetPosition(Vector3 aimPoint, Transform casterTransform)
        {
            CenterPositionRaw = aimPoint;

            Vector3 localAimPoint = aimPoint - casterTransform.position;
            if (_localCenterPosition == Vector3.zero)
            {
                _localCenterPosition = localAimPoint;
                return;
            }

            _localCenterPosition = Util.ExpDecayLerp(_localCenterPosition, localAimPoint, positionLerpDecay, Time.deltaTime);

            _centerPositionGlobal = casterTransform.position + _localCenterPosition;

            //_centerPosition = aimPoint;
            //if (_lastPosition == Vector3.zero)
            //{
            //    _lastPosition = _centerPosition;
            //}
            //_currentVelocity = Util.ExpDecayLerp(_currentVelocity, _centerPosition - _lastPosition, positionLerpDecay, Time.deltaTime);
            //_lastPosition = _centerPosition;
        }

        void FixedUpdate()
        {
            for (int i = subMasses.Count - 1; i >= 0; i--)
            {
                if (subMasses[i] == null)
                {
                    subMasses.RemoveAt(i);
                }
            }
            if (subMasses.Count == 0)
            {
                NotDestroy();
            }
        }

        public void Grow(float multiplier)
        {
            TotalMass += Time.deltaTime * multiplier;
            UpdateMass();
        }

        private void UpdateMass()
        {
            for (int i = 0; i < subMasses.Count; i++)
            {
                subMasses[i].SetMass(TotalMass / subMasses.Count);
            }
        }

        public void Fizzle()
        {
            //JAM todo add cool effects n stuff
            NotDestroy();
        }

        private void NotDestroy()
        {
            //gameObject.SetActive(false);
            //sike
            Destroy(gameObject);
        }

        private void OnEnable()
        {
            Active = true;
        }
        private void OnDisable()
        {
            Active = false;
        }
    }
}
