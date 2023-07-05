# 简介
基于<a href='https://github.com/ToxicStar8/BFramework-Ex'>BFramework-Ex框架</a>的抖音直播信息采集工具

# 操作流程
1.需要先安装Git和C++环境</br>
2.HybridCLR-Install-安装</br>
3.打包即可</br>

# 常见错误
一般都是打包环节出了问题，如下操作</br>
1.打包</br>
2.HybridCLR-CompileDLL-ActiveBuildTarget</br>
3.BFramework/Build AssetBundles</br>
4.将打好的AB包全部放到StreamingAssets</br>
5.再打包</br>


# 项目移植
查看UIMainMenu(UI)、UIMainMenu_MsgUnit(UI)、SocketRoutine(Socket)、DY(Proto)四个脚本文件，将代码用自己的方式实现到自己项目里即可</br>
