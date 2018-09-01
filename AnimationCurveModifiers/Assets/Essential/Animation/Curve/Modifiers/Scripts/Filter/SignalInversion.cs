using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Essential.Animation.Curve.Modifiers.Filter
{
    [ExecuteInEditMode]
    public class SignalInversion : MonoBehaviour
    {
        public Component component;
        public AnimationCurve animationCurve;
        public bool invert;

        void Awake()
        {
            Debug.Log("Editor causes this Awake");
        }
        
        void Update()
        {
            Start();
        }
        
        private void Start()
        {
            if (component == null)
            {
                return;
            }

            var type = component.GetType();
            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.NonPublic | 
                                                    BindingFlags.Public | 
                                                    BindingFlags.Instance);

            var filteredArray = fieldInfos.Where(element =>
            {
                Debug.Log(element.FieldType);
                return element.FieldType.IsAssignableFrom(typeof(AnimationCurve));
            }).ToArray();

            foreach (var curve in filteredArray)
            {
                InvertValues(curve.GetValue(component) as AnimationCurve);
            }
        }

        public void InvertValues(AnimationCurve animationCurve)
        {
            
            var keyframes = animationCurve.keys;
            var n = keyframes.Select((keyframe) => new Keyframe(keyframe.time, -keyframe.value)).ToArray();
            foreach (var a in n)
            {
                Debug.Log(a.value.ToString());
            }
            animationCurve = new AnimationCurve(n);
            Debug.Log(animationCurve.keys[0].value.ToString());
        }
    }
}