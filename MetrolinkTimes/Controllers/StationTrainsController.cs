using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MetrolinkTimes.Models;
using System.Text;
using System.IO;
using Newtonsoft.Json.Linq;

namespace MetrolinkTimes.Controllers
{
    public class StationTrainsController : ApiController
    {
        //
        private static Dictionary<string,List<CompleteData>> map = null;
        //

        private static long TIME = 9000000000;
        private MetrolinkTimesContext db = new MetrolinkTimesContext();

        // GET: api/StationTrains
        public IQueryable<CompleteData> GetStationTrains()
        {
            //return db.StationTrains.Include(s => s.station).Include(s=> s.train);
            //
            try
            {
                map.Add("ALL", toCompleteData(db));
                map.Add("ALL", toCompleteData(db));
            }
            catch (Exception)
            {
                return map["ALL"].AsQueryable();
            }
            //
            return map["ALL"].AsQueryable();

            //thisworks     return toCompleteData(db).AsQueryable();
        }

        // GET: api/StationTrains
        public IQueryable<CompleteData> GetStationTrains(int id)
        {
            //return db.StationTrains.Include(s => s.station).Include(s=> s.train);
            //
            try
            {
                map.Add("one", toCompleteData(db, id));
                map.Add("one", toCompleteData(db, id));
            }
            catch (Exception)
            {
                return map["one"].AsQueryable();
            }
            //
            return map["one"].AsQueryable();


            //thisworks     return toCompleteData(db, id).AsQueryable();
        }

        /*// GET: api/StationTrains/5
        [ResponseType(typeof(StationTrain))]
        public async Task<IHttpActionResult> GetStationTrain(int id)
        {
            StationTrain stationTrain = await db.StationTrains.FindAsync(id);
            if (stationTrain == null)
            {
                return NotFound();
            }

            return Ok(stationTrain);
        }*/

        // PUT: api/StationTrains/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutStationTrain(int id, StationTrain stationTrain)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != stationTrain.Id)
            {
                return BadRequest();
            }

            db.Entry(stationTrain).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StationTrainExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /*// POST: api/StationTrains
        [ResponseType(typeof(StationTrain))]
        public async Task<IHttpActionResult> PostStationTrain(UpdateData updateData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            StationTrain stationTrain = new StationTrain { time = DateTime.Parse(updateData.time), station = db.Stations.First(i => i.Name == updateData.name), train = findClosest(updateData, db.StationTrains.Where(s=> s.station.Name == updateData.name).ToList())};
            if(stationTrain.train.train_id == 0||stationTrain.train == null)
            { return BadRequest(ModelState); }
            db.StationTrains.Add(stationTrain);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = stationTrain.Id }, stationTrain);
        }*/

        // POST: api/StationTrains
        public async Task<string> PostRegistration([FromBody] string fcm_id)
        {
            RegistrationData rData = new RegistrationData { fcm_id = fcm_id };
            if (System.Diagnostics.Debugger.IsAttached == false)
            {
                System.Diagnostics.Debugger.Launch();
            }
            WebRequest request = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            request.Method = "post";
            request.ContentType = "application/json";
            var notification = new
            {
                to = rData.fcm_id
            };
            request.Headers.Add(string.Format("Authorization: key={0}", Properties.Resources.serverKey));
            byte[] array = Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(notification));
            request.GetRequestStream().Write(array, 0, array.Length);
            WebResponse response = request.GetResponse();
            String s = new StreamReader(response.GetResponseStream()).ReadToEnd();
            JObject obj = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(s);
            if ((int)obj["failure"] == 0) {
            db.Users.Add(new User() { fcm_id = rData.fcm_id });
            await db.SaveChangesAsync(); }
            return s;
        }

        // DELETE: api/StationTrains/5
        [ResponseType(typeof(StationTrain))]
        public async Task<IHttpActionResult> DeleteStationTrain(int id)
        {
            StationTrain stationTrain = await db.StationTrains.FindAsync(id);
            if (stationTrain == null)
            {
                return NotFound();
            }

            db.StationTrains.Remove(stationTrain);
            await db.SaveChangesAsync();

            return Ok(stationTrain);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StationTrainExists(int id)
        {
            return db.StationTrains.Count(e => e.Id == id) > 0;
        }

        private List<List<TempData>> order(IQueryable<StationTrain> set)
        {
            List<List<TempData>> list = new List<List<TempData>>();
            List<IGrouping<string, StationTrain>> collection = set.GroupBy(s => s.station.Name).ToList();
            foreach (IGrouping<string, StationTrain> item in collection)
            {
                List<TempData> l = new List<TempData>();
                foreach (StationTrain s in item.ToList())
                    l.Add(new TempData(s));
                list.Add(l);
            }
            return list;
        }

        private List<CompleteData> toCompleteData(MetrolinkTimesContext m)
        {
            List<CompleteData> l = new List<CompleteData>();
            List<List<TempData>> list = order(m.StationTrains.Include(s => s.station).Include(s => s.train));
            foreach (List<TempData> l1 in list)
                l.Add(new CompleteData(l1));
            return l;
        }

        private List<CompleteData> toCompleteData(MetrolinkTimesContext m, int id)
        {
            List<CompleteData> l = new List<CompleteData>();
            List<List<TempData>> list = order(m.StationTrains.Include(s => s.station).Include(s => s.train).Where(i => i.station.Line == id));
            foreach (List<TempData> l1 in list)
                l.Add(new CompleteData(l1));
            return l;
        }

        private Train findClosest(UpdateData d, List<StationTrain> list)
        {
            Train t = new Train();
            DateTime date = DateTime.Parse(d.time);
            list = list.OrderBy(s => s.time).ToList();
            foreach (StationTrain s in list)
                if (date.CompareTo(s.time) >= 0&&date.Ticks-s.time.Ticks<=TIME) {
                    t = s.train;
                    break;
                }          
            return t;
        }
    }
}