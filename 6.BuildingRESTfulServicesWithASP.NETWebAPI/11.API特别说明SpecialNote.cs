//Q: 如何向关系型表中添加对象？
//A: 很多情况下需要API能够处理带层次结构的Json数据，层次结构在这里指列表中的列表或数组中的数组，在一对多和多对多关系中非常常见。

  //一个典型的带层次结构的Json数据如下：

  [
      {
          "name": "大怪兽",
          "isSubscribedToNewsLetter": false,
          "memberShipTypeId": 2,
          "memberShipType": {
              "id": 2,
              "name": "Monthly"
          },
          "birthday": "1992-11-17T00:00:00"
      }
  ]

  //这个Json数组中只有一个元素，但是这个元素中还有另一个元素，这意味着一个对象中可以包含另一个列表对象，即表中表。这种情况通常发生在关系型数据中的
    //一对多关系(一个对象中可以包含一个其他类型对象的集合)和多对多关系(一个对象本身可以包含一个其他类型对象的集合，而一个其他对象也可以包含这个类型
    //对象的集合)。如：

    //一对多对象：一个作者可以写很多本书，而他写的每一本书的作者就是其唯一的本人，即一个作者对多本书；
    //多对多对象：一个课程可以有很多标签，一个标签可以贴很多课程，即多个课程对多个标签。

  //在.NET Framework CodeFirst中，并不支持直接在对多对多关系列表中添加新对象，CodeFirst生成的中间表无法获得引用因此不能通过对中间表操作来增删查改。
    //个人推测原因是CodeFirst操作多对多对象会造成死循环(课程表中有标签表，标签表中有课程表...)。

    //其解决方法为在两个对多对关系表之间，手动建立一个中间表，并搭建左表与中间表的多对一关系和右表与中间表的多对一关系。
    
  //如，在HeroPickHelper项目中，英雄列表Heroes与职责列表Duties为多对多关系，即一个英雄有多个职责，一个职责可以赋予给多个英雄。领域模型分别为Hero
    //和Duty，现手动建立一个中间领域模型HeroDuties，并将Hero和Duty与此领域模型相连，三个领域模型代码如下：

//领域模型Hero
namespace HeroPickHelper.Models
{
    public class Hero
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Nick_name { get; set; }

        public string Localized_name { get; set; }

        public string Url_full_portrait { get; set; }

        public string Url_small_portrait { get; set; }

        public string Url_large_portrait { get; set; }

        public string Url_vertical_portrait { get; set; }

        public string Position { get; set; }
      
        //以一对多关系连接HeroDuty模型，即一个Hero可以有多个HeroDuties
        public ICollection<DutyHero> HeroDuties { get; set; }
    }
}

//领域模型Duty
namespace HeroPickHelper.Models
{
    public class Duty
    {
        public int Id { get; set; }

        public string Name { get; set; }
      
        //以一对多关系连接HeroDuty模型，即一个Duty可以有多个HeroDuties
        public ICollection<DutyHero> DutyHeroes { get; set; }
    }
}

//领域模型HeroDuty
namespace HeroPickHelper.Models
{
    public class DutyHero
    {
        public int Id { get; set; }

        [ForeignKey("HeroId")]
        public Hero Hero { get; set; }    //以多对以关系连接Hero模型，即一个HeroDuty可以对应多个Heroes

        public int HeroId { get; set; }

        [ForeignKey("DutyId")]
        public Duty Duty { get; set; }    ////以多对以关系连接Duty模型，即一个HeroDuty可以对应多个Duties

        public int DutyId { get; set; }
    }
}

  //这样一来实际上建立的就是Hero和Duty的多对多模型。可以从中间表出发，进行类似于“返回Id为1的英雄的所有职责”的查询。

//暂时想到这么多，最后更新2018/03/27
