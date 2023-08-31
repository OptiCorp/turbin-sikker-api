using Microsoft.AspNetCore.Mvc;
namespace turbin.sikker.core.Controllers
{
    public abstract class BaseController<TModel> : Controller where TModel : class
    {
        protected TModel model;
        private readonly TurbinSikkerDbContext _context;

        protected BaseController(TModel model)
        {
            this.model = model;
        }


        public BaseController(TurbinSikkerDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<TModel> getModel()
        {
            return _context.Set<TModel>().ToList();
        }

    }
}

