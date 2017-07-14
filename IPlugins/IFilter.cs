using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace IPlugins
{
    public interface IFilter
    {
        /// <summary>
        /// 获取插件显示的名字
        /// </summary>
        /// <returns>插件名字</returns>
        string GetPluginName();
        /// <summary>
        /// 获取插件在菜单上显示的图标 若不需要显示则返回null
        /// </summary>
        /// <returns>图标 否则 null</returns>
        Image GetPluginIcon();
        /// <summary>
        /// 用于加载插件时候初始化调用
        /// </summary>
        /// <param name="strStarPath">主程序启动路径</param>
        void InitPlugin(string strStarPath);

        ResultInfo ExecFilter(Image imgSrc);
    }
}
