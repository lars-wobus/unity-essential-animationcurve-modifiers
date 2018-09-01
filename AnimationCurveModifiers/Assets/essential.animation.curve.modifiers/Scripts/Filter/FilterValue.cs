using UnityEngine;
using _Scripts.Filter.Classes;

namespace _Scripts.Filter
{
	public class FilterValue : MonoBehaviour {

		[SerializeField] private Modulation1D _modulation1D;

		public Modulation1D Modulation1D
		{
			get { return _modulation1D; }
			set { _modulation1D = value; }
		}

		public float Evaluate(float value, float time)
		{
			return Modulation1D.Evaluate(value, time);
		}
	}
}
