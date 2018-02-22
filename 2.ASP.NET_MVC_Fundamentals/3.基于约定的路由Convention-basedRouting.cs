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
    //请求时会对url模式按照从上到下的原则进行匹配，一旦匹配成功则后面的url模式就不做考虑。试想如果把最泛型的url模式，如默认模式放在第一个，随便来个
    //什么url都匹配成功，就按照默认模式去了，这绝对不是你想要的。

  //定制新的MapRoute只用添加新的MapRoute在正确的地方即可。MapRoute()方法有很多重载，最常用的跟默认的一样，要求传递三个参数，即name、url和defaults。
    //name即是定制MapRoute的名字，url指匹配模式，defaults指当url符合匹配模式但不完全时，为各个元素预设的默认值。其他几种重载看看就会，不作过多探讨。

  //如，我们要定制一个处理"movies/released/{year}/{month}"http请求的url模式的MapRoute，即按照发行年月返回电影名称列表视图。代码如下：

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)   //RegisterRoutes()方法，应用初始化时在Global.asax.cs中调用
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");   //忽略的url模式

            routes.MapRoute(
                "MoviesByReleaseDate",    //新MapRoute的名称
                "movies/released/{year}/{month}",   //新MapRoute匹配的url模式
                new { Controller = "Movies", action = "ByReleasedDate", year = 1991, month =1}    //默认参数
                //注意，只有严格的"movies/released/*/*"才匹配此url模式，虽然无法从url中读取Controller和Action，但可以通过默认参数指定
            );
            routes.MapRoute(    //默认MapRoute
                name: "Default",    //MapRoute名
                url: "{controller}/{action}/{id}",    //MapRoute匹配的ulr模式
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }   //为ulr模式设定默认值
            );
        }
    }

  //新的定制MapRoute简历完毕，我们还需要在MoviesController中添加名为ByReleasedDate的Action，返回一个ContentView检验我们传递进去的参数。代码如下：

        public ActionResult ByReleasedDate(int year, int month)
        {
            return Content(year + "/" + month);
        }

  //运行，发送一个/movies/released/1992/11的http请求，routes会将1992和11赋予到year和month中传递给MoviesController中的ByReleasedDate()方法中，
    //返回一个1992/11的结果页面。如果仅发送/movies/released的请求，则会返回1991/1的结果界面，说明即使url中不包含{year}和{month}参数，但前部分
    //能正确匹配url模式，缺失的部分又刚好能从默认值中找到，也一样能匹配成功。反之，如果没有year = 1991和month = 1的默认指定，仅/movies/released
    //的请求将无法匹配此MapRoute，取而代之会返回一个著名的404页面。

//Q: 如果传入无意义的参数怎么办？
//A: 可以使用Constrains来限制无意义的参数。如查询20112年134月的电影完全没有意义。我们可以通过Contrains来将year和month限制为4位数和2位数。

//Q: 如何使用Constrains？
//A: 这就需要试用MapRoute()方法的另一个需要传入四个参数的重载，前三个参数一样，第四个参数要求传入一个Constrains对象，我们可以直接使用匿名对象作为
  //参数传入。限制年份为4位数且月份为2位数的代码如下：

            routes.MapRoute(
                "MoviesByReleaseDate", 
                "movies/released/{year}/{month}", 
                new { Controller = "Movies", action = "ByReleasedDate", year = 1991, month =1},
                new { year = @"\d{4}", month = @"\d{2}"}    //使用正则表达式限制位数，\d表示位数
            );

  //使用@符号是为了美观，否则将有两条斜杠，如new { year = "\\d{4}", month = "\\d{2}"}，看起来很烦。

//Q: 如果我想把年份限定得更具体呢？
//A: 请学习参阅正则表达式Constrains，那里有个新天地。

//暂时想到这么多，最后更新2018/02/22
