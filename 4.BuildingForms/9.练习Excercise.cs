//项目需求：为Movies控制器下的Action也制作表单。
  //1.更改/Movies/Index视图，新增一个“添加新电影信息”的按钮，这个按钮指向一个"AddMovie"方法(Action)；
  //2."AddMovie"方法返回一个添加电影信息的表单，要求表单包括电影名、电影类别的下拉菜单、库存数量和发行日期，并且这个表单视图要有提交和取消按钮；
  //3.提交按钮新的表单要能在数据库中添加新的电影信息，取消按钮返回Index视图；
  //4.继续修改/Movies/Index视图，将视图中每个电影名称指向Details的超链接取消，替换为编辑电影信息页面，页面要与添加电影信息页面相同；
  //5.在编辑电影信息页面中，提交按钮要能将修改后的内容保存至数据库。
  
  //首先在MoviesController中建立AddMovie方法，代码如下：
  
        public ActionResult AddMovie()    //超链接将触发这个方法/Action
        {
            var genres = _context.Genres.ToList();
            var viewModel = new MovieFormViewModel    //将Genres和Movie这两个Model绑在一起传到视图中去
            {
                Genres = genres
            };
            return View("MovieForm", viewModel);    //指定返回的视图名称，以及传入的Model名称
        }
        
  //"MovieForm"视图代码很像"CustomerForm"视图代码，如下：
  
@model Vidly.ViewModels.MovieFormViewModel
@{
    ViewBag.Title = "MovieForm";
    Layout = "~/Views/Shared/_Layout.cshtml";
}

<h2>Movie</h2>

@using (Html.BeginForm("Save", "Movies"))
{
    <div class="form-group">    //电影名称表单
        @Html.LabelFor(m => m.Movie.Name)
        @Html.TextBoxFor(m => m.Movie.Name, new { @class = "form-control"})
    </div>
    <div class="form-group">    //电影发行日期表单
        @Html.LabelFor(m => m.Movie.ReleasedDate)
        @Html.TextBoxFor(m => m.Movie.ReleasedDate, "{0:d MMM yyyy}", new { @class = "form-control" })
    </div>
    <div class="form-group">    //下拉菜单表单
        @Html.LabelFor(m => m.Movie.GenreId)
        @Html.DropDownListFor(m => m.Movie.GenreId, new SelectList(Model.Genres, "Id", "Name"), "Select the genre", new { @class = "form-control"})
    </div>
    <div class="for-group">   //库存表单
        @Html.LabelFor(m => m.Movie.InStock)
        @Html.TextBoxFor(m => m.Movie.InStock, new { @class = "form-control" })
    </div>
    @Html.HiddenFor(m => m.Movie.Id)    //隐藏属性，用来判断是新建对象还是对已存在对象经行更改
    <div>
        <button type="submit" class="btn btn-primary">Save</button>   //提交按钮
        @Html.ActionLink("Cancel", "Index", "Movies", null, new { @class = "btn btn-danger"})   //取消按钮
    </div>
}

  //以下是修改后的Index视图代码：
  
@model IEnumerable<Vidly.Models.Movie>
@{
    ViewBag.Title = "Index";
    Layout = "~/Views/Shared/_Layout.cshtml";
}

<h2>Movies</h2>
<div>
    <div>
        @Html.ActionLink("Add A New Movie", "AddMovie", "Movies", null, new { @class = "btn btn-primary"})    //添加电影按钮
    </div>
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
            @foreach (var c in Model)
            {
                <li>@c.Id</li>
                <li>@Html.ActionLink(c.Name, "Edit", "Movies", new { id = c.Id}, null)</li>   //点击存在的电影名称将会开启编辑页面
            }
        </ul>
    </div>
</div>

  //以下是在MoviesController中的Edit方法/Action代码：
  
        public ActionResult Edit(int id)
        {
            var movie = _context.Movies.Single(m => m.Id == id);    //通过视图中传入的id属性从数据库中获取当前对象信息
            var genres = _context.Genres.ToList();
            var viewModel = new MovieFormViewModel
            {
                Movie = movie,
                Genres = genres
            };
            return View("MovieForm", viewModel);    //同样返回MovieForm页面，与添加电影页面不同的是该页面将从一开始就包含传入对象的信息
        }
  
  //"MoviewForm"视图中的提交按钮将建立一个"Save"的数据请求包，需要用到ModelBinding将数据回传到Model中，再通过Model保存至数据库。整个过程代码如下：
        
        [HttpPost]    //确保只有HttpPost请求才能访问这个Action，阻止HttpGet访问，如通过地址栏的Http请求
        public ActionResult Save(MovieFormViewModel viewModel)    //将Save数据包ModelBinding至viewModel中，MVC将智能识别其中的Movie模型
        {
            if (viewModel.Movie.Id == 0)    //如果数据包中的Id等于0，说明是通过添加电影按钮请求的，应该将Model中的数据作为一个对象添加至数据库
            {
                _context.Movies.Add(viewModel.Movie);   //ChangeTracker记录变化，写入内存中
            }
            else    //否则，说明是对现有的对象信息的编辑
            {
                var movieInDb = _context.Movies.Single(m => m.Id == viewModel.Movie.Id);   //ChangeTracker记录变化，写入内存中

                //以下是允许变动的字段
                movieInDb.Name = viewModel.Movie.Name;
                movieInDb.ReleasedDate = viewModel.Movie.ReleasedDate;
                movieInDb.GenreId = viewModel.Movie.GenreId;
                movieInDb.InStock = viewModel.Movie.InStock;
            }
            _context.SaveChanges();   //保存至数据库中
            return RedirectToAction("Index", "Movies");   //重导向至Movies/Index视图
        }

//暂时想到这么多，最后更新2018/03/13
