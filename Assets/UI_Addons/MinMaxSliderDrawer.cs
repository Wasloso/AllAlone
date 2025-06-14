#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
public class MinMaxSliderDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Ensure the property is a Vector2 (for min/max values)
        if (property.propertyType != SerializedPropertyType.Vector2)
        {
            EditorGUI.HelpBox(position, "Use MinMaxSlider with a Vector2 property.", MessageType.Error);
            return;
        }

        // Get the MinMaxSliderAttribute
        var minMax = attribute as MinMaxSliderAttribute;

        // Get the current min/max values from the Vector2 property
        var currentMinMax = property.vector2Value;
        var currentMin = currentMinMax.x;
        var currentMax = currentMinMax.y;

        // Calculate the rects for the label and the slider
        var controlRect = EditorGUI.PrefixLabel(position, label);

        // Define the width of the number fields on either side of the slider
        var fieldWidth = 40f;
        var sliderWidth = controlRect.width - fieldWidth * 2 - 5f; // 5f for a small gap

        var minFieldRect = new Rect(controlRect.x, controlRect.y, fieldWidth, controlRect.height);
        var sliderRect = new Rect(minFieldRect.xMax + 2f, controlRect.y, sliderWidth, controlRect.height);
        var maxFieldRect = new Rect(sliderRect.xMax + 2f, controlRect.y, fieldWidth, controlRect.height);

        // Draw the min value field
        currentMin = EditorGUI.FloatField(minFieldRect, currentMin);

        // Draw the slider
        EditorGUI.MinMaxSlider(sliderRect, ref currentMin, ref currentMax, minMax.Min, minMax.Max);

        // Draw the max value field
        currentMax = EditorGUI.FloatField(maxFieldRect, currentMax);

        // Clamp the values to the attribute's min/max and ensure min <= max
        currentMin = Mathf.Clamp(currentMin, minMax.Min, minMax.Max);
        currentMax = Mathf.Clamp(currentMax, minMax.Min, minMax.Max);

        if (currentMin > currentMax)
            currentMax = currentMin;
        if (currentMax < currentMin)
            currentMin = currentMax;

        // Apply the new values back to the SerializedProperty
        property.vector2Value = new Vector2(currentMin, currentMax);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight;
    }
}
#endif