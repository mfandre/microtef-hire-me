using AutoMapper;
using Stone.BusinessEntities;
using Stone.BusinessServices;
using Stone.Cipher;
using Stone.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Stone.WebApi.Controllers
{
    public class ClientController : Controller
    {
        private static MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CardEntity, CardEntityVM>().ReverseMap();
            });

        CardService service = new CardService();
        // GET: Client
        public ActionResult Index()
        {
            return View(service.GetAll().Cast<CardEntity>());
        }

        public ActionResult Edit(int Id)
        {
            IMapper Mapper = config.CreateMapper();
            CardEntity model = (CardEntity)service.GetById(Id);
            CardEntityVM modelVM = Mapper.Map<CardEntityVM>(model);

            modelVM.CardBrandId = model.CardBrand.Id;
            modelVM.CardTypeId = model.CardType.Id;
            modelVM.Password = StringCipher.Decrypt(model.Password);
            return View(modelVM);
        }

        public ActionResult Remove(int Id)
        {
            if (service.Delete(Id))
            {
                TempData["Success"] = "Operação realizada com sucesso!";
            }
            else
            {
                TempData["Error"] = "Ooopssss!";
            }
            return RedirectToAction("Index","Client");
        }

        [HttpPost]
        public ActionResult Edit(CardEntityVM data)
        {
            CardBrandService cardBrandService = new CardBrandService();
            data.CardBrand = (CardBrandEntity)cardBrandService.GetAll().Where(b => b.Id == data.CardBrandId).SingleOrDefault();

            CardTypeService cardTypeService = new CardTypeService();
            data.CardType = (CardTypeEntity)cardTypeService.GetAll().Where(b => b.Id == data.CardTypeId).SingleOrDefault();

            ModelState["CardBrand"].Errors.Clear();
            ModelState["CardType"].Errors.Clear();

            if (ModelState.IsValid)
            {
                if (data.Id == 0)
                    service.Create(data);
                else
                    service.Update(data.Id, data);
                TempData["Success"] = "Operação realizada com sucesso!";
            }
            return View(data);
        }

        public ActionResult Create()
        {
            CardEntityVM cardVM = new CardEntityVM();
            cardVM.ExpirationDate = DateTime.Now;
            return View(cardVM);
        }

        [HttpPost]
        public ActionResult Create(CardEntityVM data)
        {
            CardBrandService cardBrandService = new CardBrandService();
            data.CardBrand = (CardBrandEntity)cardBrandService.GetAll().Where(b => b.Id == data.CardBrandId).SingleOrDefault();

            CardTypeService cardTypeService = new CardTypeService();
            data.CardType = (CardTypeEntity)cardTypeService.GetAll().Where(b => b.Id == data.CardTypeId).SingleOrDefault();

            ModelState["CardBrand"].Errors.Clear();
            ModelState["CardType"].Errors.Clear();

            if (ModelState.IsValid)
            {
                if (data.Id == 0)
                    service.Create(data);
                else
                    service.Update(data.Id, data);
                TempData["Success"] = "Operação realizada com sucesso!";
            }
            return View(data);
        }

        public ActionResult GetBrands()
        {
            CardBrandService brandService = new CardBrandService();
            return Json(brandService.GetAll(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTypes()
        {
            CardTypeService cardTypeService = new CardTypeService();
            return Json(cardTypeService.GetAll(), JsonRequestBehavior.AllowGet);
        }
    }
}