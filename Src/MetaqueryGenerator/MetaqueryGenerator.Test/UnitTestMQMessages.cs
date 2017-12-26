﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MetaqueryGenerator.Common;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MetaqueryGenerator.Test
{
    [TestClass]
    public class UnitTestMQMessages
    {
        [TestMethod]
        public void TestSendMQMessageToJson()
        {
            int[] headList = new int[] { 1, 3};
            List<int> bodyList1 = new List<int>() { 1, 2, 3 };
            List<int> bodyList2 = new List<int>() { 2, 3 };
            List<int> bodyList3 = new List<int>() {  3 };
            List<List<int>> bodyList = new List<List<int>> { bodyList1, bodyList2, bodyList3 };

            SendMQMessage message = new SendMQMessage() { ID = 1,SupportThreshold = 0.2f, ConfidenceThreshold = 0.3f,Head = headList, Body = bodyList };

            string strMessage = JsonConvert.SerializeObject(message);
            string strJson = "{\"ID\":1,\"SupportThreshold\":0.2,\"ConfidenceThreshold\":0.3,\"Head\":[1,3],\"Body\":[[1,2,3],[2,3],[3]]}";
            Assert.AreEqual(strMessage, strJson);

        }

        [TestMethod]
        public void TestMQResultMessageToJson()
        {
            MQResultMessage resultMessage = new MQResultMessage() { ID = 1, Result = true};

            string strMessage = JsonConvert.SerializeObject(resultMessage);
            string strResultMessageJson = "{\"ID\":1,\"Result\":true}";
            Assert.AreEqual(strMessage, strResultMessageJson);

            MQAssignmentResultMessage assignmentResultMessage = new MQAssignmentResultMessage() { ID = 1,Assignment="Any assignment",ConfidenceValue=0.3f,SupportValue=0.1f };
            strMessage = JsonConvert.SerializeObject(assignmentResultMessage);
            strResultMessageJson = "{\"ID\":1,\"SupportValue\":0.1,\"ConfidenceValue\":0.3,\"Assignment\":\"Any assignment\"}";

            Assert.AreEqual(strMessage, strResultMessageJson);

        }
    }
}
