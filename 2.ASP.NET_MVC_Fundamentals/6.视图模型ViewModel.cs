//Q: 在View文件中只有一个通过Model调用数据的位置，如果想将多个对象中的数据传递给View该怎么办？
//A: 我们使用ViewModel来解决这个问题。ViewModel是一个专门为View建立的Model。它包含了所有针对某一具体View的数据和规则。继续以Vidly为例，假如项目需求
  //为在同一页面显示电影名字和租着电影的顾客名字，我们就需要通过ViewModel来实现。首先在Model中建立一个Customer类，代码如下

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

  //在Model文件夹中，我们存放域类，如Customer、Movie等等，为了不混淆，我们需要专门建立一个ViewModels文件夹来存放ViewModel类。然后在ViewModels
    //中建立RandomMovieViewModel类，这是一个约定共识，即ViewModel类的名称为行为名+ViewModel后缀。这个类的代码如下：

    public class RandomMovieViewModel
    {
        public Movie Movie { get; set; }    //项目需求中首先要显示电影名，即这个ViewModel首先需要包括Movie类
        public List<Customer> Customers { get; set; }   //其次要显示所以借过这电影的顾客名，以列表的形式储存
    }

  //然后在Action类中补充Customers列表，作为需要显示的顾客列表，然后将movie和customers传递进ViewModel中，最后将ViewModel作为参数传递给View()方法。
    //最后只用在View文件，即cshtml文件中修改ViewModel的引用即可即可完成多个Model的传递。代码如下：

        public ActionResult RandomMovie()
        {
            var movie = new Movie() { Name = "WTF" };   //实例化Movie
            var customers = new List<Customer>    //实例化Customer
            {
                new Customer { Name = "Stoneway"},
                new Customer { Name = "YourDaddy"}
            };

            var viewModel = new RandomMovieViewModel    //实例化RandomMovieViewModel，并将movie和customers传入
            {
                Movie = movie,
                Customers = customers
            };
            return View(viewModel);   //将viewModel作为参数传入View()方法，View()方法将viewModel传递到Model中，就可以通过Model调用其中所有数据
        }

  //因为这个Action的名称为RandomMovie，所以我们需要在View文件夹下的Movie控制器文件夹下建立一个新的View文件(cshtml)，并命名为RandomMovie.cstml，
    //否则抛出异常找不到View文件。代码如下：

@model Vidly.ViewModels.RandomMovieViewModel    //添加ViewModel的引用
@{
    ViewBag.Title = "RandomMovie";
    Layout = "~/Views/Shared/_Layout.cshtml";
}

<h2>@Model.Customers.Find(c => c.Name == "Stoneway").Name</h2>    //通过Model可以调用其中的任何数据
  
  //作为小结，ViewModel可以看作是Model的打包集合，可以在其中放置任意数量的Model，最后只用向View()方法传递一个这样的ViewModel实例，即可完美调用。
  
//暂时想到这么多，最后更新2018/03/01

