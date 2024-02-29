using System;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor; 
using UnityEngine;

    // This is the custom editor script
    [CustomEditor(typeof(MonoBehaviour), true)] // This tells Unity that this script edits any script that inherits from MonoBehaviour
    public class EditorButton : Editor {
        public override void OnInspectorGUI() {
            // Draw the default inspector
            DrawDefaultInspector();

            // Get the script that is being edited
            MonoBehaviour script = (MonoBehaviour)target;

            // Get all the methods of the script
            MethodInfo[] methods = script.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

            // Loop through all the methods
            foreach (MethodInfo method in methods) {
                // Get the ButtonAttribute of the method
                ButtonAttribute buttonAttribute = (ButtonAttribute)Attribute.GetCustomAttribute(method, typeof(ButtonAttribute));

                // If the method has the ButtonAttribute
                if (buttonAttribute != null) {
                    // Create a button with the text from the attribute
                    if (GUILayout.Button(buttonAttribute.buttonText)) {
                        // Invoke the method
                        method.Invoke(script, null);
                    }
                }
            }
        }
    }
#endif