using AutoMapper;
using Stone.BusinessEntities;
using Stone.BusinessServices.Config;
using Stone.Cipher;
using Stone.Common;
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
    public class TransactionService : IService
    {
        private readonly UnitOfWork _unitOfWork;

        public TransactionService()
        {
            _unitOfWork = new UnitOfWork();
        }

        public BusinessEntities.Entity GetById(int id)
        {
            var transaction = _unitOfWork.TransactionRepository.GetByID(id);
            if (transaction != null)
            {
                IMapper Mapper = AutoMapperConfig.MapperConfiguration.CreateMapper();
                var transactionModel = Mapper.Map<TransactionEntity>(transaction);

                return transactionModel;
            }
            return null;
        }

        private void SaveTransaction(int cardId, TransactionEntity.TransactionReturnEnum rtn, BusinessEntities.TransactionEntity entity)
        {
            entity.Card.Id = cardId;
            entity.TransactionDate = DateTime.Now;
            entity.TransactionReturn = rtn;
            entity.Id = this.Create(entity);
        }

        public TransactionEntity.TransactionReturnEnum DoTransaction(BusinessEntities.TransactionEntity entity)
        {
            CardService cardService = new CardService();
            CardEntity card = (CardEntity)cardService.GetByNumber(entity.Card.Number);

            if (card == null)
            {
                return TransactionEntity.TransactionReturnEnum.Cartao_invalido;
            }

            if (entity.Amount < Convert.ToDecimal(0.10))
            {
                SaveTransaction(card.Id, TransactionEntity.TransactionReturnEnum.Valor_invalido, entity);
                return TransactionEntity.TransactionReturnEnum.Valor_invalido;
            }

            string decriptedPass = StringCipher.Decrypt(entity.Card.Password);
            if (decriptedPass.Length < 4 || decriptedPass.Length > 6)
            {
                SaveTransaction(card.Id, TransactionEntity.TransactionReturnEnum.Erro_no_tamanho_da_senha, entity);
                return TransactionEntity.TransactionReturnEnum.Erro_no_tamanho_da_senha;
            }

            //Senha incorreta
            string cardRequestPass = StringCipher.Decrypt(card.Password);
            if (decriptedPass != cardRequestPass)
            {
                card.Attempts++;
                card.Password = cardRequestPass; //como o update espera o pass não criptado e ele vem criptado do banco eu preciso alterar o valor do atributo Password pra ele não criptado
                cardService.Update(card.Id, card);
                if (card.Attempts > 3)
                {
                    card.Blocked = true;
                    SaveTransaction(card.Id, TransactionEntity.TransactionReturnEnum.Cartao_bloqueado, entity);
                    return TransactionEntity.TransactionReturnEnum.Cartao_bloqueado;
                }
                SaveTransaction(card.Id, TransactionEntity.TransactionReturnEnum.Senha_invalida, entity);
                return TransactionEntity.TransactionReturnEnum.Senha_invalida;
            }

            if (card.ExpirationDate < DateTime.Now)
            {
                SaveTransaction(card.Id, TransactionEntity.TransactionReturnEnum.Cartao_vencido, entity);
                return TransactionEntity.TransactionReturnEnum.Cartao_vencido;
            }

            if (card.Blocked)
            {
                SaveTransaction(card.Id, TransactionEntity.TransactionReturnEnum.Cartao_bloqueado, entity);
                return TransactionEntity.TransactionReturnEnum.Cartao_bloqueado;
            }

            //Verificando limite e saldo
            if (TransactionTypeEntity.Types.Debito == (TransactionTypeEntity.Types)entity.TransactionType.Id)
            {
                if (card.Balance - entity.Amount < 0)
                {
                    SaveTransaction(card.Id, TransactionEntity.TransactionReturnEnum.Saldo_insuficiente, entity);
                    return TransactionEntity.TransactionReturnEnum.Saldo_insuficiente;
                }
            }
            else if (TransactionTypeEntity.Types.Credito == (TransactionTypeEntity.Types)entity.TransactionType.Id)
            {
                if (card.LimitUsed + entity.Amount > card.Limit)
                {
                    SaveTransaction(card.Id, TransactionEntity.TransactionReturnEnum.Limite_excedido, entity);
                    return TransactionEntity.TransactionReturnEnum.Limite_excedido;
                }
            }

            //Debitando os valores do cartão
            if(TransactionTypeEntity.Types.Debito == (TransactionTypeEntity.Types)entity.TransactionType.Id)
            {
                card.Balance = card.Balance - entity.Amount;
            }
            else if (TransactionTypeEntity.Types.Credito == (TransactionTypeEntity.Types)entity.TransactionType.Id)
            {
                card.LimitUsed = card.LimitUsed + entity.Amount;
            }
            card.Password = cardRequestPass; //como o update espera o pass não criptado e ele vem criptado do banco eu preciso alterar o valor do atributo Password pra ele não criptado
            cardService.Update(card.Id, card);

            SaveTransaction(card.Id, TransactionEntity.TransactionReturnEnum.Aprovado, entity);
            return TransactionEntity.TransactionReturnEnum.Aprovado;
        }

        public IEnumerable<BusinessEntities.Entity> GetAll()
        {
            var transactions = _unitOfWork.TransactionRepository.GetAll().ToList();
            if (transactions.Any())
            {
                IMapper Mapper = AutoMapperConfig.MapperConfiguration.CreateMapper();
                var transactionsModel = Mapper.Map<List<TransactionEntity>>(transactions);
                return transactionsModel;
            }
            return new List<BusinessEntities.Entity>();
        }

        public IEnumerable<BusinessEntities.Entity> GetAllByCardNumber(string cardNumber)
        {
            var transactions = _unitOfWork.TransactionRepository.GetManyQueryable(t => t.Card.Number == cardNumber).ToList();
            if (transactions.Any())
            {
                IMapper Mapper = AutoMapperConfig.MapperConfiguration.CreateMapper();
                var transactionsModel = Mapper.Map<List<TransactionEntity>>(transactions);
                return transactionsModel;
            }
            return new List<BusinessEntities.Entity>();
        }

        public int Create(BusinessEntities.Entity entity)
        {
            using (var scope = new TransactionScope())
            {
                var transaction = new DataModel.Transaction
                {
                    Number = ((TransactionEntity)entity).Number,
                    Amount = ((TransactionEntity)entity).Amount,
                    IdCard = ((TransactionEntity)entity).Card.Id,
                    IdTransactionType = ((TransactionEntity)entity).TransactionType.Id,
                    ClientCode = ((TransactionEntity)entity).ClientCode,
                    TransactionDate = ((TransactionEntity)entity).TransactionDate,
                    TransactionReturn = (int?)((TransactionEntity)entity).TransactionReturn
                };
                _unitOfWork.TransactionRepository.Insert(transaction);
                _unitOfWork.Save();
                scope.Complete();

                entity.Id = transaction.Id;

                return transaction.Id;
            }
        }

        public bool Update(int id, BusinessEntities.Entity entity)
        {
            var success = false;
            if (entity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var transaction = _unitOfWork.TransactionRepository.GetByID(id);
                    if (transaction != null)
                    {
                        transaction.Number = ((TransactionEntity)entity).Number;
                        transaction.Amount = ((TransactionEntity)entity).Amount;
                        transaction.IdCard = ((TransactionEntity)entity).Card.Id;
                        transaction.IdTransactionType = ((TransactionEntity)entity).TransactionType.Id;
                        transaction.ClientCode = ((TransactionEntity)entity).ClientCode;
                        transaction.TransactionDate = ((TransactionEntity)entity).TransactionDate;
                        transaction.TransactionReturn = (int?)((TransactionEntity)entity).TransactionReturn;
                        _unitOfWork.TransactionRepository.Update(transaction);
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
                    var transaction = _unitOfWork.TransactionRepository.GetByID(id);
                    if (transaction != null)
                    {

                        _unitOfWork.TransactionRepository.Delete(transaction);
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
