//项目需求：在导航条上设置三个连接，分别是“Vidly(指向主页)”、“Customers(指向顾客页)”和“Movies(指向电影页)”。
  //主页要求为默认；
  //顾客页要求：
    //路由：/Customers/Index
    //功能：读取数据库中的顾客信息，将两个顾客的Id、姓名、会员类型信息罗列成表，并未顾客姓名设立超链接，指向顾客详细主页信息；
  //顾客详细主页要求：
    //路由：/Customers/Index/id
    //功能：显示顾客详细信息，暂且为空，如id为空则显示页面未找到；
  //电影页要求：
    //路由：/Movies/RandomMovies
    //功能：显示数据库中的所有电影信息，将两个电影的Id、电影名信息罗列成表。
    
  //CustomerController代码如下：

using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using Vidly.Models;
using System.Data.Entity;

namespace Vidly.Controllers
{
    public class CustomersController : Controller
    {
        // GET: Customers
        private ApplicationDbContext _context = new ApplicationDbContext();
        public string result;
        
        public ActionResult Index()   //Index顾客页面
        {
            var customers = GetCustomers();
            ViewBag.Customers = customers;    //使用ViewBag装载顾客信息
            return View();
        }

        private IEnumerable<Customer> GetCustomers()    //获得数据库中的顾客信息
        {
            return _context.Customers.Include(c => c.MemberShipType).ToList();    //贪婪加载与Customers关联的MemberShipType信息
        }
        
        public ActionResult Details(int? id)    //顾客详细信息页面
        {
            if (id != null)   //如果id参数不为空，则返回顾客详细信息视图
            {
                var customerDetail = _context.Customers.Find(id);
                ViewBag.CustomerDetail = customerDetail;
                return View();
            }
            else    //如果id为空，则报错
            {
                return Content("Page Not Found");
            }
        }
    }
}

  //MoviesController代码如下：
  
  using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        public ActionResult RandomMovie()   //RandomMovie方法(Action)
        {
            var movies = new List<Movie>
            {
                return _context.Movies.ToList();    //获得数据库中的电影信息
            };
            var customers = new List<Customer>
            {
                return _context.Customers.ToList();    //获得数据库中的顾客信息
            };

            var viewModel = new RandomMovieViewModel
            {
                Movies = movies,
                Customers = customers
            };
            return View(viewModel);
        }
    }
}
  
  //Views/Customers/Index.cshtml顾客页视图文件代码如下：
  
@using Vidly.Models
@{
    ViewBag.Title = "CustomerIndex";
    Layout = "~/Views/Shared/_Layout.cshtml";
}

<h2>Customer</h2>

<div>
    <div>
        <ul>
            <li>Our VIP Customers</li>
        </ul>
    </div>
    <div>
        <ul>
            <li>Id</li>
            <li>Customer Name</li>
            <li>Membership Typer</li>
        </ul>
    </div>
    <div>
        <ul>
            @foreach (var c in ViewBag.Customers)
            {
                <li>@c.Id</li>
                <li>@Html.ActionLink((string)c.Name, "Details", "Customers", new { id = c.Id }, null)</li>    //连接
                <li>@c.MemberShipType.MemberShipTypeName</li>   //使用导航属性获得会员信息
            }
        </ul>
    </div>
</div>


  //Views/Customers/Details.cshtml会员详细信息页面视图文件代码如下：
  

@{
    ViewBag.Title = "Details";
    Layout = "~/Views/Shared/_Layout.cshtml";
}

<h2>@ViewBag.CustomerDetail.Name</h2>
<div>
    <ul>
        <li>@ViewBag.CustomerDetail.Name 's detail is empty</li>
    </ul>
</div>

  //Views/Movies/RandomMovies.cshtml电影页面视图文件代码如下：
  
@model Vidly.ViewModels.RandomMovieViewModel
@{
    ViewBag.Title = "RandomMovie";
    Layout = "~/Views/Shared/_Layout.cshtml";
}
@{ 
    var className = Model.Customers.Count > 5 ? "Popular" : null;
}

<h2 class="@className">Movies</h2>
<div>
    <div>
        <ul>
            <li>Availbale Movies</li>
        </ul>
    </div>
    <div>
        <ul>
            <li>Id</li>
            <li>Name</li>
        </ul>
    </div>
    <div>
        <ul>
            @foreach (var c in Model.Movies)
            {
                <li>@c.Id</li>
                <li>@c.Name</li>
            }
        </ul>
    </div>
</div>

  //导航条代码：
  
<div class="navbar navbar-inverse navbar-fixed-top">
    <div class="container">
        <div class="navbar-header">
            <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
            </button>
            @Html.ActionLink("Vidly", "Index", "Home", new { area = "" }, new { @class = "navbar-brand" })
        </div>
        <div class="navbar-collapse collapse">
            <ul class="nav navbar-nav">
                <li>@Html.ActionLink("Customers", "Index", "Customers")</li>
                <li>@Html.ActionLink("Movies", "RandomMovie", "Movies")</li>
            </ul>
            @Html.Partial("_LoginPartial")
        </div>
    </div>
</div>

//暂时想到这么多，最后更新2018/03/11
