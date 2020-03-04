using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryApi.Domain;
using LibraryApi.Models;
using LibraryApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static LibraryApi.Domain.Reservation;

namespace LibraryApi.Controllers
{

    public class ReservationsController : Controller
    {

        LibraryDataContext Context;
        ISendMessagesToTheReservationProcessor Processor;

        public ReservationsController(LibraryDataContext context, ISendMessagesToTheReservationProcessor processor)
        {
            Context = context;
            Processor = processor;
        }

        [HttpPost("reservations")]
        [ValidateModel]
        public async Task<ActionResult> AddAReservation([FromBody] PostReservationRequest reservation)
        {
            var reservationToSave = new Reservation
            {
                For = reservation.For,
                Books = string.Join(',', reservation.Books),
                ReservationCreated = DateTime.Now,
                Status = ReservationStatus.Pending

            };
            

            Context.Reservations.Add(reservationToSave);
            await Context.SaveChangesAsync();

            var response = MapIt(reservationToSave);
            Processor.SendReservationForProcessing(response);
            //TODO Write to MQ

            return Ok(response);

        }


        [HttpGet("reservations")]
        public async Task<ActionResult> GetAllReservations()
        {
            var response = new HttpCollection<GetReservationItemResponse>();

            var data = await Context.Reservations.ToListAsync();

            response.Data = data.Select(r => MapIt(r)).ToList();

            return Ok(response);
        }

        [HttpGet("reservations/pending")]
        public async Task<ActionResult> GetPendingReservations()
        {
            var response = new HttpCollection<GetReservationItemResponse>();

            var data = await Context.Reservations.Where(r => r.Status == ReservationStatus.Pending ).ToListAsync();

            response.Data = data.Select(r => MapIt(r)).ToList();

            return Ok(response);
        }

        [HttpGet("reservations/approved")]
        public async Task<ActionResult> GetApprovedReservations()
        {
            var response = new HttpCollection<GetReservationItemResponse>();

            var data = await Context.Reservations.Where(r => r.Status == ReservationStatus.Approved).ToListAsync();

            response.Data = data.Select(r => MapIt(r)).ToList();

            return Ok(response);
        }

        [HttpGet("reservations/cancelled")]
        public async Task<ActionResult> GetCancelledReservations()
        {
            var response = new HttpCollection<GetReservationItemResponse>();

            var data = await Context.Reservations.Where(r => r.Status == ReservationStatus.Cancelled).ToListAsync();

            response.Data = data.Select(r => MapIt(r)).ToList();

            return Ok(response);
        }

        private GetReservationItemResponse MapIt(Reservation r)
        {
            return new GetReservationItemResponse
            {
                Id = r.Id,
                For = r.For,
                ReservationCreated = r.ReservationCreated,
                Books = r.Books.Split(',')
                .Select(id => Url.ActionLink("GetABook", "Books", new { id = id })).ToList(),
                Status = r.Status
            };
        }
    }
}