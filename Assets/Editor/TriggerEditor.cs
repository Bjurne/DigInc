using UnityEditor;
using UnityEditor.EventSystems;

[CustomEditor(typeof(PriceTagTrigger))]
public class TriggerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //DrawDefaultInspector();
        //PriceTagTrigger myPriceTagTrigger = (PriceTagTrigger)target;
        //myPriceTagTrigger.priceTagType = EditorGUILayout.IntField("Type of PriceTag", 0);
        //EditorGUILayout.EnumPopup("Type of priceTag", myPriceTagTrigger.thisTagType);
    }
}
