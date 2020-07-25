using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.Mvc;
using Dal.Classes;
using Dal.Interfaces;
using Dal.Models;
using Dal.Repositories;

namespace Dal.Services
{
    public abstract class BaseService
    {
        protected String CurrentUserEmail = Thread.CurrentPrincipal.Identity.Name;
        protected Repository Repository;
        protected ILog Logger;
        protected Validator Validator;

        public void SetMessage(String message, BootstrapAlertTypes type)
        {
            ServiceTempData[Configuration.MessageMagicString] = message;
            ServiceTempData[Configuration.AlertMagicString] = GetClassString(type);
        }
        public Dictionary<String, String> ServiceTempData { protected set; get; }
        public ModelStateDictionary ServiceModelState { protected set; get; }

        public BaseService(Repository repository, ILog log)
        {
            Repository = repository;
            ServiceModelState = new ModelStateDictionary();
            ServiceTempData = new Dictionary<String, String>();
            Logger = log;
            Validator = new Validator(ServiceModelState);
        }

        private String GetClassString(BootstrapAlertTypes type)
        {
            switch (type)
            {
                default:
                case BootstrapAlertTypes.Success: return "alert-success";
                case BootstrapAlertTypes.Info: return "alert-info";
                case BootstrapAlertTypes.Warning: return "alert-warning";
                case BootstrapAlertTypes.Danger: return "alert-danger";
            }

        }
    }
}