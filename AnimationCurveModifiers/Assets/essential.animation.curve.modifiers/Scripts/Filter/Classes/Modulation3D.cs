using System;
using UnityEngine;
using _Scripts.Filter.Data;

namespace _Scripts.Filter.Classes
{
	/// <summary>
	/// Modulates 3-dimensional data.
	/// </summary>
	[Serializable]
	public class Modulation3D : Modulation2D
	{
		/// <summary>
		/// Third dimension.
		/// </summary>
		[SerializeField] private CarrierSignal _z;
		
		/// <summary>
		/// Get signal for the third dimension.
		/// </summary>
		private CarrierSignal Z
		{
			get
			{
				return _z;
			}
		}
		
		/// <summary>
		/// Evaluate strength of signal at a given time.
		/// </summary>
		/// <param name="vector">Carrier signal with 3 dimensions.</param>
		/// <param name="time">Defines specific moment.</param>
		/// <returns>Value of modulated signal for given time</returns>
		public Vector3 Evaluate(Vector3 vector, float time)
		{
			return new Vector3(
				vector.x * X.Evaluate(time),
				vector.y * Y.Evaluate(time),
				vector.z * Z.Evaluate(time));
		}
	}
}
