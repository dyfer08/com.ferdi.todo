using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System;
 
namespace Ferdi{
public class TODO_Window : EditorWindow{

    Vector2 ScrollPos;
    static TODO_Data DatabaseObject = null;
    SerializedObject SO;
    public ReorderableList TODOList = null;
    public static TODO_Window Instance { get;set; }

    [MenuItem("Window/TODO List")]
    public static void ShowWindow(){
        EditorWindow.GetWindow<TODO_Window>("TODO List");
    }
 
    void OnEnable(){
        
        DatabaseObject = (TODO_Data)AssetDatabase.LoadAssetAtPath("Packages/com.ferdi.todo/Settings/TODO.asset", typeof(TODO_Data));

        Instance = this;
        SO = new SerializedObject(DatabaseObject);
        TODOList = new ReorderableList(SO, SO.FindProperty("TODOList"),true, false, true, true);
        // TODOList.elementHeight = 25;
        TODOList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>{
            var element = TODOList.serializedProperty.GetArrayElementAtIndex(index);

            rect.y += 2;

            switch(element.FindPropertyRelative("Type").intValue){
                case 4:
                    GUI.enabled = false;
                break;

                default:
                    GUI.enabled = true;
                break;
            }

            EditorGUI.PropertyField( new Rect(rect.x, rect.y, EditorGUIUtility.currentViewWidth -55, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("Task"), GUIContent.none);
            GUI.enabled = true;

            switch(element.FindPropertyRelative("Type").intValue){

                case 0:
                case 4:
                    GUI.color = Color.white;
                break;

                case 1:
                    GUI.color = Color.green;
                break;

                case 2:
                    GUI.color = Color.yellow;
                break;

                case 3:
                    GUI.color = Color.red;
                break;

            }

            EditorGUI.PropertyField( new Rect(EditorGUIUtility.currentViewWidth -28, rect.y, 20, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("Type"), GUIContent.none);
            GUI.color = Color.white;
        };

        TODOList.onAddCallback = (ReorderableList l) => {
            var index = l.serializedProperty.arraySize;
            l.serializedProperty.arraySize++;
            l.index = index;
            var element = l.serializedProperty.GetArrayElementAtIndex(index);
            element.FindPropertyRelative("Task").stringValue = "";
            element.FindPropertyRelative("Type").intValue = 0;
        };
    }

 
    void OnGUI(){
        ScrollPos = EditorGUILayout.BeginScrollView(ScrollPos);
        
        GUILayout.Space(10);

        EditorGUI.DrawRect(new Rect(0, 0, Screen.width, 1), new Color32(0,0,0,100));
        EditorGUI.DrawRect(new Rect(0, 1, Screen.width, 32), new Color32(0,0,0,25));
        EditorGUI.DrawRect(new Rect(0, 32, Screen.width, 1), new Color32(0,0,0,100));

        EditorGUILayout.BeginHorizontal ();
            GUILayout.FlexibleSpace();
            GUILayout.Label ("TODO List", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(5);

        SO.Update();
        TODOList.DoLayoutList();
        SO.ApplyModifiedProperties();
        EditorGUILayout.EndScrollView();
    }
}
}