using System.Linq;
using System.Threading.Tasks;
using CoreRepository.Test.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoreRepository.Test
{
    [TestClass]
    public class RepositoryTests : RepositoryTestsBase
    {
        [TestMethod]
        public async Task Should_Insert_Entities()
        {
            // ARRANGE
            var blogArray = await InsertBlogsAsync();
            var repo = DIProvider.GetRepository<Blog>();

            // ACT
            var blogCount = await repo.GetAsync(q => q.Count());

            // ASSERT
            Assert.AreEqual(blogCount, blogArray.Length);
        }

        [TestMethod]
        public async Task Should_Find_Two_Cats_And_One_Dog_Blogs()
        {
            // ARRANGE
            var blogArray = await InsertBlogsAsync();
            var blogRepo = DIProvider.GetRepository<Blog>();

            // ACT
            var dogCount = await blogRepo.GetAsync(blogs => blogs.Count(b => b.Url.EndsWith("dogs")));
            var catBlogs = await blogRepo.GetAsync(blogs => blogs.Where(b => b.Url.Contains("cat")).ToArray());

            // ASSERT
            Assert.AreEqual(1, dogCount);
            Assert.AreEqual(2, catBlogs.Length);
        }

        [TestMethod]
        public async Task Should_Find_Single_Entity()
        {
            // ARRANGE
            var blogArray = await InsertBlogsAsync();
            var blogRepo = DIProvider.GetRepository<Blog>();

            // ACT
            var dogBlog = await blogRepo.GetAsync(blogs => blogs.SingleOrDefault(b => b.Url.EndsWith("dogs")));

            // ASSERT
            Assert.IsNotNull(dogBlog);
        }

        [TestMethod]
        public async Task Should_Update_Entity()
        {
            // ARRANGE
            var blogArray = await InsertBlogsAsync();
            var blogRepo = DIProvider.GetRepository<Blog>();

            // ACT
            var dogBlog = await blogRepo.GetAsync(blogs => blogs.Single(b => b.Url.EndsWith("dogs")));

            dogBlog.Url = dogBlog.Url.Replace("dogs", "bulldogs");
            await blogRepo.SaveChangesAsync();

            var blogRepo2 = DIProvider.GetRepository<Blog>();
            dogBlog = await blogRepo2.GetAsync(blogs => blogs.Single(b => b.Url.EndsWith("dogs")));

            // ASSERT
            Assert.AreEqual(dogBlog.Url, "http://sample.com/bulldogs");
        }

        [TestMethod]
        public async Task Should_Insert_Categories_And_Products()
        {
            // ARRANGE
            await InsertCategoriesAndProductsAsync();
            var categoriesRepo = DIProvider.GetSqliteRepository<Category>();
            var productsRepo = DIProvider.GetSqliteRepository<Product>();

            // ACT
            var categoriesCount = await categoriesRepo.GetAsync(q => q.Count());
            var productsCount = await productsRepo.GetAsync(q => q.Count());

            // ASSERT
            Assert.AreEqual(2, categoriesCount);
            Assert.AreEqual(24, productsCount);
        }

        [TestMethod]
        public async Task Should_Delete_Product()
        {
            // ARRANGE
            await InsertCategoriesAndProductsAsync();
            var productsRepo = DIProvider.GetSqliteRepository<Product>();

            // ACT
            var product = await productsRepo.GetAsync(q => q.Where(c => c.Name == "Citroën").Single());
            productsRepo.Remove(product);
            await productsRepo.SaveChangesAsync();

            // ASSERT
            Assert.AreEqual(23, productsRepo.All().Count());
        }
    }
}
