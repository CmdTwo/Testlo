using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testlo.Pages.Control.CreateTest
{
    public enum CreateTestTypePage:byte
    {
        InputTestNamePage = 1,
        SelectAccessPage = 2,
        SelectEvaluationPage = 3,
        SelectShowAnswerPage = 4,
        SelectTagPage = 5,
        SetTimeAndAbort = 6,
        QuestionPage = 7
    }
}
