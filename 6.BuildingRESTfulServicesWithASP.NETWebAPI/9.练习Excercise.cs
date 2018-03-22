//项目需求为：在Vidly项目中，为Movies资源搭建API，使客户端能够：
  //1.发送HttpGet请求，通过/api/movies/从数据中返回Movies资源列表；
  //2.发送HttpGet请求，通过/api/movies/{id}从数据中返回某一具体id的movie对象；
  //3.发送HttpPost请求，通过/api/movies/建立一列表的新movie对象到数据库中；
  //4.发送HttpPut请求，通过/api/movies/{id}更新某一具体id的movie对象道数据库中；
  //5.发送httpDelete请求，通过/api/movies/{id}从数据中删除某一个具体id的movie对象。
 
//首先，建立MovieDto类，在api中替代领域模型，其代码如下：

using System;
using System.ComponentModel.DataAnnotations;

namespace Vidly.Dtos
{
    public class MovieDto   //完全拷贝自领域模型Movie，删除其中的非普通数据部分
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public byte GenreId { get; set; }

        [Required]
        public DateTime? ReleasedDate { get; set; }

        [Range(1, 20)]
        [Required]
        public int InStock { get; set; }
    }
}

//然后，在Start_App文件夹中的MappingProfile问中添加如下代码：

using AutoMapper;
using Vidly.Dtos;
using Vidly.Models;

namespace Vidly.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<Customer, CustomerDto>();
            Mapper.CreateMap<CustomerDto, Customer>();
            Mapper.CreateMap<Movie, MovieDto>();    //建立从Movie到MovieDto的映射关系，即从服务端到用户端方向
            Mapper.CreateMap<MovieDto, Movie>();    //建立从MovieDto到Movie的映射关系，即从用户端到服务端方向
        }
    }
}

//最后搭建API，其代码如下：

using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Vidly.Dtos;
using Vidly.Models;

namespace Vidly.Controllers.Api
{
    public class MoviesController : ApiController
    {
        private ApplicationDbContext _context;

        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }

        // 需求1：GET /api/movies
        public IHttpActionResult GetMovies()
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var movies = _context.Movies.ToList().Select(Mapper.Map<Movie, MovieDto>);

            return Ok(movies);
        }

        // 需求2：GET /api/movies/{id}
        public IHttpActionResult GetMovie(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var movie = _context.Movies.SingleOrDefault(m => m.Id == id);

            if (movie == null)
                return NotFound();

            var movieDto = Mapper.Map<Movie, MovieDto>(movie);

            return Ok(movieDto);
        }
        
        // 需求3：POST /api/movies
        [HttpPost]
        //为满足需求3，用户端发送的请求包中的数据为一列表，这个Action需要将整个列表一次性都添加进数据库中
        public  IHttpActionResult CreatMovies(IEnumerable<MovieDto> moviesDto)    //用户端传入IEnumerable<T>类型的对象
        {
            var movies = new List<Movie>();
            int i = 0;

            if (!ModelState.IsValid)
                return BadRequest();

            foreach(var movieDto in moviesDto)    //迭代用户传入的IEnumerable<T>中的所有对象
            {
                var movie = Mapper.Map<MovieDto, Movie>(movieDto);    //将每个movieDto对象连接转换成movie
                movies.Add(movie);    //将movie对象添加到movies列表中
            }

            _context.Movies.AddRange(movies);   //将movies列表添加进数据库中
            _context.SaveChanges();   //保存

            //按照默认约定，需要向用户端返回新建的对象，包括数据库为其每个对象分配的id
            foreach(var movieDto in moviesDto)
            {
                movieDto.Id = movies[i].Id;   //为每一个movieDto对象更新从数据看返回的id信息
                i = i + 1;
            }

            //以列表的形式返回所建立对象，其Uri为/api/movies/{起始新对象id}:{末尾新对象id}
            return Created(new Uri(Request.RequestUri + "/" + movies[0].Id + ":" + movies[movies.Count - 1].Id), moviesDto);
        }

        // 需求4：PUT /api/Movies/{id}
        [HttpPut]
        public void UpdateMovie (int id, MovieDto movieDto)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var movieInDb = _context.Movies.SingleOrDefault(m => m.Id == id);

            if (movieInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            Mapper.Map(movieDto, movieInDb);    //可省略Mapper.Map<MovieDto, Movie>(movieDto, movieInDb)中的<MovieDto, Movie>

            _context.SaveChanges();
        }

        // 需求5：DELETE /api/Movies/{id}
        [HttpDelete]
        public void DeleteMovie(int id)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var movieInDb = _context.Movies.SingleOrDefault(m => m.Id == id);

            if (movieInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            _context.Movies.Remove(movieInDb);
            _context.SaveChanges();
        }
    }
}

  //据说，在处理PUT请求的时候会报错，发送错误的地方在：
  
            Mapper.Map(movieDto, movieInDb);

  //据分析，报错原因为这个方法企图将movieDto.Id赋予到moviesInDb.Id中，由于moviesInDb.Id已经存在于数据库了，作为自动生成的主键，Id属性不允许更改。
    //之前Created请求能为Id赋值是因为这是新建过程，数据库将无视带值的Id，而会重新为其分配一个Id。

  //解决更新数据库报错的方案为，在MappingProfile中设置忽略Id的更新，其代码如下：

using AutoMapper;
using Vidly.Dtos;
using Vidly.Models;

namespace Vidly.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<Customer, CustomerDto>().ForMember(m => m.Id, opt => opt.Ignore());    //忽略Id的更新
            Mapper.CreateMap<CustomerDto, Customer>();
            Mapper.CreateMap<Movie, MovieDto>().ForMember(m => m.Id, opt => opt.Ignore());    //忽略Id的更新
            Mapper.CreateMap<MovieDto, Movie>();
        }
    }
}

  //但是目前即使不说明忽略更新，AutoMapper也会自动在PUT请求中忽略Id的更新，也就是说，目前版本似乎不针对这个问题做处理也不会得到报错。

//暂时想到这么多，最后更新2018/03/22
