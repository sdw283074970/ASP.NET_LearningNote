//Q: 什么是定制验证？
//A: 定制验证即定制我们自己的业务逻辑。ASP.NET MVC5自带的那些验证逻辑根本不够用，也无法满足商业需求。如我们要添加一条“未满十八岁不能喝酒”的逻辑，仅靠
  //自带的DataAnnotation验证是不能实现的。

//Q: 如何添加和使用自定义逻辑验证？
//A: 自定义的逻辑验证是一个类，这个类必须是ValidationAttribute类的子类/衍生类。当建立好这个类并填充其逻辑后，就能将这个类当作其他DataAnnotation一样
  //在特性标签中使用了。如我们新建并了一个继承自ValidationAttribute类的Min18YearsIfAMember类，只用按如下代码即可使用：

        [Min18YearsIfAMember]   //使用自定义的验证
        public DateTime? Birthday { get; set; }

//Q: 如何填充自定义验证类的逻辑？
//A: 自定义填充来继承自ValidationAttribute类，通过覆写其中的IsValid()方法即可完成验证。IsValid方法有两个重载，一个为IsValid(object obj)，其返回
  //类型为bool，是最直白的验证判断类型。另一种为IsValid(object obj, ValidationContext validationContext)，返回类型为ValidationResult。

  //推荐使用第二种重载，因为第二种重载可以通过ValidationContext访问到以该验证修饰的字段容器中的其他字段，如Birthday的容器为Customer类，使用第二种
  //就可以通过ValidationContext访问到Customer的其他字段。同时，第二种重载可以完成第一种重载的所有逻辑。也能完成第一种重载不能完成的逻辑。

  //如项目需求为只有PayAsGo的会员种类不需要年龄验证，其他所有的会员种类要求顾客必须18岁以上。Min18YearsIfAMember类代码如下：

namespace Vidly.Models
{
    public class Min18YearsIfAMember : ValidationAttribute
    {
        //使用第二种重载
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //通过静态字段validationContext.ObjectInstance可访问目容器，注意需要转换类型
            var customer = (Customer)validationContext.ObjectInstance;

            //逻辑1：如果会员种类为PayAsGo则不需要验证，即验证成功；为了不污染视野，如果没有选择会员种类也算作验证成功，暂不做提示
            if (customer.MemberShipTypeId == 0 || customer.MemberShipTypeId == 1)
                return ValidationResult.Success;    //ValidationResult.Success为静态类，返回一个验证成功的ValidationResult

            //如果生日为空，则提示输入生日
            if (customer.Birthday == null)
                //如果要返回带错误信息的验证失败的结果，则声明一个ValidationResult的实例，并在构造器中附上想传达的错误信息
                return new ValidationResult("Please enter your birthday.");

            var age = DateTime.Today.Year - customer.Birthday.Value.Year;   //计算年龄

            //逻辑2：如果年纪大于18岁，则验证成功；否则提示必须满18岁
            return (age >= 18) 
                ? ValidationResult.Success 
                : new ValidationResult("Customer Must be greater than 18 years old.");
        }
    }
}

  //总之，使用第二种重载时，如果通过逻辑验证，则返回ValidationResult.Success，否则，返回一个ValidationResult新实例，并附上错误信息。
  //最后在CustomerForm视图中添加html辅助函数声明对生日的验证即可

    <div class="form-group">
        @Html.LabelFor(m => m.Customer.Birthday)
        @Html.TextBoxFor(m => m.Customer.Birthday, "{0:d MMM yyyy}",new { @class = "form-control" })
        @Html.ValidationMessageFor(m => m.Customer.Birthday)
    </div>  
  
  //Html.ValidationMessageFor(m => m.Customer.Birthday)会自己找上Customer.Birthday的特性注解，并执行其中的验证逻辑。

//暂时想到这么多，最后更新2018/03/18
