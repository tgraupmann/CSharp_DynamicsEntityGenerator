﻿using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.IO;

namespace DynamicsEntityGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = string.Format("AuthType=ClientSecret;Url={0};ClientId={1};ClientSecret={2}",
                Secrets.ServiceUrl,
                Secrets.ClientId,
                Secrets.Secret);

            IOrganizationService oServiceProxy = null;
            try
            {

                //Create the Dynamics 365 Connection:
                CrmServiceClient oMSCRMConn = new Microsoft.Xrm.Tooling.Connector.CrmServiceClient(connectionString);

                //Create the IOrganizationService:
                oServiceProxy = (IOrganizationService)oMSCRMConn.OrganizationWebProxyClient != null ? (IOrganizationService)oMSCRMConn.OrganizationWebProxyClient : (IOrganizationService)oMSCRMConn.OrganizationServiceProxy;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Failed to initialize connection! {0}", ex);
            }


            if (oServiceProxy != null)
            {
                //Get the current user ID:
                Guid userid = ((WhoAmIResponse)oServiceProxy.Execute(new WhoAmIRequest())).UserId;

                if (userid != Guid.Empty)
                {
                    Console.WriteLine("Connection Successful!");
                }

                AutoGeneratorClient client = new AutoGeneratorClient(oServiceProxy);
                
                string dataPath = "..\\..\\Autogenerated";

                List<AutoGeneratorClient.GenerateEntityItem> entityItems =
                    new List<AutoGeneratorClient.GenerateEntityItem>()
                {
                    new AutoGeneratorClient.GenerateEntityItem("account", "Account"),
                    new AutoGeneratorClient.GenerateEntityItem("contact", "Contact"),
                    new AutoGeneratorClient.GenerateEntityItem("systemuser", "User"),
                    new AutoGeneratorClient.GenerateEntityItem("task", "Task"),
                };

                client.GenerateClasses(dataPath, entityItems);

                client.QueryDatabaseToCSV<Account>(Path.Combine(dataPath, "Accounts.csv"));
                client.QueryDatabaseToCSV<Contact>(Path.Combine(dataPath, "Contacts.csv"));
                client.QueryDatabaseToCSV<User>(Path.Combine(dataPath, "Users.csv"));
                client.QueryDatabaseToCSV<Task>(Path.Combine(dataPath, "Tasks.csv"));

                List<Account> accounts = client.LoadCSV<Account>(Path.Combine(dataPath, "Accounts.csv"));
                client.SaveCSV(Path.Combine(dataPath, "Accounts2.csv"), accounts);

                List<Contact> contacts = client.LoadCSV<Contact>(Path.Combine(dataPath, "Contacts.csv"));
                client.SaveCSV(Path.Combine(dataPath, "Contacts2.csv"), contacts);

                List<User> users = client.LoadCSV<User>(Path.Combine(dataPath, "Users.csv"));
                client.SaveCSV(Path.Combine(dataPath, "Users2.csv"), users);

                List<Task> tasks = client.LoadCSV<Task>(Path.Combine(dataPath, "Tasks.csv"));
                client.SaveCSV(Path.Combine(dataPath, "Tasks2.csv"), tasks);
            }
            else
            {
                Console.WriteLine("Connection failed...");
            }
        }
    }
}
