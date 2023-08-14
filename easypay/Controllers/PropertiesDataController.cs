using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
//using Monopoly.Models;
using System.Diagnostics;
using easypay.Models.ViewModels;
using easypay.Models;

namespace Monopoly.Controllers
{
    public class PropertyDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/PropertyData/ListProperty
        [HttpGet]
        public IEnumerable<PropertyDto> ListProperties()
        {
            List<Property> Properties = db.Properties.ToList();
            List<PropertyDto> PropertyDtos = new List<PropertyDto>();

            Properties.ForEach(a => PropertyDtos.Add(new PropertyDto()
            {
                PropertyID = a.PropertyID,
                PropertyName = a.PropertyName,
                PropertyRent = a.PropertyRent,
                PropertyPrice = a.PropertyPrice,
                PlayerName = a.Player.PlayerName
            }));
            return PropertyDtos;
        }

        // GET: api/PropertyData/FindProperty5
        [ResponseType(typeof(Property))]
        [HttpGet]
        public IHttpActionResult FindProperty(int id)
        {
            Property Property = db.Properties.Find(id);
            PropertyDto PropertyDto = new PropertyDto()
            {
                PropertyID = Property.PropertyID,
                PropertyName = Property.PropertyName,
                PropertyRent = Property.PropertyRent,
                PropertyPrice = Property.PropertyPrice,
                PlayerName = Property.Player.PlayerName
            };
            if (Property == null)
            {
                return NotFound();
            }

            return Ok(PropertyDto);
        }

        // POST: api/PropertyData/UpdateProperty/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateProperty(int id, Property property)
        {
            Debug.WriteLine("I have reached the update property");
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Model State is invalid");
                return BadRequest(ModelState);
            }

            if (id != property.PropertyID)
            {
                Debug.WriteLine("ID mismatch");
                Debug.WriteLine("GET parameter" + id);
                Debug.WriteLine("POST parameter" + property.PropertyID);
                Debug.WriteLine("POST parameter" + property.PropertyName);
                Debug.WriteLine("POST parameter" + property.PropertyRent);
                Debug.WriteLine("POST parameter" + property.PropertyPrice);

                return BadRequest();
            }

            db.Entry(property).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PropertyExists(id))
                {
                    Debug.WriteLine("Property not found");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            Debug.WriteLine("None of the conditions triggered");
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/PropertyData/AddProperty
        [ResponseType(typeof(Property))]
        [HttpPost]
        public IHttpActionResult AddProperty(Property property)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Properties.Add(property);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = property.PropertyID }, property);
        }

        // POST: api/PropertyData/DeleteProperty/5
        [ResponseType(typeof(Property))]
        [HttpPost]
        public IHttpActionResult DeleteProperty(int id)
        {
            Property property = db.Properties.Find(id);
            if (property == null)
            {
                return NotFound();
            }

            db.Properties.Remove(property);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PropertyExists(int id)
        {
            return db.Properties.Count(e => e.PropertyID == id) > 0;
        }
    }
}
