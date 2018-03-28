//客户端除了能向服务端API发送HttpGet请求获得粗数据包(如Json格式)，还能向服务端API发送HttpPost请求在服务端数据库中建立数据。

//PostMan可以测试API的所有方法和功能，包括POST请求方法，即可以通过API直接将Json格式的方法写入服务端数据库中。

//之前介绍了将单个普通(无关系模型)的Json格式的数据写入数据库，现在介绍如何将一组无层次关系的Json数据写入数据库中。

//在项目HeroPickHelper中，我们有从Steam公布的API获取的所有Json格式的英雄数据，现我们将其稍作修改导入到自己的数据库中。其中一部分修改后的数据如下：

[
    {
        "nick_name": "AM",
        "localized_name": "Anti-Mage",
        "url_full_portrait": "http://cdn.dota2.com/apps/dota2/images/heroes/antimage_full.png",
        "name": "npc_dota_hero_antimage",
        "url_small_portrait": "http://cdn.dota2.com/apps/dota2/images/heroes/antimage_sb.png",
        "url_large_portrait": "http://cdn.dota2.com/apps/dota2/images/heroes/antimage_lg.png",
        "url_vertical_portrait": "http://cdn.dota2.com/apps/dota2/images/heroes/antimage_vert.jpg",
        "position": "12",
        "id": 1
    },
    {
        "nick_name": "Axe",
        "localized_name": "Axe",
        "url_full_portrait": "http://cdn.dota2.com/apps/dota2/images/heroes/axe_full.png",
        "name": "npc_dota_hero_axe",
        "url_small_portrait": "http://cdn.dota2.com/apps/dota2/images/heroes/axe_sb.png",
        "url_large_portrait": "http://cdn.dota2.com/apps/dota2/images/heroes/axe_lg.png",
        "url_vertical_portrait": "http://cdn.dota2.com/apps/dota2/images/heroes/axe_vert.jpg",
        "position": "34",
        "id": 2
    }
]

//据分析，这套数据为一个Json数组，数组中的每一个元素即为一个对象，元素内容即为字段。

//以下为新建API的步骤：
  //1.建立领域模型及其DTO；
  //2.在ApplicationDbContext类中声明领域模型的表；
  //3.启用迁移，同步数据库；
  //4.安装AutoMapper包，并建立映射关系；
  //5.新建API控制器；
  //6.按需声明GET/POST/PUT/DELETE等方法。

//本例中直接进入最后一步，即在API控制器中建立一个POST方法，代码如下：

        // POST /api/heroes
        [HttpPost]
        //因为输入为Json数组，所以传入参数类型为IEnumerable<T>，T为根据Json元素建立的领域模型，在这里即Hero，传入HeroDto作为参数
        public IHttpActionResult CreatedHeroes(IEnumerable<HeroDto> heroesDto)
        {
            var heroes = new List<Hero>();    //建立heroes字段，用于保存DTO转换后的数据，一次性写入数据库
            int i = 0;    //计数器

            //验证模型状态
            if (!ModelState.IsValid)
                return BadRequest();

            //迭代传入数据heroesDto中的每一元素，将这些元素由DTO转换为领域模型，并添加进heroes列表等待写入数据库
            foreach (var heroDto in heroesDto)
            {
                var hero = Mapper.Map<HeroDto, Hero>(heroDto);
                heroes.Add(hero);
            }

            _context.Heroes.AddRange(heroes);   //将heroes列表写入数据库(内存)
            _context.SaveChanges();   //保存至数据库(固化数据)

            //获得由数据库分配的Id
            foreach (var heroDto in heroesDto)
            {
                heroDto.Id = heroes[i].Id;
                i = i + 1;
            }
            
            //将写入结果返回给用户端(约定)
            return Created(new Uri(Request.RequestUri + "/" + heroes[0].Id + ":" + heroes[heroes.Count - 1].Id), heroesDto);
        }

//以上，即为非关系型数据(无层次结构)的Json格式文件通过API写入数据库的方法。PUT方法大同小异。

//暂时想到这么多，最后更新2018/03/27
