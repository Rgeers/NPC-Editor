using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Linq;

public class NPCCreator : EditorWindow {
    private int strength, intelligence, social;
    private string pickedname = "", description = "";
    private bool friendly, goodmannered, teamplayer, gunpro, meleepro, peaceful;
    public int currentTextureCount = 0;
    public Texture[] faces;

    void OnEnable() {
        faces = Resources.LoadAll("Textures/Faces", typeof(Texture)).Cast<Texture>().ToArray();
    }


    [MenuItem("NPC Editor/NPC Creator")]


    static void Init() {
        NPCCreator window = (NPCCreator)EditorWindow.GetWindow(typeof(NPCCreator));
        window.Show();
        window.minSize = new Vector2(300, 800);

    }

    #region Creation of the custom Label function
    void AddLabel(string labeltext, bool bold) {
        if (bold) {
            GUILayout.Label(labeltext, EditorStyles.boldLabel);
        } else {
            GUILayout.Label(labeltext, EditorStyles.label);
        }
    
    }
    #endregion

    #region Creation of the custom Horizontal functions
    void BeginHorizontal(bool useFlexible) {
        GUILayout.BeginHorizontal();
        if (useFlexible) {
            GUILayout.FlexibleSpace();
        }
    }

    void EndHorizontal(bool useFlexible, bool useSpace) {
        if (useFlexible) {
            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();
        if (useSpace) {
            GUILayout.Space(20);
        }
    }
    #endregion

    void OnGUI() {

        #region Biography and image loading
        BeginHorizontal(true);
        AddLabel("Biography", true);
        EndHorizontal(true, true);
        BeginHorizontal(false);
        if (GUILayout.Button("<", GUILayout.Width(30), GUILayout.Height(200))) {
            if (currentTextureCount != 0) {
                currentTextureCount--;
            } else {
                currentTextureCount = faces.Length - 1;
            }
        }
        GUILayout.Space(position.width - 70);
        if (GUILayout.Button(">", GUILayout.Width(30), GUILayout.Height(200))) {
            if (currentTextureCount != faces.Length - 1) {
                currentTextureCount++;
            } else {
                currentTextureCount = 0;
            }
        }
        EndHorizontal(false, false);

        EditorGUI.DrawPreviewTexture(new Rect(position.width / 2 - 100, 45, 200, 200), faces[currentTextureCount]);

        GUILayout.Space(20);
        #endregion

        #region Naming and description
        BeginHorizontal(true);
        AddLabel("Name:", false);
        EndHorizontal(true, true);
        pickedname = GUI.TextField(new Rect(position.width/2-100, 285, 200, 20), pickedname, 25);

        BeginHorizontal(true);
        AddLabel("Description:", false);
        EndHorizontal(true, true);
        description = GUI.TextField(new Rect(position.width / 2 - 100, 325, 200, 20), description, 25);

        GUILayout.Space(20);
        #endregion

        #region Statistics

        BeginHorizontal(true);
        AddLabel("Statistics", true);
        EndHorizontal(true, false);

        BeginHorizontal(true);
        AddLabel("Strength:", false);
        EndHorizontal(true, true);
        strength = EditorGUI.IntSlider(new Rect(position.width / 2 - 100, 405, 200, 20), strength, 1, 10);

        BeginHorizontal(true);
        AddLabel("Intelligence:", false);
        EndHorizontal(true, true);

        intelligence = EditorGUI.IntSlider(new Rect(position.width / 2 - 100, 440, 200, 20), intelligence, 1, 10);

        BeginHorizontal(true);
        AddLabel("Social skills:", false);
        EndHorizontal(true, true);

        social = EditorGUI.IntSlider(new Rect(position.width / 2 - 100, 475, 200, 20), social, 1, 10);

        #endregion

        #region Perks
        BeginHorizontal(true);
        AddLabel("Perks", true);
        EndHorizontal(true, false);
        BeginHorizontal(true);
        AddLabel("Overal skills:", false);
        EndHorizontal(true, true);
        BeginHorizontal(true);
        friendly = EditorGUILayout.Toggle("Friendly", friendly);
        EndHorizontal(true, false);
        BeginHorizontal(true);
        goodmannered = EditorGUILayout.Toggle("Goodmannered", goodmannered);
        EndHorizontal(true, false);
        BeginHorizontal(true);
        teamplayer = EditorGUILayout.Toggle("Teamplayer", teamplayer);
        EndHorizontal(true, false);
        BeginHorizontal(true);
        gunpro = EditorGUILayout.Toggle("Is good with guns", gunpro);
        EndHorizontal(true, false);
        BeginHorizontal(true);
        meleepro = EditorGUILayout.Toggle("Is goed with melee", meleepro);
        EndHorizontal(true, false);
        BeginHorizontal(true);
        peaceful = EditorGUILayout.Toggle("Is peaceful", peaceful);
        EndHorizontal(true, true);
        #endregion

        #region Load button
        if (GUI.Button(new Rect(position.width / 2 - 100, 725, 200, 20), "Load NPC")) {
            string NPCpath = EditorUtility.OpenFilePanel("Load NPC as Asset", "Assets/Resources/Saved NPCs", "asset");
            string relativePath = NPCpath.Substring(NPCpath.IndexOf("Assets/"));
            AssetDatabase.Refresh();
            NPCSaver openedNPC = UnityEditor.AssetDatabase.LoadAssetAtPath<NPCSaver>(relativePath);
            EditorUtility.FocusProjectWindow();
            currentTextureCount = openedNPC.currentTextureCount;
            strength = openedNPC.strength;
            intelligence = openedNPC.intelligence;
            social = openedNPC.social;
            pickedname = openedNPC.pickedname;
            description = openedNPC.description;
            friendly = openedNPC.friendly;
            goodmannered = openedNPC.goodmannered;
            teamplayer = openedNPC.teamplayer;
            gunpro = openedNPC.gunpro;
            peaceful = openedNPC.peaceful;
            AssetDatabase.Refresh();
            
        }
        #endregion

        #region Save button
        BeginHorizontal(true);
        if (GUI.Button(new Rect(position.width / 2 - 100, 750, 200, 20), "Save NPC")) {
            NPCSaver newNPC = ScriptableObject.CreateInstance<NPCSaver>();
            string savepath = EditorUtility.SaveFilePanelInProject("Save NPC as Asset", pickedname, "asset", "Save the NPC as an asset file for the NPC Inventory Tool to be able to work with.", (Application.dataPath + "/Saved NPCs"));
            AssetDatabase.CreateAsset(newNPC, savepath);
            #region Saving to ScriptableObjects
            newNPC.currentTextureCount = currentTextureCount;
            newNPC.strength = strength;
            newNPC.intelligence = intelligence;
            newNPC.social = social;
            newNPC.pickedname = pickedname;
            newNPC.description = description;
            newNPC.friendly = friendly;
            newNPC.goodmannered = goodmannered;
            newNPC.teamplayer = teamplayer;
            newNPC.gunpro = gunpro;
            newNPC.meleepro = meleepro;
            newNPC.peaceful = peaceful;
            #endregion
            AssetDatabase.Refresh();
            EditorUtility.SetDirty(newNPC);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();





        }
        EndHorizontal(true, true);
        #endregion

    }
}