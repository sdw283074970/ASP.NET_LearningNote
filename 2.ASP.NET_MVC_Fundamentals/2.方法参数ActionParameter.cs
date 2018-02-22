//Q: Action所需要的参数是如何通过http请求传入的？
//A: 基本上，如果Action需要参数，则会在http请求中找，找到名字一样的参数就传入。http请求有如下几种方式包含参数：
  //1.直接通过ULR。如/movies/edit/1，即等于MoviesController.Edit(1)；
  //2.通过查询字符串。如/movies/edit?id=1，即等于MoviesController.Edit(id = 1)；
  //3.通过表格数据。如id=1。
  
//Q: 如果http请求中参数名称不一样但类型一样，能否成功传递？
//A: 不行。如我们在MoviesController中新加一个Edit(int id)方法，代码如下：
        
        public ActionResult Edit(int id)
        {
            return Content("id=" + id);
        }
        
  //此时参数名称为id，我们可以直接通过/movies/edit/1这个http请求传递id=1这个参数，也可以通过/movies/edit?id=1传递id=1这个参数。但是如果我们把
    //MoviesController中的id改为movieId，不仅无法直接通过ULR传递参数(即使类型一样)，也无法通过查询字符串传递参数。不能通过ULR传递字符串的原因在
    //于Router中，RouterConfig代码如下：
    
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",    //可以看到，ULR必须匹配这个模式Router才能处理请求
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }   //此处为id，而不是movieId，ULR无法识别
            );
        }
    }
    
    //不能通过查询字符串传递movieId的原因也是类似的，查询字符串/movies/edit?id=1中只有id=1，并没有movieId=1，所以也无法识别。
    //解决办法很简单，即在RouterConfig中将id改为movieId就能使用ULR请求，或在查询字符串中直接请求/movies/edit?MovieId=1也可。
  
  //以上为ASP.NET如何映射参数的解释。

//Q: 如果有些Action参数是可选的该如何声明？
//A: 可以参数就意味着即使不传递参数进去，这些参数也有默认值，并且这些参数为非空，只用填充逻辑即可。例如，我们需要在MoviesController中添加一个
  //Index(int pageIndex, string sortBy)的Action，即当收到/movies/index请求时按需求返回主页面，参数可有可无。逻辑可以为：当有参数时，按照
  //参数情况返回主页；当没有参数时，默认pageIndex为1，sortBy为Name。代码如下：

        public ActionResult Index(int? pageIndex, string sortBy)    //int是值类型，使其称为可空需要加?符号
        {
            if (!pageIndex.HasValue)    //如果pageIndex为空则等于1
                pageIndex = 1;
            if (string.IsNullOrWhiteSpace(sortBy))    //如果sortBy为空则等于Name
                sortBy = "Name";
            return RedirectToAction("Index", "Home", new { page = pageIndex, sortBy = sortBy });    //重定向于主页
        }

  //我们可以通过ULR传递参数，但需要提前重新设置RounteConfig，好让路由认出ULR的模式，具体操作下节介绍，这里使用ULR不传递参数，即直接发送一个
    //http请求/movies/Index，我们可看到重定向后的地址为50664/?page=1&sortBy=Name。
  //我们也可以通过查询字符串来传递参数。如发送一个http请求/movies/index?sortby=WTF，可以看到重定向后的地址为50664/?page=1&sortBy=WTF，我们没有
    //指定pageIndex所以其默认为1.

//暂时想到这么多，最后更新2018/02/22
