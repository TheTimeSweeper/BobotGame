namespace ActiveStates
{
    public class DedState : BasicTimedStateSimple
    {
        protected override float baseDuration => 0;

        public override void OnExit(bool machineDed = false)
        {
            base.OnExit(machineDed);

            UnityEngine.Object.Destroy(gameObject);
        }
    }
}
