//Q: 什么是Validation？
//A: Validation即验证，验证的根本目的在于确保整个程序按照逻辑运行。在没有验证的情况下，如果执行命令不符合逻辑，程序将会中止并抛出Invalidation的异常。
  //如，某一个字段要求不能为空，如果程序为一个空的输入放行，则会产生一系列连锁效应，让整个程序无法正常执行。所以我们需要一道“安检”过程，确保程序的执行
  //符合逻辑。
  
//Q: 为什么要添加验证？
//A: 添加验证保证了程序符合逻辑地运行，同时可以在不中断程序的情况下将错误信息反馈给用户，让用户加以修正后再执行。一个常见的例子为，当我们在新网站上注册
  //时，如果两次确认密码不一样，网页会告诉我们密码不匹配，我们可以直接修正这个错误而不造成应用崩溃。这就是验证的典型作用。
  
//Q: 如何为程序添加验证？
//A: 在ASP.NET中添加验证分三步走，分别是：
  //1.设立逻辑规则。如将Customer的Name字段最大长度设为255且不能为空，其代码如下：
  
namespace Vidly.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]    //Name字段不能为空
        [StringLength(255)]   //Name字段最大长度为255
        public string Name { get; set; }

        public bool IsSubscribedToNewsLetter { get; set; }

        public MembershipType MemberShipType { get; set; }

        [Display(Name = "MemberShip Type")]
        public byte MemberShipTypeId { get; set; }

        [Display(Name = "Date of Birth")]
        public DateTime? Birthday { get; set; }
    }
}

  //2.添加验证逻辑。在ASP.NET中，有一个静态类布尔属性ModelState.IsValid来储存模型的验证状态，通过这个属性可以判断整个程序的运行是否符合逻辑。如果
    //在新建或者编辑Customer的页面中，Name字段不符合逻辑要求(如输入为空)，说明程序存在不符合逻辑的情况，ModelState.IsValid的值为False。这样，
    //我们可以通过ModelState.IsValid的值判断程序是否有问题。如果有问题，就需要返回带错误信息的视图给用户，从而阻止程序中止。
    
    //如项目需求为在添加顾客页面和编辑页面中，即使用户输入不符合逻辑的数据，程序也不能崩溃，还要能告诉用户出错的地方。
    
    //据分析，相关的视图为“CustomerForm”，相关的Controllor和Action为"Customer/Save"。我们只用修改Save()方法即可判断程序的验证性。代码如下：
    
        [HttpPost]
        public ActionResult Save(CustomerFormViewModel viewModel)
        {
            if(!ModelState.IsValid)   //通过ModelState.IsValid的值判定程序是否符合逻辑、有效。如果不有效，则返回带错误提示的视图
            {
                var customerViewModel = new CustomerFormViewModel()
                {
                    Customer = new Customer(),
                    MembershipTypes = _context.MemberShipTypes.ToList()
                };
                return View("CustomerForm", customerViewModel);   //返回带错误提示的视图
            }
            if (viewModel.Customer.Id == 0)
                _context.Customers.Add(viewModel.Customer);
            else
            {
                var customerInDb = _context.Customers.Single(c => c.Id == viewModel.Customer.Id);
                
                customerInDb.Name = viewModel.Customer.Name;
                customerInDb.Birthday = viewModel.Customer.Birthday;
                customerInDb.IsSubscribedToNewsLetter = viewModel.Customer.IsSubscribedToNewsLetter;
                customerInDb.MemberShipTypeId = viewModel.Customer.MemberShipTypeId;
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Customers");
        }
    
  //3.更改相关返回视图，将验证失败的信息添加进视图中并返回给用户。在ASP.NET中，通过辅助函数Html.ValidationMessageFor()可以将错误信息反馈给用户。
    //该方法的第一个参数要求为模型中带逻辑验证的字段，通过验证这个指定的字段来将默认的错误信息反馈给用户。更改后的视图文件如下：
    
@model Vidly.ViewModels.CustomerFormViewModel
@{
    ViewBag.Title = "CustomerForm";
    Layout = "~/Views/Shared/_Layout.cshtml";
}

<h2>New Customer</h2>

@using (Html.BeginForm("Save", "Customers"))
{
    <div class="form-group">
        @Html.LabelFor(m => m.Customer.Name)
        @Html.TextBoxFor(m => m.Customer.Name, new { @class = "form-control" })
        @Html.ValidationMessageFor(m => m.Customer.Name)    //使用Html.ValidationMessageFor()辅助函数将错误信息显示在网页中
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
    @Html.HiddenFor(m => m.Customer.Id);
    <button type="submit" class="btn btn-primary">Save</button>
    @Html.ActionLink("Cancel", "Index", "Customers", null, new { @class = "btn btn-danger" })
}

  //完成以上三步，针对Customer.Name的验证就完成。在Name字段为空的情况下调用Save()方法，不会造成程序中止，而会返回带错误信息的视图。
  
//暂时想到这么多，最后更新2018/03/15
