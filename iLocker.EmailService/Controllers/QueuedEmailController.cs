using iLocker.EmailService.DomainModels;
using iLocker.EmailService.Helper;
using iLocker.EmailService.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace iLocker.EmailService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    //[ApiExplorerSettings(GroupName = "EmailService")]
    /// <summary>
    /// Queued email api controller
    /// </summary>
    [ApiConventionType(typeof(DefaultApiConventions))]
    [SwaggerResponse(StatusCodes.Status406NotAcceptable, Type = typeof(IDictionary<string, string>), Description = "Not acceptable")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(IDictionary<string, string>), Description = "Bad request result, i.e Dictianary of error's")]
    [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, Type = typeof(IDictionary<string, string>), Description = "Internal server error")]
    public class QueuedEmailController : ControllerBase
    {

        #region Fields
        private readonly IQueuedEmailService _queuedEmailService;
        private readonly IEmailSenderService _emailSenderService;
        #endregion

        #region Ctor

        public QueuedEmailController(
            IQueuedEmailService queuedEmailService,
            IEmailSenderService emailSenderService)
        {
            _queuedEmailService = queuedEmailService;
            _emailSenderService = emailSenderService;
        }

        #endregion

        /// <summary>
        /// Get all queued emails
        /// </summary>
        /// <param name="userId">The user identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the queued emails of a user <see cref="IEnumerable{QueuedEmail}"/>
        /// </returns>
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<QueuedEmail>), Description = "List of queued email's")]
        public async Task<ActionResult<IEnumerable<QueuedEmail>>> GetQueuedEmails(long userId)
        {
            return Ok(await _queuedEmailService.GetQueuedEmailsAsync());
        }

        /// <summary>
        /// Gets a queued email
        /// </summary>
        /// <param name="id">The queued email id</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the queued email <see cref="QueuedEmail"/>
        /// </returns>
        [HttpGet("{id:long}")]
        [ActionName(nameof(QueuedEmail))]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(QueuedEmail), Description = "Queued email, requested by identifier i.e Id")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(IDictionary<string, string>), Description = "Not found")]
        public async Task<ActionResult<QueuedEmail>> QueuedEmail(long id)
        {
            var obj = await _queuedEmailService.GetQueuedEmailAsync(id);
            if (obj == null) return NotFound();
            return Ok(obj);
        }

        /// <summary>
        /// Deletes a queued email
        /// </summary>
        /// <param name="id">The queued email id</param>
        /// <returns>
        /// return NoContent result if queued email is deleted <see cref="NoContentResult"/> 
        /// </returns>
        [HttpDelete("{id:long}")]
        [SwaggerResponse(StatusCodes.Status204NoContent,  Description = "Queued email deleted")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(IDictionary<string, string>), Description = "Not found")]
        public async Task<IActionResult> DeleteQueuedEmail(long id)
        {
            var obj = await _queuedEmailService.GetQueuedEmailAsync(id);
            if (obj == null) return NotFound();
            await _queuedEmailService.DeleteQueuedEmailAsync(obj);
            return NoContent();
        }

        /// <summary>
        /// Update a queued email
        /// </summary>
        /// <param name="id">The queued email id</param>
        /// <param name="queued email">QueuedEmail object  <see cref="QueuedEmail"/></param>
        /// <returns>
        /// return NoContent result if queued email is Updated <see cref="NoContentResult"/> 
        /// </returns>
        [HttpPut("{id:long}")]
        [SwaggerResponse(StatusCodes.Status204NoContent,Description = "Queued email updated")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(IDictionary<string, string>), Description = "Not found")]
        public async Task<IActionResult> UpdateQueuedEmail(long id, [FromBody] QueuedEmail queuedEmail)
        {
            var obj = await _queuedEmailService.GetQueuedEmailAsync(id);
            if (obj == null) return NotFound();
            await _queuedEmailService.UpdateQueuedEmailAsync(queuedEmail);
            return NoContent();
        }

        /// <summary>
        /// Create new queued email
        /// </summary>
        /// <param name="userId">The user identifier</param>
        /// <param name="queued email">Queued Email object  <see cref="QueuedEmail"/></param>
        /// <returns>
        /// The createAtAction result for new queued email <see cref="QueuedEmail"/>
        /// </returns>
        [HttpPost("{userId}")]
        [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(QueuedEmail), Description = "New created queued email")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(IDictionary<string, string>), Description = "Not found")]
        public async Task<ActionResult<QueuedEmail>> CreateQueuedEmail(long userId, [FromBody] QueuedEmail queuedEmail)
        {
            var obj = await _queuedEmailService.AddQueuedEmailAsync(queuedEmail);
            if (obj == null) return BadRequest();
            return CreatedAtAction(nameof(QueuedEmail), new { Id = obj.Id }, obj);
        }


        /// <summary>
        /// Delete all queued emails
        /// </summary>
        /// <returns>
        /// return NoContent result if all queued email is deleted <see cref="NoContentResult"/> 
        /// </returns>
        [HttpPut("DeleteAllQueuedEmails")]
        [ActionName(nameof(DeleteAllQueuedEmails))]
        [SwaggerResponse(StatusCodes.Status204NoContent, Description = "Queued emails deleted")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(IDictionary<string, string>), Description = "Queued emails not deleted")]
        public async Task<IActionResult> DeleteAllQueuedEmails()
        {
           await _queuedEmailService.DeleteAllQueuedEmailsAsync();
           return NoContent();
        }


        /// <summary>
        /// Delete all queued emails
        /// </summary>
        /// <param name="createdFromUtc">Start date</param>
        /// <param name="createdToUtc">End date object</param>
        /// <returns>
        /// return NoContent result if all queued email is deleted <see cref="NoContentResult"/> 
        /// </returns>
        [HttpPut("DeleteAllQueuedEmails/{createdFromUtc:DateTime?}/{createdToUtc:DateTime?}")]
        [ActionName(nameof(DeleteAllQueuedEmails))]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(int), Description = "Queued emails deleted count")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(IDictionary<string, string>), Description = "Queued emails not deleted")]
        public async Task<IActionResult> DeleteAlreadySentEmailsAsync(DateTime? createdFromUtc, DateTime? createdToUtc)
        {
            var emailsDeletedCount = await _queuedEmailService.DeleteAlreadySentEmailsAsync(createdFromUtc, createdToUtc);
            return Ok(emailsDeletedCount);
        }
    }
}
