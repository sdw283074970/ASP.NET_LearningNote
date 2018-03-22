//Q: 什么是AutoMapper？
//A: AutoMapper是一个包，命名空间为AutoMapper，其中有众多的类、方法和接口。AutoMapper的核心功能之一是将两个类中的字段连接起来形成映射，当来源类中的
  //字段改变时，目的类对应的字段也跟着改变。

//Q: 在ASP.NET MVC中，AutoMapper有什么用？
//A: AutpMapper可以将目的类TDestination的关联字段的值赋值于起始类TSource中的关联字段。在ASP.NET中，用户端和服务端会有数据传输，如用户端向服务端
  //API发送HttpGet、HttpPost等请求，服务端再通过从API收到的请求对数据库进行操作，再通过API返回粗数据给用户端。之前分析过，不能直接返回领域模型类，
  //否则会增强客户端与API的耦合性，还会产生潜在的安全漏洞。为了解决这一问题，特别建立一套数据转换对象类DTO替代API中的领域模型返回给用户端，而
  //AutoMapper就能将领域模型与它的DTO关联起来，让DTO也能起到领域模型在API中的作用而不设计领域模型本身，即改变DTO就可以改变领域模型，反之亦然。

//Q: 如何使用AutoMapper关联领域模型类和DTO？
//A: 以Vidly为例，使用AutoMapper关联领域模型Customer和CustomerDto，需要跟随以下步骤：
  //1.首先需要在项目中安装AutoMapper。打开PackageManager，输入install-package automapper即可安装最新版本；
  //2.然后在App_Star文件夹下建立一个叫MappingProfile的类，这个类的代码如下：

using AutoMapper;
using Vidly.Dtos;
using Vidly.Models;

namespace Vidly.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //因为用户端与服务端的数据沟通是双向的，所以需要建立一个往返的映射关系，即其中任意一类发生改变，都将直接改变另一个类
            Mapper.CreateMap<Customer, CustomerDto>();    //以Customer作为起始类，CustomerDto作为目的类，建立映射关系
            Mapper.CreateMap<CustomerDto, Customer>();    //以CustomerDto为起始类，Customer作为目的类，建立映射关系
        }
    }
}

  //3.在Global.asax.cs中的Application_Start()方法中第一行添加以下命令：
  
            Mapper.Initialize(c => c.AddProfile<MappingProfile>());   //将建立的映射关系初始化

  //至此前期准备工作完毕。

//Q: 为什么说用户端和服务端的数据通信是双向的？
//A: 以Vidly为例，在不使用DTO的情况下：
  //如用户端向服务器发送GET请求，请求返回Customers列表，请求包首先抵达API，然后API执行对应动作(Action) GetCustomers，这个动作将数据库中的数据打包
    //成IEnumerable<Customer>返回给用户端，这是服务器的数据库到用户端的方向； 
  //如果用户端向服务器发送PUT请求，请求更新一个已有的Customer实例到数据库，包含改动的数据请求包首先抵达API，然后API执行UpdateCustomer，这个动作将
    //请求包中的数据写入到数据库，这是用户端到服务端的方向。

//Q: 如何在API中应用AutoMapper将领域模型替换成DTO？
//A: 调用AutoMapper中的Mapper.Map<TSource, TDestination>(TSource source)方法即可将起始类TSource转换成目的类TDestination。如，要将Customer的实
  //转换成CustomerDto的实例customerDto，则只用调用这个方法即可，代码如下：

        var customerDto = Mapper.Map<Customer, CustomerDto>(customer);

  //在Vidly例子中，如果要将GET /api/customers用AutoMapper将领域模型转换成DTO，则代码如下：

        // GET /api/customers
        public IEnumerable<CustomerDto> GetCustomers()    //通信方向为从服务端到用户端，所以将返回类型改为IEnumerable<CustomerDto>
        {
            //_context.Customers.ToList()方法返回的是IEnumerable<Customer>，//如果要返回CustomerDto就需要转换
            return _context.Customers.ToList().Select(Mapper.Map<Customer, CustomerDto>);   //使用Select()方法返回CustomerDto
        }

    //Select()方法中填充的是方法名而不是调用方法，是因为Select()方法要求其参数为一个委托，所以只能传入方法名或者委托列表。

  //如果要将GET /api/customers/1用AutoMapper将领域模型转换成DTO，则代码如下：

        // GET /api/customers/1
        public CustomerDto GetCustomer(int id)    //通信方向为从服务端到用户端，所以将返回类型改为CustomerDto
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customer == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            return Mapper.Map<Customer, CustomerDto>(customer);   //直接调用Mapper.Map()方法转换
        }

  //如果要将 POST /api/customers用AutoMapper将领域模型转换成DTO，则代码如下：

        // POST /api/customers
        [HttpPost]
        //通信方向为从用户端到服务端数据库，所以发送的请求包中的数据类型为CustomerDto，同时返回给客户端的数据类型也为CustomerDto
        public CustomerDto CreateCustomer(CustomerDto customerDto)    
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            var customer = Mapper.Map<CustomerDto, Customer>(customerDto);    //将customerDto转回数据库能识别的customer
            _context.Customers.Add(customer);   //添加customer不表
            _context.SaveChanges();

            customerDto.Id = customer.Id;   //因为数据库会为customer自动生成Id，我们要将这个Id返回给客户端

            return customerDto;
        }

  //如果要将 PUT /api/customers用AutoMapper将领域模型转换成DTO，则代码如下：

        //PUT /api/customers/1
        [HttpPut]
        //通信方向为从用户端到服务端数据库，所以发送的请求包中的数据类型为CustomerDto，但由于是更新，就不用返回给用户端
        public void UpdateCustomer(int id, CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var customerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);   //获取数据库中的数据冰添加引用
          
            if (customerInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            //直接调用Mapper.Map()方法，这里使用两个参数的重载，即直接将第一个参数连接转换并赋值给第二个参数
            //这里为将customerDto中的所以字段都赋值给customerInDb中的对应字段
            Mapper.Map<CustomerDto, Customer>(customerDto, customerInDb);

            _context.SaveChanges();
        }

  //如果要将DELETE /api/customers就不用DTO作为中介了，直接删除即可。

//Q: AutoMapper是如何将两个类关联映射到一起的？
//A: 默认约定中，AutoMapper将两个类中的同名的字段关联到一起。但是也可以覆写字段名，即让两个不同名但同类型的字段相关联，也可以创建自定义的关联类。
  //这需要阅读AutoMapper官方文档来学习如何使用。

//暂时想到这么多，最后更新2018/03/21
