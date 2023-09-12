using turbin.sikker.core.Model;
using turbin.sikker.core.Utilities;
using Xunit;

namespace turbin.sikker.core.Tests
{
    public class CategoryTests
    {
        private readonly ICategoryUtilities _categoryUtilities;

        public CategoryTests()
        {
            _categoryUtilities = new CategoryUtilities();
        }

        [Fact]
        public void isCategoryNametaken_sameName_True()
        {
            List<Category> categories = new List<Category>();
            var cat1 = new Category {
                Id = "1",
                Name = "Category 1"
            };
            var cat2 = new Category {
                Id = "2",
                Name = "Category 2"
            };
            var cat3 = new Category {
                Id = "3",
                Name = "Category 3"
            };

            categories.Add(cat1);
            categories.Add(cat2);
            categories.Add(cat3);

            var cat4 = new Category {
                Id = "4",
                Name = "Category 1"
            };

            Assert.True(_categoryUtilities.isCategoryNametaken(categories, cat4.Name));
        }

        [Fact]
        public void isCategoryNametaken_differentName_False()
        {
            List<Category> categories = new List<Category>();
            var cat1 = new Category {
                Id = "1",
                Name = "Category 1"
            };
            var cat2 = new Category {
                Id = "2",
                Name = "Category 2"
            };
            var cat3 = new Category {
                Id = "3",
                Name = "Category 3"
            };

            categories.Add(cat1);
            categories.Add(cat2);
            categories.Add(cat3);

            var cat4 = new Category {
                Id = "4",
                Name = "Category 4"
            };

            Assert.False(_categoryUtilities.isCategoryNametaken(categories, cat4.Name));
        }
    }
}