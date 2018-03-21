//Q: 如何测试自己搭建的API是否有用？
//A: 暂时不用真的在一个Web端对API进行调用。可以使用一款叫Postman的三方软件来测试API，模拟GET、POST、DELETE等Http请求，并可以立马看到响应情况。

//Q: 直接访问API控制器的地址会发生什么？
//A: 会得到一个XML超文本文件，基本上只能获得GET请求的结果。如在地址栏输入/api/customers，会返回以下文件：

//This XML file does not appear to have any style information associated with it. The document tree is shown below.
<ArrayOfCustomer xmlns:i="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://schemas.datacontract.org/2004/07/Vidly.Models">
  <Customer>
    <Birthday>2018-03-19T00:00:00</Birthday>
    <Id>1</Id>
    <IsSubscribedToNewsLetter>false</IsSubscribedToNewsLetter>
    <MemberShipType i:nil="true"/>
    <MemberShipTypeId>1</MemberShipTypeId>
    <Name>Dongwei Shi</Name>
  </Customer>
  <Customer>
    <Birthday i:nil="true"/>
    <Id>2</Id>
    <IsSubscribedToNewsLetter>false</IsSubscribedToNewsLetter>
    <MemberShipType i:nil="true"/>
    <MemberShipTypeId>2</MemberShipTypeId>
    <Name>Stoneway</Name>
  </Customer>
  <Customer>
    <Birthday i:nil="true"/>
    <Id>4</Id>
    <IsSubscribedToNewsLetter>true</IsSubscribedToNewsLetter>
    <MemberShipType i:nil="true"/>
    <MemberShipTypeId>2</MemberShipTypeId>
    <Name>Super Man</Name>
  </Customer>
  <Customer>
    <Birthday>1992-11-17T00:00:00</Birthday>
    <Id>5</Id>
    <IsSubscribedToNewsLetter>true</IsSubscribedToNewsLetter>
    <MemberShipType i:nil="true"/>
    <MemberShipTypeId>4</MemberShipTypeId>
    <Name>石东伟</Name>
  </Customer>
  <Customer>
    <Birthday i:nil="true"/>
    <Id>6</Id>
    <IsSubscribedToNewsLetter>true</IsSubscribedToNewsLetter>
    <MemberShipType i:nil="true"/>
    <MemberShipTypeId>4</MemberShipTypeId>
    <Name>Mengdie WU</Name>
  </Customer>
  <Customer>
    <Birthday>2018-03-19T00:00:00</Birthday>
    <Id>7</Id>
    <IsSubscribedToNewsLetter>false</IsSubscribedToNewsLetter>
    <MemberShipType i:nil="true"/>
    <MemberShipTypeId>1</MemberShipTypeId>
    <Name>Cuican Shi</Name>
  </Customer>
</ArrayOfCustomer>

  //之所以形成这xml文件是因为ASP.NET有一个叫做媒体格式器(Media Formatter)的东西。通过动作(Action)返回的数据会基于用户端的请求被Media Formatter
    //格式化。如果一个用户端的请求没有详细说明Content-Type的header，媒体格式器默认将数据以application/xml的格式格式化。但实际上，一般的用户端都是
    //请求将粗数据(Raw Data)以application/json的格式格式化。之前介绍的Postman就能模拟这一过程。
    
  //简而言之，Postman是一个模拟用户端发出Http请求的软件，我们可以试用Postman为Http请求添加header来详细说明我们需要返回json格式的数据。

//Q: 如何使用Postman模拟Http请求？
//A: 从Google商店中添加Postman，打开软件后，在最显眼的地址栏上贴上路由地址，然后点击发送就能模拟一次最简单的HttpGet请求。而请求结果可以在窗口的主
  //要位置看到，默认情况下Postman会自动生成json格式的数据。如，复制http://localhost:50664/api/Customers到地址栏中可以获得以下json数据：
      
[
    {
        "Id": 1,
        "Name": "Dongwei Shi",
        "IsSubscribedToNewsLetter": false,
        "MemberShipType": null,
        "MemberShipTypeId": 1,
        "Birthday": "2018-03-19T00:00:00"
    },
    {
        "Id": 2,
        "Name": "Stoneway",
        "IsSubscribedToNewsLetter": false,
        "MemberShipType": null,
        "MemberShipTypeId": 2,
        "Birthday": null
    },
    {
        "Id": 4,
        "Name": "Super Man",
        "IsSubscribedToNewsLetter": true,
        "MemberShipType": null,
        "MemberShipTypeId": 2,
        "Birthday": null
    },
    {
        "Id": 5,
        "Name": "XXX",
        "IsSubscribedToNewsLetter": true,
        "MemberShipType": null,
        "MemberShipTypeId": 4,
        "Birthday": "1992-11-17T00:00:00"
    },
    {
        "Id": 6,
        "Name": "Mengdie WU",
        "IsSubscribedToNewsLetter": true,
        "MemberShipType": null,
        "MemberShipTypeId": 4,
        "Birthday": null
    }
]
      
  //如果要模拟一次HttpPost请求，则在地址栏旁边将GET改为POST，然后在body标签下的raw区域中输入一个json格式的对象，如：


    {
        //删除掉id是因为服务端会自动生成id
        "Name": "Cuican Shi",
        "IsSubscribedToNewsLetter": false,
        "MemberShipType": null,
        "MemberShipTypeId": 1,
        "Birthday": "2018-03-19T00:00:00"
    }
      
  //注意，POST请求一定要求有在header中对Content-Type的声明，如果不声明，则这个请求的Media Type就是text/plain，ASP.NET的Media Formatter不知道要
    //如何转换成这种格式的数据，就会抛出异常。所以一定要在header中声明Content-Type。点击header标签，在字段一栏填写Content-Type，再在值一栏填写
    //application/json即可在请求中添加对Media Type的声明。点击发送请求，就成功在数据库中新增一个Customer。返回的json代码如下：


    {
        "Id": 7,    //数据库/服务端自动生成的id
        "Name": "Cuican Shi",
        "IsSubscribedToNewsLetter": false,
        "MemberShipType": null,
        "MemberShipTypeId": 1,
        "Birthday": "2018-03-19T00:00:00"
    }
      
  //通过Postman我们可以测试所有的API。

//暂时想到这么多，最后更新2018/03/20
