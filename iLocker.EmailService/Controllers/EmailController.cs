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
    /// Email account api controller
    /// </summary>
    [ApiConventionType(typeof(DefaultApiConventions))]
    [SwaggerResponse(StatusCodes.Status406NotAcceptable, Type = typeof(IDictionary<string, string>), Description = "Not acceptable")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(IDictionary<string, string>), Description = "Bad request result, i.e Dictianary of error's")]
    [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, Type = typeof(IDictionary<string, string>), Description = "Internal server error")]
    public class EmailAccountController : ControllerBase
    {

        #region Fields
        private readonly IEmailAccountService _emailAccountService;
        private readonly IEmailSenderService _emailSenderService;
        #endregion

        #region Ctor

        public EmailAccountController(
            IEmailAccountService emailAccountService,
            IEmailSenderService emailSenderService)
        {
            _emailAccountService = emailAccountService;
            _emailSenderService = emailSenderService;
        }

        #endregion

        /// <summary>
        /// Get all email accounts
        /// </summary>
        /// <param name="userId">The user identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the email accounts of a user <see cref="IEnumerable{EmailAccount}"/>
        /// </returns>
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<EmailAccount>), Description = "List of email account's")]
        public async Task<ActionResult<IEnumerable<EmailAccount>>> GetEmailAccounts(long userId)
        {
            return Ok(await _emailAccountService.GetEmailAccountsAsync());
        }

        /// <summary>
        /// Gets a email account
        /// </summary>
        /// <param name="id">The email account id</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the email account <see cref="EmailAccount"/>
        /// </returns>
        [HttpGet("{id:long}")]
        [ActionName(nameof(EmailAccount))]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(EmailAccount), Description = "Email account, requested by identifier i.e Id")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(IDictionary<string, string>), Description = "Not found")]
        public async Task<ActionResult<EmailAccount>> EmailAccount(long id)
        {
            var obj = await _emailAccountService.GetEmailAccountAsync(id);
            if (obj == null) return NotFound();
            return Ok(obj);
        }

        /// <summary>
        /// Deletes a email account
        /// </summary>
        /// <param name="id">The email account id</param>
        /// <returns>
        /// return NoContent result if email account is deleted <see cref="NoContentResult"/> 
        /// </returns>
        [HttpDelete("{id:long}")]
        [SwaggerResponse(StatusCodes.Status204NoContent, Description = "Email account deleted")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(IDictionary<string, string>), Description = "Not found")]
        public async Task<IActionResult> DeleteEmailAccount(long id)
        {
            var obj = await _emailAccountService.GetEmailAccountAsync(id);
            if (obj == null) return NotFound();
            await _emailAccountService.DeleteEmailAccountAsync(obj);
            return NoContent();
        }

        /// <summary>
        /// Update a email account
        /// </summary>
        /// <param name="id">The email account id</param>
        /// <param name="email account">EmailAccount object  <see cref="EmailAccount"/></param>
        /// <returns>
        /// return NoContent result if email account is Updated <see cref="NoContentResult"/> 
        /// </returns>
        [HttpPut("{id:long}")]
        [SwaggerResponse(StatusCodes.Status204NoContent, Description = "Email account updated")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(IDictionary<string, string>), Description = "Not found")]
        public async Task<IActionResult> UpdateEmailAccount(long id, [FromBody] EmailAccount emailAccount)
        {
            var obj = await _emailAccountService.GetEmailAccountAsync(id);
            if (obj == null) return NotFound();
            await _emailAccountService.UpdateEmailAccountAsync(emailAccount);
            return NoContent();
        }

        /// <summary>
        /// Create new email account
        /// </summary>
        /// <param name="userId">The user identifier</param>
        /// <param name="emailAccount">EmailAccount object <see cref="EmailAccount"/></param>
        /// <returns>
        /// The createAtAction result for new email account <see cref="EmailAccount"/>
        /// </returns>
        [HttpPost("{userId}")]
        [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(EmailAccount), Description = "New created email account")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(IDictionary<string, string>), Description = "Not found")]
        public async Task<ActionResult<EmailAccount>> CreateEmailAccount(long userId, [FromBody] EmailAccount emailAccount)
        {
            var obj = await _emailAccountService.AddEmailAccountAsync(emailAccount);
            if (obj == null) return BadRequest();
            return CreatedAtAction(nameof(EmailAccount), new { Id = obj.Id }, obj);
        }

        /// <summary>
        /// Sets default email account
        /// </summary>
        /// <param name="id">The email account id</param>
        /// <returns>
        /// return NoContent result if email account is Updated <see cref="NoContentResult"/> 
        /// </returns>
        [HttpPut("MarkAsDefaultEmail/{id:long}")]
        [ActionName(nameof(MarkAsDefaultEmail))]
        [SwaggerResponse(StatusCodes.Status204NoContent, Description = "Default email account updated")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(IDictionary<string, string>), Description = "Not found")]
        public async Task<IActionResult> MarkAsDefaultEmail(long id)
        {
            var obj = await _emailAccountService.GetEmailAccountAsync(id);
            if (obj == null) return NotFound();
            await _emailAccountService.SetDefaultEmailAccountAsync(obj);
            return NoContent();
        }


        /// <summary>
        /// Change password of email account
        /// </summary>
        /// <param name="model">Email account model object <see cref="EmailAccountModel"/></param>
        /// <returns>
        /// return NoContent result if email account is Updated <see cref="NoContentResult"/> 
        /// </returns>
        [HttpPut("ChangePassword")]
        [ActionName(nameof(ChangePassword))]
        [SwaggerResponse(StatusCodes.Status204NoContent, Description = "Password of email account updated")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(IDictionary<string, string>), Description = "Not found")]
        public virtual async Task<IActionResult> ChangePassword(EmailAccountModel model)
        {
            var emailAccount = await _emailAccountService.GetEmailAccountAsync(model.Id);
            if (emailAccount == null) return NotFound();
            emailAccount.Password = model.Password;
            await _emailAccountService.UpdateEmailAccountAsync(emailAccount);
            return NoContent();
        }

        /// <summary>
        /// Send a test email
        /// </summary>
        /// <param name="model">Email account model object <see cref="EmailAccountModel"/></param>
        /// <returns>
        /// return NoContent result if test email is sent <see cref="NoContentResult"/> 
        /// </returns>
        [HttpPost("SendTestEmail")]
        [SwaggerResponse(StatusCodes.Status204NoContent, Description = "Test email sent")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(IDictionary<string, string>), Description = "Test email not sent")]
        public virtual async Task<IActionResult> SendTestEmail(EmailAccountModel model)
        {
            //try to get an email account with the specified id
            var emailAccount = await _emailAccountService.GetEmailAccountAsync(model.Id);
            if (emailAccount == null) return NotFound();

            if (!CommonHelper.IsValidEmail(model.SendTestEmailTo))
            {
                return BadRequest("Wrong email");
            }
            try
            {
                var subject = CommonHelper.GetApplicationName() + " Testing email functionality.";
                var body = "Email works fine.";
                await _emailSenderService.SendEmailAsync(emailAccount, subject, body, emailAccount.Email, emailAccount.DisplayName, model.SendTestEmailTo,model.SendTestEmailTo);
                return NoContent();
            }
            catch (Exception exc)
            {
               return BadRequest(exc.Message);
            }
        }
        //#endregion
    }
}
