using PharmaGo.Domain.Entities;
using PharmaGo.Exceptions;
using PharmaGo.IBusinessLogic;
using PharmaGo.IDataAccess;

namespace PharmaGo.BusinessLogic
{
    public class CosmeticManager : ICosmeticManager
    {
        private readonly IRepository<Cosmetic> _cosmeticRepository;
        private readonly IRepository<Pharmacy> _pharmacyRepository;
        private readonly IRepository<Session> _sessionRepository;
        private readonly IRepository<User> _userRepository;

        public CosmeticManager(
            IRepository<Cosmetic> cosmeticRepo,
            IRepository<Pharmacy> pharmacyRepository,
            IRepository<Session> sessionRepository,
            IRepository<User> userRepository)
        {
            _cosmeticRepository = cosmeticRepo;
            _pharmacyRepository = pharmacyRepository;
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;
        }

        public Cosmetic Create(Cosmetic cosmetic, string token)
        {
            if (cosmetic == null)
            {
                throw new ResourceNotFoundException("Please create a cosmetic before inserting it.");
            }

            cosmetic.ValidateCosmetic();


            var guidToken = new Guid(token);
            var session = _sessionRepository.GetOneByExpression(s => s.Token == guidToken);
            var userId = session.UserId;
            User user = _userRepository.GetOneByExpression(u => u.Id == userId);
            Pharmacy pharmacyOfCosmetic = _pharmacyRepository.GetOneByExpression(p => p.Name == user.Pharmacy.Name);

            if (pharmacyOfCosmetic == null)
            {
                throw new ResourceNotFoundException("The pharmacy of the cosmetic does not exist.");
            }

            if (_cosmeticRepository.Exists(c => c.Code == cosmetic.Code && c.Pharmacy.Name == pharmacyOfCosmetic.Name))
            {
                throw new InvalidResourceException("The cosmetic already exists in that pharmacy.");
            }

            cosmetic.Pharmacy = pharmacyOfCosmetic;
            _cosmeticRepository.InsertOne(cosmetic);
            _cosmeticRepository.Save();
            return cosmetic;
        }

        public IEnumerable<Cosmetic> GetAll(int pharmacyId)
        {
            if (_pharmacyRepository.GetOneByExpression(p => p.Id == pharmacyId) == null)
            {
                throw new ResourceNotFoundException("The pharmacy does not exist.");
            }

            return _cosmeticRepository.GetAllByExpression(c => c.Pharmacy.Id == pharmacyId);
        }
    }
}