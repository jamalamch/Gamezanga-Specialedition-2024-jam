using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class ScreenshotHelper
{

    private const string ScreenshotFolderName = "Game-Screenshots/";

    [MenuItem("ACRABGames/Take screenshot %#D")]
    public static void TakeScreenShot()
    {
        CreateScreenshotFolderIfNeeded();

        // var date = DateTime.Now.ToString().Replace(" ", "_").Replace("/", "-").Replace(":", "-");
        var screenshotBaseFilePath = ScreenshotFolderName + Application.productName + "-" + Screen.width + "x" +
                                     Screen.height;
        var screenshotFinalFilePath = GetIterationNameForFile(screenshotBaseFilePath) + ".png";
        ScreenCapture.CaptureScreenshot(screenshotFinalFilePath);
        Debug.Log(
            "[MWM-Utils] Screenshot located at: " + screenshotFinalFilePath + "\n" +
            "Wait for a few seconds for the screenshots to be written on your disk before calling this method again");
    }

    private static void CreateScreenshotFolderIfNeeded()
    {
        if (!Directory.Exists(ScreenshotFolderName))
        {
            Directory.CreateDirectory(ScreenshotFolderName);
        }
    }

    private static string GetIterationNameForFile(string baseFilePath)
    {
        if (!File.Exists(baseFilePath + ".png")) return baseFilePath;

        var iteration = 1;
        while (File.Exists(baseFilePath + "_" + iteration + ".png"))
        {
            iteration++;
        }

        return baseFilePath + "_" + iteration;
    }

    [MenuItem("ACRABGames/RemplaceMaterialWithToonyColors")]
    public static void ResetSelectedMaterials()
    {
        foreach (var o in Selection.objects)
        {
            if (o is Material material)
            {
                var mainTexture = material.mainTexture;
                var color = material.color;
                material.shader = Shader.Find("Toony Colors Pro 2/Hybrid Shader");
                material.SetTexture("_BaseMap", mainTexture);
                material.SetColor("_BaseColor", color);
            }
        }
    }

    [MenuItem("ACRABGames/GetLayerIndex")]
    public static void GetLayerIndex()
    {
        foreach (var o in Selection.objects)
        {
            if (o is GameObject gameObject)
            {
                Debug.Log(gameObject.name + " Layer Index : " + gameObject.layer);
            }
        }
    }

    [MenuItem("ACRABGames/RemplaceGameObjectWithPrefabs")]
    public static void ResetSelectedGameObjects()
    {
        List<GameObject> levelData = new List<GameObject>();
        string[] assets = AssetDatabase.FindAssets("t:GameObject", new[] { "Assets/_Game/Prefabs/LevelObjet" });

        for (int i = 0; i < assets.Length; i++)
        {
            GameObject data = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(assets[i]));
            if (data != null)
                levelData.Add(data);
        }

        Dictionary<string, GameObject> meshPrefab = new Dictionary<string, GameObject>();
        foreach (var item in levelData)
        {
            string mesh = item.name;
            if (!meshPrefab.ContainsKey(mesh))
                meshPrefab.Add(mesh, item);
        }

        foreach (var o in Selection.objects)
        {
            if (o is GameObject gameobjetct)
            {
                Transform transform = gameobjetct.transform;
                List<Transform> newTrs = new List<Transform>();

                for (int i = 0; i < transform.childCount; i++)
                {
                    if (newTrs.Contains(transform.GetChild(i)))
                        continue;
                    Transform oldTrans = transform.GetChild(i);
                    string name = oldTrans.name;

                    if (name.Contains(' '))
                    {
                        name = name.Split(' ')[0];
                    }

                    if (meshPrefab.ContainsKey(name))
                    {
                        Transform newPrefabTrs = PrefabUtility.InstantiatePrefab(meshPrefab[name].transform, transform) as Transform;
                        newPrefabTrs.position = oldTrans.position;
                        newPrefabTrs.rotation = oldTrans.rotation;
                        newPrefabTrs.localScale = oldTrans.localScale;

                        GameObject.DestroyImmediate(oldTrans.gameObject);
                        i--;

                        newTrs.Add(newPrefabTrs);
                    }
                }
            }
        }
    }
}
