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

            //Created()方法第一个参数为Uri，即Unified Resource Identifier，
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

