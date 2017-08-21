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
    public class CardController : ApiController
    {
        CardService service = new CardService();

        // GET api/card
        public ResultData<CardEntity> Get()
        {
            ResultData<CardEntity> result = new ResultData<CardEntity>();
            try
            {
                result.Data = service.GetAll().Cast<CardEntity>();
                result.Success = true;
            }
            catch
            {
                result.Data = null;
                result.Success = false;
                result.Msg = "Falha ao Recuperar Cartões";
            }
            return result;
        }

        // GET api/card/5
        public ResultData<CardEntity> Get(int id)
        {
            ResultData<CardEntity> result = new ResultData<CardEntity>();
            try
            {
                result.Data = new List<CardEntity> { service.GetById(id) as CardEntity };
                result.Success = true;
            }
            catch
            {
                result.Data = null;
                result.Success = false;
                result.Msg = "Falha ao Recuperar Cartão";
            }
            return result;
        }

        // POST api/values
        public ResultData<CardEntity> Post([FromBody]CardEntity value)
        {
            ResultData<CardEntity> result = new ResultData<CardEntity>();
            try
            {
                service.Create(value);
                result.Success = true;
            }
            catch
            {
                result.Data = null;
                result.Success = false;
                result.Msg = "Falha ao Inserir Cartão";
            }
            return result;
        }

        // PUT api/values/5
        public ResultData<CardEntity> Put(int id, [FromBody]CardEntity value)
        {
            ResultData<CardEntity> result = new ResultData<CardEntity>();
            try
            {
                service.Update(id, value);
                result.Success = true;
            }
            catch
            {
                result.Data = null;
                result.Success = false;
                result.Msg = "Falha ao Atualizar Cartão";
            }
            return result;
        }

        // DELETE api/values/5
        public ResultData<CardEntity> Delete(int id)
        {
            ResultData<CardEntity> result = new ResultData<CardEntity>();
            try
            {
                service.Delete(id);
                result.Success = true;
            }
            catch
            {
                result.Data = null;
                result.Success = false;
                result.Msg = "Falha ao Remover Cartão";
            }
            return result;
        }
    }
}
