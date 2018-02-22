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
