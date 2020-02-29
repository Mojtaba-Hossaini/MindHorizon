using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MindHorizon.Common;
using MindHorizon.Data.Contracts;
using MindHorizon.Services.Contracts;
using MindHorizon.ViewModels.DynamicAccess;
using MindHorizon.ViewModels.Settings;
using MindHorizon.ViewModels.SiteSetting;

namespace MindHorizon.Areas.Admin.Controllers
{
    [DisplayName("مدیریت تنظیمات وب سایت")]
    public class SiteSettingController : BaseController
    {
        private readonly IUnitOfWork _uw;
        private readonly IWritableOptions<SiteSettings> _writableLocations;
        private readonly IHostingEnvironment _env;
        public SiteSettingController(IUnitOfWork uw, IWritableOptions<SiteSettings> writableLocations, IHostingEnvironment env)
        {
            _writableLocations = writableLocations;
            _uw = uw;
            _env = env;
        }

        [HttpGet, DisplayName("نمایش تنظیمات سایت")]
        [Authorize(Policy = ConstantPolicies.DynamicPermission)]
        public IActionResult Index()
        {
            var settings = new SettingsViewModel()
            {
                Title = _writableLocations.Value.SiteInfo.Title,
                LogoName= _writableLocations.Value.SiteInfo.Logo,
                FaviconName= _writableLocations.Value.SiteInfo.Favicon,
                Description = _writableLocations.Value.SiteInfo.Description,
                MetaDescriptionTag = _writableLocations.Value.SiteInfo.MetaDescriptionTag,
                EmailHost= _writableLocations.Value.EmailSetting.Host,
                EmailUsername = _writableLocations.Value.EmailSetting.Username,
                EmailPassword = _writableLocations.Value.EmailSetting.Password,
                EmailPort = _writableLocations.Value.EmailSetting.Port,
                SenderEmail = _writableLocations.Value.EmailSetting.Email,
            };
            return View(settings);
        }

        [HttpPost, DisplayName("ذخیره تغییرات سایت")]
        [Authorize(Policy = ConstantPolicies.DynamicPermission)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(SettingsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.Favicon != null)
                {
                    viewModel.FaviconName = viewModel.Favicon.FileName;
                    await viewModel.Favicon.UploadFileAsync($"{_env.WebRootPath}/assets/img/{viewModel.FaviconName}");
                }
                else
                    viewModel.FaviconName = _writableLocations.Value.SiteInfo.Favicon;

                if (viewModel.Logo != null)
                {
                    viewModel.LogoName = viewModel.Logo.FileName;
                    await viewModel.Logo.UploadFileAsync($"{_env.WebRootPath}/assets/img/{viewModel.LogoName}");
                }
                else
                    viewModel.LogoName = _writableLocations.Value.SiteInfo.Logo;

                _writableLocations.Update(opt =>
                {
                    opt.EmailSetting.Username = viewModel.EmailUsername;
                    opt.EmailSetting.Password = viewModel.EmailPassword;
                    opt.EmailSetting.Port = viewModel.EmailPort;
                    opt.EmailSetting.Host = viewModel.EmailHost;
                    opt.EmailSetting.Email = viewModel.SenderEmail;
                    opt.SiteInfo.Title = viewModel.Title;
                    opt.SiteInfo.Description = viewModel.Description;
                    opt.SiteInfo.MetaDescriptionTag = viewModel.MetaDescriptionTag;
                    opt.SiteInfo.Logo = viewModel.LogoName;
                    opt.SiteInfo.Favicon = viewModel.FaviconName;
                });

                ViewBag.Alert = "ویرایش اطلاعات با موفقیت انجام شد.";
            }

            return View(viewModel);
        }
    }
}