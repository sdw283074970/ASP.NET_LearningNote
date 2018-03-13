//Q: 什么是ModelBinding？
//A: ModelBinding是指，MVC框架会将数据请求绑定保存到目标Model中。当提交一个表单，表单中的数据就是一个数据请求，类似于事件，这个请求会触发一个Action，
  //而这个Action就是最开始在View视图模型中建立表单前在Html.BeginForm()方法中声明的Action，被触发的这个Action应该存在于控制器中。

//Q: 触发的这个Action是干什么用的？
//A: 类似于事件，当提交表单后，数据会被打包传入到这个Action中，并赋予到这个Action中的Model中，而这个Model应是这个Action的参数。MVC足够聪明，如果
  //参数中的Model能匹配请求的数据包，那么就会将这些数据保存在Model中，方便接下来通过模型写入到数据库。

  //如，项目需求为将之前的表单填入的数据通过ModelBinding保存到模型中。首先要建立一个submit类的按钮，New视图文件代码如下：

@model Vidly.ViewModels.NewCustomerViewModel
@{
    ViewBag.Title = "New";
    Layout = "~/Views/Shared/_Layout.cshtml";
}

<h2>New Customer</h2>

@using (Html.BeginForm("Creat", "Customers"))   //这个方法的两个参数分别为Action和Controller
{
    <div class="form-group">
        @Html.LabelFor(m => m.Customer.Name)
        @Html.TextBoxFor(m => m.Customer.Name, new { @class = "form-control" })
    </div>
    <div class="form-group">
        @Html.LabelFor(m => m.Customer.Birthday)
        @Html.TextBoxFor(m => m.Customer.Birthday, new { @class = "form-control" })
    </div>
    <div class="checkbox">
        <label>
            @Html.CheckBoxFor(m => m.Customer.IsSubscribedToNewsLetter) Subscribed?
        </label>
    </div>
    <div class="form-group">
        @Html.LabelFor(m => m.Customer.MemberShipTypeId)
        @Html.DropDownListFor(m => m.Customer.MemberShipTypeId,
            new SelectList(Model.MembershipTypes, "Id", "MemberShipTypeName"),
            "Select Membership Type",
            new { @class = "form-control" })
    </div>
    <button type="submit" class="btn btn-primary">Save</button>   //在最后增加一个提交按钮，命名为Save
}

  //类似于事件，当提交按钮按下后，如BeginForm()函数声明的，会出发一个名为"Creat"的Action，这个Action应存在于CustomerController中。
    //在CutomerController新增的Creat方法(Action)代码如下：
    
        [HttpPost]    //保证这个方法只有在HttpPost(提交请求，如提交按钮)的时候才能访问调用，在HttpGet(获取请求，如地址栏访问)的时候不能调用
        public ActionResult Creat(NewCustomerViewModel viewModel)   //参数中的Model就是MVC自动匹配绑定保存数据的地方
        {
            return View();
        }

  //NewCustomerViewModel中包含了Customer这个Model，如果将这个Action的参数直接改为Customer，MVC也可以识别并保存。

//暂时想到这么多，最后更新2018/03/12
