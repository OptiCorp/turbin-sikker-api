using turbin.sikker.core.Model;


namespace turbin.sikker.core.Utilities
{
public interface ICategoryUtilities
    {
        bool IsCategoryNametaken(IEnumerable<Category> categories, string categoryName);
    }
}