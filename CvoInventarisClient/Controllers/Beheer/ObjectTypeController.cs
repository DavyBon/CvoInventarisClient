using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CvoInventarisClient.Models;
using CvoInventarisClient.ServiceReference;
namespace CvoInventarisClient.Controllers
{
    public class ObjectTypeController : Controller
    {
        public ActionResult Index()
        {
            return View(GetObjectTypes());
        }

        public ActionResult Edit(int id)
        {
            return View(ObjectTypeGetById(id));
        }

        [HttpPost]
        public ActionResult Edit(ObjectTypeModel ot)
        {
            if (UpdateObjectType(ot))
            {
                ViewBag.EditMesage = "Row updated";
                //return View("Index");
                return View();
            }
            else
            {
                ViewBag.EditMesage = "Row not updated";
                return View();
            }
        }

        public ActionResult Create()
        {
            return View(new ObjectTypeModel());
        }

        [HttpPost]
        public ActionResult Create(ObjectTypeModel ot)
        {
            if (InsertObjectType(ot) >= 0)
            {
                ViewBag.CreateMesage = "Row inserted";
                //return View("Index");
                return View();
            }
            else
            {
                ViewBag.CreateMesage = "Row not inserted";
                return View();
            }
        }

        public ActionResult Details(int id)
        {
            return View(ObjectTypeGetById(id));
        }

        public ActionResult Delete(int id)
        {
            return View(ObjectTypeGetById(id));
        }

        [HttpPost]
        public ActionResult Delete(ObjectTypeModel ot)
        {
            if (DeleteObjectType(ot))
            {
                ViewBag.DeleteMesage = "Row deleted";
                //return View("Index");
                return View();
            }
            else
            {
                ViewBag.DeleteMesage = "Row not deleted";
                return View();
            }
        }

        public List<ObjectTypeModel> GetObjectTypes()
        {
            CvoInventarisServiceClient service = new CvoInventarisServiceClient();
            ObjectTypes[] o = new ObjectTypes[] { };

            try
            {
                o = service.ObjectTypeGetAll();
            }
            catch (Exception)
            {

            }

            List<ObjectTypeModel> objectTypes = new List<ObjectTypeModel>();

            foreach (ObjectTypes objectType in o)
            {
                ObjectTypeModel ot = new ObjectTypeModel();
                ot.IdObjectType = objectType.Id;
                ot.Omschrijving = objectType.Omschrijving;
                objectTypes.Add(ot);
            }

            return objectTypes;
        }

        public ObjectTypeModel ObjectTypeGetById(int id)
        {
            CvoInventarisServiceClient service = new CvoInventarisServiceClient();
            ObjectTypes objectType = new ObjectTypes();

            try
            {
                objectType = service.ObjectTypeGetById(id);
            }
            catch (Exception)
            {

            }

            ObjectTypeModel ot = new ObjectTypeModel();
            ot.IdObjectType = objectType.Id;
            ot.Omschrijving = objectType.Omschrijving;
            return ot;
        }

        public bool UpdateObjectType(ObjectTypeModel objectType)
        {
            CvoInventarisServiceClient service = new CvoInventarisServiceClient();

            ObjectTypes ot = new ObjectTypes();
            ot.Id = objectType.IdObjectType;
            ot.Omschrijving = objectType.Omschrijving;

            try
            {
                return service.ObjectTypeUpdate(ot);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int InsertObjectType(ObjectTypeModel objectType)
        {
            CvoInventarisServiceClient service = new CvoInventarisServiceClient();

            ObjectTypes ot = new ObjectTypes();
            ot.Omschrijving = objectType.Omschrijving;

            try
            {
                return service.ObjectTypeCreate(ot);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public bool DeleteObjectType(ObjectTypeModel objectType)
        {
            CvoInventarisServiceClient service = new CvoInventarisServiceClient();

            int id = objectType.IdObjectType;

            try
            {
                return service.ObjectTypeDelete(id);
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}