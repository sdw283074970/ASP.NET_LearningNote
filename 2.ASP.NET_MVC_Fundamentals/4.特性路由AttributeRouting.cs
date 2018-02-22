//Q: 什么是特性路由？
//A: 特性路由AttributeRouting指使用C#的特性Attribute来设定、自定义路由的方法。

//Q: 为什要用特性路由？
//A: 这种方法比上一节讲到的在RouteConfig.cs文件中自定义路由的方法要更高效更准确。如，之前的MapRoute()方法中，默认参数一栏的Controller和Action都是
  //以string的形式定义的，这意味着如果更改Controller或Action的名字，需要手动更改路由配置，非常麻烦。
  
  //特性是非string的，可以通过RenameReactor一键更改所有引用，如只用右键MoviesController中的ByReleasedDate()方法名字，选择RenameReactor，
    //输入新名字，特性中的Action名字也会一并更改，再也不用手动更改了。
    
//Q: 为什么还要学习之前的自定义路由方法？
//A: 一些现存的项目用的老方法，必须都学会才能胜任工作。

//Q: 如何使用特性路由？
//A: 首先要启用特性路由。在RouteConfig.cs文件中的RegisterRoutes()方法中调用MapMvcAttributeRoutes()方法即可启用。代码如下：

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();   //启用特性路由
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
    
  //要同样达到上一节的处理"movies/released/{year}/{month}"请求的效果，可以通过特性路由实现。在目标控制器的目标Action前面，即MoviesController中的
    //ByReleasedDate()方法前，使用[Route("url")]特性，代码如下：
    
    public class MoviesController : Controller
    {
        [Route("movies/released/{year}/{month}")]//特性路由，即默认指定当前类为Controller，修饰的方法为Action，并将url请求的剩余参数传如方法
        public ActionResult ByReleasedDate(int year, int month)
        {
            return Content(year + "/" + month);
        }
    }
    
  //也可以直接在url中应用Constrains，用法为在需要限制的变量后加冒号，再像调用函数一样加具体限制。多个限制可用多个冒号串联，如以下代码：
  
        [Route("movies/released/{year:Regex(\\d{4}):range(1991, 2018)}/{month:Regex(\\d{2}):range(1,12)}")]
        public ActionResult ByReleasedDate(int year, int month)
        {
            return Content(year + "/" + month);
        }
    
  //以上限制含义为，将year必须为4位数，范围为1991~2018；month必须为2位数，范围为1~12。
  
  //一些其他常用的限制如min()、max()、minlength()、maxlength()、int()、float()、guid()等都是被ASP.NET支持的。并不需要去记住这些限制，需要用到
    //的时候只用百度/谷歌 ASP.NET MVC Attribute Route Constrains 即可。
    
//暂时想到这么多，最后更新2018/02/22
