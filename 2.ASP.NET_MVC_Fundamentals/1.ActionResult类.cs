//Q: 什么是ActionResult类？
//A: ActionResult是ASP.NET中所有Action Results的基类，即很多其他呈现给用户的Result类都是基于这个类衍生出来的，根据Action的种类返回一个起源于这个
  //基类的Result实例，如ViewResult。在之前的示例代码中，我们声明了一个Random()方法，返回一个ActionResult类，可以看到具体是通过一View()方法返回的。
  //这个View()方法在基类Controller中，其返回的是一个ViewResult类。所以，之前的Random()方法也可以将View()方法等价替换成一个ViewResult示例，代码如下：

        public ActionResult Random()
        {
            var movie = new Movie() { Name = "WTF"};
            return new ViewResult();    //直接返回一个ViewResult实例，说明ViewResult是起源于ActionResult的子类型，可直接上转型
        }

//Q: 这种情况下为什么还要用基类ActionResult作为返回对象？直接用目标类如ViewResult不行吗？
//A: 的确可以，把ActionResult类替换成具体派生类不仅准确，还能提升单元测试效率(下一部分说明)。但是有时候，在一个Action中可能会有不同的路径或返回多个
  //不同的ActionResult派生类，这个时候还是用基类比较有道理。

//Q: 有多少种ActionResult派生类？
//A: 常见的有这么几种：
  //1.ViewResult：通过辅助函数View()返回View；
  //2.PartialViewResult：通过辅助函数PartialView()返回PartialView；
  //3.ContentResult：通过辅助函数Content()返回一系列简单的文本；
  //4.RedrictResult：通过辅助函数Redirect()将用户重新定向ULR；
  //5.RedirectToRouteResult：通过辅助函数RedirectToAction()将用户重新指定一个Action而不是URL；
  //6.JsonResult：通过辅助函数Json()返回一个序列化的Json对象；
  //7.FileResult：通过辅助函数File()返回一个File；
  //8.HttpNotFoundResult：通过辅助函数HttpNotFound()返回一个著名的404页面；
  //9.EmptyResult：相当于Void()，用于不需要返回任何值的时候。

  //可以看到，除了EmptyResult，其他所有Action Reults在基类Controller中都有对应的辅助函数。
  //其他都没什么好说的，都是很简单的应用，值得一提的是RedirectToAction()方法返回的RedirectToRouteResult，即以向用户重新指定Action和Controller
    //的方式实施重定向，如要将用户重定向至主页，代码如下：

        public ActionResult Random()
        {
            var movie = new Movie() { Name = "WTF"};
            //return RedirectToAction("Index", "Home");   //按照Action，Controller的顺序传入参数，等于Controller/Action的HTTP请求
            return RedirectToAction("Index", "Home", new { page = 1, sortBy = "Name" });    //如Action需要传入参数，则传入一个匿名实例转载参数
            //以上等于vidly.com/?page=1&sortBy=Name
        }

  //基本上Action Results看起很多，但不用全背下来。以上所提到的可能常见，但常用的只有三个：ViewResult、HttpNotFoundResult和RedirectResult。

//暂时想到这么多，最后跟新2018/02/22
