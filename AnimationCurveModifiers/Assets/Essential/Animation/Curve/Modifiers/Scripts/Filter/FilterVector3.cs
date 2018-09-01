using Essential.Animation.Curve.Modifiers.Filter.Classes;
using UnityEngine;

namespace Essential.Animation.Curve.Modifiers.Filter
{
	public class FilterVector3 : MonoBehaviour
	{
		[SerializeField] private Modulation3D _modulation3D;

		public Modulation3D Modulation3D
		{
			get { return _modulation3D; }
			set { _modulation3D = value; }
		}

		public Vector3 Evaluate(Vector3 vector, float time)
		{
			return Modulation3D.Evaluate(vector, time);
		}
	}
}
