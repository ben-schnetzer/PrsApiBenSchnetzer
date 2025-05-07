using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrsApi.DTO;
using PrsApi.Models;

namespace PrsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly PrsDbContext _context;

        public RequestsController(PrsDbContext context)
        {
            _context = context;
        }

        // GET: api/Requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequests()
        {
            return await _context.Requests.ToListAsync();
        }

        // GET: api/Requests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);

            if (request == null)
            {
                return NotFound();
            }

            return request;
        }
        //GET REQUEST
        //Get requests ready for review
        //get back a list of requests
        //get requests in Review Status and req.userId != to userId

        //-------------------------------------------------RequestFrontEndBackEndMethod---------------------------------------------------------------
        //[HttpPost("{PublicFrontEnd}")]

        [HttpPost]
        public async Task<ActionResult<Request>> CreateRequest([FromBody] RequestDTO requestDTO)
        {

            string generatedRequestNumber = getNextRequestNumber();
            // Convert DTO to Entity
            var request = new Request
            {
                UserId = requestDTO.UserId,
                Description = requestDTO.Description,
                Justification = requestDTO.Justification,
                DateNeeded = requestDTO.DateNeeded,
                DeliveryMode = requestDTO.DeliveryMode,
                RequestNumber = generatedRequestNumber, //Assign the generated Request Number
                Status = "NEW", // Default value
                Total = 0.0m, // Default value
                SubmittedDate = DateTime.Now // Default value

            };

            _context.Requests.Add(request);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRequest), new { id = request.Id }, request);
        }

        private string getNextRequestNumber()
        {
            string requestNbr = "R";
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            requestNbr += today.ToString("yyMMdd");

            string maxReqNbr = _context.Requests.Max(r => r.RequestNumber);
            string reqNbr = "";

            if (maxReqNbr != null)
            {
                string tempNbr;
                if (maxReqNbr.Length != 11)
                {
                    tempNbr = "00000000000";
                }
                else
                {
                    tempNbr = maxReqNbr.Substring(7, 4);//Substring(10) was previously 7 (Set it back to 7)
                }
                int nbr = Int32.Parse(tempNbr);
                nbr++;
                reqNbr = nbr.ToString().PadLeft(4, '0');
            }
            else
            {
                reqNbr = "0001";
            }

            requestNbr += reqNbr;
            return requestNbr;
        }

        // PUT: api/Requests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(int id, Request request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            _context.Entry(request).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        //Request
        //Insert a new Request using only 5 properties and set defaults as needed
        //return new object

        // POST: api/Requests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        //THIS IS THE ORIGINAL POST THAT IS NOT NECESSARY AND CAN BE REMOVED
        //[HttpPost]
        //public async Task<ActionResult<Request>> PostRequest(Request request)
        //{
        //    _context.Requests.Add(request);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetRequest", new { id = request.Id }, request);
        //}

        // DELETE: api/Requests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //------------------------RequestSubmit------------------------------
        [HttpPut("submit-review/{id}")]
        public async Task<IActionResult> SubmitForReview(int id)
        {
            var request = await _context.Requests.FindAsync(id);

            if (request == null)
            {
                return NotFound($"Request with ID {id} not found.");
            }

            if (request.Total <= 50)
            {
                request.Status = "APPROVED";
                request.SubmittedDate = DateTime.Now;
            }
            else
            {
                request.Status = "REVIEW";
                request.SubmittedDate = DateTime.Now;
            }

            _context.Requests.Update(request);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Request status updated successfully.", Status = request.Status });
        }
        //--------------------------------------------------------------------
        //--------------------List of Requests------------------------------
        [HttpGet("list-review/{userId}")]
        public async Task<ActionResult<IEnumerable<Request>>> GetReviewRequests(int userId)
        {
            var requests = await _context.Requests
                .Where(r => r.Status == "REVIEW" && r.UserId != userId)
                .ToListAsync();

            if (!requests.Any())
            {
                return NotFound("No requests found for review.");
            }

            return Ok(requests);
        }
        //------------------------------------------------------
        //----------------RequestApprove------------------------
        [HttpPut("approve/{id}")]
        public async Task<ActionResult<Request>> ApproveRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);

            if (request == null)
            {
                return NotFound($"Request with ID {id} not found.");
            }

            if (request.Status != "REVIEW")
            {
                return BadRequest("Request is not in REVIEW status and cannot be approved.");
            }

            // Update the status to APPROVED
            request.Status = "APPROVED";

            // Save changes to the database
            _context.Requests.Update(request);
            await _context.SaveChangesAsync();

            return Ok(request); // Returns the Approved request instance
        }
        //------------------------------------------------------
        //---------------RequestRejected------------------------
        [HttpPut("reject/{id}")]
        public async Task<ActionResult<Request>> RejectRequest(int id, [FromBody] string reasonForRejection)
        {
            var request = await _context.Requests.FindAsync(id);

            if (request == null)
            {
                return NotFound($"Request with ID {id} not found.");
            }

            if (request.Status != "REVIEW")
            {
                return BadRequest("Only requests in REVIEW status can be rejected.");
            }

            // Update status and reasonForRejection
            request.Status = "REJECTED";
            request.ReasonForRejection = reasonForRejection;

            _context.Requests.Update(request);
            await _context.SaveChangesAsync();

            return Ok(request); // Returns the Rejected request instance
        }
        //------------------------------------------------------
        private bool RequestExists(int id)
        {
            return _context.Requests.Any(e => e.Id == id);
        }
    }
}
