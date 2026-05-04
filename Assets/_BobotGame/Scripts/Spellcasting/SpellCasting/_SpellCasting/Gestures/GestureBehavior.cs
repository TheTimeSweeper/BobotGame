namespace SpellCasting
{
    public abstract class GestureBehavior : ScriptableObjectBehavior
    {
        public abstract bool QualifyGesture(InputBank bank, InputState lastHeldInput);
        public virtual void ResetGesture() { }
    }

    public abstract class GestureBehavior<T> : GestureBehavior where T : AimGesture
    {
        private T _infoObject;
        public T InfoObject
        {
            get
            {
                if (_infoObject == null)
                {
                    _infoObject = base.infoObject as T;
                }
                return _infoObject;
            }
        }
    }
}