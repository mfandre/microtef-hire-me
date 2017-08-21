using AutoMapper;
using Stone.BusinessEntities;
using Stone.BusinessServices.Config;
using Stone.DataModel;
using Stone.DataModel.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Stone.BusinessServices
{
    public class CardTypeService : IService
    {
        private readonly UnitOfWork _unitOfWork;

        public CardTypeService()
        {
            _unitOfWork = new UnitOfWork();
        }

        public BusinessEntities.Entity GetById(int id)
        {
            var type = _unitOfWork.CardTypeRepository.GetByID(id);
            if (type != null)
            {
                IMapper Mapper = AutoMapperConfig.MapperConfiguration.CreateMapper();
                var typeModel = Mapper.Map<CardTypeEntity>(type);

                return typeModel;
            }
            return null;
        }

        public IEnumerable<BusinessEntities.Entity> GetAll()
        {
            var types = _unitOfWork.CardTypeRepository.GetAll().ToList();
            if (types.Any())
            {
                IMapper Mapper = AutoMapperConfig.MapperConfiguration.CreateMapper();
                var typesModel = Mapper.Map<List<CardTypeEntity>>(types);
                return typesModel;
            }
            return new List<BusinessEntities.Entity>();
        }

        public int Create(BusinessEntities.Entity entity)
        {
            using (var scope = new TransactionScope())
            {
                var type = new CardType
                {
                    Name = ((CardTypeEntity)entity).Name,
                };
                _unitOfWork.CardTypeRepository.Insert(type);
                _unitOfWork.Save();
                scope.Complete();

                entity.Id = type.Id;

                return type.Id;
            }
        }

        public bool Update(int id, BusinessEntities.Entity entity)
        {
            var success = false;
            if (entity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var type = _unitOfWork.CardTypeRepository.GetByID(id);
                    if (type != null)
                    {
                        type.Name = ((CardTypeEntity)entity).Name;
                        _unitOfWork.CardTypeRepository.Update(type);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        public bool Delete(int id)
        {
            var success = false;
            if (id > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var type = _unitOfWork.CardTypeRepository.GetByID(id);
                    if (type != null)
                    {

                        _unitOfWork.CardTypeRepository.Delete(type);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }
    }
}
