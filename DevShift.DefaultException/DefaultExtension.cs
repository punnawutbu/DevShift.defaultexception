using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;

namespace DevShift.defaultexception
{
public static class DefaultExtension
    {
        public static JObject DefaultValidate(this Object model)
        {
            var jObject = new JObject();
            var validationContext = new ValidationContext(model, null, null);
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(model, validationContext, validationResults, true))
            {
                foreach (var r in validationResults)
                    jObject.Add(r.MemberNames.First(), r.ErrorMessage);

                return jObject;
            }

            return jObject;
        }
        
        public static IApplicationBuilder UseDefaultException(this IApplicationBuilder app, ILogger logger, bool isDev)
        {
            return app.UseExceptionHandler(configure =>
            {
                configure.Run
                (
                    async handler =>
                    {
                        var response = handler.Response;
                        response.ContentType = "application/json";

                        var handlerFeature = handler.Features.Get<IExceptionHandlerFeature>();

                        if (handlerFeature != null)
                        {
                            var exResponse = new DefaultResponse();
                            var ex = handlerFeature.Error as DefaultException;

                            if (ex != null)
                            {
                                exResponse.Error = HttpStatusCode.BadRequest.ToString();
                                response.StatusCode = (Int32)ex.Status;
                            }
                            else
                                response.StatusCode = (Int32)HttpStatusCode.InternalServerError;                                    

                            exResponse.StackTrace = handlerFeature.Error.StackTrace.Replace("\n", ".");
                            exResponse.Exception = $"{handlerFeature.Error.TargetSite.DeclaringType.FullName}.{new StackTrace(handlerFeature.Error).GetFrame(0).GetMethod().Name}";
                            exResponse.Message = handlerFeature.Error.Message;
                            exResponse.Path = ((ExceptionHandlerFeature)handlerFeature).Path;
                            exResponse.Status = response.StatusCode;
                            
                            logger.Error($"{exResponse.Message} {exResponse.StackTrace}");
                            if(isDev) {
                                await response.WriteAsync(JsonConvert.SerializeObject(exResponse)).ConfigureAwait(false);
                            }
                        }
                    }
                );
            });
        }
    }

}