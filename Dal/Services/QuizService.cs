using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dal.Repositories;
using Dal.Interfaces;
using Dal.Models;

namespace Dal.Services
{
    public class QuizService : BaseService
    {
        public QuizService(Repository repository, ILog logger)
            : base(repository, logger)
            { }

        public List<Question> QuestionListByQuizId(Int32 id)
        {
            var item = Repository.Questions().Where(x => x.QuizId == id).ToList();

            return item;
        }

        public Question GetQuestionById(Int32 id)
        {
            var item = Repository.Questions()
                .SingleOrDefault(x => x.QuestionId == id);

            return item;
        }

        public List<Answer> AnswerListByQuestionId(Int32 id)
        {
            var item = Repository.Answers().Where(x => x.QuestionId == id).ToList();

            return item;
        }

        public Answer GetAnswerById(Int32 id)
        {
            var item = Repository.Answers()
                .SingleOrDefault(x => x.AnswerId == id);

            return item;
        }

        public List<Option> OptionListByQuestionId(Int32 id)
        {
            var item = Repository.Options().Where(x => x.QuestionId == id).OrderBy(x => x.Value).ToList();

            return item;
        }

        public Option GetOptionById(Int32 id)
        {
            var item = Repository.Options()
                .SingleOrDefault(x => x.OptionId == id);

            return item;
        }
    }
}