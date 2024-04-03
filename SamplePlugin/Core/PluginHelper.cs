//C# sourcecode
/**********************************************************************************************************************
//Author: Param Varun Reddy
//Created: 2022-02-01
//Description: Plugin Helper for Development, Contains Extensions are common functionalities to be used.
***********************************************************************************************************************/

namespace RexStudios.PluginDevelopment
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Json;
    using Microsoft.Xrm.Sdk;

    public static class PluginHelper
    {
        public static EntityReference LookupObject(string entityLogicalName, Guid id)
        {
            return new EntityReference(entityLogicalName, id);
        }

        public static EntityReference LookupObject(Entity entity)
        {
            return entity.ToEntityReference();
        }

        public static string ToJson(Type type, object classObject)
        {
            var memoryStream = new MemoryStream();
            var serializer = new DataContractJsonSerializer(type);
            serializer.WriteObject(memoryStream, classObject);
            memoryStream.Position = 0;
            StreamReader streamReader = new StreamReader(memoryStream);
            return streamReader.ReadToEnd();
        }

        public static void Trace(string Message, ITracingService tracingService)
        {
            if (tracingService != null)
            {
                tracingService.Trace(Message);
            }

        }

        public static string[] SplitatSeperator(string values, char character)
        {
            return values.Split(character);
        }

    }

}
