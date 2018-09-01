using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Essential.Animation.Curve.Modifiers.Filter.Editor
{
    public abstract class AnimationCurveModifier : EditorWindow
    {
        protected Component Component { get; set; }
        protected int DropDownIndex { get; set; }
        
        protected void DisplayHeader(string text)
        {
            EditorGUI.indentLevel = 0;
            EditorGUILayout.LabelField(text, EditorStyles.boldLabel);
            EditorGUI.indentLevel = 1;
        }
        
        protected FieldInfo[] GetFields()
        {
            var fieldInfos = GetAllFieldsForComponentType(Component.GetType());
            var array = FilterArrayForAnimationCurves(fieldInfos);

            if (array.Length == 0)
            {
                EditorGUILayout.HelpBox("Selected component does not have any fields for AnimationCurves.", MessageType.Warning);
                // return;
            }

            return array;
        }
        
        protected string[] GetFieldNames(FieldInfo[] fieldInfos)
        {
            return fieldInfos == null ? null : fieldInfos.Select((element) => element.Name).ToArray();
        }
        
        private FieldInfo[] FilterArrayForAnimationCurves(FieldInfo[] fieldInfos)
        {
            return fieldInfos.Where(element =>
            {
                Debug.Log(element.FieldType);
                return element.FieldType.IsAssignableFrom(typeof(AnimationCurve));
            }).ToArray();
        }
        
        private FieldInfo[] GetAllFieldsForComponentType(IReflect componentType)
        {
            return componentType.GetFields(BindingFlags.NonPublic | 
                                           BindingFlags.Public | 
                                           BindingFlags.Instance);
        }
    }
}