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
        private List<Tag> TagList;
        private List<List<object>> AvailableTestList;
        private Server Server;

        public ContentManager()
        {
            Server = Server.Instance;
            if (_instance == null)
                _instance = this;
            Server.GetTagListResponse += Server_GetTagListResponse;
            Server.GetAvailableTestsResponse += Server_GetAvailableTestsResponse;

            Server.GetTagList();
        }

        private void Server_GetAvailableTestsResponse(List<List<object>> obj)
        {
            AvailableTestList = obj;
            AvailableTestListUpdated(AvailableTestList);
        }

        private void Server_GetTagListResponse(List<Tag> obj)
        {
            TagList = obj;
            TagListUpdated(TagList);
            SetAvailableTestList();
        }

        private void SetTagList()
        {
            if (TagList == null)
                Server.GetTagList();
        }

        private void SetAvailableTestList()
        {
            if (AvailableTestList == null)
                Server.GetAvailableTests();
        }

        //public List<Tag> GetTagList()
        //{
        //    if (TagList == null)
        //        Server.GetTagList();
        //    return TagList;
        //}

        //public List<List<object>> GetAvailableTestList()
        //{
        //    if (AvailableTestList == null)
        //        Server.GetAvailableTests();
        //    return AvailableTestList;
        //}

        public static ContentManager GetCurrentManager { get { return _instance; } }

        public event Action AvailableTestUpdated;
        public event Action<List<Tag>> TagListUpdated;
        public event Action<List<List<object>>> AvailableTestListUpdated;
    }
}
