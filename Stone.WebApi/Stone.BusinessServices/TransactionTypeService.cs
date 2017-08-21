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
    public class TransactionTypeService : IService
    {
        private readonly UnitOfWork _unitOfWork;

        public TransactionTypeService()
        {
            _unitOfWork = new UnitOfWork();
        }

        public BusinessEntities.Entity GetById(int id)
        {
            var type = _unitOfWork.TransactionTypeRepository.GetByID(id);
            if (type != null)
            {
                IMapper Mapper = AutoMapperConfig.MapperConfiguration.CreateMapper();
                var typeModel = Mapper.Map<TransactionTypeEntity>(type);

                return typeModel;
            }
            return null;
        }

        public IEnumerable<BusinessEntities.Entity> GetAll()
        {
            var types = _unitOfWork.TransactionTypeRepository.GetAll().ToList();
            if (types.Any())
            {
                IMapper Mapper = AutoMapperConfig.MapperConfiguration.CreateMapper();
                var typesModel = Mapper.Map<List<TransactionTypeEntity>>(types);
                return typesModel;
            }
            return new List<BusinessEntities.Entity>();
        }

        public int Create(BusinessEntities.Entity entity)
        {
            using (var scope = new TransactionScope())
            {
                var type = new TransactionType
                {
                    Name = ((TransactionTypeEntity)entity).Name,
                };
                _unitOfWork.TransactionTypeRepository.Insert(type);
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
                    var type = _unitOfWork.TransactionTypeRepository.GetByID(id);
                    if (type != null)
                    {
                        type.Name = ((TransactionTypeEntity)entity).Name;
                        _unitOfWork.TransactionTypeRepository.Update(type);
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
                    var type = _unitOfWork.TransactionTypeRepository.GetByID(id);
                    if (type != null)
                    {

                        _unitOfWork.TransactionTypeRepository.Delete(type);
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
