using System.Linq;
using Essential.Core.Extensions;
using UnityEditor;
using UnityEngine;

namespace Essential.Animation.Curve.Modifiers.Filter.Editor
{
    public class InvertWindow : AnimationCurveModifier
    {
        private AnimationCurve InversionByX { get; set; }
        private AnimationCurve InversionByY { get; set; }
        private AnimationCurve InversionByXy { get; set; }
        
        private bool KeepCurveInPlace { get; set; }
        private bool Normalize { get; set; }
        
        [MenuItem("Window/Animation Curve/Inverter")]
        private static void Init()
        {
            var window = (InvertWindow)GetWindow(typeof(InvertWindow));
            window.Show();
        }

        private void OnEnable()
        {
            Debug.Log("OnEnable");
            InversionByX = new AnimationCurve();
            InversionByY = new AnimationCurve();
            InversionByXy = new AnimationCurve();
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();
            DisplayHeader("Input");
            var isValid = DisplayInputFields();
            var fields = GetFields();
            var names = GetFieldNames(fields);
            DisplayField(!isValid, names);
            
            EditorGUILayout.Space();
            DisplayHeader("Output");
            if (isValid)
            {
                var animationCurve = fields[DropDownIndex].GetValue(Component) as AnimationCurve;
                if (animationCurve != null)
                {
                    var keys = animationCurve.keys;
                    InversionByX.keys = keys.InvertValues();
                    InversionByY.keys = keys.InvertTimes();
                    InversionByXy.keys = InversionByX.keys.InvertTimes();

                    if (KeepCurveInPlace)
                    {
                        var bMin = keys.First().time;
                        var bMax = keys.Last().time;
                        InversionByX.keys = InversionByX.keys.RemapTimes(bMin, bMax);
                        InversionByY.keys = InversionByY.keys.RemapTimes(bMin, bMax);
                        InversionByXy.keys = InversionByXy.keys.RemapTimes(bMin, bMax);
                    }

                    if (Normalize)
                    {
                        var bMin = 0;
                        var bMax = 1;
                        InversionByX.keys = InversionByX.keys.RemapTimes(bMin, bMax);
                        InversionByY.keys = InversionByY.keys.RemapTimes(bMin, bMax);
                        InversionByXy.keys = InversionByXy.keys.RemapTimes(bMin, bMax);
                    }
                }
            }
            DisplayOutputFields(!isValid);
        }

        private bool DisplayInputFields()
        {
            Component = (Component) EditorGUILayout.ObjectField("Script", Component, typeof(Component), true);

            if (Component == null)
            {
                EditorGUILayout.HelpBox("Component null", MessageType.Warning);
                return false;
            }

            if (Component.GetType() == typeof(Transform))
            {
                EditorGUILayout.HelpBox("Transform component does not contain Animation Curves. You might have dragged a gameobject into the input field.", MessageType.Warning);
                return false;
            }

            return true;
        }
        
        private void DisplayField(bool disabled, string[] names)
        {
            EditorGUI.BeginDisabledGroup(disabled);
            DropDownIndex = EditorGUILayout.Popup("Field", DropDownIndex, names);
            KeepCurveInPlace = EditorGUILayout.Toggle("Keep Curve in Place", KeepCurveInPlace);
            Normalize = EditorGUILayout.Toggle("Normalize", Normalize);
            EditorGUI.EndDisabledGroup();
        }
        
        private void DisplayOutputFields(bool disabled)
        {
            EditorGUI.BeginDisabledGroup(disabled);
            EditorGUILayout.CurveField("Flip Horizontal", InversionByX);
            EditorGUILayout.CurveField("Flip Vertical", InversionByY);
            EditorGUILayout.CurveField("Flip Both", InversionByXy);
            EditorGUI.EndDisabledGroup();
        }
    }
}