using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace UniqueLeadFullName
{
    public class UniqueLeadFullName : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var service = (IOrganizationService)serviceProvider.GetService(typeof(IOrganizationService));
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var lead = (Entity)context.InputParameters["Target"];

            string fetchXml =
            @"<fetch mapping='logical'>
                <entity name='lead'>
                <attribute name='fullname'/>
                <filter type='and'>
                    <condition attribute='fullname' operator='eq' value='" + lead.Attributes["fullname"].ToString() + "'/>" +
                "</filter>" +
                "</entity>" +
            "</fetch>";

            EntityCollection extractedLeads = service.RetrieveMultiple(new FetchExpression(fetchXml));

            if (extractedLeads.Entities.Count > 0)
            {
                throw new InvalidPluginExecutionException("A lead with the same name already exists");
            }
        }
    }
}
