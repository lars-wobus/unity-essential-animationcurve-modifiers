using UnityEngine;
using _Scripts.Filter.Classes;

namespace _Scripts.Filter
{
	public class FilterVector2 : MonoBehaviour
	{
		[SerializeField] private Modulation2D _modulation2D;

		public Modulation2D Modulation2D
		{
			get { return _modulation2D; }
			set { _modulation2D = value; }
		}

		public Vector2 Evaluate(Vector2 vector, float time)
		{
			return Modulation2D.Evaluate(vector, time);
		}
	}
}
