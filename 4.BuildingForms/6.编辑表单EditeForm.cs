//Q: 如何编辑已经存在的对象？
//A: 大体思路为：获取当前对象数据->将获取的数据赋予到新建对象的页面中->保存编辑后的数据。
  //如，项目需求为将Customer列表中指向详细信息的超链接替换为编辑Customer信息页面。按照思路，步骤为下：
  //1.替换超连接，即更改ActionToLink()方法中的参数，将"Details"改为"Edit"；
  //2.在CustomerController中创建Edit方法(Action)；
  //3.在Edit方法中从数据库获取到当前Customer对象数据；
  //4.将Customer对象传入到新建对象视图(New视图)中，为更准确，将New视图更名为CustomerForm，将NewCustomerViewModel改名为CustomerFormViewModel。

  //Customers/Index视图中涉及更改的代码如下：

    <div>
        <ul>
            @foreach (var c in ViewBag.Customers)
            {
                <li>@c.Id</li>
                <li>@Html.ActionLink((string)c.Name, "Edit", "Customers", new { id = c.Id }, null)</li>   //将"Details"改为"Edit"
                <li>@c.MemberShipType.MemberShipTypeName</li>
            }
        </ul>
    </div>

  //Edit方法(Action)代码如下：
  
        public ActionResult Edit(int id)    //参数为id，通过id获取对象
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);   //通过id获取数据库中的对象
            var viewModel = new CustomerFormViewModel   //将获取的对象传入到CustomerFormViewModel中(原NewCustomerViewModel)
            {
                Customer = customer,    //传入customer对象
                MembershipTypes = _context.MemberShipTypes.ToList()   //MembershipTypes在目标视图中是必须的，所以也要传入MemberShipTypes对象
            };
            return View("CustomerForm", viewModel);   //使用View()方法中的另一个重载，即第一参数为指定传入的View的名称
        }

  //完成编辑页面。

//暂时想到这么多，最后更新2018/03/12
