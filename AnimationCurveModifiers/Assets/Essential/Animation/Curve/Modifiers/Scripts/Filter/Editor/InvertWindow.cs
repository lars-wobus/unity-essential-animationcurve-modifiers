using UnityEngine;
using UnityEditor;
using System;
using System.CodeDom;
using System.Collections.Generic;
using UnityEngine;
using _Scripts.Filter.Data;
using System.Reflection;
using System.Linq;
using UnityEngine.UI;
using Essential.Core.Extensions;

namespace _Scripts.Filter
{
    public class InvertWindow : AnimationCurveModifier//EditorWindow
    {
        //private Component Component { get; set; }

        //private int DropDownIndex { get; set; }

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
                    InversionByX.keys = keys.InvertValues();//InvertValues(keys);
                    InversionByY.keys = keys.InvertTimes();//InvertKeys(keys);
                    InversionByXy.keys = InversionByX.keys.InvertTimes();//InvertKeys(InversionByX.keys);

                    if (KeepCurveInPlace)
                    {
                        var bMin = keys.First().time;
                        var bMax = keys.Last().time;
                        InversionByX.keys = InversionByX.keys.RemapTimes(bMin, bMax);//Remap(InversionByX.keys, bMin, bMax);
                        InversionByY.keys = InversionByY.keys.RemapTimes(bMin, bMax);//Remap(InversionByY.keys, bMin, bMax);
                        InversionByXy.keys = InversionByXy.keys.RemapTimes(bMin, bMax);//Remap(InversionByXy.keys, bMin, bMax);
                    }

                    if (Normalize)
                    {
                        var bMin = 0;
                        var bMax = 1;
                        InversionByX.keys = InversionByX.keys.RemapTimes(bMin, bMax);//Remap(InversionByX.keys, bMin, bMax);
                        InversionByY.keys = InversionByY.keys.RemapTimes(bMin, bMax);//Remap(InversionByY.keys, bMin, bMax);
                        InversionByXy.keys = InversionByXy.keys.RemapTimes(bMin, bMax);//Remap(InversionByXy.keys, bMin, bMax);
                    }
                }
            }
            DisplayOutputFields(!isValid);
        }
        
        /*private void DisplayHeader(string text)
        {
            EditorGUI.indentLevel = 0;
            EditorGUILayout.LabelField(text, EditorStyles.boldLabel);
            EditorGUI.indentLevel = 1;
        }*/

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
        
        /*private FieldInfo[] GetFields()
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
        
        private string[] GetFieldNames(FieldInfo[] fieldInfos)
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
        }*/
        
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

        /*private FieldInfo[] GetAllFieldsForComponentType(IReflect componentType)
        {
            return componentType.GetFields(BindingFlags.NonPublic | 
                                  BindingFlags.Public | 
                                  BindingFlags.Instance);
        }*/
        
        /*private Keyframe[] InvertValues(IEnumerable<Keyframe> keyframes)
        {
            return keyframes.Select((keyframe) => new Keyframe(keyframe.time, -keyframe.value, -keyframe.inTangent, -keyframe.outTangent)).ToArray();
        }
        
        private Keyframe[] InvertKeys(IEnumerable<Keyframe> keyframes)
        {
            return keyframes.Select((keyframe) => new Keyframe(-keyframe.time, keyframe.value, -keyframe.inTangent, -keyframe.outTangent)).ToArray();
        }

        private Keyframe[] Remap(Keyframe[] keyframes, float bMin, float bMax)
        {
            float aMin = keyframes.First().time;
            float aMax = keyframes.Last().time;
            return keyframes.Select((keyframe) => new Keyframe(keyframe.time.Remap(aMin, aMax, bMin, bMax), keyframe.value, -keyframe.inTangent, -keyframe.outTangent)).ToArray();
        }*/

        /*private float Remap(float value, float aMin, float aMax, float bMin, float bMax)
        {
            return Mathf.Lerp(bMin, bMax, Mathf.InverseLerp(aMin, aMax, value));
        }*/
    }
}