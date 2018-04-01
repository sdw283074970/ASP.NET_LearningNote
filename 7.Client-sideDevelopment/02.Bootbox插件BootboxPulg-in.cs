//Q: 如何更改JavaScript的源生样式？
//A: 首先要加载目标bootstrap.css文件，然后阅读这个文件的说明文档，根据指示来使用。
  //另一种不用bootst.css的情况下，使用相应的js插件也可以更改样式。如Bootbox.js插件(bootboxjs.com)可以替换源生的JavaScript对话框样式。
  
  //Bootbox.js提供了一系列简单的函数，可以被用来建立不同风格样式的对话框，更多详情请阅读官方文档。
  
//Q: 如何使用js插件？
//A: 在Package Manager下载和管理。如，安装bootbox.js只用在PM中输入 install-package bootbox 就可以安装最新版本的插件。安装完毕后可以获得两个文件，
  //分别是bootbox.js和bootbox.min.js(bootbox.js的压缩版)，这两个文件默认被添加进Scripts文件夹中。然后在捆绑文件中添加bootbox.js的引用即可。
  //捆绑文件BundleConfig.cs的代码如下：
  
using System.Web.Optimization;

namespace HeroPickHelper
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootbox.js"，   //在此处添加bootbox的引用，将bootbox添加到"~/bundles/bootstrap"这个捆绑包中
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}

//Q: 如何在代码中使用插件中的方法？
//A: 以bootbox为例，我们可以在视图文件中的jQuery脚本代码部分使用它，也可以在专门的xxx.js脚本文件中使用，最后再添加这个脚本引用到视图文件即可。
  //如直接在视图文件中使用bootbox的confirm方法。假设项目需求为将确认删除对话框改为bootbox样式，代码如下：
  
//接HTML文档
@section scripts
{
    <script>
        $(document).ready(function () {
            $("#heroes .js-delete").on("click", function () {
                bootbox.confirm("确定在此页面删除这个英雄？");   //调用bootbox.confirm()方法
            });
        });
    </script>
}

//暂时想到这么多，最后更新2018/04/01
