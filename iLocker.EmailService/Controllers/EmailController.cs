using iLocker.EmailService.Helper;
using iLocker.EmailService.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace iLocker.EmailService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        #region Methods

        public virtual async Task<IActionResult> MarkAsDefaultEmail(long id)
        {
            var defaultEmailAccount = await _emailAccountService.GetEmailAccountAsync(id);
            if (defaultEmailAccount == null)
                return NotFound();

            return Ok();
        }

        public virtual async Task<IActionResult> Create()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageEmailAccounts))
                return AccessDeniedView();

            //prepare model
            var model = await _emailAccountModelFactory.PrepareEmailAccountModelAsync(new EmailAccountModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual async Task<IActionResult> Create(EmailAccountModel model, bool continueEditing)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageEmailAccounts))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var emailAccount = model.ToEntity<EmailAccount>();

                //set password manually
                emailAccount.Password = model.Password;
                await _emailAccountService.InsertEmailAccountAsync(emailAccount);

                //activity log
                await _customerActivityService.InsertActivityAsync("AddNewEmailAccount",
                    string.Format(await _localizationService.GetResourceAsync("ActivityLog.AddNewEmailAccount"), emailAccount.Id), emailAccount);

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.EmailAccounts.Added"));

                return continueEditing ? RedirectToAction("Edit", new { id = emailAccount.Id }) : RedirectToAction("List");
            }

            //prepare model
            model = await _emailAccountModelFactory.PrepareEmailAccountModelAsync(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual async Task<IActionResult> Edit(int id, bool showtour = false)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageEmailAccounts))
                return AccessDeniedView();

            //try to get an email account with the specified id
            var emailAccount = await _emailAccountService.GetEmailAccountByIdAsync(id);
            if (emailAccount == null)
                return RedirectToAction("List");

            //prepare model
            var model = await _emailAccountModelFactory.PrepareEmailAccountModelAsync(null, emailAccount);

            //show configuration tour
            if (showtour)
            {
                var customer = await _workContext.GetCurrentCustomerAsync();
                var hideCard = await _genericAttributeService.GetAttributeAsync<bool>(customer, NopCustomerDefaults.HideConfigurationStepsAttribute);
                var closeCard = await _genericAttributeService.GetAttributeAsync<bool>(customer, NopCustomerDefaults.CloseConfigurationStepsAttribute);

                if (!hideCard && !closeCard)
                    ViewBag.ShowTour = true;
            }

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public virtual async Task<IActionResult> Edit(EmailAccountModel model, bool continueEditing)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageEmailAccounts))
                return AccessDeniedView();

            //try to get an email account with the specified id
            var emailAccount = await _emailAccountService.GetEmailAccountByIdAsync(model.Id);
            if (emailAccount == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                emailAccount = model.ToEntity(emailAccount);
                await _emailAccountService.UpdateEmailAccountAsync(emailAccount);

                //activity log
                await _customerActivityService.InsertActivityAsync("EditEmailAccount",
                    string.Format(await _localizationService.GetResourceAsync("ActivityLog.EditEmailAccount"), emailAccount.Id), emailAccount);

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.EmailAccounts.Updated"));

                return continueEditing ? RedirectToAction("Edit", new { id = emailAccount.Id }) : RedirectToAction("List");
            }

            //prepare model
            model = await _emailAccountModelFactory.PrepareEmailAccountModelAsync(model, emailAccount, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [FormValueRequired("changepassword")]
        public virtual async Task<IActionResult> ChangePassword(EmailAccountModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageEmailAccounts))
                return AccessDeniedView();

            //try to get an email account with the specified id
            var emailAccount = await _emailAccountService.GetEmailAccountByIdAsync(model.Id);
            if (emailAccount == null)
                return RedirectToAction("List");

            //do not validate model
            emailAccount.Password = model.Password;
            await _emailAccountService.UpdateEmailAccountAsync(emailAccount);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.EmailAccounts.Fields.Password.PasswordChanged"));

            return RedirectToAction("Edit", new { id = emailAccount.Id });
        }

        [HttpPost, ActionName("Edit")]
        [FormValueRequired("sendtestemail")]
        public virtual async Task<IActionResult> SendTestEmail(EmailAccountModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageEmailAccounts))
                return AccessDeniedView();

            //try to get an email account with the specified id
            var emailAccount = await _emailAccountService.GetEmailAccountByIdAsync(model.Id);
            if (emailAccount == null)
                return RedirectToAction("List");

            if (!CommonHelper.IsValidEmail(model.SendTestEmailTo))
            {
                _notificationService.ErrorNotification(await _localizationService.GetResourceAsync("Admin.Common.WrongEmail"));
                return View(model);
            }

            try
            {
                if (string.IsNullOrWhiteSpace(model.SendTestEmailTo))
                    throw new NopException("Enter test email address");
                var store = await _storeContext.GetCurrentStoreAsync();
                var subject = store.Name + ". Testing email functionality.";
                var body = "Email works fine.";
                await _emailSender.SendEmailAsync(emailAccount, subject, body, emailAccount.Email, emailAccount.DisplayName, model.SendTestEmailTo, null);

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.EmailAccounts.SendTestEmail.Success"));

                return RedirectToAction("Edit", new { id = emailAccount.Id });
            }
            catch (Exception exc)
            {
                _notificationService.ErrorNotification(exc.Message);
            }

            //prepare model
            model = await _emailAccountModelFactory.PrepareEmailAccountModelAsync(model, emailAccount, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Delete(int id)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageEmailAccounts))
                return AccessDeniedView();

            //try to get an email account with the specified id
            var emailAccount = await _emailAccountService.GetEmailAccountByIdAsync(id);
            if (emailAccount == null)
                return RedirectToAction("List");

            try
            {
                await _emailAccountService.DeleteEmailAccountAsync(emailAccount);

                //activity log
                await _customerActivityService.InsertActivityAsync("DeleteEmailAccount",
                    string.Format(await _localizationService.GetResourceAsync("ActivityLog.DeleteEmailAccount"), emailAccount.Id), emailAccount);

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.EmailAccounts.Deleted"));

                return RedirectToAction("List");
            }
            catch (Exception exc)
            {
                await _notificationService.ErrorNotificationAsync(exc);
                return RedirectToAction("Edit", new { id = emailAccount.Id });
            }
        }

        #endregion
    }
}
