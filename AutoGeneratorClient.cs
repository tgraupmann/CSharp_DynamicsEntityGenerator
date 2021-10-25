﻿using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace DynamicsEntityGenerator
{
    public class AutoGeneratorClient
    {
        private IOrganizationService _mServiceProxy;

        public AutoGeneratorClient(IOrganizationService oServiceProxy)
        {
            _mServiceProxy = oServiceProxy;
        }

        public string GetTableName<T>() where T : IAutogeneratedClass
        {
            foreach (object attribute in typeof(T).GetCustomAttributes(false))
            {
                if (attribute is TableNameAttribute)
                {
                    return (attribute as TableNameAttribute).Name;
                }

            }
            return null;
        }

        public QueryExpression NewQueryExpression<T>() where T : IAutogeneratedClass
        {
            string tableName = GetTableName<T>();
            if (string.IsNullOrEmpty(tableName))
            {
                return new QueryExpression();
            }
            return new QueryExpression(tableName);
        }

        public ConditionExpression NewConditionExpression<T>(string attributeName, ConditionOperator conditionOperator, object value) where T : IAutogeneratedClass
        {
            string tableName = GetTableName<T>();
            if (string.IsNullOrEmpty(tableName))
            {
                return new ConditionExpression();
            }
            ConditionExpression expression = new ConditionExpression(attributeName, conditionOperator, value);
            expression.EntityName = tableName;
            return expression;
        }

        public T Retrieve<T>(Guid id, ColumnSet columnSet) where T : IAutogeneratedClass, new()
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            Dictionary<string, PropertyInfo> dictProp = new Dictionary<string, PropertyInfo>();
            foreach (PropertyInfo p in properties)
            {
                dictProp[p.Name] = p;
            }
            var record = _mServiceProxy.Retrieve(GetTableName<T>(), id, columnSet);
            if (record != null)
            {
                T item = new T();
                //attributes
                foreach (var ac in record.Attributes)
                {
                    string attribute = ac.Key;
                    if (dictProp.ContainsKey(attribute))
                    {
                        PropertyInfo p = dictProp[attribute];
                        p.SetValue(item, ac.Value);
                    }
                    else
                    {
                        Console.Error.WriteLine("Attribute Not Mapped! Key={0} Value={1}", ac.Key, ac.Value);
                    }
                }
                //formatted values
                foreach (var fv in record.FormattedValues)
                {
                    string formattedValue = string.Format("formatted_value_{0}", fv.Key);
                    if (dictProp.ContainsKey(formattedValue))
                    {
                        PropertyInfo p = dictProp[formattedValue];
                        p.SetValue(item, fv.Value);
                    }
                    else
                    {
                        Console.Error.WriteLine("Formatted Value Not Mapped! Key={0} Value={1}", fv.Key, fv.Value);
                    }
                }
                return item;
            }
            return default(T);
        }

        public List<T> RetrieveMultiple<T>(QueryBase query) where T : IAutogeneratedClass, new()
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            Dictionary<string, PropertyInfo> dictProp = new Dictionary<string, PropertyInfo>();
            foreach (PropertyInfo p in properties)
            {
                dictProp[p.Name] = p;
            }
            List<T> list = new List<T>();
            var results = _mServiceProxy.RetrieveMultiple(query);
            if (results != null && results.Entities != null)
            {
                foreach (var record in results.Entities)
                {
                    T item = new T();
                    //attributes
                    foreach (var ac in record.Attributes)
                    {
                        string attribute = ac.Key;
                        if (dictProp.ContainsKey(attribute))
                        {
                            PropertyInfo p = dictProp[attribute];
                            p.SetValue(item, ac.Value);
                        }
                        else
                        {
                            Console.Error.WriteLine("Attribute Not Mapped! Key={0} Value={1}", ac.Key, ac.Value);
                        }
                    }
                    //formatted values
                    foreach (var fv in record.FormattedValues)
                    {
                        string formattedValue = string.Format("formatted_value_{0}", fv.Key);
                        if (dictProp.ContainsKey(formattedValue))
                        {
                            PropertyInfo p = dictProp[formattedValue];
                            p.SetValue(item, fv.Value);
                        }
                        else
                        {
                            Console.Error.WriteLine("Formatted Value Not Mapped! Key={0} Value={1}", fv.Key, fv.Value);
                        }
                    }
                    list.Add(item);
                }
            }
            return list;
        }

        public void Update<T>(T record) where T : IAutogeneratedClass, new()
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            if (record != null)
            {
                Entity entity = new Entity(GetTableName<T>());
                foreach (PropertyInfo p in properties)
                {
                    bool isAttribute = true;
                    string key = string.Empty;
                    object val = p.GetValue(record);
                    foreach (Attribute attribute in p.GetCustomAttributes())
                    {
                        if (attribute is NameAttribute)
                        {
                            NameAttribute nameAttribute = attribute as NameAttribute;
                            if (nameAttribute.Names.Length > 0)
                            {
                                string name = nameAttribute.Names[0];
                                const string tokenFormattedValue = "formatted_value_";
                                if (name.StartsWith(tokenFormattedValue))
                                {
                                    isAttribute = false;
                                    key = name.Substring(tokenFormattedValue.Length);
                                }
                                else
                                {
                                    key = name;
                                }
                                break;
                            }
                        }
                    }
                    if (isAttribute)
                    {
                        KeyValuePair<string, object> ac = new KeyValuePair<string, object>(key, val);
                        entity.Attributes.Add(ac);
                    }
                    else
                    {
                        string strVal = null;
                        if (val != null)
                        {
                            strVal = val as string;
                        }
                        KeyValuePair<string, string> fv = new KeyValuePair<string, string>(key, strVal);
                        entity.FormattedValues.Add(fv);
                    }
                }

                _mServiceProxy.Update(entity);
                Console.WriteLine("Record {0} Updated!", typeof(T));
            }
        }

        public T NewRecord<T>(Guid ownerId) where T : IAutogeneratedClass, new()
        {
            T item = new T();

            DateTime now = DateTime.Now;

            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo p in properties)
            {
                if (p.PropertyType == typeof(DateTime))
                {
                    p.SetValue(item, now);
                }
                else if (p.PropertyType == typeof(EntityReference))
                {
                    switch (p.Name)
                    {
                        case "createdby":
                        case "ownerid":
                            {
                                EntityReference owner = new EntityReference();
                                owner.Id = ownerId;
                                owner.LogicalName = "systemuser";
                                p.SetValue(item, owner);
                            }
                            break;
                    }
                }
            }
            return item;
        }

        static object GetDefault(Type type)
        {
            if (type == typeof(Guid))
            {
                return Guid.Empty;
            }
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }

        public Guid Add<T>(T record) where T : IAutogeneratedClass, new()
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            if (record != null)
            {
                Entity entity = new Entity(GetTableName<T>());
                foreach (PropertyInfo p in properties)
                {
                    bool isAttribute = true;
                    string key = string.Empty;
                    object val = p.GetValue(record);
                    foreach (Attribute attribute in p.GetCustomAttributes())
                    {
                        if (attribute is NameAttribute)
                        {
                            NameAttribute nameAttribute = attribute as NameAttribute;
                            if (nameAttribute.Names.Length > 0)
                            {
                                string name = nameAttribute.Names[0];
                                const string tokenFormattedValue = "formatted_value_";
                                if (name.StartsWith(tokenFormattedValue))
                                {
                                    isAttribute = false;
                                    key = name.Substring(tokenFormattedValue.Length);
                                }
                                else
                                {
                                    key = name;
                                }
                                break;
                            }
                        }
                    }
                    if (isAttribute)
                    {
                        if (val != null && !val.Equals(GetDefault(p.PropertyType)))
                        {
                            KeyValuePair<string, object> ac = new KeyValuePair<string, object>(key, val);
                            entity.Attributes.Add(ac);
                        }
                    }
                    else
                    {
                        string strVal = null;
                        if (val != null)
                        {
                            strVal = val as string;
                        }
                        KeyValuePair<string, string> fv = new KeyValuePair<string, string>(key, strVal);
                        entity.FormattedValues.Add(fv);
                    }
                }

                Guid id = Guid.Empty;
                try
                {
                    id = _mServiceProxy.Create(entity);
                    Console.WriteLine("Record {0} Created!", typeof(T));
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to add {0} record! {0}", typeof(T), ex);
                }
                return id;
            }
            return Guid.Empty;
        }

        void GenerateEntity(string outputPath, GenerateEntityItem entityItem, SortedList<string, Type> attributes, SortedList<string, Type> formattedValues)
        {
            string path = Path.Combine(outputPath, string.Format("{0}.cs", entityItem.ClassName));
            using (FileStream fs = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.WriteLine("[DynamicsEntityGenerator.TableName(\"{0}\")]", entityItem.EntityName);
                    sw.WriteLine("public class {0} : DynamicsEntityGenerator.IAutogeneratedClass", entityItem.ClassName);
                    sw.WriteLine("{0}", "{");

                    sw.WriteLine("{0}", "\tpublic static class Attribute");
                    sw.WriteLine("{0}", "\t{");
                    foreach (KeyValuePair<string, Type> attribute in attributes)
                    {
                        sw.WriteLine("\t\tpublic const string {0} = \"{0}\";", attribute.Key);
                    }
                    sw.WriteLine("{0}", "\t}");
                    sw.WriteLine();

                    foreach (KeyValuePair<string, Type> attribute in attributes)
                    {
                        sw.WriteLine("\t[CsvHelper.Configuration.Attributes.Name(\"{0}\")]", attribute.Key);
                        sw.WriteLine("\tpublic {0} {1} {2}", attribute.Value, attribute.Key, "{ get; set; }");
                        sw.WriteLine();
                    }
                    foreach (KeyValuePair<string, Type> formattedValue in formattedValues)
                    {
                        sw.WriteLine("\t[CsvHelper.Configuration.Attributes.Name(\"formatted_value_{0}\")]", formattedValue.Key);
                        sw.WriteLine("\tpublic string formatted_value_{0} {1}", formattedValue.Key, "{ get; set; }");
                        sw.WriteLine();
                    }
                    sw.WriteLine("{0}", "}");
                    sw.Flush();
                }
            }
        }

        public class GenerateEntityItem
        {
            public string EntityName { get; set; }
            public string ClassName { get; set; }
            public GenerateEntityItem(string entityName, string className)
            {
                EntityName = entityName;
                ClassName = className;
            }
        }

        public void GenerateClasses(string outputPath, List<GenerateEntityItem> entityItems)
        {
            foreach (GenerateEntityItem entityItem in entityItems)
            {
                var query = new QueryExpression(entityItem.EntityName);
                query.ColumnSet = new Microsoft.Xrm.Sdk.Query.ColumnSet(true);
                //query.TopCount = 1;
                var results = _mServiceProxy.RetrieveMultiple(query);
                if (results != null)
                {
                    SortedList<string, Type> attributes = new SortedList<string, Type>();
                    SortedList<string, Type> formattedValues = new SortedList<string, Type>();
                    foreach (Entity result in results.Entities)
                    {
                        //var myJSON = JsonConvert.SerializeObject(result);

                        int index = 0;
                        foreach (KeyValuePair<string, object> kvp in result.Attributes)
                        {
                            if (!attributes.ContainsKey(kvp.Key))
                            {
                                attributes.Add(kvp.Key, kvp.Value.GetType());
                                //Console.WriteLine("Key={0} Value={1} Type={2}", kvp.Key, kvp.Value, kvp.Value.GetType());
                            }
                            ++index;
                        }

                        foreach (var fvc in result.FormattedValues)
                        {
                            if (!formattedValues.ContainsKey(fvc.Key))
                            {
                                formattedValues.Add(fvc.Key, fvc.Value.GetType());
                                //Console.WriteLine("Key={0} Value={1} Type={2}", fvc.Key, fvc.Value, fvc.Value.GetType());
                            }
                        }
                    }
                    GenerateEntity(outputPath, entityItem, attributes, formattedValues);
                }
            }
        }

        static string Delegate_ReferenceHeaderPrefix(ReferenceHeaderPrefixArgs args)
        {
            return string.Format("{0}.", args.MemberName);
        }

        public void SaveCSV<T>(string outputPath, List<T> results) where T : IAutogeneratedClass
        {
            if (results != null)
            {
                using (FileStream fsWrite = File.Open(outputPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(fsWrite, Encoding.UTF8))
                    {
                        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                        {
                            ReferenceHeaderPrefix = Delegate_ReferenceHeaderPrefix
                        };

                        using (var csv = new CsvWriter(writer, config))
                        {
                            csv.WriteHeader<T>();
                            csv.NextRecord();
                            foreach (var record in results)
                            {
                                csv.WriteRecord(record);
                                csv.NextRecord();
                            }
                            writer.Flush();
                            Console.WriteLine("Saved {0} records to {1}", typeof(T), outputPath);
                        }
                    }
                }
            }
        }

        public void SaveCSV<T>(string outputPath, T record) where T : IAutogeneratedClass
        {
            if (record != null)
            {
                using (FileStream fsWrite = File.Open(outputPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(fsWrite, Encoding.UTF8))
                    {
                        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                        {
                            ReferenceHeaderPrefix = Delegate_ReferenceHeaderPrefix
                        };

                        using (var csv = new CsvWriter(writer, config))
                        {
                            csv.WriteHeader<T>();
                            csv.NextRecord();
                            csv.WriteRecord(record);
                            csv.NextRecord();
                            writer.Flush();
                            Console.WriteLine("Saved {0} record to {1}", typeof(T), outputPath);
                        }
                    }
                }
            }
        }

        public void QueryDatabaseToCSV<T>(string pathCSV) where T : IAutogeneratedClass, new()
        {
            QueryExpression query = NewQueryExpression<T>();
            query.ColumnSet = new ColumnSet(true);
            //query.TopCount = 1;
            List<T> records = RetrieveMultiple<T>(query);
            SaveCSV(pathCSV, records);
        }

        public List<T> LoadCSV<T>(string inputPath) where T : IAutogeneratedClass
        {
            List<T> list = new List<T>();
            using (var reader = new StreamReader(inputPath, Encoding.UTF8))
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    ReferenceHeaderPrefix = Delegate_ReferenceHeaderPrefix
                };
                using (var csv = new CsvReader(reader, config))
                {
                    IEnumerable<T> tempT = csv.GetRecords<T>();
                    foreach (T record in tempT)
                    {
                        list.Add(record);
                    }
                }
            }
            Console.WriteLine("Loaded {0} records from {1}", typeof(T), inputPath);
            return list;
        }
    }
}
