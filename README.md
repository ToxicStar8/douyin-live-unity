# 简介
基于<a href='https://github.com/ToxicStar8/BFramework-Ex'>BFramework-Ex框架</a>的抖音直播信息采集工具

# 操作流程
1.需要先安装Git和C++环境</br>
2.HybridCLR-Install-安装</br>
3.打包即可</br>

# 常见错误
黑屏、报Null、元数据不匹配等，一般都是打包环节出了问题，如下操作</br>
1.HybridCLR-Setting-Hot Update Assemblies-新增输入Assembly-CSharp-保存</br>
2.打包(因为ab包存放包体内部所以需要先打包，如果ab包存放云端可以忽略)</br>
3.HybridCLR-CompileDLL-ActiveBuildTarget</br>
4.BFramework/Build AssetBundles</br>
5.将打好的AB包全部放到StreamingAssets</br>
6.再打包</br>


# 项目移植
查看UIMainMenu(UI)、UIMainMenu_MsgUnit(UI)、SocketRoutine(Socket)、DY(Proto)四个脚本文件，将代码用自己的方式实现到自己项目里即可</br>
