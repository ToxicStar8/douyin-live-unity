/*********************************************
 * BFramework
 * 图集生成工具
 * 创建时间：2023/01/29 16:37:36
 *********************************************/
using System.Globalization;
using System.IO;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

namespace Framework
{
    /// <summary>
    /// 图集生成
    /// </summary>
    public class BuildSpriteAtlas : Editor
    {
        /// <summary>
        /// 图集磁盘路径
        /// </summary>
        public static string SpritePath = Application.dataPath + "/GameData/Art/Sprite";

        /// <summary>
        /// 图片代码磁盘路径
        /// </summary>
        public static string SpriteScriptPath = Application.dataPath + "/GameData/Scripts/Define/AtlasSprite.cs";

        /// <summary>
        /// 图集代码磁盘路径
        /// </summary>
        public static string AtlasScriptPath = Application.dataPath + "/GameData/Scripts/Define/AtlasName.cs";

        [MenuItem("BFramework/Build SpriteAtlas")]
        public static void BuildAtlas()
        {
            GenerateSpriteAtlas();
            GenerateSpriteScript();
            GenerateAtlasScript();
            Debug.Log("图集代码生成完毕!");
            //回收资源
            System.GC.Collect();
            //刷新编辑器
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 生成图片的图集
        /// </summary>
        private static void GenerateSpriteAtlas()
        {
            var pathDir = new DirectoryInfo(SpritePath);
            //循环目录下的文件和子目录 但是不包含子目录下的文件和子目录
            foreach (var item in pathDir.GetFileSystemInfos("*.*", SearchOption.TopDirectoryOnly))
            {
                var parentDir = item as DirectoryInfo;
                //空值就代表它是文件
                if (parentDir != null)
                {
                    var file = new FileInfo(parentDir.FullName + "/" + parentDir.Name + ".spriteatlas");
                    if (file.Exists)
                    {
                        File.Delete(file.FullName);
                    }

                    var atlas = new SpriteAtlas();// 设置参数 可根据项目具体情况进行设置
                    SpriteAtlasPackingSettings packSetting = new SpriteAtlasPackingSettings()
                    {
                        blockOffset = 1,
                        enableRotation = false,
                        enableTightPacking = false,
                        padding = 2,
                    };
                    atlas.SetPackingSettings(packSetting);

                    SpriteAtlasTextureSettings textureSetting = new SpriteAtlasTextureSettings()
                    {
                        readable = false,
                        generateMipMaps = false,
                        sRGB = true,
                        filterMode = FilterMode.Bilinear,
                    };
                    atlas.SetTextureSettings(textureSetting);

                    TextureImporterPlatformSettings platformSetting = new TextureImporterPlatformSettings()
                    {
                        maxTextureSize = 2048,
                        format = TextureImporterFormat.Automatic,
                        crunchedCompression = true,
                        textureCompression = TextureImporterCompression.Compressed,
                        compressionQuality = 50,
                    };
                    atlas.SetPlatformSettings(platformSetting);

                    var assetPath = "Assets/GameData/Art/Sprite/";
                    AssetDatabase.CreateAsset(atlas, assetPath + parentDir.Name + "/" + file.Name);
                    // 2、添加文件夹
                    Object obj = AssetDatabase.LoadAssetAtPath(assetPath + parentDir.Name, typeof(Object));
                    atlas.Add(new[] { obj });
                    AssetDatabase.SaveAssets();
                }
            }
            Debug.Log("图集生成完毕!");
        }

        /// <summary>
        /// 生成图片代码文件
        /// </summary>
        private static void GenerateSpriteScript()
        {
            string temp = @"/*********************************************
 * 自动生成代码，禁止手动修改文件
 * 脚本名：AtlasSprite.cs
 * 创建时间：#Time
 *********************************************/
using Framework;
using UnityEngine;

namespace GameData
{
    /// <summary>
    /// 快捷获得Sprite
    /// </summary>
    public static class AtlasSprite
    {#SpriteList
    }
}
";
            string spListStr = string.Empty;
            var pathDir = new DirectoryInfo(SpritePath);
            //最外层 用于生成图集名称
            foreach (var dirInfo in pathDir.GetDirectories("*.*", SearchOption.TopDirectoryOnly))
            {
                //第二层 生成脚本文件的地方
                foreach (var item in dirInfo.GetFileSystemInfos("*.*", SearchOption.AllDirectories))
                {
                    var fileInfo = item as FileInfo;
                    if (fileInfo != null)
                    {
                        var file = new FileInfo(fileInfo.FullName);
                        if (file.Extension != ".meta" && file.Extension != ".spriteatlas")
                        {
                            //将图片带下划线的全部转换为首字母大写并去除下划线
                            var fnName = "Get_";
                            var strArr = fileInfo.Name.Replace(fileInfo.Extension, "").Split('_');
                            //如果有下划线 分割拼接
                            if (strArr.Length != 1)
                            {
                                for (int i = 0, length = strArr.Length; i < length; i++)
                                {
                                    //全部首字母大写
                                    fnName += CultureInfo.CurrentCulture.TextInfo.ToTitleCase(strArr[i]);
                                }
                            }
                            //没有下划线 即使用文件名
                            else
                            {
                                //首字母大写并且不修改其它字母
                                var fileName = fileInfo.Name.Replace(fileInfo.Extension, "");
                                fnName += CultureInfo.CurrentCulture.TextInfo.ToTitleCase(fileName.Substring(0, 1)) + fileName.Remove(0, 1);
                            }
                            //文本添加
                            spListStr += $"\r\n        public static Sprite {fnName}(LoadHelper loadHelper) => loadHelper.GetSprite(AtlasName.{dirInfo.Name},\"{fileInfo.Name}\");";
                        }
                    }
                }
            }

            //导出文件 替换文本
            var scripts = File.CreateText(SpriteScriptPath);
            temp = temp.Replace("#Time", System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            temp = temp.Replace("#SpriteList", spListStr);
            scripts.Write(temp);
            scripts.Close();
        }

        /// <summary>
        /// 生成图集代码文件
        /// </summary>
        private static void GenerateAtlasScript()
        {
            string temp = @"/*********************************************
 * 自动生成代码，禁止手动修改文件
 * 脚本名：AtlasName.cs
 * 创建时间：#Time
 *********************************************/
using Framework;
using UnityEngine;

namespace GameData
{
    /// <summary>
    /// 快捷获得图集名
    /// </summary>
    public static class AtlasName
    {#AtlasList
    }
}
";
            string atlasNameStr = string.Empty;
            var pathDir = new DirectoryInfo(SpritePath);
            //最外层 用于生成图集名称
            foreach (var dirInfo in pathDir.GetDirectories("*.*", SearchOption.TopDirectoryOnly))
            {
                atlasNameStr += $"\r\n        public static string {dirInfo.Name} => \"{dirInfo.Name}.spriteatlas\";";
            }

            //导出文件 替换文本
            var scripts = File.CreateText(AtlasScriptPath);
            temp = temp.Replace("#Time", System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            temp = temp.Replace("#AtlasList", atlasNameStr);
            scripts.Write(temp);
            scripts.Close();
        }
    }
}
