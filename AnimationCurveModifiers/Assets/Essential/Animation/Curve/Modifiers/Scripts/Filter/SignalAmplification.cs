using _Scripts.Filter.Data;

namespace _Scripts.Filter
{
    public class SignalAmplification : CarrierSignal
    {
        public float Evaluate(float time)
        {
            return -AnimationCurve.Evaluate(time);
        }
    }
}