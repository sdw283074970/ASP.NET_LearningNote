//Q: 什么是RESTfulConvention？
//A: REST指Representational State Tranfer，这是一种设计约定，遵守这种设计约定设计的API我们称之为RESTful API。

//Q: REST约定的具体内容是什么？
//A: 具体内容为将访问资源的动作(Action)省略，具体执行动作交由HTTP请求来划定。如之前的例子，在不使用RESTful风格API的情况下：
  //访问Customers资源的默认页面是通过路由/Customers/Index或直接/Customers(因为路由默认值的设置)，这是一个HttpGet请求；
  //获取一个具体Customer访问的是Customers/Details/1，这也是一个HttpGet请求；
  //新建一个Customer并同步至数据库访问的是Customers/Save，这是一个HttpPost请求；
  //更新一个Customer并同步至数据库访问的是Customers/Edit/1(虽然返回的视图相同)，这是一个HttpPut请求；
  //删除一个Customer并同步至数据库访问的是Customers/Delete(假设有这个Action)，这是一个HttpDelete请求。

  //我们可以看到，只要是获取资源，都是HttpGet请求；只要是新建资源都是HttpPost请求；只要是更新资源都是HttpPut请求；只要是删除资源都是HttpDelete请求。

  //既然请求就这么几种，又何必分那么多的Action呢？于是REST约定就来了：在API中省略Action，加以Http请求标记即可。以上例子的路由在RESTful API中如下：

  //访问Customer主页： GET /api/Customers
  //获取一个Customer： GET /api/Customers/1
  //新建一个Customer： POST /api/Customers
  //更新一个Custoemr： PUT /api/Customers/1
  //删除一个Customer： DELETE /api/Customer/1

  //所以我们省略了Action。在后面搭建API的时候，我们就使用如上RESTful风格的API约定。

//暂时想到这么多，最后更新2018/03/20
