//Q: 如何将返回的验证错误信息更改为自定义的表述？
//A: 通过更改对应Model中的对应字段的发生错误的DataAnnotation实现。
  //如，将Name表单的验证错误信息由"Name field is required."改为"Please enter customer name."只用更改修饰Customer类中的Name字段的[Required]
  //DataAnnotation即可，不需要做其他的改动。代码如下：

    public class Customer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter customer name.")]    //在此处覆写错误信息
        [StringLength(255)]
        public string Name { get; set; }

        public bool IsSubscribedToNewsLetter { get; set; }

        public MembershipType MemberShipType { get; set; }

        [Display(Name = "MemberShip Type")]
        public byte MemberShipTypeId { get; set; }

        [Display(Name = "Date of Birth")]
        public DateTime? Birthday { get; set; }
    }

  //但是要注意，ASP.NET MVC5在设计的时候并没有考虑与FluentAPI的兼容配合。通常情况下来FluentAPI也能起作用，但并没有设计与MVC中组件的通信，即如果使用
    //FluentAPI是没有办法覆写ErrorMessage的。这种情况下只能使用DataAnnotation。

  //所以之前的MemberShipType类就要改写。首先删除掉EntityTypeConfiguration文件夹及其中的文件，删除ApplicationDbContext类中的OnModelCreating()方法
    //中涉及到FluentAPI的语句，然后建立迁移文件同步至数据库。

  //然后重新使用DataAnnotation为Customer类中的MemberShipTypeId设立逻辑规则，即[Required]，然后再建立迁移文件并同步至数据库。Customer类的代码如下：

    public class Customer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter customer name.")]
        [StringLength(255)]
        public string Name { get; set; }

        public bool IsSubscribedToNewsLetter { get; set; }

        public MembershipType MemberShipType { get; set; }

        [Display(Name = "MemberShip Type")]
        [Required(ErrorMessage = "Please select a membership type.")]   //在此覆写错误信息
        public byte MemberShipTypeId { get; set; }

        [Display(Name = "Date of Birth")]
        public DateTime? Birthday { get; set; }
    }

//Q: ASP.NET只支持DataAnnotation么？
//A: 没有错。请不要在ASP.NET MVC5项目中使用FluentAPI。另外ASP.NET MVC5还支持以下的DataAnnotation特性标签：

[Required]  //即覆写该字段不能为空
[StringLength(255)]   //即覆写该字段的最大长度
[Range(1, 10)]    //即覆写该字段必须在范围内
[Compare("OtherProperty")]    //即与其他字段相比较，必须与被比较的字段相同，如与"Password"字段比较
[Phone]   //即覆写该字段必须为电话
[EmailAddress]    //即覆写该字段必须为电邮地址
[Url]   //即覆写该字段必须为网址
[RegularExpression("...")]    //即覆写该字段必须满足某种正则表达式，其实上面几个特性标签都是由正则表达式写成的

//暂时想到这么多，最后更新2018/03/18
