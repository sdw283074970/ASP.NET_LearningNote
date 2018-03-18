//Q: 什么是验证信息总结？
//A: 很多开发者喜欢将当前页面的所有验证失败的信息汇总传递给用户。为了方便快捷，可使用辅助函数Html.ValidationSummary()。这个函数将渲染一系列的HTML
  //标签，用来展示所有的验证错误信息。如随便在CustomForm视图中添加这个辅助函数，如在最开始添加，就会得到所有的错误信息。

  //在上例中，我们可以发连隐藏属性Id也被报错，着个隐藏属性最开始是用来作为是新数据还是编辑数据的判断依据。被报告的原因是Id默认就是Required的，但是
    //在新数据中，这个Id为Null，最后是在数据库中自动添加的。解决方案为将设计到这个Id的Model初始化即可，初始化后默认Id就为0，也就不会报错了。继续在
    //上例中，据分析，报错发生在新建顾客页面中，即通过Customer/New返回的视图。所以我们在CustomerControllor中找到New()方法，为其Model初始化一个
    //Customer实例即可。代码如下：

        public ActionResult New()
        {
            var membershipTypes = _context.MemberShipTypes.ToList();
            var viewModel = new CustomerFormViewModel
            {
                Customer = new Customer(),    //声明一个Customer实例，即初始化所有其中的字段。byte类字段初始化默认为0
                MembershipTypes = membershipTypes
            };
            return View("CustomerForm", viewModel);
        }

  //另外，辅助函数Html.ValidationSummary()有多达13中重载，功能及其强大。如，我们可以使用Html.ValidationSummary(bool excludedPropertyErrors)
    //来忽略所有验证错误信息，也可以使用Html.ValidationSummary(bool excludedPropertyErrors, string message)来忽略验证信息的同时，传递自定义的
    //信息，甚至可以使用Html.ValidationSummary(... object htmlAttributes)来设定该块标签的class、style等等。不做赘述。

//暂时想到这么多，最后更新2018/03/18
