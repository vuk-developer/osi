﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OSI.Models;

namespace OSI.Controllers
{
    [Authorize]
    public class OSIController : Controller
    {
        OSIContext context = new OSIContext();
        // GET: OSIController
        public ActionResult Index()
        {
            return View();
        }

        // GET: OSIController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        [Route("/osi/coreid")]
        public ActionResult Login()
        {
            return Redirect("/Identity/Account/Login");
        }
        [Route("/osi/clan")]
        public ActionResult CreateClan()
        {
            return View();
        }
        [Route("/osi/clanske-karte")]
        public ActionResult ClanskeKarte()
        {
            Clanovi[] clanovi = context.Clanovi.ToArray();
            ViewBag.ClanoviTransport = clanovi;
            return View();
        }
        [Route("/osi/knjiga")]
        public ActionResult CreateKnjiga()
        {
            return View();
        }
        [Route("/osi/registar-clanova")]
        public ActionResult RegistarClanova()
        {
            Clanovi[] clanovi = context.Clanovi.ToArray();
            ViewBag.Clanovi = clanovi;
            return View();
        }
        [Route("/osi/nepostojeca-knjiga")]
        public ActionResult NepostojecaKnjiga()
        {
            return View();
        }

        [Route("/osi/knjige-lokator-form")]
        public ActionResult KnjigeLokatorForm()
        {
            List<Knjige> knjige = context.Knjige.ToList();
            ViewBag.Knjige = knjige;
            return View();
        }

        [Route("/osi/registar-knjiga")]
        public ActionResult RegistarKnjiga()
        {
            Knjige[] knjiges1 = context.Knjige.ToArray();
            Clanovi[] clanovis = context.Clanovi.ToArray();
            List<string> knjigeZaOdmrz = new List<string>();
            List<int> knjigeZaOdmrzInt = new List<int>();

            foreach (Clanovi clan in clanovis)
            {
                if (clan.Evidencija != null)
                {
                    string[] s = clan.Evidencija.Split(", ");
                    foreach (string st in s)
                    {
                        knjigeZaOdmrz.Add(st);
                    }
                }

            }

            foreach (string knjigeodmrz in knjigeZaOdmrz)
            {
                if (knjigeodmrz != null)
                {
                    int intermidiate = int.Parse(knjigeodmrz);
                    knjigeZaOdmrzInt.Add(intermidiate);
                }
            }

            foreach (Knjige knjigeodint in knjiges1)
            {
                if (knjigeodint != null)
                {
                    int i = (int)knjigeodint.Id;
                    if (knjigeodint.Status == "Nije na stanju")
                    {
                        if (knjigeZaOdmrzInt.Contains(i))
                        {

                        }
                        else
                        {
                            knjigeodint.Status = "Na stanju";
                        }
                    }
                }
                
            }
            context.SaveChanges();
            Knjige[] knjiges = context.Knjige.ToArray();
            ViewBag.Knjige = knjiges;
            return View();
        }
        [HttpPost("/napravi/knjiga")]
        public ActionResult NapraviK(Knjige knjiga)
        {
            Knjige knjigaToAdd = knjiga;
            DateTime lokalnoVreme = DateTime.Now;
            TimeZoneInfo cestZona = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            DateTime cestVreme = TimeZoneInfo.ConvertTime(lokalnoVreme, TimeZoneInfo.Local, cestZona);
            knjigaToAdd.DatumUnosa = cestVreme;
            context.Knjige.Add(knjigaToAdd);
            context.SaveChanges();
            return Redirect("/osi/registar-knjiga");
        }
        [HttpPost("/napravi/clan")]
        public ActionResult NapraviC(Clanovi clan)
        {
            context.Clanovi.Add(clan);
            context.SaveChanges();
            return Redirect("/osi/registar-clanova");
        }
        [HttpGet("/uredi-clana/{Id}")]
        public async Task<IActionResult> UrediClana(string Id)
        {
            Clanovi clan = await context.Clanovi.FindAsync(Id);
            List<Knjige> knjige = new List<Knjige>();
            if (clan.Evidencija != null)
            {
                string[] evidencijaArray = clan.Evidencija.Split(", ");
                foreach (string evidence in evidencijaArray)
                {
                    int i = int.Parse(evidence);
                    Knjige knjiga = await context.Knjige.FindAsync(i);
                    Console.WriteLine(knjiga.ImeKnjige);
                    knjige.Add(knjiga);
                }
            }
            ViewBag.KnjigeTransfer = knjige;
            ViewBag.ClanTransfer = clan;
            return View();
        }
        [HttpGet("/osi/obrisi-clana/{Id}")]
        public async Task<IActionResult> ObrisiClana(string Id)
        {
            Clanovi clan = await context.Clanovi.FindAsync(Id);
            context.Clanovi.Remove(clan);
            context.SaveChanges();
            return Redirect("/osi/registar-clanova");
        }
        [HttpGet("/osi/obrisi-knjigu/{Id}")]
        public async Task<IActionResult> ObrisiKnjigu(int Id)
        {
            Knjige knjiga = await context.Knjige.FindAsync(Id);
            context.Knjige.Remove(knjiga);
            context.SaveChanges();
            return Redirect("/osi/registar-knjiga");
        }
        [HttpGet("/uredi-knjigu/{Id}")]
        public async Task<IActionResult> UrediKnjigu(int Id)
        {
            Knjige knjiga = await context.Knjige.FindAsync(Id);
            ViewBag.KnjigaTransfer = knjiga;
            return View();
        }
        [HttpPost("/uredic")]
        public ActionResult UrediC(Clanovi clan)
        {
            try
            {
                List<string> knjigeUID = new List<string>();
                if (clan.Evidencija != null)
                {
                    List<string> knjigeUIDm = clan.Evidencija.Split(", ").ToList();
                    knjigeUID = knjigeUIDm;
                    List<int> knjigeUINT = new List<int>();
                    foreach (var item in knjigeUID)
                    {
                        int interm = int.Parse(item);
                        knjigeUINT.Add(interm);
                    }
                    foreach (var item in knjigeUINT)
                    {
                        Knjige knjiga = context.Knjige.Find(item);
                        if (knjiga == null)
                        {
                            return Redirect("/osi/nepostojeca-knjiga");
                        }
                    }
                    foreach (var item in knjigeUINT)
                    {
                        Knjige knjiga = context.Knjige.Find(item);
                        if (knjiga != null)
                        {
                            knjiga.Status = "Nije na stanju";
                        }
                    }
                }
                context.Clanovi.Update(clan);
                context.SaveChanges();
                return Redirect("/osi/registar-knjiga");
            }
            catch (Exception e)
            {
                return Redirect("/osi/nepostojeca-knjiga");
            }

        }
        [HttpPost("/uredik")]
        public ActionResult UrediK(Knjige knjiga)
        {
            Knjige knjigaToEdit = knjiga;
            DateTime lokalnoVreme = DateTime.Now;
            TimeZoneInfo cestZona = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            DateTime cestVreme = TimeZoneInfo.ConvertTime(lokalnoVreme, TimeZoneInfo.Local, cestZona);
            knjigaToEdit.DatumUnosa = cestVreme;
            context.Knjige.Update(knjiga);
            context.SaveChanges();
            return Redirect("/osi/registar-knjiga");
        }
        // POST: OSIController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OSIController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OSIController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OSIController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OSIController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
