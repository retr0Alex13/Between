using Between;
using Between.Data;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class MenuBarTools
{
    private const string MENU_ROOT = "Between/";

    static MenuBarTools() { }

    [MenuItem(MENU_ROOT + "Open Level Selector", priority = 0)]
    public static void OpenLevelSelectorWindow()
    {
        LevelSelectorWindow.ShowWindow();
    }
}

public class LevelSelectorWindow : EditorWindow
{
    private GameObjectsData _gameObjectsData;
    private Vector2 _scrollPos;
    private string _searchQuery = "";

    public static void ShowWindow()
    {
        var window = GetWindow<LevelSelectorWindow>("Level Selector");
        window.minSize = new Vector2(300, 400);
        window.LoadGameObjectsData();
        window.Show();
    }

    private void OnEnable() => LoadGameObjectsData();

    private void LoadGameObjectsData()
    {
        string[] guids = AssetDatabase.FindAssets("t:GameObjectsData");

        if (guids.Length == 0)
        {
            _gameObjectsData = null;
            return;
        }

        string path = AssetDatabase.GUIDToAssetPath(guids[0]);
        _gameObjectsData = AssetDatabase.LoadAssetAtPath<GameObjectsData>(path);
    }

    private void OnGUI()
    {
        GUILayout.Space(8);
        GUILayout.Label("Level Selector", EditorStyles.boldLabel);
        GUILayout.Space(4);

        if (_gameObjectsData == null)
        {
            EditorGUILayout.HelpBox("GameObjectsData ScriptableObject не знайдено у проєкті.", MessageType.Error);

            if (GUILayout.Button("Спробувати знайти знову"))
                LoadGameObjectsData();

            return;
        }

        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.ObjectField("Data", _gameObjectsData, typeof(GameObjectsData), false);
        EditorGUI.EndDisabledGroup();

        GUILayout.Space(4);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Search:", GUILayout.Width(50));
        _searchQuery = GUILayout.TextField(_searchQuery);
        if (GUILayout.Button("✕", GUILayout.Width(22)))
            _searchQuery = "";
        GUILayout.EndHorizontal();

        GUILayout.Space(4);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        var levels = _gameObjectsData.Levels;

        if (levels == null || levels.Length == 0)
        {
            EditorGUILayout.HelpBox("Масив Levels у GameObjectsData порожній.", MessageType.Warning);
            return;
        }

        _scrollPos = GUILayout.BeginScrollView(_scrollPos);

        int shownCount = 0;
        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i] == null) continue;

            string levelName = levels[i].name;

            if (!string.IsNullOrEmpty(_searchQuery) &&
                !levelName.ToLower().Contains(_searchQuery.ToLower()))
                continue;

            shownCount++;
            DrawLevelRow(levelName, i);
        }

        GUILayout.EndScrollView();

        GUILayout.Space(4);
        GUILayout.Label($"Shown: {shownCount} / {levels.Length}", EditorStyles.miniLabel);
    }

    private void DrawLevelRow(string levelName, int levelIndex)
    {
        bool isCurrent = PlayerPrefs.GetInt(Constants.CURRENT_LEVEL_KEY, -1) == levelIndex;

        var style = isCurrent
            ? new GUIStyle(EditorStyles.helpBox) { normal = { background = Texture2D.grayTexture } }
            : EditorStyles.helpBox;

        GUILayout.BeginHorizontal(style);
        GUILayout.Label($"{levelIndex + 1}. {levelName}", GUILayout.ExpandWidth(true));

        if (isCurrent)
            GUILayout.Label("✓", GUILayout.Width(18));

        if (GUILayout.Button("Select", GUILayout.Width(46)))
            SelectLevel(levelIndex);

        GUILayout.EndHorizontal();
        GUILayout.Space(2);
    }

    private void SelectLevel(int levelIndex)
    {
        PlayerPrefs.SetInt(Constants.CURRENT_LEVEL_KEY, levelIndex);
        Repaint();
    }
}