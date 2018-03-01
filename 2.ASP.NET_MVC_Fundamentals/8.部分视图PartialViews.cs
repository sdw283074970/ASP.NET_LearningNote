//Q: 什么是部分视图？
//A: 部分视图即视图的一部分，可以将视图分割成若干部分分开保存，这样做的好处是易于维护。注意部分视图(PartialView)并不是布局(Layout)。

//Q: 如何分割创建部分视图？
//A: 对于已有的视图，如果我们想将其分割，需要先建立一个部分视图文件，然后将需要分割的部分拷贝到部分视图文件中，最后在原视图文件中调用部分视图即可。
  //以ASP.NET默认的视图布局_Layout为例，如果我们要将_Layout视图文件中的导航条部分拆出来单独做成导航条部分视图，步骤如下：

  //1.建立部分视图文件。右键_Layout所在文件夹，选择新建View，约定共识将部分视图名称前加下划线，即命名为_NavBar，勾选PartialView，完成建立；
  //2.剪切粘贴目标区块。打开_Layout文件，找到导航条区块，按住Ctrl连按两下M键将其折叠，剪切粘贴到_NavBar文件中，保存；
  //3.在原位置调用@Html.Partial()方法，将部分视图作为参数传递进去。代码如下：

  //导航条部分组成的部分视图代码：

<div class="navbar navbar-inverse navbar-fixed-top">
    <div class="container">
        <div class="navbar-header">
            <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
            </button>
            @Html.ActionLink("Application name", "Index", "Home", new { area = "" }, new { @class = "navbar-brand" })
        </div>
        <div class="navbar-collapse collapse">
            <ul class="nav navbar-nav">
                <li>@Html.ActionLink("Home", "Index", "Home")</li>
                <li>@Html.ActionLink("About", "About", "Home")</li>
                <li>@Html.ActionLink("Contact", "Contact", "Home")</li>
            </ul>
            @Html.Partial("_LoginPartial")
        </div>
    </div>
</div>

  //在_Layout原导航条位置调用@Html.Partial("_NavBar")方法
  
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>@ViewBag.Title - My ASP.NET Application</title>
    @Styles.Render("~/Content/css")
    @Scripts.Render("~/bundles/modernizr")

</head>
<body>
    @Html.Partial("_NavBar")    //在此处调用Html.Partial()方法
    <div class="container body-content">
        @RenderBody()
        <hr />
        <footer>
            <p>&copy; @DateTime.Now.Year - My ASP.NET Application</p>
        </footer>
    </div>

    @Scripts.Render("~/bundles/jquery")
    @Scripts.Render("~/bundles/bootstrap")
    @RenderSection("scripts", required: false)
</body>
</html>

//暂时想到这么多，最后更新2018/03/01
