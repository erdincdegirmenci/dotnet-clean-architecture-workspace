using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Application.Managers;
using Template.Infrastructure.Common;

namespace Template.Infrastructure.Managers
{
    public class TransactionContextManager : ITransactionContextManager
    {
        readonly IHttpContextAccessor _httpContextAccessor;

        public TransactionContextManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        #region Transaction GUID
        public void SetTransaction(bool forcedCreateNewIdentifier = false)
        {
            if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
            {
                //Yeni transaction üretimine zorlanıyorsa
                if (forcedCreateNewIdentifier)
                {
                    _httpContextAccessor.HttpContext.Session.SetString("TransactionContext", Guid.NewGuid().ToString());
                }
                else
                {
                    if (string.IsNullOrEmpty(_httpContextAccessor.HttpContext.Session.GetString("TransactionContext")))
                    {
                        //Oncelikli olarak headerdan gelen set edilir
                        var transactionIdentifier = _httpContextAccessor.HttpContext.Request.Headers[TransactionContextConst.TRANSACTION_IDENTIFIER_HEADER];
                        if (!String.IsNullOrEmpty(transactionIdentifier))
                        {
                            _httpContextAccessor.HttpContext.Session.SetString("TransactionContext", transactionIdentifier);
                        }
                        else
                        {
                            _httpContextAccessor.HttpContext.Session.SetString("TransactionContext", Guid.NewGuid().ToString());
                        }

                    }
                }
            }
        }

        public string GetTransaction()
        {
            if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
            {

                string transactionIdentifier = _httpContextAccessor.HttpContext.Session.GetString("TransactionContext");

                if (string.IsNullOrEmpty(transactionIdentifier))
                {
                    SetTransaction();
                    transactionIdentifier = _httpContextAccessor.HttpContext.Session.GetString("TransactionContext");
                }

                return transactionIdentifier;
            }
            else
            {
                return string.Empty;
            }
        }

        public void DeleteTransaction()
        {
            if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
            {
                _httpContextAccessor.HttpContext.Session.Remove("TransactionContext");
            }
        }
        #endregion

        #region Security Token
        public void SetJwtToken(string token)
        {
            if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
            {
                _httpContextAccessor.HttpContext.Session.SetString("TransactionContextJwtToken", token);
            }
        }

        public string GetJwtToken()
        {
            if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
            {
                return _httpContextAccessor.HttpContext.Session.GetString("TransactionContextJwtToken");
            }
            else
            {
                return string.Empty;
            }
        }

        public void DeleteJwtToken()
        {
            if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
            {
                _httpContextAccessor.HttpContext.Session.Remove("TransactionContextJwtToken");
            }
        }

        public void SetLanguage(string language)
        {
            if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
            {
                _httpContextAccessor.HttpContext.Session.SetString("TransactionLanguage", language);
            }
        }

        public string GetLanguage()
        {
            if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
            {
                //Eger language daha once set edilmediyse headera bakar ve geleni set eder
                if (string.IsNullOrEmpty(_httpContextAccessor.HttpContext.Session.GetString("TransactionLanguage")))
                {
                    var transactionlanguage = _httpContextAccessor.HttpContext.Request.Headers[TransactionContextConst.TRANSACTION_LANGUAGE_HEADER];
                    if (!String.IsNullOrEmpty(transactionlanguage))
                    {
                        SetLanguage(transactionlanguage);
                    }
                    else
                    {
                        return "tr";
                    }
                }

                return _httpContextAccessor.HttpContext.Session.GetString("TransactionLanguage");

            }
            else
            {
                return string.Empty;
            }
        }

        public void DeleteLanguage()
        {
            if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
            {
                _httpContextAccessor.HttpContext.Session.Remove("TransactionLanguage");
            }
        }
        #endregion
    }
}
