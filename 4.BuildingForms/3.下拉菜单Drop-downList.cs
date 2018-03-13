//Q: 如何建立下拉菜单？
//A: 使用@Html.DropDownListFor()函数即可。这个函数有7个重载，一般我们用有四个参数签名的那一个，这四个参数分别是：
  //1.Func<TModel, TEmun>: 匿名表达式，传入一个Model，返回一个IEnumerable类型的对象；
  //2.SelectList对象，即下拉菜单的实体，这个类的构造器有很多重载，一般用(IEnumerable items, string dataValueField, string dataTextField)
    //这个重载，即在声明下拉菜单对象实例的时候传入三个参数，分别是IEnumerable对象(拉出来有哪些选项)、每个选项代表的值、每个选项的名字；
  //3.一个string对象，可以为空""，即下拉菜单默认占位符；
  //4.HtmlAttributes对象，即Html标签属性。

  //如在注册会员页面中新增一个会员类型选项，需要用到下拉菜单，首先需要在控制器中传入MemberShipType这个Model，同时还要传入Customer的Model，所以新建
    //一个NewCustomerViewModel来封装这两个Model，其代码如下：

namespace Vidly.ViewModels
{
    public class NewCustomerViewModel
    {
        public IEnumerable<MembershipType> MembershipTypes { get; set; }    //使用IEnumerable的目的在于让整个程序更松耦合
        public Customer Customer { get; set; }
    }
}

  //在CustomerController中首先将MemberShipType和Customer传入这个NewCustomerMViewodel中，再将这个ViewModel传入View视图中，控制器新增代码如下：

        public ActionResult New()
        {
            var membershipTypes = _context.MemberShipTypes.ToList();    //从数据库获取会员种类数据
            var viewModel = new NewCustomerViewModel    //将会员种类数据传入ViewModel中
            {
                MembershipTypes = membershipTypes
            };
            return View(viewModel);   //将ViewModel传入到View中
        }

  //在View视图中，创建下拉菜单的代码如下：

    <div class="form-group">
        @Html.LabelFor(m => m.Customer.MemberShipTypeId)    //下拉菜单的标签名，即MemberShipTypeId
        @Html.DropDownListFor(m => m.Customer.MemberShipTypeId,   //选择Id做对象是因为要将MemberShipTypeId值保存到Customer中
            new SelectList(Model.MembershipTypes, "Id", "MemberShipTypeName"),    //创建下拉菜单实体
            "Select Membership Type",   //空白占位符
            new { @class = "form-control" })    //Html标签属性
    </div>
    
//暂时想到这么多，最后更新201803/12
