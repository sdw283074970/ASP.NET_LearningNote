//Q: 为什么要更改标签名？
//A: 有时候我们想显示的是更加友好的标签名而不是与模型中字段相同的具有浓郁编程风格的名字，如一个字段为Date_Of_Birth_20180231，我们仅仅想要
  //Date Of Birth即可。

//Q: 如何更改标签名？
//A: 两种方法，一种为使用DataAnnotaion特性注解更改标签名，另一种为直接使用HTML标签更改标签名。
  //第一种方法使用DataAnnotation特性注解。如果要将Customer中的Birthday字段改为Date Of Birth，只用在Customer类中的对应字段前加特性注解即可，如：

namespace Vidly.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSubscribedToNewsLetter { get; set; }
        public MembershipType MemberShipType { get; set; }
        public byte MemberShipTypeId { get; set; }
      
        [Display(Name = "Date of Birth")]
        public DateTime? Birthday { get; set; }
    }
}

  //第二种方法为在视图文件中直接使用HTML标签。如：

@model Vidly.Models.Customer
@{
    ViewBag.Title = "New";
    Layout = "~/Views/Shared/_Layout.cshtml";
}

<h2>New Customer</h2>

@using (Html.BeginForm("Creat", "Customers"))
{
    <div class="form-group">
        @Html.LabelFor(m => m.Name)
        @Html.TextBoxFor(m => m.Name, new { @class = "form-control"})
    </div>
    <div class="form-group">
        //@Html.LabelFor(m => m.Birthday) 不用HTML辅助函数
        <Lable for="Birthday">Date of Birth</Lable>   //使用HTML标签
        @Html.TextBoxFor(m => m.Birthday, new { @class = "form-control" })
    </div>
    <div class="checkbox">
        <label>
            @Html.CheckBoxFor(m => m.IsSubscribedToNewsLetter) Subscribed?
        </label>
    </div>
}

//Q: 哪一个方法比较好？
//A: 这两个方法都已缺陷。第一种方法的缺陷是，每改一次Birthday的代码，就需要重现编译一次程序。第二种方法的缺陷是，当Birthday变量更改后标签中的
  //for属性不会自动更改，需要手动更改。这两种方法都可以用，看个人喜好。个人推荐第一种方法。

//暂时想到这么多，最后更新2018/03/12
