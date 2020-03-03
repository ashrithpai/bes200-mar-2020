using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Services
{
    public static class ControllerExtensions
    {
        public static ActionResult<T> Maybe<T>(this Controller controller, T entity)
        {
            if(entity == null)
            {
                return new NotFoundResult();
            }
            else
            {
                return new OkObjectResult(entity);
            }
        }

        public static ActionResult Either<Some,None>(this Controller controller, bool condition)
            where Some: ActionResult, new()
            where None: ActionResult, new()
        {
            if (condition)
            {
                return new Some();
            }
            else
            {
                return new None();
            }
        }
    }
}
