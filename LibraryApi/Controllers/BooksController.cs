using LibraryApi.Domain;
using LibraryApi.Models;
using LibraryApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    public class BooksController : Controller
    {
        IMapBooks Mapper;

        public BooksController(IMapBooks mapper)
        {
            Mapper = mapper;
        }

        [HttpPut("books/{id:int}/numberofpages")]
        public async Task<ActionResult> UpdateNumberOfPages(int id, [FromBody] int newPages)
        {
            bool didUpdate = await Mapper.UpdateNumberOfPages(id, newPages);

            //if(didUpdate)
            //{ return NoContent(); }
            //else
            //{
            //    return NotFound();
            //}

            return this.Either<NoContentResult, NotFoundResult>(didUpdate);

        }

        [HttpDelete("books/{id:int}")]
        public async Task<ActionResult> RemoveABook(int id)
        {
            await Mapper.Remove(id);
            return NoContent();
        }


        /// <summary>
        /// Add a Book To The Inventory
        /// </summary>
        /// <param name="bookToAdd">The details of the book to add</param>
        /// <returns></returns>
        [HttpPost("books")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ValidateModel]
        public async Task<ActionResult<GetABookResponse>> AddABook([FromBody] PostBooksRequest bookToAdd)
        {
            GetABookResponse response = await Mapper.AddABook(bookToAdd);
            return CreatedAtRoute("books#getabook", new { id = response.Id }, response);
        }


        [HttpGet("books/{id:int}", Name ="books#getabook")]
        public async Task<ActionResult<GetABookResponse>> GetABook(int id)
        {
            GetABookResponse response = await Mapper.GetBookById(id);

            //if(response == null)
            //{
            //    return NotFound();
            //} else
            //{
            //    return Ok(response);
            //}
            return this.Maybe(response);

        }

        [HttpGet("books")]
        public async Task<ActionResult<GetBooksResponse>> GetAllBooks([FromQuery] string genre = "all")
        {
            GetBooksResponse response = await Mapper.GetAllBooks(genre);
            return response;

            
        }

       
        
    }
}
