//Q: 如何将编辑后的数据保存至数据库？
//A: 站在EntityFramwork的角度，只用将数据库里的数据作为对象载入到内存中，对内存中的数据进行更改，然后调用SaveChanges()即可。但在MVC中需要涉及的方面
  //会多很多。如，项目需求为将编辑好的表单更新至数据库，有两个方法来实现。第一种是新建和更新表单使用同一个Action，第二种是分开使用，即一个Creat方法
  //一个Update方法(Action)。显然第一种省时省力，让代码复用率更高。实现步骤为：
  
  //1.在CustomerController和CustomerForm视图中将Creat方法(Action)改为Save，以达到形容准确的目的；
  //2.在CustomerForm视图中添加隐藏属性，即Customer的Id，以此作为判断是新建还是更新现有对象的依据；
  //3.修改Save方法(原Creat方法)，当Customer的Id为0时，说明是建立新对象，否则是更新对象；
  //4.保存即可。

  //CustomerForm视图文件代码如下：

@model Vidly.ViewModels.CustomerFormViewModel
@{
    ViewBag.Title = "New";
    Layout = "~/Views/Shared/_Layout.cshtml";
}

<h2>New Customer</h2>

@using (Html.BeginForm("Save", "Customers"))    //将Creat方法改为Save方法，达到准确形容的目的
{
    <div class="form-group">
        @Html.LabelFor(m => m.Customer.Name)
        @Html.TextBoxFor(m => m.Customer.Name, new { @class = "form-control" })
    </div>
    <div class="form-group">
        @Html.LabelFor(m => m.Customer.Birthday)
        @Html.TextBoxFor(m => m.Customer.Birthday, "{0:d MMM yyyy}",new { @class = "form-control" })
    </div>

    <div class="form-group">
        @Html.LabelFor(m => m.Customer.MemberShipTypeId)
        @Html.DropDownListFor(m => m.Customer.MemberShipTypeId,
            new SelectList(Model.MembershipTypes, "Id", "MemberShipTypeName"),
            "Select Membership Type",
            new { @class = "form-control" })
    </div>
    <div class="checkbox">
        <label>
            @Html.CheckBoxFor(m => m.Customer.IsSubscribedToNewsLetter) Subscribed?
        </label>
    </div>
    @Html.HiddenFor(m => m.Customer.Id);    //新增影藏属性，以此作为判断是新对象还是现有对象的依据
    <button type="submit" class="btn btn-primary">Save</button>
}

  //CustomerController中的Save方法(Action)代码如下：
  
        [HttpPost]
        public ActionResult Save(CustomerFormViewModel viewModel)   //Save按钮仍然生成一个数据请求包，绑定至这个Model中
        {
            if (viewModel.Customer.Id == 0)   //如果Customer的Id为0，则说明是新建对象
                _context.Customers.Add(viewModel.Customer);   //将模型中的数据保存到数据库中(此时保存状态仍在内存中)
            else    //如果Id不为0则说明是更新对象
            {
                //使用Single()而不是SingleOrDefault()的原因是一定能找到数据库中的对象，因为Id都在这里摆着了
                var customerInDb = _context.Customers.Single(c => c.Id == viewModel.Customer.Id);   //获取数据库中的对象，以Id作为索引

                //这里有三个方法来更新对象数据。
                //第一种为使用TryUpdateModel()方法，复杂又有安全漏洞，用户将可以随意更改任何部分，下同
                //第二种为自动关联，如Mapper.Map(customer, customerInDb)，使用这种方法必须掌握TDO对象，即TransferDatabaseObject，才能保证安全
                //第三种为手动更新需要的对象，这里强烈推荐第三种方法，虽然写得多，但是安全，能精准规定哪些部分能改哪些部分不能改
                customerInDb.Name = viewModel.Customer.Name;
                customerInDb.Birthday = viewModel.Customer.Birthday;
                customerInDb.IsSubscribedToNewsLetter = viewModel.Customer.IsSubscribedToNewsLetter;
                customerInDb.MemberShipTypeId = viewModel.Customer.MemberShipTypeId;
            }
            _context.SaveChanges();   //保存更改
            return RedirectToAction("Index", "Customers");    //重定向
        }
        
//暂时想到这么多，最后更新2018/03/12
