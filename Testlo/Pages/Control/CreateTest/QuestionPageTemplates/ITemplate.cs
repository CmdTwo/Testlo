using System.Collections.Generic;
using TServer.Common.Content;
using System.Windows;

namespace Testlo.Pages.Control.CreateTest.QuestionPageTemplates
{
    interface ITemplate
    {
        string GetQuestionText();
        List<UIElement> GetAnsweElementsrList();
    }
}
