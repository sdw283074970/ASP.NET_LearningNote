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

        // GET /api/movies
        public IHttpActionResult GetMovies()
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var movies = _context.Movies.ToList().Select(Mapper.Map<Movie, MovieDto>);

            return Ok(movies);
        }

        // GET /api/movies/{id}
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
        
        // POST /api/movies
        [HttpPost]
        public  IHttpActionResult CreatMovies(IEnumerable<MovieDto> moviesDto)
        {
            var movies = new List<Movie>();
            int i = 0;

            if (!ModelState.IsValid)
                return BadRequest();

            foreach(var movieDto in moviesDto)
            {
                var movie = Mapper.Map<MovieDto, Movie>(movieDto);
                movies.Add(movie);
            }

            _context.Movies.AddRange(movies);
            _context.SaveChanges();

            foreach(var movieDto in moviesDto)
            {
                movieDto.Id = movies[i].Id;
                i = i + 1;
            }

            return Created(new Uri(Request.RequestUri + "/" + movies[0].Id + ":" + movies[movies.Count - 1].Id), moviesDto);
        }

        // PUT /api/Movies/{id}
        [HttpPut]
        public void UpdateMovie (int id, MovieDto movieDto)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var movieInDb = _context.Movies.SingleOrDefault(m => m.Id == id);

            if (movieInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            Mapper.Map(movieDto, movieInDb);

            _context.SaveChanges();
        }

        // DELETE /api/Movies/{id}
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
