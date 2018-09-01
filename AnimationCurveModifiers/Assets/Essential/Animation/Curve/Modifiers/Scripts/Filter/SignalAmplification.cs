using Essential.Animation.Curve.Modifiers.Filter.Data;

namespace Essential.Animation.Curve.Modifiers.Filter
{
    public class SignalAmplification : CarrierSignal
    {
        public float Evaluate(float time)
        {
            return -AnimationCurve.Evaluate(time);
        }
    }
}