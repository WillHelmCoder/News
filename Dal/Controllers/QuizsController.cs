using Dal.AuthControllers;
using Dal.Models;
using Dal.Repositories;
using Dal.Services;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace News.Controllers
{
    public class QuizsController : BasicController
    {
        private EFDataBase db = new EFDataBase();
        private readonly QuizService QuizService;

        public QuizsController(QuizService quizService)
        {
            QuizService = quizService;
        }
        // GET: Quizs
        public ActionResult Index()
        {
            try
            {
                if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
                int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

                return View(db.QuizesList.Where(x => !x.IsDeleted).Where(x => x.MagazineId == magId).ToList());
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // GET: Quizs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quiz quiz = db.QuizesList.Find(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            return View(quiz);
        }

        // GET: Quizs/Create
        public ActionResult Create()
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }

            return View();
        }

        // POST: Quizs/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "QuizId,Title,Description")] Quiz quiz)
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            quiz.CreationDate = DateTime.Now;
            quiz.MagazineId = magId;

            if (ModelState.IsValid)
            {
                db.QuizesList.Add(quiz);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(quiz);
        }

        // GET: Quizs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quiz quiz = db.QuizesList.Find(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            return View(quiz);
        }

        // POST: Quizs/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "QuizId,Title,Description")] Quiz quiz)
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            quiz.CreationDate = DateTime.Now;
            quiz.MagazineId = magId;

            if (ModelState.IsValid)
            {
                db.Entry(quiz).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(quiz);
        }

        // GET: Quizs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quiz quiz = db.QuizesList.Find(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            return View(quiz);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Quiz quiz = db.QuizesList.Find(id);

            quiz.IsDeleted = true;

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult QuestionDetails(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var question = QuizService.GetQuestionById(id);
            var questions = QuizService.QuestionListByQuizId(question.QuizId);
            var model = new QuestionIndexViewModel()
            {
                QuestionId = id,
                QuizId = question.QuizId,
                QuestionList = questions,
                Description = question.Description,
                Open = question.Open,
                CreationDate = question.CreationDate
            };

            return View(model);
        }
        
        public ActionResult QuestionIndex(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var questions = QuizService.QuestionListByQuizId(id);
            var model = new QuestionIndexViewModel()
            {
                QuestionList = questions,
                QuizId = id
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult QuestionIndex([Bind(Include = "QuestionId,Description,Open,QuizId")] Question question)
        {
            question.CreationDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.QuestionsList.Add(question);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult QuestionEdit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var question = QuizService.GetQuestionById(id);
            var questions = QuizService.QuestionListByQuizId(question.QuizId);
            var model = new QuestionIndexViewModel()
            {
                QuestionId = id,
                QuizId = question.QuizId,
                QuestionList = questions,
                Description = question.Description
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult QuestionEdit([Bind(Include = "QuestionId,Description,Open,QuizId")] Question question)
        {
            question.CreationDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("QuestionIndex", new { id = question.QuizId });
            }

            return View();
        }

        public ActionResult QuestionDelete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var question = QuizService.GetQuestionById(id);
            var questions = QuizService.QuestionListByQuizId(question.QuizId);
            var model = new QuestionIndexViewModel()
            {
                QuestionId = id,
                QuizId = question.QuizId,
                QuestionList = questions,
                Description = question.Description
            };

            return View(model);
        }

        [HttpPost, ActionName("QuestionDelete")]
        public ActionResult QuestionDeleteConfirm(int id)
        {
            var question = db.QuestionsList.Find(id);

            question.IsDeleted  = true;
            db.SaveChanges();

            return RedirectToAction("QuestionIndex", new { id = question.QuizId });
        }

        public ActionResult AnswerDetails(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var question = QuizService.GetAnswerById(id);
            var questions = QuizService.AnswerListByQuestionId(question.QuestionId);
            var model = new AnswerIndexViewModel()
            {
                AnswerId = id,
                QuestionId = question.QuestionId,
                AnswersList = questions,
                Description = question.Description,
                CreationDate = question.CreationDate
            };

            return View(model);
        }

        public ActionResult AnswerIndex(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var questionOpen = QuizService.GetQuestionById(id);
            ViewBag.Open = questionOpen.Open;
            var questionOptions = QuizService.OptionListByQuestionId(id);

            ViewBag.Value = new SelectList(questionOptions, "OptionId", "Description");

            var questions = QuizService.AnswerListByQuestionId(id);
            var model = new AnswerIndexViewModel()
            {
                AnswersList = questions,
                QuestionId = id
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult AnswerIndex([Bind(Include = "AnswerId,Description,Value,QuestionId")] Answer answer)
        {
            var optionId = QuizService.GetOptionById((answer.Value.HasValue ? answer.Value.Value : 0));

            answer.CreationDate = DateTime.Now;
            answer.Value = optionId.Value;

            if(answer.Description == null)
            {
                answer.Description = optionId.Description;
            }

            if (ModelState.IsValid)
            {
                db.AnswersList.Add(answer);
                db.SaveChanges();
                return RedirectToAction("AnswerIndex", new { id = answer.QuestionId });
            }

            return View();
        }

        public ActionResult AnswerEdit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var question = QuizService.GetAnswerById(id);
            var questions = QuizService.AnswerListByQuestionId(question.QuestionId);
            var model = new AnswerIndexViewModel()
            {
                AnswerId = id,
                QuestionId = question.QuestionId,
                AnswersList = questions,
                Description = question.Description
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult AnswerEdit([Bind(Include = "AnswerId,Description,QuestionId")] Answer answer)
        {
            answer.CreationDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Entry(answer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AnswerIndex", new { id = answer.QuestionId });
            }

            return View();
        }

        public ActionResult AnswerDelete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var question = QuizService.GetAnswerById(id);
            var questions = QuizService.AnswerListByQuestionId(question.QuestionId);
            var model = new AnswerIndexViewModel()
            {
                AnswerId = id,
                QuestionId = question.QuestionId,
                AnswersList = questions,
                Description = question.Description
            };

            return View(model);
        }

        [HttpPost, ActionName("AnswerDelete")]
        public ActionResult AnswerDeleteConfirm(int id)
        {
            var answer = db.AnswersList.Find(id);

            answer.IsDeleted = true;

            db.SaveChanges();

            return RedirectToAction("AnswerIndex", new { id = answer.QuestionId });
        }

        public ActionResult OptionDetails(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var opcion = QuizService.GetOptionById(id);
            var opcions = QuizService.OptionListByQuestionId(opcion.QuestionId);
            var model = new KindOfQuestionViewModel()
            {
                OptionId = id,
                QuestionId = opcion.QuestionId,
                OptionsList = opcions,
                Description = opcion.Description,
                Value = opcion.Value,
                CreationDate = opcion.CreationDate
            };

            return View(model);
        }

        public ActionResult KindOfQuestion(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var opcions = QuizService.OptionListByQuestionId(id);
            var model = new KindOfQuestionViewModel()
            {
                OptionsList = opcions,
                QuestionId = id
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult KindOfQuestion([Bind(Include = "OptionId,Description,Value,QuestionId")] Option option)
        {
            option.CreationDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.OptionList.Add(option);
                db.SaveChanges();
                return RedirectToAction("KindOfQuestion", new { id = option.QuestionId });
            }

            return View();
        }

        public ActionResult OptionEdit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var opcion = QuizService.GetOptionById(id);
            var opcions = QuizService.OptionListByQuestionId(opcion.QuestionId);
            var model = new KindOfQuestionViewModel()
            {
                OptionId = id,
                QuestionId = opcion.QuestionId,
                OptionsList = opcions,
                Description = opcion.Description,
                Value = opcion.Value
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult OptionEdit([Bind(Include = "OptionId,Description,Value,QuestionId")] Option option)
        {
            option.CreationDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Entry(option).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("KindOfQuestion", new { id = option.QuestionId });
            }

            return View();
        }

        public ActionResult OptionDelete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var opcion = QuizService.GetOptionById(id);
            var opcions = QuizService.OptionListByQuestionId(opcion.QuestionId);
            var model = new KindOfQuestionViewModel()
            {
                OptionId = id,
                QuestionId = opcion.QuestionId,
                OptionsList = opcions,
                Description = opcion.Description
            };

            return View(model);
        }

        [HttpPost, ActionName("OptionDelete")]
        public ActionResult OptionDeleteConfirm(int id)
        {
            var option = db.OptionList.Find(id);

            option.IsDeleted = true;

            db.SaveChanges();

            return RedirectToAction("KindOfQuestion", new { id = option.QuestionId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
