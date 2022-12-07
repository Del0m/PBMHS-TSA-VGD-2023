using UnityEngine;
using UnityEditor;

//Make it as a creatable object inside the project 
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public bool isImmediatelyConsumable = false;
    public Sprite icon = null;

    [Header("Item Parameters")]
    public itemClass itemType;
    public float Amount = 0f;
    //for the purposes of shops
    public float value = 0f;
    // for duration of its effects
    
    [HideInInspector]
    public float duration = 0f;

    [HideInInspector]
    public bool isJumpType = false;

    [Header("Collider Type")]
    public colliderType colliderType;

    [HideInInspector]
    public float radius = 0.095f;

    [HideInInspector] 
    public Vector2 size = new Vector2(0.2f, 0.2f);
    
    [Header("Item Object Size")]
    public Vector2 objectSize = new Vector3(4, 4, 1);

    private void OnValidate()
    {
        CheckVariables();
#if UNITY_EDITOR
        if (showFloat.instance != null)
        {
            showFloat.instance.OnInspectorGUI();
        }
        else
        {
            return;
        }
#endif
    }

    void CheckVariables()
    {
        float i = duration;
        if (i == 0 && itemType == itemClass.movementOrJump)
        {
            Debug.LogWarning(this.name + " 's duration is set to 0");
            return;
        }
        if (Amount == 0f && itemType != itemClass.coins)
        {
            Debug.LogWarning(this.name + "'s amount are set to 0");
            return;
        }
        else if (!isImmediatelyConsumable || itemType == itemClass.coins)
        {
            if (value == 0f)
            {
                Debug.LogWarning(this.name + "'s value are set to 0");
                return;
            }
        }
    }
}

public enum itemClass { health, movementOrJump, coins}
public enum colliderType { circle, box}

#if UNITY_EDITOR
[CustomEditor(typeof(Item))]
public class showFloat : Editor
{
    public static showFloat instance;

    private void OnValidate()
    {
        if(instance != this)
        {
            instance = this;
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Item item = (Item)target;

        if (item.itemType == itemClass.movementOrJump)
        {
            EditorGUILayout.LabelField("Movement or Jump Parameters", EditorStyles.boldLabel, GUILayout.MinHeight(24));
            item.duration = EditorGUILayout.FloatField("Duration", item.duration);
            item.isJumpType = EditorGUILayout.Toggle("Is Jump Type", item.isJumpType);
        }
        colliderToggle(item);
        saveButton(item);
    }

    static void saveButton(Item item)
    {
        if(GUILayout.Button("Save Changes"))
        {
            EditorUtility.SetDirty(item);
            AssetDatabase.SaveAssets();
            PrefabUtility.RecordPrefabInstancePropertyModifications(item);
        }
    }

    void colliderToggle(Item item)
    {
        if(item.colliderType == colliderType.circle)
        {
            EditorGUILayout.LabelField("Cirlce Collider parameters", EditorStyles.boldLabel, GUILayout.MinHeight(24));
            item.radius = EditorGUILayout.FloatField("Radius", item.radius);
        }
        else if(item.colliderType == colliderType.box)
        {
            EditorGUILayout.LabelField("Box Collider Parameters", EditorStyles.boldLabel, GUILayout.MinHeight(24));
            item.size = EditorGUILayout.Vector2Field("Box size", item.size);
        }
    }
}
#endif