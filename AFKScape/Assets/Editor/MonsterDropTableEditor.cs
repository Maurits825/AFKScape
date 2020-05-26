using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MonsterDropTableAdder))]
public class MonsterDropTableEditor : Editor
{
    SerializedProperty monsterDropTableHandler;
    SerializedProperty generalDropTables;

    MonsterDropTable tempDropTable;

    private string monsterName;
    private int monsterInd = 0;
    private int prevMonsterInd = 0;

    private bool JSONLoaded = false;

    void OnEnable()
    {
        monsterDropTableHandler = serializedObject.FindProperty("monsterDropTableHandler");
        generalDropTables = monsterDropTableHandler.FindPropertyRelative("generalDropTables");
    }

    public override void OnInspectorGUI()
    {
        MonsterDropTableAdder monsterDropTableAdder = (MonsterDropTableAdder)target;

        if (GUILayout.Button("Load Bosses"))
        {
            Database.LoadBosses();
        }

        monsterInd = EditorGUILayout.Popup("Select Boss:", monsterInd, Database.bossesNames.ToArray());
        if (Database.bossesNames.Count != 0)
        {
            monsterName = Database.bossesNames[monsterInd];
        }

        if (monsterInd != prevMonsterInd)
        {
            JSONLoaded = false;
        }

        if (GUILayout.Button("Load JSON"))
        {
            monsterDropTableAdder.monsterDropTableHandler = JsonHandler.GetMonsterDropTableHandler(monsterName);
            serializedObject.Update();
            prevMonsterInd = monsterInd;
            JSONLoaded = true;
        }

        if (monsterName != null && JSONLoaded)
        {
            EditorGUILayout.LabelField("General Info", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(monsterDropTableHandler.FindPropertyRelative("rolls"));

            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField("100% Drop and Tertiary", EditorStyles.boldLabel);
            if (GUILayout.Button("Add"))
            {
                monsterDropTableAdder.monsterDropTableHandler.generalDropTables.Add(new GeneralDropTable());
            }
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(generalDropTables, false);
            if (generalDropTables.isExpanded)
            {
                EditorGUI.indentLevel++;
                foreach (SerializedProperty table in generalDropTables)
                {
                    EditorGUILayout.PropertyField(table, false);
                    if (table.isExpanded)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(table.FindPropertyRelative("name"));
                        EditorGUILayout.PropertyField(table.FindPropertyRelative("numRolls"));
                        EditorGUILayout.PropertyField(table.FindPropertyRelative("lootItems"));
                        EditorGUI.indentLevel--;
                    }
                }
                EditorGUI.indentLevel--;

            }
            EditorGUI.indentLevel--;

            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField("General Loot", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(monsterDropTableHandler.FindPropertyRelative("baseChance"));

            if (GUILayout.Button("Add Loot from temp.json"))
            {
                tempDropTable = JsonHandler.GetMonsterDropTable("temp");
                monsterDropTableAdder.monsterDropTableHandler.basicLoots = tempDropTable.basicLoots;
                monsterDropTableAdder.monsterDropTableHandler.baseChance = tempDropTable.baseChance;
            }

            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(monsterDropTableHandler.FindPropertyRelative("basicLoots"));
            EditorGUI.indentLevel--;

            EditorGUILayout.LabelField("Other tables", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            if (GUILayout.Button("Add Rare Drop Table"))
            {
                monsterDropTableAdder.monsterDropTableHandler.preMadeTables.Add(new MonsterDropTableHandler.TableInfo("rare_drop_table", 0));
            }
            if (GUILayout.Button("Add Tree-Herb Seed Table"))
            {
                monsterDropTableAdder.monsterDropTableHandler.preMadeTables.Add(new MonsterDropTableHandler.TableInfo("tree_herb_seed", 0));
            }
            if (GUILayout.Button("Add Custom"))
            {
                monsterDropTableAdder.monsterDropTableHandler.monsterDropTables.Add(new MonsterDropTable());
            }
            EditorGUILayout.PropertyField(monsterDropTableHandler.FindPropertyRelative("monsterDropTables"));

            SerializedProperty preMadeTables = monsterDropTableHandler.FindPropertyRelative("preMadeTables");
            EditorGUILayout.PropertyField(preMadeTables);

            EditorGUI.indentLevel--;

            EditorGUI.indentLevel--;
            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Apply"))
            {
                JsonHandler.SaveJsonFile(monsterDropTableAdder.monsterDropTableHandler, monsterName);
            }
        }
    }
}
