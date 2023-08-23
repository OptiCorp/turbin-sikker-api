using turbin.sikker.core.Model;

namespace turbin.sikker.core.Utilities
{
public class CategoryUtilities : ICategoryUtilities
	{
        public bool isCategoryNametaken(IEnumerable<Category> categories, string categoryName)
        {
            return categories.Any(c => c.Name == categoryName);
        }
    }
}