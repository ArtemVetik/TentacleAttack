#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using UnityEditor;
using System.Text;
[CustomEditor(typeof(AccessoryDataBase))]
public class AccessoryDataBaseEditor : Editor
{
    private AccessoryDataBase _dataBase;
    private SerializedProperty _dataBaseList;
    private FieldInfo _dataBaseListInfo;
    private int _currentRenderIndex;
    private GameObject _previewGameObject;
    private Editor _previewGameObjectEditor;

    private void OnEnable()
    {
        _dataBase = target as AccessoryDataBase;

        FieldInfo[] allFields = _dataBase.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
        _dataBaseListInfo = allFields.ToList().Find((field) => field.FieldType.IsEquivalentTo(typeof(List<AccessoryData>)));

        _currentRenderIndex = 0;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        _dataBaseList = serializedObject.FindProperty(_dataBaseListInfo.Name);

        GUIStyle headerTextStyle = new GUIStyle();
        headerTextStyle.fontSize = 35;
        headerTextStyle.normal.textColor = Color.green;

        GUIStyle defaultTextStyle = new GUIStyle();
        defaultTextStyle.fontSize = 18;
        defaultTextStyle.normal.textColor = Color.white;

        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField("Accessory DATA BASE", headerTextStyle);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(50);

        var failedIndexes = GetFailedIndexes(_dataBaseList);
        if (failedIndexes.Count > 0)
        {
            StringBuilder value = new StringBuilder();
            foreach (var index in failedIndexes)
                value.Append(index + ", ");

            EditorGUILayout.HelpBox("Не вся база заполнена!\nПроверьте следующие элементы:\n" + value, MessageType.Warning);
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("_defaultAccessoryIndex"));
        GUILayout.Space(20);

        EditorGUILayout.LabelField("Accessory count: " + _dataBaseList.arraySize, defaultTextStyle);
        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Current Accessory index: " + _currentRenderIndex, defaultTextStyle);

        if (GUILayout.Button(new GUIContent("<", "Предыдущий элемент"), GUILayout.Width(30), GUILayout.Height(30)))
            _currentRenderIndex = (_currentRenderIndex > 0) ? _currentRenderIndex - 1 : _currentRenderIndex;
        if (GUILayout.Button(new GUIContent(">", "Следующий элемент"), GUILayout.Width(30), GUILayout.Height(30)))
            _currentRenderIndex = (_currentRenderIndex < _dataBaseList.arraySize - 1) ? _currentRenderIndex + 1 : _currentRenderIndex;
        GUILayout.FlexibleSpace();
        if (GUILayout.Button(new GUIContent("+", "Добавить"), GUILayout.Width(30), GUILayout.Height(30)))
        {
            _dataBase.Add(new AccessoryData());
            return;
        }
        if (GUILayout.Button(new GUIContent("-", "Удалить"), GUILayout.Width(30), GUILayout.Height(30)))
            _dataBase.RemoveAt(_currentRenderIndex);

        if (_currentRenderIndex >= _dataBase.Data.Count())
            _currentRenderIndex = _dataBase.Data.Count() - 1;
        if (_currentRenderIndex < 0)
            _currentRenderIndex = 0;

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.Space(30);

        if (_dataBase.Data.Count() != 0)
        {
            var element = _dataBaseList.GetArrayElementAtIndex(_currentRenderIndex);
            RenderElement(element);
        }

        GUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
    }

    private void RenderElement(SerializedProperty element)
    {
        EditorGUILayout.PropertyField(element.FindPropertyRelative("_name"));
        EditorGUILayout.PropertyField(element.FindPropertyRelative("_preview"));
        EditorGUILayout.PropertyField(element.FindPropertyRelative("_prefab"));

        var sprite = element.FindPropertyRelative("_preview").objectReferenceValue as Sprite;
        var preview = element.FindPropertyRelative("_prefab").objectReferenceValue as GameObject;

        GUILayout.Space(50);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (sprite)
            GUILayout.Label(sprite.texture, GUILayout.Width(128), GUILayout.Height(128));

        GUILayout.FlexibleSpace();

        if (preview)
        {
            if (_previewGameObject == null || preview.Equals(_previewGameObject) == false)
                _previewGameObjectEditor = Editor.CreateEditor(preview);

            _previewGameObject = preview;

            if (_previewGameObject != null)
                _previewGameObjectEditor.OnInteractivePreviewGUI(GUILayoutUtility.GetRect(200, 200), GUIStyle.none);
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    private List<int> GetFailedIndexes(SerializedProperty array)
    {
        var failedList = new List<int>();

        for (int i = 0; i < array.arraySize; i++)
        {
            var fieldList = new List<object>();
            var element = _dataBaseList.GetArrayElementAtIndex(i);

            fieldList.Add(element.FindPropertyRelative("_name").stringValue);
            fieldList.Add(element.FindPropertyRelative("_preview").objectReferenceValue);
            fieldList.Add(element.FindPropertyRelative("_prefab").objectReferenceValue);

            if (fieldList.Any(field => field == null))
                failedList.Add(i);
        }

        return failedList;
    }
}

#endif