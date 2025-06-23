    using PharmaGo.Domain.Entities;
    using PharmaGo.Domain.SearchCriterias;
    using PharmaGo.Exceptions;
    using PharmaGo.IBusinessLogic;
    using PharmaGo.IDataAccess;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace PharmaGo.BusinessLogic
    {
        public class PharmacyManager : IPharmacyManager
        {
            private readonly IRepository<Pharmacy> _pharmacyRepository;
            private readonly IRepository<User> _userRepository;
            private readonly IRepository<Session> _sessionRepository;

           
            public PharmacyManager(IRepository<Pharmacy> pharmacyRepository, IRepository<User> userRepository, IRepository<Session> sessionRepository)
            {
                _pharmacyRepository = pharmacyRepository;
                _userRepository = userRepository;
                _sessionRepository = sessionRepository;
            }


            public IEnumerable<Pharmacy> GetAll(PharmacySearchCriteria pharmacySearchCriteria)
            {
                Pharmacy pharmacyToSearch = new Pharmacy 
                { 
                    Name = pharmacySearchCriteria.Name, 
                    Address = pharmacySearchCriteria.Address 
                };
                return _pharmacyRepository.GetAllByExpression(pharmacySearchCriteria.Criteria(pharmacyToSearch));
            }

            public Pharmacy GetByEmployee(string employeeToken)
            {
                var session = _sessionRepository.GetOneByExpression(s => s.Token.ToString() == employeeToken);

                var user = _userRepository.GetOneByExpression(u => u.Id == session.UserId);
                
                if(user == null)
                {
                    throw new ResourceNotFoundException("The user does not exist.");
                }

                return user.Pharmacy;
            }

            public Pharmacy GetById(int id)
            {
                Pharmacy pharmacySaved = _pharmacyRepository.GetOneByExpression(p => p.Id == id);
                if(pharmacySaved == null)
                {
                    throw new ResourceNotFoundException("The pharmacy does not exist.");
                }
                return pharmacySaved;
            }

            public Pharmacy Create(Pharmacy pharmacy)
            {
                if(pharmacy == null)
                {
                    throw new InvalidResourceException("The pharmacy to create is invalid.");
                }
                pharmacy.ValidOrFail();
                Pharmacy pharmacySaved = _pharmacyRepository.GetOneByExpression(p => p.Name == pharmacy.Name);
                if(pharmacySaved != null)
                {
                    throw new InvalidResourceException("The pharmacy already exist.");
                }
                _pharmacyRepository.InsertOne(pharmacy);
                _pharmacyRepository.Save();
                return pharmacy;
            }

            public Pharmacy Update(int id, Pharmacy updatedPharmacy)
            {
                if (updatedPharmacy == null)
                {
                    throw new InvalidResourceException("The updatedPharmacy is invalid.");
                }
                updatedPharmacy.ValidOrFail();
                Pharmacy pharmacySaved = _pharmacyRepository.GetOneByExpression(p => p.Id == id);
                if (pharmacySaved == null)
                {
                    throw new ResourceNotFoundException("The pharmacy to update does not exist.");
                }
                pharmacySaved.Name = updatedPharmacy.Name;
                pharmacySaved.Address = updatedPharmacy.Address;
                _pharmacyRepository.UpdateOne(pharmacySaved);
                _pharmacyRepository.Save();
                return pharmacySaved;
            }

            public void Delete(int id)
            {
                var pharmacySaved = _pharmacyRepository.GetOneByExpression(p => p.Id == id);
                if (pharmacySaved == null)
                {
                    throw new ResourceNotFoundException("The pharmacy to delete does not exist.");
                }
                _pharmacyRepository.DeleteOne(pharmacySaved);
                _pharmacyRepository.Save();
            }
        }
    }
