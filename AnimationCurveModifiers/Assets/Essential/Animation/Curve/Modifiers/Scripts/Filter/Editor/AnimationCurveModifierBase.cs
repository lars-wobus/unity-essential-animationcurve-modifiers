using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Essential.Animation.Curve.Modifiers.Filter.Editor
{
    /// <summary>
    /// Base class for all AnimationCurve modifiers. 
    /// </summary>
    public abstract class AnimationCurveModifierBase : EditorWindow
    {
        /// <summary>
        /// Property to keep selected target script.
        /// </summary>
        protected Component Component { get; set; }
        
        /// <summary>
        /// Property to keep index of selected AnimationCurve.
        /// </summary>
        protected int DropDownIndex { get; set; }
        
        /// <summary>
        /// Add section header to window.
        /// </summary>
        /// <param name="text">Header name</param>
        protected void DisplayHeader(string text)
        {
            EditorGUI.indentLevel = 0;
            EditorGUILayout.LabelField(text, EditorStyles.boldLabel);
            EditorGUI.indentLevel = 1;
        }
        
        /// <summary>
        /// Get FieldInfos pointing to AnimationCurves
        /// </summary>
        /// <returns>Array of FieldInfo</returns>
        protected FieldInfo[] GetFields()
        {
            if (Component == null)
            {
                return null;
            }
            
            var fieldInfos = GetAllFieldsForComponentType(Component.GetType());
            var array = FilterArrayForAnimationCurves(fieldInfos);

            if (array.Length == 0)
            {
                EditorGUILayout.HelpBox("Selected component does not have any fields for AnimationCurves.", MessageType.Warning);
            }

            return array;
        }
        
        /// <summary>
        /// Extract array of field names from array of FieldInfo.
        /// </summary>
        /// <param name="fieldInfos">Array of FieldInfo</param>
        /// <returns>Array of field names</returns>
        protected string[] GetFieldNames(FieldInfo[] fieldInfos)
        {
            return fieldInfos == null ? null : fieldInfos.Select((element) => element.Name).ToArray();
        }
        
        /// <summary>
        /// Filter array of FieldInfo for AnimationCurves.
        /// </summary>
        /// <param name="fieldInfos"></param>
        /// <returns>Subset of input array</returns>
        private FieldInfo[] FilterArrayForAnimationCurves(FieldInfo[] fieldInfos)
        {
            return fieldInfos.Where(element =>
            {
                Debug.Log(element.FieldType);
                return element.FieldType.IsAssignableFrom(typeof(AnimationCurve));
            }).ToArray();
        }
        
        /// <summary>
        /// Returns an array of FieldInfo objects that correspond to all fields of the current class.
        /// </summary>
        /// <param name="componentType">Type of component</param>
        /// <returns>Array of FieldInfo</returns>
        private FieldInfo[] GetAllFieldsForComponentType(IReflect componentType)
        {
            return componentType.GetFields(BindingFlags.NonPublic | 
                                           BindingFlags.Public | 
                                           BindingFlags.Instance);
        }
    }
}