#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public static class PrototypeSceneBuilder
{
    [MenuItem("Nitro Battle Royale/Create Prototype Arena")]
    public static void Create()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        var lightObject = new GameObject("Sun");
        var sun = lightObject.AddComponent<Light>();
        sun.type = LightType.Directional;
        sun.intensity = 1.1f;
        lightObject.transform.rotation = Quaternion.Euler(45, -30, 0);

        var ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground.name = "Arena Ground";
        ground.transform.position = new Vector3(0, -0.5f, 0);
        ground.transform.localScale = new Vector3(70, 1, 70);
        SetColor(ground, new Color(0.30f, 0.60f, 0.30f));

        for (int i = 0; i < 18; i++)
        {
            float a = i * Mathf.PI * 2f / 18f;
            var obstacle = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obstacle.name = "Cover_" + i;
            obstacle.transform.position = new Vector3(Mathf.Cos(a) * 19, 1.5f, Mathf.Sin(a) * 19);
            obstacle.transform.localScale = new Vector3(3, 3, 3);
            SetColor(obstacle, new Color(0.50f, 0.48f, 0.42f));
        }

        var cameraObject = new GameObject("Main Camera");
        cameraObject.tag = "MainCamera";
        cameraObject.AddComponent<Camera>();
        cameraObject.AddComponent<AudioListener>();
        cameraObject.AddComponent<SimpleFollowCamera>();
        cameraObject.transform.position = new Vector3(0, 6, -9);

        var managerObject = new GameObject("NetworkManager");
        var manager = managerObject.AddComponent<NetworkManager>();
        managerObject.AddComponent<UnityTransport>();

        var player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "PrototypePlayer";
        Object.DestroyImmediate(player.GetComponent<CapsuleCollider>());
        player.transform.position = new Vector3(0, 1.1f, 0);
        player.AddComponent<CharacterController>();
        player.AddComponent<NetworkObject>();
        player.AddComponent<Unity.Netcode.Components.NetworkTransform>();
        player.AddComponent<PlayerMotor>();
        player.AddComponent<PlayerHealth>();
        SetColor(player, new Color(0.18f, 0.65f, 0.92f));

        const string prefabPath = "Assets/PrototypePlayer.prefab";
        var prefab = PrefabUtility.SaveAsPrefabAsset(player, prefabPath);
        Object.DestroyImmediate(player);
        var serializedManager = new SerializedObject(manager);
        serializedManager.FindProperty("NetworkConfig.PlayerPrefab").objectReferenceValue = prefab;
        serializedManager.ApplyModifiedPropertiesWithoutUndo();

        var menu = new GameObject("NetworkMenu");
        menu.AddComponent<NetworkMenu>();
        var ui = menu.AddComponent<PrototypeOnScreenMenu>();
        ui.menu = menu.GetComponent<NetworkMenu>();

        System.IO.Directory.CreateDirectory("Assets/Scenes");
        EditorSceneManager.SaveScene(scene, "Assets/Scenes/PrototypeArena.unity");
        var scenes = EditorBuildSettings.scenes;
        EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene("Assets/Scenes/PrototypeArena.unity", true) };
        AssetDatabase.SaveAssets();
        Debug.Log("Nitro prototype arena created. Open Assets/Scenes/PrototypeArena.unity.");
    }

    private static void SetColor(GameObject obj, Color color)
    {
        var renderer = obj.GetComponent<Renderer>();
        var material = new Material(Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard"));
        material.color = color;
        renderer.sharedMaterial = material;
    }
}
#endif
