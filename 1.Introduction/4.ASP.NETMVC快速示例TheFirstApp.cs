//Q: 如何建立一个ASP.NET项目？
//A: 选择新建项目->Template->Web->ASP.NET Web App，并命名Vidly，然后取消Source Control勾选。再出现的对话框中选择MVC模板，取消勾选HostInTheCloud
  //选项。至此新项目创建完成。

//Q: 用MVC模板创建的解决方案中有很多文件夹和杂七杂八的文件，这些是干什么用的？
//A: 以下为各个文件夹/文件的介绍：
  //1.App_Data文件夹：这里储存数据库文件；
  //2.App_Start文件夹：这里储存当应用初始化时会被调用的类，如配置类RouteConfig，代码如下：

    public class RouteConfig    //包含路由规则
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(    //默认的路由
                name: "Default",    //路由名称
                url: "{controller}/{action}/{id}",    //URL路径，任何匹配此路径的http请求都会通过此路由选择到正确的控制器并调用相应的方法(Action)
                defaults: new 
                { 
                  controller = "Home",    //指定默认控制器为Home
                  action = "Index",    //指定默认方法为Index
                  id = UrlParameter.Optional    //默认id为可选
                }   
            );
        }
    }
  
    //如收到一个为/movies/popular的http请求，这个url能匹配上"{controller}/{action}/{id}"，通过这个路由能选择到MoviesController控制器，
      //并调用MoviesController下的Popular()方法。又如收到一个为/movies/edit/1的http请求，这个路由也能选择到MoviesController控制器，并调用
      //Edit(int 1)方法。如果http请求没有"{controller}/{action}/{id}"这样的一个模式，到域名就没了，则会使用默认值。如http请求为vidly.com，
      //则路由会选择HomeController，并调用Index()方法。同理如果http请求只有controller没有以后的东西，则Action会自动调用Index()方法。

  //3.Content文件夹：储存CSS文件、图片等用客户端的资产；
  //4.Controllers文件夹：储存控制器类文件，任何控制器类文件都遵循View名+Controller的命名方式，默认有三个控制器类，即：
    //AccountController.cs：包括login、logout等方法；
    //HomeController.cs：返回主页的方法；
    //MangeController.cs：提供一系列处理请求的方法；
  //5.fonts文件夹：储存字体类文件，也可以放在Content文件夹中；
  //6.Models文件夹：储存所有域类(Domain Class)；
  //7.Scripts文件夹：储存JavaScript文件；
  //8.Views文件夹：储存一系列展示给用户看的视图文件夹，每个视图文件夹都对应一个控制器类，如Home视图文件夹对应HomeController，通过对应的控制器类
    //返回可视化的视图给用户。在ASP.NET中的可视类文件为cshtml格式，即储存将C#转为html标准的文件。除成一一映射的控制器和视图文件夹，还有一个Shared
    //文件夹，即储存在所有控制器都可以返回的视图，如著名的404视图；
  //9.favicon.ico：图标文件，显示网页应用的图标；
  //10.Global.asax：包含了一个类，为整个应用的使用过程中的各种事件提供全局管理，展开后可以看到Global.asax.cs文件，代码如下：

    public class MvcApplication : System.Web.HttpApplication    //这个类将在应用的一开始就调用
    {
        protected void Application_Start()    //这个方法将在应用的一开始就调用
        {
            //调用以下各类中的方法，一些类即是之前说的在App_Start文件夹中的类，就是在这里调用的
            AreaRegistration.RegisterAllAreas();    
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }

  //11.packages.configs：用来管理NuGet Package的配置文件，即管理与这个应用相关的扩展库；
  //12.Project_Readme.html：读我！
  //13.Startup.cs：即将在ASP.NET CORE 1.0中启用的新的启动类，将抛弃并取代Global.asax，现未完成；
  //14.Web.config：为xml超文本文件，用来保存一些配置，有如之前控制台应用的App.config。该xml代码很长，但一般来说我们只用关心两个地方，一个是之前
    //EntityFramework中提到的<connectionString>标签，用来配置数据库连接的，另一个是<appSettings>，定义应用的配置。

//暂时想到这么多，最后更新2018/02/21


