using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class ItemIdAttribute: PropertyAttribute
{
    public ItemIdAttribute()
    {

    }
}
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ItemIdAttribute))]
public class ItemIdPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        Rect idRect = position;
        idRect.width = idRect.width / 2;
        EditorGUI.PropertyField(idRect, property);
        

        Rect nameRect = position;
        nameRect.x = nameRect.x + idRect.width;

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

        Rect spriteRect = nameRect;
        spriteRect.x = position.width /2;
        GUI.DrawTexture(spriteRect, Resources.Load<Texture>("Icons/" + property.intValue.ToString()), ScaleMode.ScaleToFit);
        EditorGUI.EndProperty();
    }
}
#endif
