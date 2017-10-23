using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        string valueString;

        switch (property.propertyType)
        {
            case SerializedPropertyType.Integer:
                valueString = property.intValue.ToString();
                break;
            case SerializedPropertyType.Boolean:
                valueString = property.boolValue.ToString();
                break;
            case SerializedPropertyType.Float:
                valueString = property.floatValue.ToString("0.00000");
                break;
            case SerializedPropertyType.String:
                valueString = property.stringValue;
                break;
            default:
                valueString = string.Format("Unsupported Type ({0})", property.GetType());
                break;
        }
        EditorGUI.LabelField(position, label.text, valueString);
    }
}