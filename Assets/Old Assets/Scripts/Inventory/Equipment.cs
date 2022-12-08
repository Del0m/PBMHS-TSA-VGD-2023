using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Equipment", menuName = "Inventory/Equipment")]
public class Equipment : ScriptableObject
{
    new public string name = "New Equipment";
    public bool isDefaultItem = false;
    public Sprite icon = null;

    [Header("Equipment Properties")]
    public EquipementClass equipementClass;
    public float damageModifier = 0f;
    public float armorModifier = 0f;
    public float movementModifier = 0f;
    public float value = 0f;

    [Header("Collider Type")]
    public EquipmentColliderType colliderType;

    [HideInInspector]
    public float radius = 0.095f;

    [HideInInspector]
    public Vector2 size = new Vector2(0.2f, 0.2f);

    [Header("Equipment Object ")]
    public Vector2 objectSize = new Vector3(4, 4, 1);

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (showParameters.instance != null)
        {
            showParameters.instance.OnInspectorGUI();
        }
        else
        {
            return;
        }
#endif
    }
}
public enum EquipementClass { ChestPlateOrBracelet, Sandles }
public enum EquipmentColliderType { Circle, Box}

#if UNITY_EDITOR
[CustomEditor(typeof(Equipment))]
public class showParameters : Editor
{
    public static showParameters instance;

    private void OnValidate()
    {
        if (instance != this)
        {
            instance = this;
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Equipment equipment = (Equipment)target;

        if(equipment.colliderType == EquipmentColliderType.Circle)
        {
            EditorGUILayout.LabelField("Cirlce Collider parameters", EditorStyles.boldLabel, GUILayout.MinHeight(24));
            equipment.radius = EditorGUILayout.FloatField("Radius", equipment.radius);
        }else if(equipment.colliderType == EquipmentColliderType.Box)
        {
            EditorGUILayout.LabelField("Box Collider Parameters", EditorStyles.boldLabel, GUILayout.MinHeight(24));
            equipment.size = EditorGUILayout.Vector2Field("Box size", equipment.size);
        }
        saveButton(equipment);
    }

    static void saveButton(Equipment equipment)
    {
        if (GUILayout.Button("Save Changes"))
        {
            EditorUtility.SetDirty(equipment);
            AssetDatabase.SaveAssets();
            PrefabUtility.RecordPrefabInstancePropertyModifications(equipment);
        }
    }
}
#endif