using Stone.BusinessEntities;
using Stone.BusinessServices;
using Stone.WebApi.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Stone.WebApi.Controllers
{
    [CaptureRequests]
    public class TransactionController : ApiController
    {
        TransactionService service = new TransactionService();

        // GET api/Transaction
        public ResultData<TransactionEntity> Get()
        {
            ResultData<TransactionEntity> result = new ResultData<TransactionEntity>();
            try
            {
                result.Data = service.GetAll().Cast<TransactionEntity>();
                result.Success = true;
            }
            catch
            {
                result.Data = null;
                result.Success = false;
                result.Msg = "Falha ao Recuperar Transações";
            }
            return result;
        }

        public ResultData<TransactionEntity> GetByClientCode(Guid Id)
        {
            ResultData<TransactionEntity> result = new ResultData<TransactionEntity>();
            try
            {
                result.Data = service.GetAll().Cast<TransactionEntity>().Where(r => r.ClientCode == Id);
                result.Success = true;
            }
            catch
            {
                result.Data = null;
                result.Success = false;
                result.Msg = "Falha ao Recuperar Transações";
            }
            return result;
        }

        // GET api/Transaction/5
        public ResultData<TransactionEntity> Get(int id)
        {
            ResultData<TransactionEntity> result = new ResultData<TransactionEntity>();
            try
            {
                result.Data = new List<TransactionEntity> { service.GetById(id) as TransactionEntity };
                result.Success = true;
            }
            catch
            {
                result.Data = null;
                result.Success = false;
                result.Msg = "Falha ao Recuperar Transação";
            }
            return result;
        }

        // POST api/Transaction
        public ResultData<TransactionEntity.TransactionReturnEnum> Post([FromBody]TransactionEntity value)
        {
            ResultData<TransactionEntity.TransactionReturnEnum> result = new ResultData<TransactionEntity.TransactionReturnEnum>();
            try
            {
                result.Data = new List<TransactionEntity.TransactionReturnEnum> { service.DoTransaction(value) };
                result.Success = true;
            }
            catch
            {
                result.Data = null;
                result.Success = false;
                result.Msg = "Oooopssss Servidor não está passando muito bem... Tente novamente :(";
            }
            return result;
        }

        // PUT api/Transaction/5
        public ResultData<TransactionEntity> Put(int id, [FromBody]TransactionEntity value)
        {
            ResultData<TransactionEntity> result = new ResultData<TransactionEntity>();
            try
            {
                service.Update(id, value);
                result.Success = true;
            }
            catch
            {
                result.Data = null;
                result.Success = false;
                result.Msg = "Falha ao Atualizar Transação";
            }
            return result;
        }

        // DELETE api/Transaction/5
        public ResultData<TransactionEntity> Delete(int id)
        {
            ResultData<TransactionEntity> result = new ResultData<TransactionEntity>();
            try
            {
                service.Delete(id);
                result.Success = true;
            }
            catch
            {
                result.Data = null;
                result.Success = false;
                result.Msg = "Falha ao Remover Transação";
            }
            return result;
        }
    }
}
