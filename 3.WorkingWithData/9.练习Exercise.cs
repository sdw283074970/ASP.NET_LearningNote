//项目需求：为电影页面中的电影添加详细页面，详细页面中要包括电影的种类。

//MovieController代码如下：

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;
using System.Data.Entity;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        public ActionResult RandomMovie()   //返回电影页面
        {
            var viewModel = new RandomMovieViewModel
            {
                Movies = _context.Movies.ToList(),
                Customers = _context.Customers.ToList()
            };
            return View(viewModel);
        }

        public ActionResult Details(int? id)    //返回电影详细页面
        {
            if (id != null)   //如果ID不为空，则返回该Id对应的详细信息
            {
                var movies = _context.Movies.Include(c => c.Genre).Single(c => c.Id == id);   //贪懒加载Genre信息
                ViewBag.Movies = movies;
                return View();
            }
            else    //否则报错
                return Content("Page not found");
        }
    }
}

//Views/Movies/Details电影详细页面视图代码如下：


@{
    ViewBag.Title = "Details";
    Layout = "~/Views/Shared/_Layout.cshtml";
}

<h2>@ViewBag.Movies.Name</h2>
<div>
    <ul>
        <li>Genre: @ViewBag.Movies.Genre.Name</li>    //使用导航属性获得Genre信息
    </ul>
</div>

//暂时想到这么多，最后更新2018/03/11
