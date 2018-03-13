//Q: 如何将数据保存至数据库中？
//A: 请参照另一个Repository：MicrosoftSQLServer_EntityFramework/7.UpdatingData 中的所有章节。
  //如，项目需求为将客户填写的表单数据保存至数据库中，Creat()方法(Action)的代码如下：

        [HttpPost]
        public ActionResult Creat(NewCustomerViewModel viewModel)
        {
            _context.Customers.Add(viewModel.Customer);   //试用Add()方法将ModelBinding保存的递数据添加到数据库中，此时数据仅存在内存中
            _context.SaveChanges();   //将内存中的数据保存至数据库中
            return RedirectToAction("Index", "Customers");    //重导向
        }

  //就是这么简单。

//暂时想到这么多，最后更新2018/03/12
