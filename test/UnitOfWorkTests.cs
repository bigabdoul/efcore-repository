using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoreRepository.Test.Data;
using CoreRepository.Test.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoreRepository.Test
{
    [TestClass]
    public class UnitOfWorkTests : RepositoryTestsBase
    {
        [TestMethod]
        public async Task Should_Create_Categories()
        {
            // ARRANGE
            var unit = GetUnitOfWork();
            var categories = unit.Repository<Category>();

            // ACT
            AddCategories(categories);
            await unit.CommitAsync();

            // ASSERT
            Assert.AreEqual(2, categories.All().Count());
        }

        [TestMethod]
        public async Task Should_Create_Categories_And_Products()
        {
            // ARRANGE
            await Should_Create_Categories(); // required in order to add products
            var unit = GetUnitOfWork();
            var products = unit.Repository<Product>();
            var carsCategory = await unit.Repository<Category>().GetAsync(q => q.Where(c => c.Name == "Cars").Single());

            // ACT
            AddCars(products, carsCategory);
            await unit.CommitAsync();

            // ASSERT
            Assert.AreEqual(12, products.All().Count());
        }
        
        IUnitOfWork GetUnitOfWork() => DIProvider.GetService<IUnitOfWork>();
    }
}
