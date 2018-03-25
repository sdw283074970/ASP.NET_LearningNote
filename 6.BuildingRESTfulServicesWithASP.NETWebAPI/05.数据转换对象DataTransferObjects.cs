//Q: 什么是数据转换对象？
//A: 数据转换对象Data Transfer Object(DTO)是一种模型，被用作为位于用户端和服务器之间的中转媒介。DTO中只包普通类型的数据，如string、int、byte等，
  //其负责将用户端的数据转换到服务端中或反过来。

//Q: 什么情况下会用到DTO？
//A: 在搭建API过程中，API控制器中的任何动作(Action)不应该也不能返回领域模型(Domain Model)实例对象，如不应该返回Customer。这样是因为直接访问领域模型
  //会触碰到很多执行细节，如直接访问模型内的字段，而这些字段在实际环境中是会经常按需求变动的。任何细节字段的变动都有可能导致依赖这些领域模型的用户端发生
  //引用断裂(Breaking)。如对Customer模型中的某一字段重命名就会发生这种断裂。因此，客户端与服务端之间的联系应该是稳定的，即使需要改变，也应该是很长一段
  //时间才改变一点。

  //作为解决方案，我们将API中所有返回领域模型实例的动作(Action)返回类型都改为这个领域模型的数据转换对象，如将Customer换成CustomerDto，CustomerDto
    //只包含与Customer对应的普通数据，如CustomerDto代码如下：

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vidly.Models;

namespace Vidly.Dtos
{
    public class CustomerDto
    {
        //移除了非普通数据，如MemberShipType(类型为MemberShipType)，只留下int、string、byte、bool、DateTime等普通数据
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter customer name.")]
        [StringLength(255)]
        public string Name { get; set; }

        public bool IsSubscribedToNewsLetter { get; set; }
        
        [Required(ErrorMessage = "Please select a membership type.")]
        public byte MemberShipTypeId { get; set; }
        
        [Min18YearsIfAMember]
        public DateTime? Birthday { get; set; }
    }
}

//Q: 使用DTO有什么好处？
//A: 正如之前所述，使用DTO可以防止依赖断裂，API控制器中的所有Action都不应该返回领域模型。另外一方面，使用DTO还能填补安全漏洞。如，直接使用领域
  //模型会让黑客有可乘之机，修改领域模型中的所有字段。而使用DTO则可以指定哪些字段能跟客户端通信，不想暴露的字段直接不写进DTO，这大大提高了安全系数。

//Q: 目前为止DTO只是改了个名字而已。如何才能确认DTO中的字段与原领域模型的字段挂钩？
//A: 使用AutoMapper即可。在下节中详细讲解。

///暂时想到这么多，最后更新2018/03/20
