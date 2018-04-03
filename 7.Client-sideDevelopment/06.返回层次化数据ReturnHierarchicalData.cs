//Q: 什么叫返回层次化数据？
//A: 关系型数据返回的json文件就有层级结构，即表中有表，列中有列。如Vidly例子中顾客和会员类型就为一对多关系，即一个顾客只能有一种会员类型，而一种会员
  //类型可以同时被很多客户拥有。当我们对客户进行查询的时候，除了一般的新名等字段，还应该有会员类型字段，这个会员类型字段就又是一个json数列(或对象)，
  //保存了会员类型具体的信息，这就是层次化数据。

  //当前情况customer信息中并不包含MemberShipType，因为并没有在customerDto中建立这个字段。要想返回层次化的数据，首先在Dto中添加，代码如下：

namespace Vidly.Dtos
{
    public class MemberShipTypeDto    //新建一个MemberShipDto，用来承载来自数据库中的MemberShipType的信息
    {
        //为了安全，只取得两个字段
        public byte Id { get; set; }    
        public string MemberShipTypeName { get; set; }
    }
}

  //然后在CustomerDto中添加MemberShipDto字段。插入以下代码：
  
        public MemberShipTypeDto MemberShipType { get; set; }
  
  //最后在MappingProfile.cs中添加两者的配对，添加以下代码：

        Mapper.CreateMap<MembershipType, MemberShipTypeDto>();

  //目前通过GET方法请求/api/customers返回的数据如下：

[
    {
        "id": 14,
        "name": "CK",
        "isSubscribedToNewsLetter": false,
        "memberShipTypeId": 1,
        "memberShipType": []   //这就是一个次级结构，为会员对象本身的信息，但目前为空
        "birthday": "2018-03-21T00:00:00"
    },
    //其他数据...
]

  //会员类型对象为空的原因在于，在EF中，要想加载关系型对象必须启用贪懒加载或显示加载。在api控制器中更改GET方法启用贪懒加载，代码如下：

        // GET /api/customers
        public IHttpActionResult GetCustomers()
        {
            var customerDtos = _context.Customers
                .Include(c => c.MemberShipType)   //这里启用贪懒加载
                .ToList()
                .Select(Mapper.Map<Customer, CustomerDto>);
            return Ok(customerDtos);
        }

  //至此，API可以被正确返回。返回结果如下：

[
    {
        "id": 14,
        "name": "CK",
        "isSubscribedToNewsLetter": false,
        "memberShipTypeId": 1,
        "memberShipType": {   //贪懒加载后，可以正确显示内容
            "id": 1,
            "memberShipTypeName": "Pay As Go"
        },
        "birthday": "2018-03-21T00:00:00"
    },
    //其他数据...
]

  //现在可以更改DataTable的ajax数据源，让会员类型部分得到正确渲染。代码如下：

            $("#customers").DataTable({
                ajax: {
                    url: "/api/customers",
                    dataSrc: ""
                },
                columns: [
                    {
                        data: "name",
                        render: function (data, type, customer) {
                            return "<a href='/customer/edit" + customer.id +"'>" + customer.name + "</a>";
                        }
                    },
                    {
                        data: "memberShipType.memberShipTypeName"   //直接引用json包中的会员类型数据
                    },
                    {
                        data: "id",
                        render: function (data) {
                            return "<button class='btn btn-link js-delete' data-customer-id=" + data + ">Delete</button>"
                        }
                    }
                ]
            });

//暂时想到这么多，最后更新2018/04/02
