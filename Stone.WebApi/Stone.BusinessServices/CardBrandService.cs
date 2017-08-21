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
    public class CardBrandService : IService
    {
        private readonly UnitOfWork _unitOfWork;

        public CardBrandService()
        {
            _unitOfWork = new UnitOfWork();
        }

        public BusinessEntities.Entity GetById(int id)
        {
            var brand = _unitOfWork.CardBrandRepository.GetByID(id);
            if (brand != null)
            {
                IMapper Mapper = AutoMapperConfig.MapperConfiguration.CreateMapper();
                var brandModel = Mapper.Map<CardBrandEntity>(brand);

                return brandModel;
            }
            return null;
        }

        public IEnumerable<BusinessEntities.Entity> GetAll()
        {
            var brands = _unitOfWork.CardBrandRepository.GetAll().ToList();
            if (brands.Any())
            {
                IMapper Mapper = AutoMapperConfig.MapperConfiguration.CreateMapper();
                var brandsModel = Mapper.Map<List<CardBrandEntity>>(brands);
                return brandsModel;
            }
            return new List<BusinessEntities.Entity>();
        }

        public int Create(BusinessEntities.Entity entity)
        {
            using (var scope = new TransactionScope())
            {
                var type = new CardBrand
                {
                    Name = ((CardBrandEntity)entity).Name,
                };
                _unitOfWork.CardBrandRepository.Insert(type);
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
                    var brand = _unitOfWork.CardBrandRepository.GetByID(id);
                    if (brand != null)
                    {
                        brand.Name = ((CardBrandEntity)entity).Name;
                        _unitOfWork.CardBrandRepository.Update(brand);
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
                    var brand = _unitOfWork.CardBrandRepository.GetByID(id);
                    if (brand != null)
                    {

                        _unitOfWork.CardBrandRepository.Delete(brand);
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
