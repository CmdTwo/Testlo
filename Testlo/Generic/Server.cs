using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using TServer.Common;
using TServer.Common.Content;

namespace Testlo.Generic
{
    public class Server
    {
        private IPEndPoint ServerEndPoint;
        private TcpClient TcpClient;
        private StateObject State;

        public static Server Instance;

        public Server(string ip, int port)
        {
            if (Instance == null)
                Instance = this;
            
            TcpClient = new TcpClient(); ;
            ServerEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            State = new StateObject();
        }

        public void ConnectToServer()
        {
            TcpClient.Connect(ServerEndPoint);
            TcpClient.GetStream().BeginRead(State.Buffer, 0, StateObject.BufferSize, new AsyncCallback(ReadCallback), State);
        }

        public void Disconect()
        {
            TcpClient.Client.Disconnect(false);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            int bytesRead = TcpClient.Client.EndReceive(ar);
            if (bytesRead > 0)
            {
                MessageManager messageManager = new MessageManager();
                Dictionary<ReceiveMessageParam, object> message = messageManager.DeserializeMessage<ReceiveMessageParam>(state.Buffer, bytesRead);

                if ((bool)(message[ReceiveMessageParam.IsRequest]))
                {
                    //switch ((byte)message[ReceiveMessageParam.CommandType])
                    //{
                    //    case ((byte)ReceiveCommand.Authorization):
                    //        Request_Authorization(message[ReceiveMessageParam.Params] as Dictionary<ParameterType, object>);
                    //        break;
                    //}
                }
                else
                {
                    switch ((byte)message[ReceiveMessageParam.CommandType])
                    {
                        case ((byte)ResponseCommand.AuthorizationResponse):
                            Response_Authorization(message[ReceiveMessageParam.Params] as Dictionary<ParameterType, object>);
                            break;
                        case ((byte)ResponseCommand.GetPartOfProfileResponse):
                            Response_GetPartOfProfile(message[ReceiveMessageParam.Params] as Dictionary<ParameterType, object>);
                            break;
                        case ((byte)ResponseCommand.GetAvailableAccessListResponse):
                            Response_GetAvailableAccessList(message[ReceiveMessageParam.Params] as Dictionary<ParameterType, object>);
                            break;
                        case ((byte)ResponseCommand.GetAvailableSubgroupListResponse):
                            Response_GetAvailableSubgroupList(message[ReceiveMessageParam.Params] as Dictionary<ParameterType, object>);
                            break;
                        case ((byte)ResponseCommand.GetUserListResponse):
                            Response_GetUserList(message[ReceiveMessageParam.Params] as Dictionary<ParameterType, object>);
                            break;
                        case ((byte)ResponseCommand.GetTagListResponse):
                            Response_GetTagList(message[ReceiveMessageParam.Params] as Dictionary<ParameterType, object>);
                            break;
                        case ((byte)ResponseCommand.AddNewTestResponse):
                            Response_AddNewTest(message[ReceiveMessageParam.Params] as Dictionary<ParameterType, object>);
                            break;
                        case ((byte)ResponseCommand.GetAvailableTestsResponse):
                            Response_GetAvailableTests(message[ReceiveMessageParam.Params] as Dictionary<ParameterType, object>);
                            break;
                        case ((byte)ResponseCommand.GetFailedTestsResponse):
                            Response_GetAvailableTests(message[ReceiveMessageParam.Params] as Dictionary<ParameterType, object>);
                            break;
                        case ((byte)ResponseCommand.GetComplitedTestsResponse):
                            Response_GetAvailableTests(message[ReceiveMessageParam.Params] as Dictionary<ParameterType, object>);
                            break;
                        case ((byte)ResponseCommand.GetTestResponse):
                            Response_GetTestResponse(message[ReceiveMessageParam.Params] as Dictionary<ParameterType, object>);
                            break;
                    }
                }
                TcpClient.GetStream().BeginRead(State.Buffer, 0, StateObject.BufferSize, new AsyncCallback(ReadCallback), State);
            }
        }

        #region API

        public void Authorization(string login, string password)
        {
            Dictionary<ReceiveMessageParam, object> request = new Dictionary<ReceiveMessageParam, object>() {
                    { ReceiveMessageParam.CommandType, RequestCommand.Authorization },
                    { ReceiveMessageParam.IsRequest, true },
                    { ReceiveMessageParam.Params, new Dictionary<ParameterType, object> {
                        { ParameterType.login, login },
                        { ParameterType.password, password },
                    } } };
            MessageManager messageManager = new MessageManager();
            byte[] bytes = messageManager.SerializeMessage(request);
            TcpClient.GetStream().Write(bytes, 0, bytes.Length);
        }
        public void GetPartOfProfile()
        {
            Dictionary<ReceiveMessageParam, object> request = new Dictionary<ReceiveMessageParam, object>() {
                    { ReceiveMessageParam.CommandType, RequestCommand.GetPartOfProfile },
                    { ReceiveMessageParam.IsRequest, true },
                    { ReceiveMessageParam.Params, null } };
            MessageManager messageManager = new MessageManager();
            byte[] bytes = messageManager.SerializeMessage(request);
            TcpClient.GetStream().Write(bytes, 0, bytes.Length);
        }
        public void GetAvailableAccessList()
        {
            Dictionary<ReceiveMessageParam, object> request = new Dictionary<ReceiveMessageParam, object>() {
                    { ReceiveMessageParam.CommandType, RequestCommand.GetAvailableAccessList },
                    { ReceiveMessageParam.IsRequest, true },
                    { ReceiveMessageParam.Params, null } };
            MessageManager messageManager = new MessageManager();
            byte[] bytes = messageManager.SerializeMessage(request);
            TcpClient.GetStream().Write(bytes, 0, bytes.Length);
        }      
        public void GetAvailableSubgroupList(int ID)
        {
            Dictionary<ReceiveMessageParam, object> request = new Dictionary<ReceiveMessageParam, object>() {
                    { ReceiveMessageParam.CommandType, RequestCommand.GetAvailableSubgroupList },
                    { ReceiveMessageParam.IsRequest, true },
                    { ReceiveMessageParam.Params, new Dictionary<ParameterType, object> {
                        { ParameterType.groupID, ID }
                    } } };
            MessageManager messageManager = new MessageManager();
            byte[] bytes = messageManager.SerializeMessage(request);
            TcpClient.GetStream().Write(bytes, 0, bytes.Length);
        }
        public void GetUsers()
        {
            Dictionary<ReceiveMessageParam, object> request = new Dictionary<ReceiveMessageParam, object>() {
                    { ReceiveMessageParam.CommandType, RequestCommand.GetUserList },
                    { ReceiveMessageParam.IsRequest, true },
                    { ReceiveMessageParam.Params, null } };
            MessageManager messageManager = new MessageManager();
            byte[] bytes = messageManager.SerializeMessage(request);
            TcpClient.GetStream().Write(bytes, 0, bytes.Length);
        }
        public void GetTagList()
        {
            Dictionary<ReceiveMessageParam, object> request = new Dictionary<ReceiveMessageParam, object>() {
                    { ReceiveMessageParam.CommandType, RequestCommand.GetTagList },
                    { ReceiveMessageParam.IsRequest, true },
                    { ReceiveMessageParam.Params, null } };
            MessageManager messageManager = new MessageManager();
            byte[] bytes = messageManager.SerializeMessage(request);
            TcpClient.GetStream().Write(bytes, 0, bytes.Length);
        }
        public void AddNewTest(Test newTest)
        {
            Dictionary<ReceiveMessageParam, object> request = new Dictionary<ReceiveMessageParam, object>() {
                    { ReceiveMessageParam.CommandType, RequestCommand.AddNewTest },
                    { ReceiveMessageParam.IsRequest, true },
                    { ReceiveMessageParam.Params, new Dictionary<ParameterType, object> {
                        { ParameterType.newTest, newTest}
                    } } };
            MessageManager messageManager = new MessageManager();
            byte[] bytes = messageManager.SerializeMessage(request);
            TcpClient.GetStream().Write(bytes, 0, bytes.Length);
        }
        public void GetAvailableTests()
        {
            Dictionary<ReceiveMessageParam, object> request = new Dictionary<ReceiveMessageParam, object>() {
                    { ReceiveMessageParam.CommandType, RequestCommand.GetAvailableTests },
                    { ReceiveMessageParam.IsRequest, true },
                    { ReceiveMessageParam.Params, null } };
            MessageManager messageManager = new MessageManager();
            byte[] bytes = messageManager.SerializeMessage(request);
            TcpClient.GetStream().Write(bytes, 0, bytes.Length);
        }
        public void GetFailedTests()
        {
            Dictionary<ReceiveMessageParam, object> request = new Dictionary<ReceiveMessageParam, object>() {
                    { ReceiveMessageParam.CommandType, RequestCommand.GetFailedTests },
                    { ReceiveMessageParam.IsRequest, true },
                    { ReceiveMessageParam.Params, null } };
            MessageManager messageManager = new MessageManager();
            byte[] bytes = messageManager.SerializeMessage(request);
            TcpClient.GetStream().Write(bytes, 0, bytes.Length);
        }
        public void GetComplitedTests()
        {
            Dictionary<ReceiveMessageParam, object> request = new Dictionary<ReceiveMessageParam, object>() {
                    { ReceiveMessageParam.CommandType, RequestCommand.GetComplitedTests },
                    { ReceiveMessageParam.IsRequest, true },
                    { ReceiveMessageParam.Params, null } };
            MessageManager messageManager = new MessageManager();
            byte[] bytes = messageManager.SerializeMessage(request);
            TcpClient.GetStream().Write(bytes, 0, bytes.Length);
        }
        public void GetTest(int testID)
        {
            Dictionary<ReceiveMessageParam, object> request = new Dictionary<ReceiveMessageParam, object>() {
                    { ReceiveMessageParam.CommandType, RequestCommand.GetTest },
                    { ReceiveMessageParam.IsRequest, true },
                    { ReceiveMessageParam.Params, new Dictionary<ParameterType, object> {
                        { ParameterType.testID, testID }
                    } } };
            MessageManager messageManager = new MessageManager();
            byte[] bytes = messageManager.SerializeMessage(request);
            TcpClient.GetStream().Write(bytes, 0, bytes.Length);
        }

        #endregion

        #region RequestMethods

        private void Request_Authorization(Dictionary<ParameterType, object> args)
        {

        }

        #endregion

        #region ResponseMethods

        private void Response_Authorization(Dictionary<ParameterType, object> args)
        {
            AuthorizationResponse((bool)args[ParameterType.authorized]);
        }

        private void Response_GetPartOfProfile(Dictionary<ParameterType, object> args)
        {
            GetPartOfProfileResponse((string)args[ParameterType.name]);
        }

        private void Response_GetAvailableAccessList(Dictionary<ParameterType, object> args)
        {
            GetAvailableAccessListResponse((List<Access>)args[ParameterType.availableAccessList]);
        }

        private void Response_GetAvailableSubgroupList(Dictionary<ParameterType, object> args)
        {
            GetAvailableSubgroupListResponse((List<SubAccess>)args[ParameterType.availableSubAccessList]);
        }

        private void Response_GetUserList(Dictionary<ParameterType, object> args)
        {
            GetUserListResponse((Dictionary<int, string>)args[ParameterType.usersList]);
        }

        private void Response_GetTagList(Dictionary<ParameterType, object> args)
        {
            GetTagListResponse((List<Tag>)args[ParameterType.tagList]);
        }

        private void Response_AddNewTest(Dictionary<ParameterType, object> args)
        {
            AddNewTestResponse((bool)args[ParameterType.responseStatus]);
        }

        private void Response_GetAvailableTests(Dictionary<ParameterType, object> args)
        {
            GetAvailableTestsResponse((List<object[]>)args[ParameterType.responseStatus]);
        }
        private void Response_GetFailedTests(Dictionary<ParameterType, object> args)
        {
            GetFailedTestsResponse((List<object[]>)args[ParameterType.responseStatus]);
        }
        private void Response_GetComplitedTests(Dictionary<ParameterType, object> args)
        {
            GetComplitedTestsResponse((List<object[]>)args[ParameterType.responseStatus]);
        }

        private void Response_GetTestResponse(Dictionary<ParameterType, object> args)
        {
            GetTestResponse((Test)args[ParameterType.test]);
        }
        #endregion

        public event Action<bool> AuthorizationResponse;
        public event Action<string> GetPartOfProfileResponse;
        public event Action<List<Access>> GetAvailableAccessListResponse;
        public event Action<List<SubAccess>> GetAvailableSubgroupListResponse;
        public event Action<Dictionary<int, string>> GetUserListResponse;
        public event Action<List<Tag>> GetTagListResponse;
        public event Action<bool> AddNewTestResponse;
        public event Action<List<object[]>> GetAvailableTestsResponse;
        public event Action<List<object[]>> GetFailedTestsResponse;
        public event Action<List<object[]>> GetComplitedTestsResponse;
        public event Action<Test> GetTestResponse;
    }
}
