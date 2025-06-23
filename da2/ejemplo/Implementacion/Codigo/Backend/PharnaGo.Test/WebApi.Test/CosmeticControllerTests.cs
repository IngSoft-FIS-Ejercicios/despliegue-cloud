using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PharmaGo.Domain.Entities;
using PharmaGo.IBusinessLogic;
using PharmaGo.WebApi.Controllers;
using PharmaGo.WebApi.Models.In;
using PharmaGo.WebApi.Models.Out;

namespace PharmaGo.Test.WebApi.Test
{
    [TestClass]
    public class CosmeticControllerTests
    {
        private Mock<ICosmeticManager> _cosmeticManagerMock;
        private CosmeticController _controller;

        [TestInitialize]
        public void SetupController()
        {
            _cosmeticManagerMock = new Mock<ICosmeticManager>(MockBehavior.Strict);
            _controller = new CosmeticController(_cosmeticManagerMock.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        [TestMethod]
        public void PostCosmeticOk()
        {
            var model = CreateTestCosmeticRequestModel();
            var expectedCosmetic = model.ToEntity();
            
            _cosmeticManagerMock.Setup(m => m.Create(It.IsAny<Cosmetic>(), It.IsAny<string>())).Returns(expectedCosmetic);

            var result = _controller.Create(model);
            var objectResult = result as ObjectResult;
            
            _cosmeticManagerMock.VerifyAll();
            Assert.AreEqual(200, objectResult?.StatusCode);
        }

        [TestMethod]
        public void Test_Cosmetic_Model()
        {
            var model = CreateTestCosmeticRequestModel();
            var expectedCosmetic = model.ToEntity();

            Assert.AreEqual(model.Name, expectedCosmetic.Name);
            Assert.AreEqual(model.Description, expectedCosmetic.Description);
            Assert.AreEqual(model.Price, expectedCosmetic.Price);
            Assert.AreEqual(model.Code, expectedCosmetic.Code);
            Assert.AreEqual(model.PharmacyName, expectedCosmetic.Pharmacy.Name);
        }

        [TestMethod]
        public void Test_Cosmetic_Model_Response()
        {
            var cosmetic = CreateTestCosmetic();
            var model = new CosmeticResponseModel(cosmetic);

            Assert.AreEqual(model.Name, cosmetic.Name);
            Assert.AreEqual(model.Description, cosmetic.Description);
            Assert.AreEqual(model.Price, cosmetic.Price);
            Assert.AreEqual(model.Code, cosmetic.Code);
            Assert.AreEqual(model.PharmacyName, cosmetic.Pharmacy.Name);
        }

        [TestMethod]
        public void GetCosmeticsOk()
        {
            int pharmacyId = 1;
            var expectedCosmetics = GenerateCosmeticList();
            _cosmeticManagerMock.Setup(x => x.GetAll(pharmacyId)).Returns(expectedCosmetics);

            var result = _controller.GetAll(pharmacyId);
            var objectResult = result as ObjectResult;

            _cosmeticManagerMock.VerifyAll();
            Assert.AreEqual(200, objectResult?.StatusCode);
            Assert.IsNotNull(objectResult?.Value);

            var cosmetics = objectResult.Value as CosmeticResponseModel[];
            Assert.IsNotNull(cosmetics);
            Assert.AreEqual(expectedCosmetics.Count, cosmetics.Length);

            for (int i = 0; i < cosmetics.Length; i++)
            {
                Assert.AreEqual(expectedCosmetics[i].Id, cosmetics[i].Id);
                Assert.AreEqual(expectedCosmetics[i].Name, cosmetics[i].Name);
                Assert.AreEqual(expectedCosmetics[i].Price, cosmetics[i].Price);
            }
        }


        private CosmeticRequestModel CreateTestCosmeticRequestModel()
        {
            return new CosmeticRequestModel
            {
                Name = "Test",
                Description = "Test",
                Price = 1,
                Code = "hols123",
                PharmacyName = "Test"
            };
        }

        private Cosmetic CreateTestCosmetic()
        {
            return new Cosmetic
            {
                Name = "Test",
                Description = "Test",
                Price = 1,
                Code = "hols123",
                Pharmacy = new Pharmacy { Name = "Test" }
            };
        }

        private List<Cosmetic> GenerateCosmeticList()
        {
            return new List<Cosmetic>
            {
                new Cosmetic { Name = "Cosmetic1", Description = "Description1", Price = 10, Code = "C123", Pharmacy = new Pharmacy { Id = 1, Name = "Pharmacy1" } },
                new Cosmetic { Name = "Cosmetic2", Description = "Description2", Price = 15, Code = "C124", Pharmacy = new Pharmacy { Id = 1, Name = "Pharmacy1" } }
            };
        }
    }
}
