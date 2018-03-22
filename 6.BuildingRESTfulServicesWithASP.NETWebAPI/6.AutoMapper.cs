//Q: 什么是AutoMapper？
//A: AutoMapper是一个包，命名空间为AutoMapper，其中有众多的类、方法和接口。AutoMapper的核心功能之一是将两个类中的字段连接起来形成映射，当来源类中的
  //字段改变时，目的类对应的字段也跟着改变。

//Q: 在ASP.NET MVC中，AutoMapper有什么用？
//A: 用户端和服务端会有数据传输，如用户端向服务端API发送HttpGet、HttpPost等请求，服务端再通过从API收到的请求对数据库进行操作，再通过API返回粗数据
  //给用户端。之前分析过，不能直接返回领域模型类，否则会增强客户端与API的耦合性，还会产生潜在的安全漏洞。为了解决这一问题，特别建立一套数据转换对象类
  //DTO替代API中的领域模型返回给用户端，而AutoMapper就能将领域模型与它的DTO关联起来，让DTO也能起到领域模型在API中的作用而不设计领域模型本身。

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
//A:
