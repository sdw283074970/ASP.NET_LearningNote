//Q: 如何在ASP.NET中搭建API?
//A: 首先，在Controllers文件夹中建立一个新的文件夹专门储存api，将这个文件夹命名为Api。以Vidly为例，现在搭建一个针对/Customers资源的api。
  //1.在Api文件中添加一个Controller，选择Web API 2 Controller模板，将这个控制器命名为CustomersController(约定)；
  //2.如果首次在项目中添加ApiController，需要在Global.asax.cs文件中的Application_Start()方法第一行加入以下命令：

            GlobalConfiguration.Configure(WebApiConfig.Register);

  //在Api文件夹下的CustomersController就是针对Customers资源搭建的API。假设项目需求为：建立一个Customers资源的API，通过调用这个API能够：
    //1.获取Customers资源列表的粗数据；
    //2.获取某一具体的Customer资源的粗数据；
    //3.新建一个Customer资源；
    //4.编辑一个具体的Customer资源；
    //5.删除一个棘突的Custoemr资源。

  //满足以上需求的API代码如下：

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vidly.Models;

namespace Vidly.Controllers.Api
{
    public class CustomersController : ApiController
    {
        private ApplicationDbContext _context;    //定义数据库字段

        public CustomersController()    //通过构造器注入数据库实例
        {
            _context = new ApplicationDbContext();
        }

        // GET /api/customers 获取Customers资源列表的粗数据
        public IEnumerable<Customer> GetCustomers()   //返回资源列表
        {
            return _context.Customers.ToList();
        }

        // GET /api/customers/1 获取某一具体的Customer资源的粗数据
        public Customer GetCustomer(int id)   //返回一个Customer资源
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);   //根据id查询数据库的目标并返回

            if (customer == null)   //如果数据不存在，则抛出404
                throw new HttpResponseException(HttpStatusCode.NotFound);
            return customer;
        }

        // POST /api/customers 新建一个Customer资源
        [HttpPost]  //保证只有HttpPost请求能访问这个方法
        //约定为，当新建一个资源，则将这个新资源返回给客户端。因为这个资源可能会携带服务器生成的Id
        public Customer CreateCustomer(Customer customer)   //签名中的customer为请求发送者，ASP.NET会自动将其初始化
        {
            if (!ModelState.IsValid)    //首先验证模型状态，如果存在验证失败的情况，则抛出BadRequest异常
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            _context.Customers.Add(customer);   //如果模型没问题，则将请求发送者customer添加到数据库中
            _context.SaveChanges();   //保存至数据库

            return customer;
        }

        //PUT /api/customers/1 编辑一个具体的Customer资源
        [HttpPut]   //保证只有HttpPut请求能访问这个方法
        //这里返回一个资源实体或者不返回都可以
        public void UpdateCustomer(int id, Customer customer)   //同上，customer为请求发送者，会被自动初始化
        {
            if (!ModelState.IsValid)    //检查模型状态是否通过验证
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var customerIDB = _context.Customers.SingleOrDefault(c => c.Id == id);    //获取数据库中需要被更改的对象
            if (customerIDB == null)    //如果数据库中没有此对象，则抛出404
                throw new HttpResponseException(HttpStatusCode.NotFound);

            //以下为更新具体数据，还可以用DTO、AutoMapper
            customerIDB.Name = customer.Name;
            customerIDB.IsSubscribedToNewsLetter = customer.IsSubscribedToNewsLetter;
            customerIDB.MemberShipTypeId = customer.MemberShipTypeId;
            customerIDB.Birthday = customer.Birthday;

            _context.SaveChanges();   //保存至数据库
        }

        // DELETE /api/customer/1 删除一个棘突的Custoemr资源
        [HttpDelete]    //保证只有HttpDelete请求能访问这个方法
        public void DeleteCustomer(int id)
        {
            if (!ModelState.IsValid)    //检查模型状态
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var customerIDB = _context.Customers.SingleOrDefault(c => c.Id == id);    //从数据库中获取需要删除的对象
            if (customerIDB == null)    //如果查无此对象，则抛出404
                throw new HttpResponseException(HttpStatusCode.NotFound);

            _context.Customers.Remove(customerIDB);   //在Change Tracker内存中移除对象
            _context.SaveChanges();   //保存变化到数据库
        }
    }
}

  //至此，一个满足需求的API搭建完成。

//暂时想到这么多，最后更新2018/03/20

