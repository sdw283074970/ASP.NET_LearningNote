//本节内容与MicrosoftSQLServer_EntityFramework/3.CodeFirstWorkFlow/3.迁移:启用迁移EnablingMigrations.cs完全一样

//Q: 到底什么是迁移？
//A: 在我们使用CFW时，我们的Model不可能一直不变。当我们的Model改变时，需要通知数据库做相应的改变，即同步。其原理为EF能感知Model的变化(主要是与之前所
  //有的迁移文件经行对比，如果之前没有文件记录，则认为这是新数据库，会将整个Model视为改变)，会先自动生成一个C#语言的变更操作记录，EF再将这一系列操作记录
  //转换为Sql语言通知数据库执行，然后数据库就与我们的Model同步了。这个过程就叫做迁移(Migration)。

//Q: 如何启用迁移？
//A: 在PM中输入enable-migrations即可启用迁移。启用后，解决方案中会生成一个Migrations的文件夹，里面将储存每一次EF生成的C#语言Model改变记录。

//Q: 如何初始化迁移？
//A: 在启用迁移后，我们就要做第一次迁移，即初始化。在已存在的数据库项目基础上的迁移由于没有先前迁移文件记录，EF会将整个Model视为改变，这将产生一个重复
  //创建数据的问题。为了解决这个问题，只用在第一次建立迁移文件时在PM中输入-IgnoreChanges，即在PM输入add-migration InitialModel -IgnoreCHanges，
  //则会忽略所有改变，生成一个空的迁移文件。作为结论，在所有基于已存在数据库项目上建立的CodeFirstModel在初始化迁移时必须加上-IgnoreChanges参数。
  //空的迁移文件代码即迁移类方法含义如下：

    public partial class InitialModel : DbMigration
    {
        public override void Up()
        {
            //Up方法，即更新方法。EF会比较现有Model与之前的迁移文件，然后基于它们的差异生成一个Up方法，执行后EF会将Up方法翻译成Sql文件并让数据库执行。
            //我们也可以在这里植入一些Sql查询语句来对数据库进行操作。
        }

        public override void Down()
        {
            //Down方法，即还原/降级方法。此方法与Up方法无脑相反，即通过调用这个方法来将数据库还原到调用Up方法之前的状态，EF也会自动生成此方法代码。
            //但是请切记，任何在Up方法做过变动后都要检查Down方法，千万要保持Down方法与所有Up方法无脑相反，否则数据库GG。
        }
    }
  
  //此外附上做迁移的黄金准则：每当做一次做小规模改变后就要做一次小规模迁移，最好一个改变一次迁移。

//最后更新2018/03/01
