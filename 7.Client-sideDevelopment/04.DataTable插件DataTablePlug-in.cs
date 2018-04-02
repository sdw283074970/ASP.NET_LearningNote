//Q: 什么是DataTable插件？
//A: DataTable是一款jQuery表格插件，可以为几乎任何HTML表格添加高级的互动功能，如搜索、排序、分页等。

//Q: 如何安装DataTable插件？
//A: 在PackageManger中，输入 install-package jquery.datatables 即可在项目中安装最新版本的Datatable插件。

//Q: 如何使用插件?
//A: 在安装DataTable插件后，可以在项目中的Scripts文件夹中看到名为DataTable的文件夹，这个文件夹中就是所有的DataTable的JavaScript文件。其中，本体文件
  //名为 jquery.dataTables.js，将这个js文件添加进js捆绑包，再在目标视图文件(如_Layout视图)中加载该捆绑包即可在视图中调用插件中的函数了。

//Q: 如何将js文件添加进js捆绑包？
//A: 在Vidly项目中，我们用到了大量三方的js插件、库，完全可以将这些要用到的三方库捆绑到同一个捆绑包中，在BundleConfig.cs文件中修改，代码如下：

using System.Web.Optimization;

namespace Vidly
{
    public class BundleConfig
    {
        //这个方法在Global.asax.cs中注册，效果为在应用初始的时候就加载
        public static void RegisterBundles(BundleCollection bundles)
        {
            //将所有三方js文件整合到同一个整合包中，取名lib
            bundles.Add(new ScriptBundle("~/bundles/lib").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/respond.js",
                        "~/Scripts/bootbox.js",
                        "~/Scripts/DataTables/jquery.dataTables.js",    //添加datatable主文件
                        "~/Scripts/DataTables/dataTables.bootstrap.js"    //这个文件能将datatable转为bootstrap风格，也添加进捆绑包
                        ));

            //这是jqueryvalidation文件，其作用为实现客户端方的验证，基本上是将默认DataAnnotation的验证翻译给客户端
            //如果需要实现自定义的客户端验证，请将自己写的jquery验证文件添加到此捆绑包中
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            
            //将所有css文件添加进同一个捆绑包
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/DataTables/css/dataTables.bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}

  //以上的js捆绑包通过视图文件中的@Scripts.Render()方法加载渲染。

//Q: 如何将DataTable的高级功能应用到页面中？
//A: 最简单的用法为直接在视图文件中调用DataTable()方法。如以下代码：

@section scripts
{
    <script>
        $(document).ready(function () {
            $("#customers").DataTable();    //选择id为customers的元素(<Table>标签)，直接调用DataTable()方法
        });
    </script>
}

  //其效果为为整个表添加了排序、搜索和分页的功能。
  
//暂时想到这么多，最后更新2018/04/02
