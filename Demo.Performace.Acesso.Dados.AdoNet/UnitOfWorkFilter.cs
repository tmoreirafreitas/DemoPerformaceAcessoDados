using Demo.Performace.Acesso.Domain.Repository;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Demo.Performace.Acesso.Dados.AdoNet
{
    public class UnitOfWorkFilter : IAsyncActionFilter
    {
        private readonly IUnitOfWork _work;
        private readonly ILogger _logger;

        public UnitOfWorkFilter(IUnitOfWork work, ILoggerFactory loggerFactory)
        {
            _work = work;
            _logger = loggerFactory.CreateLogger<UnitOfWorkFilter>();
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Method.Equals("Post", StringComparison.OrdinalIgnoreCase)
                && context.HttpContext.Request.Method.Equals("Put", StringComparison.OrdinalIgnoreCase))
                await Task.CompletedTask;

            string msg;
            if (_work.GetConnection().State != ConnectionState.Open)
            {
                msg = "O provedor de conexão não está aberto!";
                _logger.LogInformation(msg);
                throw new NotSupportedException(msg);
            }
            _work.GetTransaction();
            var executedContext = await next.Invoke();
            if (executedContext.Exception == null)
            {
                msg = "Salvando mudanças para unidade de trabalho";
                _logger.LogInformation(msg);
                _work.Commit();
            }
            else
            {
                msg = "Evite salvar as alterações para a unidade de trabalho devido a uma exceção";
                _logger.LogInformation(msg);
                _work.Rollback();
            }
        }
    }
}