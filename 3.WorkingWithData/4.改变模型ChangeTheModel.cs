//本节内容为基于MicrosoftSQLServer_EntityFramework/3.CodeFirstWorkFlow/4.迁移:添加类AddingANewClass.cs~6.迁移:删除类DeletingAnExistingClass.cs
  //的延伸，即针对完成Vidly项目需求的过程而非阐述改变模型知识点本身，如有需要请查阅以上内容

//Q: 什么是改变模型？
//A: 模型，即是独立于视图的部分。对模型做修改后需要同步至数据库，如项目需求为Customer类增加四个新属性，即是否为新订阅者、会员类型、会员类型名称、会员
  //类型Id。修改方法很直接，即打开Customer类添加以上属性即可(需首先建立会员类型MembershipType类)。Customer类新代码如下：

namespace Vidly.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSubscribedToNewsLetter { get; set; }    //添加完这个属性后建立迁移文件并同步至数据库
        public MembershipType MemberShipType { get; set; }    //导航属性，添加完这个属性即后两个属性后建立迁移文件并同步至数据库
        public byte MemberShipTypeId { get; set; }    //约定共识以导航属性类名+Id即为该导航属性Id外键
        public string MemberShipName { get; set; }    //会员类型名称属性
    }
}

  //会员类型类代码如下：

namespace Vidly.Models
{
    public class MembershipType
    {
        public short SignUpFee { get; set; }    //入会费
        public byte DurationInMonths { get; set; }    //会员持续时间
        public byte DiscountRate { get; set; }    //折扣
        public byte Id { get; set; }    //Id主键
        public string MemberShipTypeName { get; set; }    //会员类型名称
    }
}

//暂时想到这么多，最后更新2018/03/02
