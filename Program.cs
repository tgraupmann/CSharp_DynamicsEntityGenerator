using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;

namespace DynamicsEntityGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string connection = string.Format("AuthType=ClientSecret;Url={0};ClientId={1};ClientSecret={2}",
                Secrets.ServiceUrl,
                Secrets.ClientId,
                Secrets.Secret);

            //Create the Dynamics 365 Connection:
            CrmServiceClient oMSCRMConn = new Microsoft.Xrm.Tooling.Connector.CrmServiceClient(connection);

            //Create the IOrganizationService:
            IOrganizationService oServiceProxy = (IOrganizationService)oMSCRMConn.OrganizationWebProxyClient != null ? (IOrganizationService)oMSCRMConn.OrganizationWebProxyClient : (IOrganizationService)oMSCRMConn.OrganizationServiceProxy;


            if (oServiceProxy != null)
            {
                //Get the current user ID:
                Guid userid = ((WhoAmIResponse)oServiceProxy.Execute(new WhoAmIRequest())).UserId;

                if (userid != Guid.Empty)
                {
                    Console.WriteLine("Connection Successful!");
                }

                string[] entities =
                {
                    "account",
                    "contact",
                    "systemuser",
                    "task",
                };

                AutoGeneratorClient client = new AutoGeneratorClient(oServiceProxy);

                client.GenerateClasses(entities);

                var query = client.NewQueryExpression<Account>();
                query.ColumnSet = new Microsoft.Xrm.Sdk.Query.ColumnSet(true);
                //query.TopCount = 1;
                var results = client.RetrieveMultiple<Account>(query);
                if (results != null)
                {

                }
            }
            else
            {
                Console.WriteLine("Connection failed...");
            }
        }
    }
}
