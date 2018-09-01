using System;
using UnityEngine;
using _Scripts.Filter.Data;

namespace _Scripts.Filter.Classes
{
	/// <summary>
	/// Modulates 2-dimensional data.
	/// </summary>
	[Serializable]
	public class Modulation2D : Modulation1D
	{
		/// <summary>
		/// Second dimension.
		/// </summary>
		[SerializeField] private CarrierSignal _y;
		
		/// <summary>
		/// Get signal for the second dimension.
		/// </summary>
		protected CarrierSignal Y
		{
			get
			{
				return _y;
			}
		}
		
		/// <summary>
		/// Evaluate strength of signal at a given time.
		/// </summary>
		/// <param name="vector">Carrier signal with 2 dimensions.</param>
		/// <param name="time">Defines specific moment.</param>
		/// <returns>Value of modulated signal for given time</returns>
		public Vector2 Evaluate(Vector2 vector, float time)
		{
			return new Vector2(
				vector.x * X.Evaluate(time),
				vector.y * Y.Evaluate(time));
		}
	}
}
