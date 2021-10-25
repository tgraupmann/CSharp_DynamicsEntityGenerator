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

        static void Example_QuerySingleRecord(AutoGeneratorClient client, string dataPath)
        {
            ColumnSet columnSet = new ColumnSet(true);
            Account record = client.Retrieve<Account>(Guid.Parse("93c71621-bd9f-e711-8122-000d3a2ba2ea"), columnSet);
        }

        static void Example_QueryMultipleRecords(AutoGeneratorClient client, string dataPath)
        {
            QueryExpression query = client.NewQueryExpression<Account>();
            query.ColumnSet = new ColumnSet(true);
            query.TopCount = 100;
            List<Account> records = client.RetrieveMultiple<Account>(query);
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

        static Account Example_AddRecord(AutoGeneratorClient client, string dataPath, Guid ownerId)
        {
            string accountName = string.Format("Test - {0}", Guid.NewGuid());

            Account account = client.NewRecord<Account>(ownerId);
            account.name = accountName;
            account.accountid = client.Add(account);

            if (account.accountid != Guid.Empty)
            {
                client.SaveCSV(Path.Combine(dataPath, "Account_Created.csv"), account);

                ColumnSet columnSet = new ColumnSet(true);
                var result = client.Retrieve<Account>(account.accountid, columnSet);
                client.SaveCSV(Path.Combine(dataPath, "Account_Created_Queried.csv"), account);

                return account;
            }
            return null;
        }

        static void Example_DeleteRecord(AutoGeneratorClient client, Account account)
        {
            if (null == account || account.accountid == Guid.Empty)
            {
                return;
            }

            client.Delete<Account>(account.accountid);
        }

        static void Example_QueryFilter(AutoGeneratorClient client, string dataPath)
        {
            QueryExpression query = client.NewQueryExpression<Account>();
            query.ColumnSet = new ColumnSet(true);
            query.TopCount = 10;

            FilterExpression filter = query.Criteria;

            ConditionExpression condition =
                client.NewConditionExpression<Account>(
                    Account.Attribute.name,
                    ConditionOperator.Equal,
                    "Microsoft");

            filter.AddCondition(condition);

            List <Account> records = client.RetrieveMultiple<Account>(query);
            if (records.Count > 0)
            {
                client.SaveCSV(Path.Combine(dataPath, "Account_Filtered.csv"), records);
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

                Example_QueryFilter(client, dataPath);

                Account account = Example_AddRecord(client, dataPath, userid);
                if (account != null)
                {
                    Example_DeleteRecord(client, account);
                }

                Example_GenerateEntityClasses(client, dataPath);

                Example_QuerySingleRecord(client, dataPath);

                Example_QueryMultipleRecords(client, dataPath);

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
