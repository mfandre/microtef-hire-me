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
    public class CardService : IService
    {
        private readonly UnitOfWork _unitOfWork;

        public CardService()
        {
            _unitOfWork = new UnitOfWork();
        }

        public BusinessEntities.Entity GetById(int id)
        {
            var card = _unitOfWork.CardRepository.GetByID(id);
            if (card != null)
            {
                IMapper Mapper = AutoMapperConfig.MapperConfiguration.CreateMapper();
                var cardModel = Mapper.Map<CardEntity>(card);

                return cardModel;
            }
            return null;
        }

        public BusinessEntities.Entity GetByNumber(string number)
        {
            try
            {
                var card = _unitOfWork.CardRepository.GetManyQueryable(a => a.Number == number).SingleOrDefault();
                if (card != null)
                {
                    IMapper Mapper = AutoMapperConfig.MapperConfiguration.CreateMapper();
                    var cardModel = Mapper.Map<CardEntity>(card);
                    return cardModel;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<BusinessEntities.Entity> GetAll()
        {
            var cards = _unitOfWork.CardRepository.GetAll().ToList();
            if (cards.Any())
            {
                IMapper Mapper = AutoMapperConfig.MapperConfiguration.CreateMapper();
                var cardsModel = Mapper.Map<List<CardEntity>>(cards);
                return cardsModel;
            }
            return new List<BusinessEntities.Entity>();
        }

        public int Create(BusinessEntities.Entity entity)
        {
            using (var scope = new TransactionScope())
            {
                var card = new Card
                {
                    Number = ((CardEntity)entity).Number,
                    Password = Stone.Cipher.StringCipher.Encrypt(((CardEntity)entity).Password.Trim()),
                    CardholderName = ((CardEntity)entity).CardholderName,
                    ExpirationDate = ((CardEntity)entity).ExpirationDate,
                    IdCardType = ((CardEntity)entity).CardType.Id,
                    IdCardBrand = ((CardEntity)entity).CardBrand.Id,
                    HasPassword = ((CardEntity)entity).CardType.Id == 2 ? true : false,
                    Balance = ((CardEntity)entity).Balance,
                    Blocked = ((CardEntity)entity).Blocked,
                    Limit = ((CardEntity)entity).Limit,
                    LimitUsed = ((CardEntity)entity).LimitUsed,
                    Attempts = ((CardEntity)entity).Attempts
                };
                _unitOfWork.CardRepository.Insert(card);
                _unitOfWork.Save();
                scope.Complete();

                entity.Id = card.Id;

                return card.Id;
            }
        }

        public bool Update(int id, BusinessEntities.Entity entity)
        {
            var success = false;
            if (entity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var card = _unitOfWork.CardRepository.GetByID(id);
                    if (card != null)
                    {
                        card.Number = ((CardEntity)entity).Number;
                        card.Password = Stone.Cipher.StringCipher.Encrypt(((CardEntity)entity).Password.Trim());
                        card.CardholderName = ((CardEntity)entity).CardholderName;
                        card.ExpirationDate = ((CardEntity)entity).ExpirationDate;
                        card.IdCardType = ((CardEntity)entity).CardType != null ? ((CardEntity)entity).CardType.Id : 0;
                        card.IdCardBrand = ((CardEntity)entity).CardBrand != null ? ((CardEntity)entity).CardBrand.Id : 0;
                        card.HasPassword = ((CardEntity)entity).CardType != null ? (((CardEntity)entity).CardType.Id == 2 ? true : false) : false;
                        card.Balance = ((CardEntity)entity).Balance;
                        card.Blocked = ((CardEntity)entity).Blocked;
                        card.Limit = ((CardEntity)entity).Limit;
                        card.LimitUsed = ((CardEntity)entity).LimitUsed;
                        card.Attempts = ((CardEntity)entity).Attempts;
                        _unitOfWork.CardRepository.Update(card);
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
                    var card = _unitOfWork.CardRepository.GetByID(id);
                    if (card != null)
                    {

                        _unitOfWork.CardRepository.Delete(card);
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
