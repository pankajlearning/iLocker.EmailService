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
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(EmailAccount), Description = "Email account deleted")]
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
        /// <param name="email account">EmailAccount object</param>
        /// <returns>
        /// return NoContent result if email account is Updated <see cref="NoContentResult"/> 
        /// </returns>
        [HttpPut("{id:long}")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(EmailAccount), Description = "Email account updated")]
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
        /// <param name="email account">EmailAccount object</param>
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
        [HttpPut("{id:long}")]
        [ActionName(nameof(MarkAsDefaultEmail))]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(EmailAccount), Description = "Default email account updated")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(IDictionary<string, string>), Description = "Not found")]
        public async Task<IActionResult> MarkAsDefaultEmail(long id)
        {
            var obj = await _emailAccountService.GetEmailAccountAsync(id);
            if (obj == null) return NotFound();
            await _emailAccountService.SetDefaultEmailAccountAsync(obj);
            return NoContent();
        }


        ////[HttpPost, ActionName("Edit")]
        //public virtual async Task<IActionResult> ChangePassword(EmailAccountModel model)
        //{
        //    //if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageEmailAccounts))
        //    //    return AccessDeniedView();

        //    ////try to get an email account with the specified id
        //    //var emailAccount = await _emailAccountService.GetEmailAccountByIdAsync(model.Id);
        //    //if (emailAccount == null)
        //    //    return RedirectToAction("List");

        //    ////do not validate model
        //    //emailAccount.Password = model.Password;
        //    //await _emailAccountService.UpdateEmailAccountAsync(emailAccount);

        //    //_notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.EmailAccounts.Fields.Password.PasswordChanged"));

        //    //return RedirectToAction("Edit", new { id = emailAccount.Id });
        //}

        ////[HttpPost, ActionName("Edit")]
        ////[FormValueRequired("sendtestemail")]
        //public virtual async Task<IActionResult> SendTestEmail(EmailAccountModel model)
        //{
        //    //if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageEmailAccounts))
        //    //    return AccessDeniedView();

        //    ////try to get an email account with the specified id
        //    //var emailAccount = await _emailAccountService.GetEmailAccountByIdAsync(model.Id);
        //    //if (emailAccount == null)
        //    //    return RedirectToAction("List");

        //    //if (!CommonHelper.IsValidEmail(model.SendTestEmailTo))
        //    //{
        //    //    _notificationService.ErrorNotification(await _localizationService.GetResourceAsync("Admin.Common.WrongEmail"));
        //    //    return View(model);
        //    //}

        //    //try
        //    //{
        //    //    if (string.IsNullOrWhiteSpace(model.SendTestEmailTo))
        //    //        throw new NopException("Enter test email address");
        //    //    var store = await _storeContext.GetCurrentStoreAsync();
        //    //    var subject = store.Name + ". Testing email functionality.";
        //    //    var body = "Email works fine.";
        //    //    await _emailSender.SendEmailAsync(emailAccount, subject, body, emailAccount.Email, emailAccount.DisplayName, model.SendTestEmailTo, null);

        //    //    _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.EmailAccounts.SendTestEmail.Success"));

        //    //    return RedirectToAction("Edit", new { id = emailAccount.Id });
        //    //}
        //    //catch (Exception exc)
        //    //{
        //    //    _notificationService.ErrorNotification(exc.Message);
        //    //}

        //    ////prepare model
        //    //model = await _emailAccountModelFactory.PrepareEmailAccountModelAsync(model, emailAccount, true);

        //    ////if we got this far, something failed, redisplay form
        //    //return View(model);
        //}

        //[HttpPost]
        //public virtual async Task<IActionResult> Delete(int id)
        //{
        //    //if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageEmailAccounts))
        //    //    return AccessDeniedView();

        //    ////try to get an email account with the specified id
        //    //var emailAccount = await _emailAccountService.GetEmailAccountByIdAsync(id);
        //    //if (emailAccount == null)
        //    //    return RedirectToAction("List");

        //    //try
        //    //{
        //    //    await _emailAccountService.DeleteEmailAccountAsync(emailAccount);

        //    //    //activity log
        //    //    await _customerActivityService.InsertActivityAsync("DeleteEmailAccount",
        //    //        string.Format(await _localizationService.GetResourceAsync("ActivityLog.DeleteEmailAccount"), emailAccount.Id), emailAccount);

        //    //    _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.EmailAccounts.Deleted"));

        //    //    return RedirectToAction("List");
        //    //}
        //    //catch (Exception exc)
        //    //{
        //    //    await _notificationService.ErrorNotificationAsync(exc);
        //    //    return RedirectToAction("Edit", new { id = emailAccount.Id });
        //    //}
        //}

        //#endregion
    }
}
