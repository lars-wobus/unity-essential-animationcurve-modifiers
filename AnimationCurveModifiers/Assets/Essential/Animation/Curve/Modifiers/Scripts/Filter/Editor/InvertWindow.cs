using System.Linq;
using Essential.Core.Extensions;
using UnityEditor;
using UnityEngine;

namespace Essential.Animation.Curve.Modifiers.Filter.Editor
{
    /// <summary>
    /// Window to mirror selected AnimationCurve in X, Y or both axes.
    /// </summary>
    public class InvertWindow : AnimationCurveModifierBase
    {
        private const string MenuItemName = "Window/Animation Curve/Inverter";
        private static string InvertWindow_1stHeader = "Input";
        private static string InvertWindow_2ndHeader = "Output";
        
        /// <summary>
        /// Reflection across the X-axis.
        /// </summary>
        private AnimationCurve InversionByX { get; set; }
        
        /// <summary>
        /// Reflection across the Y-axis.
        /// </summary>
        private AnimationCurve InversionByY { get; set; }
        
        /// <summary>
        /// Reflection across the X- and Y-axes.
        /// </summary>
        private AnimationCurve InversionByXy { get; set; }
        
        /// <summary>
        /// When flag is enabled, reflections will take up the same X-range as the original.
        /// </summary>
        private bool KeepCurveInPlace { get; set; }
        
        /// <summary>
        /// When flag is enabled, all keys of reflections will be remapped between 0 and 1.  
        /// </summary>
        private bool Normalize { get; set; }
        
        /// <summary>
        /// Add window under Window menu.
        /// </summary>
        [MenuItem(MenuItemName)]
        private static void Init()
        {
            var window = (InvertWindow)GetWindow(typeof(InvertWindow));
            window.Show();
        }

        /// <inheritdoc cref="EditorWindow.OnEnable"/>
        private void OnEnable()
        {
            InversionByX = new AnimationCurve();
            InversionByY = new AnimationCurve();
            InversionByXy = new AnimationCurve();
        }

        /// <summary>
        /// Populate window.
        /// </summary>
        private void OnGUI()
        {
            EditorGUILayout.Space();
            DisplayHeader(InvertWindow_1stHeader);
            var isValid = DisplayInputFields();
            var fields = GetFields();
            var names = GetFieldNames(fields);
            DisplayPrimaryInputFields(!isValid, names);
            DisplaySecondaryInputFields(!isValid);
            
            EditorGUILayout.Space();
            DisplayHeader(InvertWindow_2ndHeader);
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
                        RemapOutputCurves(bMin, bMax);
                    }

                    if (Normalize)
                    {
                        RemapOutputCurves(0, 1);
                    }
                }
            }
            DisplayOutputFields(!isValid);
        }

        private void RemapOutputCurves(float bMin, float bMax)
        {
            InversionByX.keys = InversionByX.keys.RemapTimes(bMin, bMax);
            InversionByY.keys = InversionByY.keys.RemapTimes(bMin, bMax);
            InversionByXy.keys = InversionByXy.keys.RemapTimes(bMin, bMax);
        }

        /// <summary>
        /// Display component field used to select target script.
        /// </summary>
        /// <returns>True when field is filled. False when field is empty.</returns>
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

        /// <summary>
        /// Display 1 field to select AnimationCurve from selected target script.
        /// </summary>
        /// <param name="disabled">Hide fields on true. Show fields on false.</param>
        /// <param name="names">Field names belonging to AnimationCurves in target script.</param>
        private void DisplayPrimaryInputFields(bool disabled, string[] names)
        {
            EditorGUI.BeginDisabledGroup(disabled);
            DropDownIndex = EditorGUILayout.Popup("Field", DropDownIndex, names ?? new string[0]);
            EditorGUI.EndDisabledGroup();
            
        }
        
        /// <summary>
        /// Display 3 fields to adjust the outcome.
        /// </summary>
        /// <param name="disabled">Hide fields on true. Show fields on false.</param>
        private void DisplaySecondaryInputFields(bool disabled)
        {
            EditorGUI.BeginDisabledGroup(disabled);
            KeepCurveInPlace = EditorGUILayout.Toggle("Keep Curve in Place", KeepCurveInPlace);
            Normalize = EditorGUILayout.Toggle("Normalize", Normalize);
            EditorGUI.EndDisabledGroup();
        }
        
        /// <summary>
        /// Display 3 reflected AnimationCurves based on selected AnimationCurve.
        /// </summary>
        /// <param name="disabled">Hide fields on true. Show fields on false.</param>
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