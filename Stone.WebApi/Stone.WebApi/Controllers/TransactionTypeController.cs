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
    public class TransactionTypeController : ApiController
    {
        TransactionTypeService service = new TransactionTypeService();

        // GET api/TransactionType
        public ResultData<TransactionTypeEntity> Get()
        {
            ResultData<TransactionTypeEntity> result = new ResultData<TransactionTypeEntity>();
            try
            {
                result.Data = service.GetAll().Cast<TransactionTypeEntity>();
                result.Success = true;
            }
            catch
            {
                result.Data = null;
                result.Success = false;
                result.Msg = "Falha ao Recuperar Tipos de Transações";
            }
            return result;
        }

        // GET api/TransactionType/5
        public ResultData<TransactionTypeEntity> Get(int id)
        {
            ResultData<TransactionTypeEntity> result = new ResultData<TransactionTypeEntity>();
            try
            {
                result.Data = new List<TransactionTypeEntity> { service.GetById(id) as TransactionTypeEntity };
                result.Success = true;
            }
            catch
            {
                result.Data = null;
                result.Success = false;
                result.Msg = "Falha ao Recuperar Tipo Transação";
            }
            return result;
        }

        // POST api/TransactionType
        public ResultData<TransactionTypeEntity> Post([FromBody]TransactionTypeEntity value)
        {
            ResultData<TransactionTypeEntity> result = new ResultData<TransactionTypeEntity>();
            try
            {
                service.Create(value);
                result.Success = true;
            }
            catch
            {
                result.Data = null;
                result.Success = false;
                result.Msg = "Falha ao Inserir Tipo Transação";
            }
            return result;
        }

        // PUT api/TransactionType/5
        public ResultData<TransactionTypeEntity> Put(int id, [FromBody]TransactionTypeEntity value)
        {
            ResultData<TransactionTypeEntity> result = new ResultData<TransactionTypeEntity>();
            try
            {
                service.Update(id, value);
                result.Success = true;
            }
            catch
            {
                result.Data = null;
                result.Success = false;
                result.Msg = "Falha ao Atualizar Tipo Transação";
            }
            return result;
        }

        // DELETE api/TransactionType/5
        public ResultData<TransactionTypeEntity> Delete(int id)
        {
            ResultData<TransactionTypeEntity> result = new ResultData<TransactionTypeEntity>();
            try
            {
                service.Delete(id);
                result.Success = true;
            }
            catch
            {
                result.Data = null;
                result.Success = false;
                result.Msg = "Falha ao Remover Tipo Transação";
            }
            return result;
        }
    }
}
