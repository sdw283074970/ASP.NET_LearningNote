//Q: 什么是MVC架构？
//A: MVC代表Model、View、Controller。MVC架构于20世纪70年代提出，至今在网页应用中广泛应用，因此很多Web开发框架强制使用MVC架构经行开发，如ASP.NET、
  //Ruby on Rails 以及 Express 等等。

//Q: 什么是MVC架构中的Model？
//A: Model(模型)是独立于UI的应用数据和行为。如在这次的租碟应用中，主要的Model将包括Movie、Customer、Rental、Transaction等类，这些类拥有属性和方法，
  //这些属性方法纯粹用来表示应用的状态和规则，换句话说这些类并不直接于UI绑定，这意味着我们可以完全将这些类保留下来直接应用在其他平台的应用中，如WPF等。
  //这些Model类被称为Plain Old CLR Objects(POCOs)。

//Q: 什么是MVC架构中的View？
//A: View可以直接理解为UI，在网页应用中是以html的形式呈现给用户。

//Q: 什么是MVC架构中的Controller？
//A: 控制器负责处理HTTP请求。如我们在网页地址栏输入的就是http请求，控制器将收到这些请求，并将其传给Model中对应的部分，并转换为View呈现给用户。如
  //在地址栏输入vidly.com/movies这个请求，通过movies这个控制器就能获取所有数据库中的movies数据并转换成可视页面返回给用户。如果没有/movies这个控制器
  //则返回报错页面，如著名的404页面。

//Q: 如何才能选择到正确的控制器？
//A: 通过Router。Router其实是MVC框架中的另一个重要部分，它负责选择出正确的控制器去处理请求。Router基于某种规则可以认出HTTP请求中的控制器部分，
  //如vidly.com/movies这一个HTTP请求就是Router通过/movies分辨出需要找到movies控制器来处理请求。

//Q: 控制器是怎么回应请求的？
//A: 通过控制器类中的方法来回应请求。如Router通过vidly.com/movies请求知道需要选择MoviesController来处理，MoviesController是一个类，类中就有很多
  //方法，在ASP.NET中我们称之为Action。因此更准确的说法是处理HTTP请求的其实是控制器中的方法(Action)。

//暂时想到这么多，最后更新2018/02/21
