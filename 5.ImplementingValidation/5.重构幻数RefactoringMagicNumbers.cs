//Q: 什么是幻数？
//A: 幻数又称魔数，英文Magic Number，是欧美程序员对表述不清的数字的戏称。这些数字没有引用，是单纯的的数字。由于没有引用，接手维护的程序员通常不知道这
  //些数字指代什么意思，又不敢乱改，干脆称其Magic Number，意为“或许很厉害但不知道干什么用的”的意思。同理，还有Magic String的说法。在一般的工作情况
  //中，应该尽量避免这种情况，以降低程序的维护难度。

//Q: 如何重构幻数？
//A: 通常情况下为其命名为静态只读变量即可。如在之前的自定义验证的例子中，有逻辑就包含了幻数，代码如下：

    public class Min18YearsIfAMember : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var customer = (Customer)validationContext.ObjectInstance;

            //这里的数字0和1就是幻数。虽然知道这代表着会员种类，但并不知道具体是什么种类。这样就增大了维护难度。
            if (customer.MemberShipTypeId == 0 || customer.MemberShipTypeId == 1)
                return ValidationResult.Success;

            if (customer.Birthday == null)
                return new ValidationResult("Please enter your birthday.");

            var age = DateTime.Today.Year - customer.Birthday.Value.Year;

            return (age >= 18) 
                ? ValidationResult.Success 
                : new ValidationResult("Customer Must be greater than 18 years old.");
        }
    }
    
  //我们可以为所有的会员种类ID都建立一个静态只读变量。如以下代码：

namespace Vidly.Models
{
    public class MembershipType
    {
        public byte Id { get; set; }
        public short SignUpFee { get; set; }
        public byte DurationInMonths { get; set; }
        public byte DiscountRate { get; set; }
        public string MemberShipTypeName { get; set; }

        public static readonly byte Unknow = 0;     //默认不选择下拉菜单的Id为0
        public static readonly byte PayAsGo = 1;    //Id为1即Pay As Go
        public static readonly byte Monthly = 2;    //Id为2即月度会员
        public static readonly byte Quaterly = 3;   //Id为3即季度会员
        public static readonly byte Annual = 4;   //Id为4即年度会员
    }
}

  //如此，其验证逻辑可改为：

namespace Vidly.Models
{
    public class Min18YearsIfAMember : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var customer = (Customer)validationContext.ObjectInstance;

            if (customer.MemberShipTypeId == MembershipType.Unknow ||   //以MembershipType.Unknow替换0，一目了然
                customer.MemberShipTypeId == MembershipType.PayAsGo)   //同理，以MembershipType.PayAsGo替换1
                return ValidationResult.Success;

            if (customer.Birthday == null)
                return new ValidationResult("Please enter your birthday.");

            var age = DateTime.Today.Year - customer.Birthday.Value.Year;

            return (age >= 18) 
                ? ValidationResult.Success 
                : new ValidationResult("Customer Must be greater than 18 years old.");
        }
    }
}

//Q: 为什不直接用枚举来表示？
//A: 一般情况下枚举Enum类型也可以实现相同的作用。但是有些时候需要将其转型，如这里Id为Byte类型不为Int类型，则需要转型，如：

                customer.MemberShipTypeId == (byte) MembershipType.Unknow   //强制转型

//这显然会降低程序运行效率。

//暂时想到这么多，最后更新2018/03/18
