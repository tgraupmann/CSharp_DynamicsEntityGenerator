using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.IO;

namespace DynamicsEntityGenerator
{
    class Program
    {
        static void Example_GenerateEntityClasses(AutoGeneratorClient client, string dataPath)
        {
            List<AutoGeneratorClient.GenerateEntityItem> entityItems =
                    new List<AutoGeneratorClient.GenerateEntityItem>()
                {
                    new AutoGeneratorClient.GenerateEntityItem("account", "Account"),
                    new AutoGeneratorClient.GenerateEntityItem("contact", "Contact"),
                    new AutoGeneratorClient.GenerateEntityItem("systemuser", "User"),
                    new AutoGeneratorClient.GenerateEntityItem("task", "Task"),
                };

            client.GenerateClasses(dataPath, entityItems);
        }

        static void Example_QueryDatabasetoCsv(AutoGeneratorClient client, string dataPath)
        {
            client.QueryDatabaseToCSV<Account>(Path.Combine(dataPath, "Accounts.csv"));
            client.QueryDatabaseToCSV<Contact>(Path.Combine(dataPath, "Contacts.csv"));
            client.QueryDatabaseToCSV<User>(Path.Combine(dataPath, "Users.csv"));
            client.QueryDatabaseToCSV<Task>(Path.Combine(dataPath, "Tasks.csv"));
        }

        static void Example_ValidateCsvData(AutoGeneratorClient client, string dataPath)
        {
            List<Account> accounts = client.LoadCSV<Account>(Path.Combine(dataPath, "Accounts.csv"));
            client.SaveCSV(Path.Combine(dataPath, "Accounts2.csv"), accounts);

            List<Contact> contacts = client.LoadCSV<Contact>(Path.Combine(dataPath, "Contacts.csv"));
            client.SaveCSV(Path.Combine(dataPath, "Contacts2.csv"), contacts);

            List<User> users = client.LoadCSV<User>(Path.Combine(dataPath, "Users.csv"));
            client.SaveCSV(Path.Combine(dataPath, "Users2.csv"), users);

            List<Task> tasks = client.LoadCSV<Task>(Path.Combine(dataPath, "Tasks.csv"));
            client.SaveCSV(Path.Combine(dataPath, "Tasks2.csv"), tasks);
        }

        static void Example_SaveSingleRecord(AutoGeneratorClient client, string dataPath)
        {
            QueryExpression query = client.NewQueryExpression<Account>();
            query.ColumnSet = new ColumnSet(true);
            query.TopCount = 2;
            List<Account> accounts = client.RetrieveMultiple<Account>(query);

            if (accounts.Count > 1)
            {
                Account account = accounts[1];
                ColumnSet columnSet = new ColumnSet(true);
                var result = client.Retrieve<Account>(account.accountid, columnSet);
                client.SaveCSV(Path.Combine(dataPath, "Account_Single.csv"), result);
            }
        }

        static void Example_UpdateRecord(AutoGeneratorClient client, string dataPath)
        {
            QueryExpression query = client.NewQueryExpression<Account>();
            query.ColumnSet = new ColumnSet(true);
            query.TopCount = 2;
            List<Account> accounts = client.RetrieveMultiple<Account>(query);

            if (accounts.Count > 1)
            {
                Account account = accounts[1];
                ColumnSet columnSet = new ColumnSet(true);
                var result = client.Retrieve<Account>(account.accountid, columnSet);
                result.name = string.Format("{0}*", result.name); //add an asterisk so we can see it change
                client.Update(result);
            }

            accounts = client.RetrieveMultiple<Account>(query);
            if (accounts.Count > 1)
            {
                Account account = accounts[1];
                ColumnSet columnSet = new ColumnSet(true);
                var result = client.Retrieve<Account>(account.accountid, columnSet);
                client.SaveCSV(Path.Combine(dataPath, "Account_Changed.csv"), result);
            }
        }

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

                Example_GenerateEntityClasses(client, dataPath);

                Example_QueryDatabasetoCsv(client, dataPath);

                Example_ValidateCsvData(client, dataPath);

                Example_SaveSingleRecord(client, dataPath);

                Example_UpdateRecord(client, dataPath);
            }
            else
            {
                Console.WriteLine("Connection failed...");
            }
        }
    }
}
