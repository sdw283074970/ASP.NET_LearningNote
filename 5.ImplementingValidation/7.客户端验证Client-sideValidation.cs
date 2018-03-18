//Q: 什么是客户端验证？
//A: 客户端验证指在客户端而不是服务器上对逻辑经行验证。

//Q: 为什么要经行客户端验证？
//A: 两个好处。第一个是反应快，命令不用传回服务器经行验证，在用户端的本地就能进行，从而加快了响应速度。第二个好处是节省服务器资源，服务器资源应该留给
  //一些有关安全的逻辑运行，类似于检测表单是否为空这种无关痛痒的工作交给客户端本地就好了。

//Q: 如何使用客户端验证？
//A: 默认情况下，在ASP.NET MVC5中客户端验证是关闭的。通过在我们想启用客户端验证的视图中加入以下代码即可启用：

@section scripts
{
    @Scripts.Render("~/bundles/jqueryval")    //加载该路径的脚本
}

  //至此客户端验证启用完成。如果我们使用元素查看，可以追踪当前页面的网络活动情况。如果我们点击Save按钮，则看不到任何网络活动，这意味着启用完成。

//Q: 这个路径是哪来的？
//A: 在App_Start文件中的BundleConfig.cs文件有对这个路径进行定义。整个文件代码如下：

using System.Web;
using System.Web.Optimization;

namespace Vidly
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(    //路径自此由来，通过bundles.Add(...).Include(...)函数声明
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}

  //以上bundles.Add(...).Include(...)这条命令的意思为将~/Scripts文件夹中所有符合/jquery.validate*命名的脚本文件全部结合、压缩到同一个捆绑包中，
    //而这个捆绑的路径就为"~/bundles/jqueryval"，通过这个路径可供其他方法下载，如Scripts.Render()方法。

//Q: 为什么加入一个脚本捆绑包就可以达成客户端验证？
//A: 整个过程是这样：首先我们通过DataAnnotation定义逻辑来实施验证，如Name字段是Required的，ASP.NET NVC5会将这些DataAnnotation同时运用在服务端验证
  //和客户端验证中，会在所有被DataAnnotation标记的元素中添加 data-val-xxx 的属性，如：

<select class="form-control input-validation-error" data-val="true" data-val-number="The field MemberShip Type must be a number." 
  data-val-required="Please select a membership type." id="Customer_MemberShipTypeId" name="Customer.MemberShipTypeId">

  //然后jQUery脚本能识别处这些属性，当点击Save按钮时，jQuery会校验这些属性和对应的值，如果某个字段没通过验证，jQuery就将验证错误信息渲染进这个字段。
  
  //但是，在上例中，如果进一步试验就可以发现，如果我们填充了Name字段、选择了MemberShipTypeId过后，就留一个Birthday字段为空，这时点击Save按钮，还是
    //会侦测到网络活动。这是因为挂在Birthday上的验证是自定义验证，项目自带的默认jQuery是不能对自定义验证进行客户端验证的。一旦无法进行客户端验证，
    //ASP.NET就会自动将验证任务返回给服务端执行。
  
//Q: 如何才能执行客户端的自定义验证？
//A: 需要自己写jQuery脚本代码。但一般情况并不推荐让所有的验证都交由客户端执行，因为自定义验证往往都是业务逻辑验证，业务逻辑验证又往往是安全相关的验证，
  //如果交由客户端执行那么及其容易引发安全漏洞。如别有用心的黑客可以跳过18岁验证注册到年度会员。另一个原因是维护问题，如果逻辑规则更改，那么要同时修改
  //服务端和客户端的jQuery代码。都所以一般只有标准的、默认的验证交由客户端执行，如验证表单是否是电话号码的格式等。
  
  //默认情况下支持的客户端DataAnnotation验证仅包含以下几种：
  
[Required]  //即覆写该字段不能为空
[StringLength(255)]   //即覆写该字段的最大长度
[Range(1, 10)]    //即覆写该字段必须在范围内
[Compare("OtherProperty")]    //即与其他字段相比较，必须与被比较的字段相同，如与"Password"字段比较
[Phone]   //即覆写该字段必须为电话
[EmailAddress]    //即覆写该字段必须为电邮地址
[Url]   //即覆写该字段必须为网址
[RegularExpression("...")]    //即覆写该字段必须满足某种正则表达式，其实上面几个特性标签都是由正则表达式写成的

//暂时想到这么多，最后更新2018/03/18
