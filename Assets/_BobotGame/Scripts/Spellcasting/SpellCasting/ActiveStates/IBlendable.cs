namespace ActiveStates
{
    public interface IBlendable<T> where T : ICloneable<T>
    {
        T Blend(T other, float t);
    }
}
