//Q: 什么是反伪造令牌？
//A: 反伪造令牌(Anti-forgeryToken)是ASP.NET自带的保证所有Http请求都来自于原本的网页页面的防御手段。

//Q: 为什么需要防御？
//A: 没有这个令牌的情况下网页应用有一个很大的漏洞，容易受到CSRF攻击。

//Q: 什么是CSRF攻击？
//A: CSRF攻击即指跨网页伪造请求攻击(Cross-site Request Forgery)。其方式为通过其他网页页面来发送被攻击网页的请求，来达到其不可告人的目的。

  //一般来说，用户对某个服务器都有一段时间的授权，默认20分钟，即如果把页面打开后即使不使用，这个页面对进入服务器有20分钟的授权。假设Vidly应用已经商用，
  //一个音像租赁店正在试用它来管理客户和音像存货，一个黑客建立了一个恶意页面来引导音像店管理员去点击，在页面中黑客可以在某一张图片中嵌入一些JavaScript
  //代码，当恶意页面被加载，它就可以通过音像管理员的浏览器发送HttpPost请求到音像店服务器中。因为用户的浏览器此时仍然有进入服务器的授权(20分钟)，这些恶
  //意请求就会被执行。

  //这种情况下，黑客就通过其他网页伪造了音像店网页的请求，这意味着黑客可以为所欲为的假装管理员对整个数据库经行破坏。最关键的是，数据库记录仅会显示这些
    //请求都来自音像店的IP而不是黑客的，这也意味着无法追踪到黑客。

//Q: 如何阻止CSRF攻击？
//A: 只用保证所有的请求都来自于Vidly应用中的CustomerForm页面即可。按两步走来完成防御：
  //1.在需要防御的页面视图中使用辅助函数@Html.AntiForgeryToken()，这会渲染出一个隐藏的标签字段，这个字段带一个乱码口令string值，这个值也会存在用户的
    //Cookie中，以另一个加密格式储存。当用户进入这个页面，服务器就会将这连个口令发给用户，当用户发送一个HttpPost请求时，服务器就会拿请求中的口令和
    //Cookie中的加密口令匹配，如果匹配成功则说明是从正确的网页中发出的请求，否则就是CSRF攻击，因为如果黑客引导用户重定向一个恶意页面，他就不能访问
    //这个隐藏的字段，因为这个隐藏字段只有当用户真正访问到这个页面时才会生成；
  //2.在HttpPost的Action上修饰[ValidateAntiForgeryToken]即可完成防御，如以下代码：

        [HttpPost]
        [ValidateAntiForgeryToken]    //启用反伪造防御
        public ActionResult Save(CustomerFormViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                var customerViewModel = new CustomerFormViewModel()
                {
                    Customer = new Customer(),
                    MembershipTypes = _context.MemberShipTypes.ToList()
                };
                return View("CustomerForm", customerViewModel);
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

//暂时想到这么多，最后更新2018/03/18
