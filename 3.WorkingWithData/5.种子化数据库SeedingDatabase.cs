//本节内容基于MicrosoftSQLServer_EntityFramework/3.CodeFirstWorkFlow/9.种子化数据库SeedingDatabase.cs

//Q: 如何种子化数据库？
//A: 两种方法，一种是在迁移文件中使用SQL查询语句插入条目，另一种是在迁移文件夹中的Configuration.cs中的Seed方法中添加条目。

//Q: 两种方法有什么区别？
//A: 第一种在迁移文件中是用SQL查询语句中添加，仅在同步了这个迁移文件后生效。第二种在Seed方法中添加条目使用的是C#语言，每当使用Update-Database命令
  //后都会执行一次，看情况需要而定。在Vidly项目中，会员类型是一成不变的，所以用第二种方法。即使有改变，也只用回Seed方法中修改后同步即可。

  //假设有四种会员类型，将这四种类型会员添加进种子数据库代码如下：

namespace Vidly.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Vidly.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        //在每一次更新到最新迁移文件版本时就会调用这个方法
        protected override void Seed(Vidly.Models.ApplicationDbContext context)
        {
            context.MemberShipTypes.AddOrUpdate(m => m.MemberShipTypeName,
                new MembershipType    //会员种类1
                {
                    Id = 1,
                    MemberShipTypeName = "Pay As Go",
                    SignUpFee = 0,
                    DiscountRate = 0,
                    DurationInMonths = 0
                },
                new MembershipType    //会员种类2
                {
                    Id = 2,
                    MemberShipTypeName = "Monthly",
                    SignUpFee = 30,
                    DiscountRate = 10,
                    DurationInMonths = 1
                },
                new MembershipType    //会员种类3
                {
                    Id = 3,
                    MemberShipTypeName = "Quaterly",
                    SignUpFee = 90,
                    DiscountRate = 15,
                    DurationInMonths = 3
                },
                new MembershipType    //会员种类4
                {
                    Id = 4,
                    MemberShipTypeName = "Annually",
                    SignUpFee = 300,
                    DiscountRate = 20,
                    DurationInMonths = 12
                });
        }
    }
}

  //最后使用一次update-database命令后即可种子化数据库。打开数据库的MemberShipType表即可看到以上数据。
  
//暂时想到这么多，最后更新2018/03/02
