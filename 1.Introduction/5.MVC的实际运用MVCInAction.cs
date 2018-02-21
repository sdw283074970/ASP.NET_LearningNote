//Q: Model、View和Controller是如何一起工作的？
//A: 简单的过程为，收到http请求->Route寻找正确的Controller->Controller从Model中调用正确的Action(方法)->Action返回View给用户。我们以一个简单的实例
  //来示范这一个过程。假设项目需求为处理movies/random的http请求，返回随机的一个电影。首先我们需要建立一个Movies类保存电影的属性，这个类不与View绑定，
  //属于应用的独立数据，所以这应该是Model，将其储存在Models文件夹中，代码如下：

namespace Vidly.Models
{
    public class Movie    //储存电影属性的类
    {
        public int Id { get; set; }   //电影Id
        public string Name { get; set; }    //电影名字
    }
}

  //然后我们希望能处理movies/random的http请求，则需要一个MoviesController，并且对应的需要一个Movies的视图View。首先建立MoviesController，右键
    //Controllers文件夹，选择添加->Controller，可以看到很多MVC Controller模板，这些模板被称为预制脚手架(Scaffolding)以后熟悉了自己都能建这些模板，
    //这里从头演示，所以选择空模板，将这个Controller命名为MoviesController，控制器建立完毕，更改其代码如下：
    
namespace Vidly.Controllers
{
    public class MoviesController : Controller    //Movies控制器继承自Controller类
    {
        //新建Random()方法来处理 Movies/Random 请求
        public ActionResult Random()    //返回类型为ActionResult，基本上这表示呈送给用户的视图，后面详细介绍这个类
        {
            var movie = new Movie() { Name = "WTF"};    //实例化一个Movie类，这里可以通过数据库获取数据，此处仅仅演示
            return View(movie);    //将movie作为Model对象以返回一个视图，目前没有任何视图，继续往下
        }
    }
} 
    
  //对应的我们还需要Movies/Random视图，右键Views文件夹，选择添加->View，在弹出的对话框中，命名Random，并选择布局，这里用Shared下的默认布局
    //_Layout.cshtml，这样我们就在Views文件夹下的Movies文件夹下获得了一个Random.cshtml的视图文件。.cshtml装载的是可被转换为html标准的C#语句，其
    //原理为使用@{}代码块包裹C#代码，编译器会编译@{}代码块中的C#代码并将其转译为html标准语言(猜想，未证实)。Random.cshtml代码如下：

@Model Vidly.Models.Movie   //指定Model需要引用的命名空间
@{
    //以下是两个网页的属性
    ViewBag.Title = "Random";   //网页的名字
    Layout = "~/Views/Shared/_Layout.cshtml";   //网页用的布局
}

<h2>@Model.Name</h2>    //网页显示给用户的标题
  
  //完成后，按住Ctrl+F5即可查看运行结果。
  
  //小结一下，此例中的过程为：用户发送/movies/random请求，Route查找到这个ulr请求匹配"{controller}/{action}/{id}"，于是选择MoviesController，并
    //调用MoviesController中的Random()方法，Random()方法创建一个随机包含电影对象，并将这个实例作为参数对象传递给View()方法，View()方法找到
    //Views文件夹下的Movies文件夹中的Random.cshtml文件，根据这个文件内容返回一个ActionResult类，这个类向用户展示了请求结果。
  
//暂时想到这么多，最后更新2018/02/21
