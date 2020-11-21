using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreWithVueJs.Business.Factories.Interfaces;
using CoreWithVueJs.Models.Interfaces.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreWithVueJs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoreApiController : ControllerBase
    {
        private readonly IDataFactory _dataFactory;

        public CoreApiController(IDataFactory dataFactory)
        {
            _dataFactory = dataFactory;
        }

        [HttpGet, Route("getposts")]
        public async Task<IActionResult> GetPostsAsync()
        {
            var posts = await _dataFactory.GetAllOfTypeAsync<IPost>().ConfigureAwait(false);

            return Ok(posts);
        }

        [HttpGet, Route("getcomments")]
        public async Task<IActionResult> GetCommentsAsync()
        {
            var comments = await _dataFactory.GetAllOfTypeAsync<IComment>().ConfigureAwait(false);

            return Ok(comments);
        }

        // Work on this later once token-handling is implemented
        [Authorize]
        [HttpGet, Route("getpost")]
        public async Task<IActionResult> GetPostAsync([FromQuery] Guid id, [FromBody] string token)
        {
            return Ok();
        }
    }
}
