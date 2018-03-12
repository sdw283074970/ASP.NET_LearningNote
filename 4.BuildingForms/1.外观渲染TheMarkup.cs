//Q: 什么是Markup？
//A: Markup指将纯HTML视图通过CSS(如Bootsrap)渲染成更加视觉友好的视图。

//Q: 如何使用Markup？
//A: 首先要加载Bootstrap主题文件。针对目标元素，在标签中使用class= "xxx-xxx"即可使用对应的Markup效果。更多对应细节上 getbootstrap.com/css 查看。
  //如，项目需求建立顾客注册页面，以HTML框架建立的表单是很丑的，在对应元素声明与bootstrap对应的class即可完成渲染。如Customers/New的返回视图为：

@model Vidly.Models.Customer
@{
    ViewBag.Title = "New";
    Layout = "~/Views/Shared/_Layout.cshtml";
}

<h2>New Customer</h2>

//以下为Razor语法
//Html.BeginForm函数本身渲染一个<Form>标签，使用Using代码块会在结束时调用Disposal()方法，生成一个</Form>标签
@using (Html.BeginForm("Creat", "Customers"))   
{
    <div class="form-group">    //"form-group"即是对此div块进行渲染
        @Html.LabelFor(m => m.Name)   //使用辅助函数建立Label，参数为模型Customer的Name字段，等同<Label>@m.Name</Label>
        @Html.TextBoxFor(m => m.Name, new { @class = "form-control"})   //同上，第二个匿名参数等同为在标签中声明class属性
    </div>
    <div class="form-group">    //同是表单，同上
        @Html.LabelFor(m => m.Birthday)
        @Html.TextBoxFor(m => m.Birthday, new { @class = "form-control" })
    </div>
    <div class="checkbox">    //checkbox块有专有的Markup，登陆bootstrap.com/css可查看详细
        <label>
            @Html.CheckBoxFor(m => m.IsSubscribedToNewsLetter) Subscribed?    //使用辅助函数HTML建立checkbox
        </label>
    </div>
}

    //需要注意的是，所有Html.XxxFor()函数等同于在标签中声明<Xxx for="">属性，效果为点击这个区域等同点击指定区域，如点击顾客姓名，等于点击表单。

//暂时想到这么多，最后更新2018/03/12
