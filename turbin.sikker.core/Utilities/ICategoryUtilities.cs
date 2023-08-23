using turbin.sikker.core.Model;


namespace turbin.sikker.core.Utilities
{
public interface ICategoryUtilities
    {
        bool isCategoryNametaken(IEnumerable<Category> categories, string categoryName);
    }
}