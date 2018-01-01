﻿using MetaqueryGenerator.Common;
using MetaqueryGenerator.DS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetaqueryGenerator.BL
{
    public class ProcessMQDetails
    {
        
        public static int RelationsCount { get; set; }
        public static int VariablesCount { get; set; }
        public static int MaxVariablesInRelation { get; set; }
        public static List<int> RelationsVarCount { get; set; }
        
        static ProcessMQDetails()
        {
            string connectionString = @"Data Source =.\SQLEXPRESS; Initial Catalog = TestMQ; Integrated Security = True";
            MetaqueryGenerator.DS.ProcessingModelDS modelDS = new MetaqueryGenerator.DS.ProcessingModelDS(connectionString);
            RelationsCount = modelDS.GetRelationsCount();
            VariablesCount = modelDS.GetVariablesCount();
            MaxVariablesInRelation = modelDS.GetMaxVariablesInRelation();
            RelationsVarCount = modelDS.GetRelationsVarCount();
        }
    }

    public class MQGenerator
    {
        //public int SupportThreshold { get; set; }
        //public int ConfidenceThreshold { get; set; }

        public MQGenerator()
        {
            
        }
        
        public void Start()
        {
            //create first level
            Console.WriteLine("start  : {0}:{1}:{2}:{3}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
            Metaquery rootMQ = Metaquery.GetRootMQ();

            Console.WriteLine(rootMQ.ToString());
            Console.WriteLine("before 1: {0}:{1}:{2}:{3}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
            int MaxVariablesInRelation = ProcessMQDetails.MaxVariablesInRelation;
            List<Metaquery> list = rootMQ.Expand(MaxVariablesInRelation);
            Console.WriteLine(" after 1: {0}:{1}:{2}:{3}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
            foreach(Metaquery q in list)
                Console.WriteLine(q.ToString());
        }

        public List<Metaquery> VariableExpand(Metaquery query)
        {
            List<Metaquery> mqList = new List<Metaquery>();

            Metaquery newMQ;
            //mqList.AddRange(AddVariable(query.Head, newMQ));
            
            //Check the possibility of adding variable to Head Relation
            if (query.Head.Arity < ProcessMQDetails.MaxVariablesInRelation)
            {

                newMQ = query.Clone();
                newMQ.Head.AddVariable(query.Head.Arity + 1);
                mqList.Add(newMQ);
            }
            List<int> relationsVarCount = ProcessMQDetails.RelationsVarCount;
            //foreach (Relation bodyRelation in query.Body.List)
            for (int i = 0; i< query.Body.Count; i++)
            {
                Relation bodyRelation = query.Body[i];
                if (bodyRelation.Arity < relationsVarCount[i])
                {
                    newMQ = query.Clone();
                    newMQ.Body[i].AddVariable(bodyRelation.Arity + 1);
                    mqList.Add(newMQ);
                }
                //mqList.AddRange(AddVariable(bodyRelation, newMQ));
            }
            return mqList;
        }
    }
}
