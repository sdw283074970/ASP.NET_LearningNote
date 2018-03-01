//Q: 如何向视图中传递数据？
//A: 之前的简介中有向View中传递数据的示例，如：

namespace Vidly.Controllers
{
    public class MoviesController : Controller
        public ActionResult Random()
        {
            var movie = new Movie() { Name = "WTF"};
            return View(movie);   //向View()方法传递movie对象作为参数后，就可通过在cshtml文件中调用Model.Name来调用movie.Name
        }
    }
} 

  //但还有两外两种方法传递数据。一种是ViewData字典，这种方法使用起来相当丑陋麻烦，所以直接略过。另一种方法是微软工程师用来提升ViewData表现的方法，
    //引入ViewBag这一动态类型变量来装载对象。ViewBag也是换汤不换药，所以也直接掠过。

  //所以，如果要向View中传递参数，请使用之前的方法，即向View()函数中传递对象作为参数，然后在对应Action的View文件，即cshtml文件中添加参数对象的引用，
    //如此例中对movie对象类型的引用，即可在View中调用参数。如代码所示：

@Model Vidly.Models.Movie   //添加movie的类型Movie的引用
@{
    ViewBag.Title = "Random";
    Layout = "~/Views/Shared/_Layout.cshtml";
}

<h2>@Model.Name</h2>    //通过Model可直接访问参数

//Q: 为什么通过Model就可以直接访问movie的字段？
//A: 在Contorller类中的View()方法将movie传递给了Model，所以可以直接从Model调用。我们不需要重写这个过程。
  
//暂时想到这么多，最后更新2018/03/01
