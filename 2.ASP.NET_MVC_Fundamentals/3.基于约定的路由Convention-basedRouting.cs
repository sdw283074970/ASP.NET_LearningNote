//Q: 为什么要自己定制路由连接？
//A: 默认的MapRoute，即"{controller}/{action}/{id}"能满足大部分的需求。但实际上，经常会需要处理一些特定模式的http请求，如/movies/released/2015/04，
  //即选择MoviesController，按年/月的格式将日期作为参数传递给Released()方法，显然这种"{controller}/{action}/{year}/{month}"的模式不匹配默认模式，
  //需要定制MapRoute才能处理。
  
//Q: 如何定制MapRoute？
//A: 在App_Start文件夹下的RouteConfig.cs中就储存了路由匹配模式，在这里添加删除即可。原始代码如下：

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)   //RegisterRoutes()方法，应用初始化时在Global.asax.cs中调用
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");   //忽略的url模式

            routes.MapRoute(    //默认MapRoute
                name: "Default",    //MapRoute名
                url: "{controller}/{action}/{id}",    //MapRoute匹配的ulr模式
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }   //为ulr模式设定默认值
            );
        }
    }
    
    //可以注意到IgnoreRoute()方法是忽略某种具体的url模式，可以通过调用多个IgnoreRoute()方法来忽略多种url模式，同样，也可以通过调用多个MapRoute()
      //方法来添加路由可以识别的url模式。需要注意的是，忽略url模式顺序无所谓，但调用MapRoute()方法的顺序非常重要，从上倒下应该按照从最具体到最泛型
      //的原则进行排序。其原理类似于类中的重载方法，最上面的重载方法会优先匹配，若匹配成功则下面的重载方法将不做考虑。这里也是一样，当路由收到http
      //请求时会对url模式按照从上到下的原则进行匹配，一旦匹配成功则后面的url模式就不做考虑。
