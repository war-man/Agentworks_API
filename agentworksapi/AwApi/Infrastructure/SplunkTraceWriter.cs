using System;
using System.Net.Http;
using System.Web.Http.Tracing;
using AwApi.Auth;
using AwApi.ViewModels;
using MoneyGram.Common.Localization;

namespace AwApi.Infrastructure
{
    public class SplunkTraceWriter : ITraceWriter
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public void Trace(HttpRequestMessage request, string category, TraceLevel level, Action<TraceRecord> traceAction)
        {
            var rec = new TraceRecord(request, category, level);
            traceAction(rec);

            var username = ClaimsManager.HasClaims() ? ClaimsManager.GetClaimValue(ClaimsNames.Sub) : null;

            if(!string.IsNullOrEmpty(username))
            {
                //[DATE TIME] [SEVERITY] [APPLICATION] [METHOD] DETAIL
                var status = string.IsNullOrWhiteSpace(rec.Status.ToString()) || rec.Status.ToString() == "0" ? "none" : rec.Status.ToString();
                var message = GetTranslated(rec.Exception != null
                    ? (!string.IsNullOrEmpty(rec.Exception.Message) ? rec.Exception.Message : string.Empty)
                    : (!string.IsNullOrEmpty(rec.Message) ? rec.Message : string.Empty)
                );

                logger.Info("API='{0}'; STATUS='{1}'; Msg='{2}", rec.Operation, status, message);
            }
        }

        public string GetTranslated(string key)
        {
            if(!key.IsLocalizedStringKey())
            {
                return key;
            }

            var translation = LocalizationProvider.GetTranslation(LocalizationProvider.DefaultApplication, new string[] { }, key, key);

            return translation;
        }
    }
}