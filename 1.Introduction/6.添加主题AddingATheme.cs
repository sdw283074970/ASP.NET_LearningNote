//Q: 默认模板很难看，之前有提到过ASP.NET有放CSS文件的地方，是否可以通过添加更改CSS文件让应用前端更美观？
//A: 没有错。ASP.NET的前端也是用的Bootstrap框架，基于HTML、CSS和Javascript。可以通过替换Bootstrap模板来未应用快速更换新面孔。如登陆网站
  //bootswatch.com 下载一个好看的css文件，如Darkly，将其命名未bootstrap_Darkly.css保存在Content文件夹下。然后要做的就是替换引用。在App_Satrt
  //文件夹下的BundleConfig.cs文件中找到原来的bootstrap.css引用，将其替换为bootstrap_Darkly.css。

  //BundleConfig.cs文件是我们定义和绑定客户端资产的地方，如，我们可以捆绑和压缩多个JavaScript或多个css文件到一个捆绑包中，这种绑定的方法可以减少
    //http请求获取资产的次数，直接让页面载入更快。BundleConfig.cs文件代码如下：

namespace Vidly
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));    //jquery捆绑包，其中包括jquery的脚本

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));   //jquery validation插件捆绑包，捆绑所有符合jquery.validate*命名方式的文件

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));    //modernizer捆绑包

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));   //bootstrap捆绑包

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap_Darkly.css",    //在此处更改bootstrap文件引用
                      "~/Content/site.css"));   //css捆绑包
        }
    }
}

  //保存后Ctrl+F5即可看到新效果

//暂时想到这么多，最后更新2018/02/21
