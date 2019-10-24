using UnityEngine;
using UnityEditor;
#if UNITY_EDITOR
public class AITypeAttribute : PropertyAttribute {
    
    private string[] names;
    private int[] indices;
    private AIType aiType;

    public string[] Names {
        get {
            if(names == null) {
                names = new string[0];
            }

            return names;
        }
        set {
            names = value;
        }
    }

    public int[] Indices {
        get {
            if (indices == null) {
                indices = new int[0];
            }

            return indices;
        }
        set {
            indices = value;
        }
    }


    public AITypeAttribute() {
        aiType = Resources.Load<AIType>("AI/AIType");
        if (IsValid) {
            names = new string[aiType.ais.Length];
            indices = new int[aiType.ais.Length];
            for (int i = 0; i < aiType.ais.Length; i++) {
                names[i] = aiType.ais[i].GetData.Name;
                indices[i] = i;
            }
        }
    }

    public bool IsValid {
        get {
            if (aiType.ais == null || aiType.ais.Length == 0)
                return false;
            return true;
        }
    }
}

[CustomPropertyDrawer(typeof(AITypeAttribute))]
public class AITypeDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

        AITypeAttribute aiType = attribute as AITypeAttribute;

        if (property.propertyType == SerializedPropertyType.Integer) {
            property.intValue = EditorGUI.IntPopup(position, property.name, property.intValue, aiType.Names, aiType.Indices);
        }

    }
}
#endif