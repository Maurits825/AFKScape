using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ItemIdAttribute: PropertyAttribute
{
    public ItemIdAttribute()
    {

    }
}

[CustomPropertyDrawer(typeof(ItemIdAttribute))]
public class ItemIdPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        Rect idRect = position;
        idRect.width = idRect.width / 2;
        EditorGUI.PropertyField(position, property);

        Rect nameRect = position;
        nameRect.x = nameRect.x + position.width;

        string itemName;
        try
        {
            itemName = Database.items[property.intValue].name;
        }
        catch (KeyNotFoundException)
        {
            itemName = "Not Found in Dict";
        }
        EditorGUI.LabelField(nameRect, "Name: " + itemName);
        EditorGUI.EndProperty();
    }
}

