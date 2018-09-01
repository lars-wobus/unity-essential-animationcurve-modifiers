using UnityEngine;

namespace Essential.Animation.Curve.Modifiers.Filter.Data
{
    /// <inheritdoc />
    /// <summary>
    /// Data class for modulating information signals (also called data signals).
    /// More precisely, it can be used to amplify and filter signals.
    /// </summary>
    public class CarrierSignal : MonoBehaviour
    {
        /// <summary>
        /// Allows interpolation of the modulation strength. The default describes a constant function with x[0,1] and y[1]. 
        /// </summary>
        [SerializeField] private AnimationCurve _animationCurve = AnimationCurve.Constant(0, 1, 1);

        /// <summary>
        /// Gets or sets the curve used to interpolate the modulation strength.
        /// </summary>
        public AnimationCurve AnimationCurve
        {
            get { return _animationCurve; }
            set
            {
                if (value == null) return; 
                _animationCurve = value;
            }
        }
        
        /// <summary>
        /// Evaluate the curve at time.
        /// </summary>
        /// <param name="time">Function argument X</param>
        /// <returns>Function value Y</returns>
        public float Evaluate(float time)
        {
            return AnimationCurve.Evaluate(time);
        }
    }
}
