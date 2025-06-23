using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PharmaGo.BusinessLogic;
using PharmaGo.Domain.Entities;
using PharmaGo.Exceptions;
using PharmaGo.IDataAccess;
using System.Collections.Generic;

namespace PharmaGo.Test.BusinessLogic.Test
{
    [TestClass]
    public class CosmeticManagerTests
    {
        private Mock<IRepository<Cosmetic>> _cosmeticRepository;
        private Mock<IRepository<Pharmacy>> _pharmacyRepository;
        private Mock<IRepository<Session>> _sessionRepository;
        private Mock<IRepository<User>> _userRepository;

        private Cosmetic _cosmetic;
        private Pharmacy _pharmacy;
        private Session _session;
        private User _user;
        private CosmeticManager _cosmeticManager;
        private string _token = "c80da9ed-1b41-4768-8e34-b728cae25d2f";

        [TestInitialize]
        public void InitTest()
        {
            _cosmeticRepository = new Mock<IRepository<Cosmetic>>();
            _pharmacyRepository = new Mock<IRepository<Pharmacy>>();
            _sessionRepository = new Mock<IRepository<Session>>();
            _userRepository = new Mock<IRepository<User>>();

            _pharmacy = new Pharmacy() { Id = 1, Name = "pharmacy", Address = "address", Users = new List<User>() };
            _cosmetic = new Cosmetic() { Id = 1, Name = "cosmetic", Code = "12345", Description = "description", Price = 1, Pharmacy = _pharmacy };
            _session = new Session { Id = 1, Token = new Guid(_token), UserId = 1 };
            _user = new User() { Id = 1, UserName = "test", Email = "test@gmail.com", Address = "test" };

            _cosmeticManager = new CosmeticManager(_cosmeticRepository.Object, _pharmacyRepository.Object, _sessionRepository.Object, _userRepository.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _cosmeticRepository.VerifyAll();
        }

        [TestMethod]
        public void GetCosmeticsOk()
        {
            var expectedCosmetics = GenerateCosmeticList();
            _cosmeticRepository.Setup(r => r.GetAllByExpression(It.IsAny<Expression<Func<Cosmetic, bool>>>())).Returns(expectedCosmetics);
            _pharmacyRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Pharmacy, bool>>>())).Returns(_pharmacy);

            var cosmeticsReturned = _cosmeticManager.GetAll(_pharmacy.Id);

            Assert.AreEqual(expectedCosmetics.Count, cosmeticsReturned.Count());
        }
        
        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void GetCosmeticsOfInexistentPharmacy()
        {
            _pharmacyRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Pharmacy, bool>>>())).Returns((Pharmacy)null);

            var cosmeticsReturned = _cosmeticManager.GetAll(_pharmacy.Id);

            Assert.Fail("Expected ResourceNotFoundException not thrown.");
        }


        [TestMethod]
        public void CreateCosmeticOk()
        {
            _cosmeticRepository.Setup(r => r.Exists(It.IsAny<Expression<Func<Cosmetic, bool>>>())).Returns(false);
            _pharmacyRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Pharmacy, bool>>>())).Returns(_pharmacy);
            _sessionRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Session, bool>>>())).Returns(_session);
            _userRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<User, bool>>>())).Returns(_user);

            _cosmeticRepository.Setup(x => x.InsertOne(It.IsAny<Cosmetic>()));
            _cosmeticRepository.Setup(x => x.Save());

            var cosmeticReturned = _cosmeticManager.Create(_cosmetic, _token);
            
            Assert.IsNotNull(cosmeticReturned);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void CreateCosmeticWithExistentCode()
        {
            _cosmeticRepository.Setup(r => r.Exists(It.Is<Expression<Func<Cosmetic, bool>>>(c => 
                c.Compile()(_cosmetic)))).Returns(true);
            _pharmacyRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Pharmacy, bool>>>())).Returns(_pharmacy);
            _sessionRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Session, bool>>>())).Returns(_session);
            _userRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<User, bool>>>())).Returns(_user);

            _cosmeticManager.Create(_cosmetic, _token);
        }


        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void CreateCosmeticWithNonExistentPharmacy()
        {
            _pharmacyRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Pharmacy, bool>>>())).Returns((Pharmacy)null);

            _sessionRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Session, bool>>>())).Returns(_session);
            _userRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<User, bool>>>())).Returns(_user);

            _cosmeticManager.Create(_cosmetic, _token);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateCosmeticWithInvalidCode()
        {
            _cosmetic.Code = "";
            _cosmeticManager.Create(_cosmetic, _token);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateCosmeticWithInvalidName()
        {
            _cosmetic.Name = "VeryLongInvalidNameExceedingLimit";
            _cosmeticManager.Create(_cosmetic, _token);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateCosmeticWithInvalidDescription()
        {
            _cosmetic.Description = new string('a', 500);
            _cosmeticManager.Create(_cosmetic, _token);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateCosmeticWithInvalidPrice()
        {
            _cosmetic.Price = -1;
            _cosmeticManager.Create(_cosmetic, _token);
        }

        private List<Cosmetic> GenerateCosmeticList()
        {
            return new List<Cosmetic>
            {
                new Cosmetic { Id = 1, Name = "Cosmetic1", Code = "C123", Description = "Description1", Price = 10, Pharmacy = _pharmacy },
                new Cosmetic { Id = 2, Name = "Cosmetic2", Code = "C124", Description = "Description2", Price = 20, Pharmacy = _pharmacy }
            };
        }
    }
}
