//本节内容基于MicrosoftSQLServer_EntityFramework/4.OverridingCodeFirstConventions/4.FluentAPI基础.cs~8.FluentAPI组织化配置.cs

//Q: 这个小项目用哪一种覆写？
//A:DataAnnotation就可以满足。但为了达到练习目的，还是用FluentAPI来覆写约定。假设项目需求为：
  //1.将MemberShipTypeName设为非可空；
  //2.将MemberShipTypeName长度设为255；
  
  //如要在ASP.NET项目中使用FluentAPI实现以上需求，需要首先找到DbContext类，即ApplicationDbContext，位于Models文件夹的Identity.cs文件中。
    //默认这个DbContext类没有OnCreatingModel类，需要手动添加。代码如下：

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<MembershipType> MemberShipTypes { get; set; }
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //手动添加OnModelCreating类
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MembershipType>()
                .Property(m => m.MemberShipTypeName)
                .IsRequired();    //将MemberShipTypeName覆写为不可控
            modelBuilder.Entity<MembershipType>()
                .Property(m => m.MemberShipTypeName)
                .HasMaxLength(255);   //将MemberShipTypeName的最大长度覆写为255
            base.OnModelCreating(modelBuilder);
        }
    }

  //此例需求很少所以直接在OnModelCreating方法中添加覆写没有问题。但是如果覆写一多就很难管理，需要将这些覆写组织化配置。此例中都是针对MemberShipType
    //进行覆写，可以为这个类专门建立覆写文件统一管理，在Models文件夹下建立EntityTypeConfiguration文件夹，建立MemberShipTypeConfigurationl类文件，
    //该类继承自EntityTypeConfiguration<MemberShipType>代码如下：
    
namespace Vidly.Models.EntityTypeConfiguration
{
    public class MemberShipTypeConfigurationl : EntityTypeConfiguration<MembershipType>
    {
        public MemberShipTypeConfigurationl()
        {
            Property(m => m.MemberShipTypeName).IsRequired();   //将MemberShipTypeName覆写为不可控
            Property(m => m.MemberShipTypeName).HasMaxLength(255);    //将MemberShipTypeName的最大长度覆写为255
        }
    }
}

  //最后，只用在OnModelCreating()方法中使用modelBuilder.Configurations.Add()调用这个类的实例就行了，代码如下：
  
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new MemberShipTypeConfigurationl());    //调用MemberShipType类的覆写配置
        }

//暂时想到这么多，最后更新2018/03/02
