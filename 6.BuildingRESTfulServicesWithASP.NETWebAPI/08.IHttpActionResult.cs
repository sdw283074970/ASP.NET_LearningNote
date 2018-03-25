//Q: 什么是IHttpActionResult接口？
//A: 这个接口很像ActionResult，有很多辅助函数返回的类都有这种接口，如BadRequest()方法，即返回一个BadRequest页面。

//Q: 这个接口有什么用？
//A: 在RESTful风格的Api中最重要的是遵循REST约定。按照之前的代码，POST请求返回的仍然是200代码，OK状态。在REST约定中，GET请求返回200代码和Ok状态没有
  //问题，但是POST请求约定规定为返回201代码和Created。我们可以通过最后调用Creat()方法返回相同的结果，而且是201代码和Created状态，但是Creat方法返回
  //的是CreatedNegotiatedContentResult<T>类型，而这个类型正是一个执行了IHttpActionResult接口的类，所以可以将IHttpActionResult作为API动作返回
  //的类型。同理，还有Ok()方法返回200代码和Ok状态，其返回对象OkNegotiatedContentResult<T>也执行了IHttpActionResult。换句话说，可以将API所有的返回
  //类都改为IHttpActionResult。

//Q: 如何使用IHttpActionResult重构整个API?
//A: 重构后的代码如下：

using AutoMapper;
using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using Vidly.Dtos;
using Vidly.Models;

namespace Vidly.Controllers.Api
{
    public class CustomersController : ApiController
    {
        private ApplicationDbContext _context;

        public CustomersController()
        {
            _context = new ApplicationDbContext();
        }

        // GET /api/customers
        public IHttpActionResult GetCustomers()   //将CustomerDto类型改为IHttpActionResult
        {
            return Ok(_context.Customers.ToList().Select(Mapper.Map<Customer, CustomerDto>));   //Ok()方法中直接传入返回的对象
        }

        // GET /api/customers/1
        public IHttpActionResult GetCustomer(int id)    //将CustomerDto类型改为IHttpActionResult
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customer == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            return Ok(Mapper.Map<Customer, CustomerDto>(customer));   //Ok()方法中直接传入返回的对象
        }

        // POST /api/customers
        [HttpPost]
       public IHttpActionResult CreateCustomer(CustomerDto customerDto)    //将CustomerDto类型改为IHttpActionResult  
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var customer = Mapper.Map<CustomerDto, Customer>(customerDto);
            _context.Customers.Add(customer);
            _context.SaveChanges();

            customerDto.Id = customer.Id;

            //Created()方法第一个参数为Uri(Unified Resource Identifier)，即当前的请求地址+新建的对象的Id，这样状态一栏才会是Created
            //第二个参数就为原本要返回的对象，这里即是customerDto
            return Created(new Uri(Request.RequestUri + "/" + customer.Id), customerDto);
        }

        //PUT /api/customers/1
        [HttpPut]
        public void UpdateCustomer(int id, CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var customerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);
            if (customerInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            Mapper.Map<CustomerDto, Customer>(customerDto, customerInDb);

            _context.SaveChanges();
        }

        // DELETE /api/customer/1
        [HttpDelete]
        public void DeleteCustomer(int id)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var customerIDB = _context.Customers.SingleOrDefault(c => c.Id == id);
            if (customerIDB == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            _context.Customers.Remove(customerIDB);
            _context.SaveChanges();
        }
    }
}

  //需要注意的是，在此例中我们需要将CustomerDto类做一些修改，即将Birthday字段的自定义验证条件删除，该自定义验证的代码如下：
  
    public class Min18YearsIfAMember : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //这里是将挂载的对象实例转换为Customer，但实际挂载对象是CustomerDto，者两个类只有连接关系没有继承关系，继续挂载会抛出异常
            var customer = (Customer)validationContext.ObjectInstance;    
            //...余下代码省略
        }
    }
    
  //理想情况下我们应该只有一种方法去新建一个Customer实例，但是目前有两个通往前端的点。一个是API，一个是返回Razor视图的正常的MVC动作。通常情况下只用
    //其中的一个End-point。只使用API的项目被称为纯后端项目，使用Razor视图和正常MVC动作(Action)的被称为全端(Full Stack)项目。如果想将这个项目转成
    //纯后端项目，则需要弃用所有的正常的Action，然后更改视图来发布API。目前情况，只用将CustomerDto中的自定义验证特性删除就行了。

//暂时想到这么多，最后更新2018/03/21
