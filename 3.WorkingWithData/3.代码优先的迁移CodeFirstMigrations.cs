//本节内容为基于MicrosoftSQLServer_EntityFramework/3.CodeFirstWorkFlow/3.迁移:启用迁移EnablingMigrations.cs的衍生

//Q: 如何在ASP.NET MVC5项目中启用迁移？
//A: 在PM中输入enable-migrations即可，首次启用后便可不用再启用。启用成功后继续在PM中输入add-migration InitialModel建立初始迁移文件。

//Q: 初始迁移文件中建立的"dbo.AspNetRoles""dbo.AspNetUserRoles""dbo.AspNetUsers"这些是哪来的?
//A: 这些是ASP.NET自动创建的验证表格，用于验证用户的控制权限。我们可以在Models文件夹中找到IdentityModels.cs文件，此文件中有两个类，代码如下：

using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Vidly.Models
{
    public class ApplicationUser : IdentityUser   //ApplicationUser类，即用户类
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>    //DbContext域类，即通往数据库的类，属于ASP.NET EntityFramework
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}

//Q: 为什么没有Movie和Customer列表？
//A: 首先这两个列表还没有被建立，即使存在，DbContext类中没有引用这两个表，所以EntityFramework不知道他们的存在。

//Q: 如何建立表？
//A: 在DbContext类，在这里即ApplicationDbContext建立表即可，可以将ApplicationDbContext视为PlutoDbContext(在另一个Repository中的例子)。
  //建立Movies和Customers表代码如下：
  
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Movie> Movies { get; set; }    //建立电影列表
        public DbSet<Customer> Customers { get; set; }    //建立顾客列表
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
    
  //最后可以使用add-migration InitialModel -force强制重新建立初始化迁移。我们可以看到Up()方法中多了新建两个表的方法：

            CreateTable(
                "dbo.Customers",    //建立Customers表
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Movies",   //建立Movies表
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);

//Q: 现在并没有设置数据库connectionString，在哪里生成数据库文件？迁移同步会通向哪里？
//A: 可以直接通过update-database生成本地数据库文件而不用建立Microsoft Sever。生成成功后，可在App_Data文件夹中找到扩展名为.mdf的数据库文件，
  //前提条件是要显示所有文件，该数据库文件默认为隐藏。双击就可以打开数据库浏览器。

//最后更新2018/03/01
