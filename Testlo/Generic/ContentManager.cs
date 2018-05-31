using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testlo.Logic.NewLogic;
using TServer.Common.Content;

namespace Testlo.Generic
{
    public class ContentManager
    {
        private static ContentManager _instance;



        public ContentManager()
        {
            if(_instance == null)
                _instance = this;
        }

        //public List<Test> GetAvailableTests()
        //{
            
        //}

        public static ContentManager GetCurrentManager { get { return _instance; } }

        public event Action AvailableTestUpdated;
    }
}
