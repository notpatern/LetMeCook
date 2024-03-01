using Level.LevelDesign;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(MovingPlatformKey))]
public class PlatformKeyEditor : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        // Create a new VisualElement to be the root the property UI
        var container = new VisualElement();

        var popup = new UnityEngine.UIElements.PopupWindow
        {
            text = "Key Details"
        };
        popup.Add(new PropertyField(property.FindPropertyRelative("position"), "Position"));
        popup.Add(new PropertyField(property.FindPropertyRelative("rotation"), "Rotation"));
        popup.Add(new PropertyField(property.FindPropertyRelative("pauseBeforeMoving"), "Time Before Travel"));
        popup.Add(new PropertyField(property.FindPropertyRelative("travelTime"), "Travel Duration (s)"));
        container.Add(popup);

        // Return the finished UI
        return container;
    }
}