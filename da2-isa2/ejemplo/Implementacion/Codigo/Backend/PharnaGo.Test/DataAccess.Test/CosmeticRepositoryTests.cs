using Microsoft.EntityFrameworkCore;
using PharmaGo.DataAccess;
using PharmaGo.DataAccess.Repositories;
using PharmaGo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PharmaGo.Test.DataAccess.Test
{
    [TestClass]
    public class CosmeticRepositoryTests
    {
        private PharmacyGoDbContext _context;
        private List<Cosmetic> _cosmeticsSaved;
        private CosmeticRepository _cosmeticRepository;
        private const int invalidId = 500;
        private Pharmacy _pharmacy;
        private Cosmetic _newCosmetic;
        private const string invalidCosmeticCode = "invalidCode";

        [TestInitialize]
        public void InitTest()
        {
            _cosmeticsSaved = new List<Cosmetic>();
            _pharmacy = new Pharmacy() { Id = 1, Name = "pharmacy", Address = "address", Users = new List<User>() };
            _newCosmetic = new Cosmetic()
            {
                Code = "newCosmeticCode",
                Name = "newCosmeticName",
                Description = "Anti-aging cream",
                Price = 99.99m,
                Pharmacy = _pharmacy
            };
        }

        [TestCleanup]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
        }

        private void CreateDataBase(string name)
        {
            _cosmeticsSaved = CreateDummyCosmetics();
            var options = new DbContextOptionsBuilder<PharmacyGoDbContext>().UseInMemoryDatabase(databaseName: name).Options;
            _context = new PharmacyGoDbContext(options);
            _cosmeticsSaved.ForEach(c => _context.Set<Cosmetic>().Add(c));
            _context.Set<Cosmetic>().Include(c => c.Pharmacy); 
            _context.SaveChanges();
            _cosmeticRepository = new CosmeticRepository(_context);
        }

        [TestMethod]
        public void GetAllCosmeticsOfPharmacyOk()
        {
            CreateDataBase("getCosmeticsTestDb");
            var retrievedCosmetics = _cosmeticRepository.GetAllByExpression(c => c.Pharmacy.Id == _pharmacy.Id);
            Assert.AreEqual(10, retrievedCosmetics.Count());
        }

        [TestMethod]
        public void InsertCosmeticOk()
        {
            CreateDataBase("insertCosmeticTestDb");
            _cosmeticRepository.InsertOne(_newCosmetic);
            _cosmeticRepository.Save();
            var retrievedCosmetics = _cosmeticRepository.GetAllByExpression(c => c.Pharmacy.Id == _pharmacy.Id);
            var retrievedCosmetic = retrievedCosmetics.FirstOrDefault(c => c.Code == _newCosmetic.Code);
            Assert.AreEqual(retrievedCosmetic.Code, _newCosmetic.Code);
        }

        private List<Cosmetic> CreateDummyCosmetics()
        {
            Pharmacy pharmacy = new Pharmacy() { Id = 1, Name = "pharmacy", Address = "address", Users = new List<User>() };
            var cosmeticList = new List<Cosmetic>();
            for (int i = 1; i < 11; i++)
            {
                cosmeticList.Add(new Cosmetic()
                {
                    Id = i,
                    Code = $"cosmeticCode{i}",
                    Name = $"cosmeticName{i}",
                    Description = $"cosmetic description {i}",
                    Price = 100,
                    Pharmacy = pharmacy
                });
            }
            return cosmeticList;
        }
    }
}
