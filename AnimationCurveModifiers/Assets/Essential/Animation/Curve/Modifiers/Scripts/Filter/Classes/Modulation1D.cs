using System;
using Essential.Animation.Curve.Modifiers.Filter.Data;
using UnityEngine;

namespace Essential.Animation.Curve.Modifiers.Filter.Classes
{
	/// <summary>
	/// Modulates 1-dimensional data.
	/// </summary>
	[Serializable]
	public class Modulation1D
	{
		/// <summary>
		/// First dimension.
		/// </summary>
		[SerializeField] private CarrierSignal _x;
		
		/// <summary>
		/// Get signal for the first dimension.
		/// </summary>
		protected CarrierSignal X
		{
			get
			{
				return _x;
			}
		}
		
		/// <summary>
		/// Evaluate strength of signal at a given time.
		/// </summary>
		/// <param name="value">Carrier signal with 1 dimension.</param>
		/// <param name="time">Defines specific moment.</param>
		/// <returns>Value of modulated signal for given time.</returns>
		public float Evaluate(float value, float time)
		{
			return value * X.Evaluate(time);
		}
	}
}
