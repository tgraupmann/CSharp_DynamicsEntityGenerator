﻿[DynamicsEntityGenerator.TableName("task")]
public class Task : DynamicsEntityGenerator.IAutogeneratedClass
{
	[CsvHelper.Configuration.Attributes.Name("attribute_activityid")]
	public System.Guid attribute_activityid { get; set; }

	[CsvHelper.Configuration.Attributes.Name("attribute_activitytypecode")]
	public System.String attribute_activitytypecode { get; set; }

	[CsvHelper.Configuration.Attributes.Name("attribute_actualdurationminutes")]
	public System.Int32 attribute_actualdurationminutes { get; set; }

	[CsvHelper.Configuration.Attributes.Name("attribute_actualstart")]
	public System.DateTime attribute_actualstart { get; set; }

	[CsvHelper.Configuration.Attributes.Name("attribute_createdby")]
	public Microsoft.Xrm.Sdk.EntityReference attribute_createdby { get; set; }

	[CsvHelper.Configuration.Attributes.Name("attribute_createdon")]
	public System.DateTime attribute_createdon { get; set; }

	[CsvHelper.Configuration.Attributes.Name("attribute_crm_outlooktaskid")]
	public System.String attribute_crm_outlooktaskid { get; set; }

	[CsvHelper.Configuration.Attributes.Name("attribute_description")]
	public System.String attribute_description { get; set; }

	[CsvHelper.Configuration.Attributes.Name("attribute_isbilled")]
	public System.Boolean attribute_isbilled { get; set; }

	[CsvHelper.Configuration.Attributes.Name("attribute_isregularactivity")]
	public System.Boolean attribute_isregularactivity { get; set; }

	[CsvHelper.Configuration.Attributes.Name("attribute_isworkflowcreated")]
	public System.Boolean attribute_isworkflowcreated { get; set; }

	[CsvHelper.Configuration.Attributes.Name("attribute_modifiedby")]
	public Microsoft.Xrm.Sdk.EntityReference attribute_modifiedby { get; set; }

	[CsvHelper.Configuration.Attributes.Name("attribute_modifiedon")]
	public System.DateTime attribute_modifiedon { get; set; }

	[CsvHelper.Configuration.Attributes.Name("attribute_ownerid")]
	public Microsoft.Xrm.Sdk.EntityReference attribute_ownerid { get; set; }

	[CsvHelper.Configuration.Attributes.Name("attribute_owningbusinessunit")]
	public Microsoft.Xrm.Sdk.EntityReference attribute_owningbusinessunit { get; set; }

	[CsvHelper.Configuration.Attributes.Name("attribute_owninguser")]
	public Microsoft.Xrm.Sdk.EntityReference attribute_owninguser { get; set; }

	[CsvHelper.Configuration.Attributes.Name("attribute_percentcomplete")]
	public System.Int32 attribute_percentcomplete { get; set; }

	[CsvHelper.Configuration.Attributes.Name("attribute_prioritycode")]
	public Microsoft.Xrm.Sdk.OptionSetValue attribute_prioritycode { get; set; }

	[CsvHelper.Configuration.Attributes.Name("attribute_processid")]
	public System.Guid attribute_processid { get; set; }

	[CsvHelper.Configuration.Attributes.Name("attribute_regardingobjectid")]
	public Microsoft.Xrm.Sdk.EntityReference attribute_regardingobjectid { get; set; }

	[CsvHelper.Configuration.Attributes.Name("attribute_scheduleddurationminutes")]
	public System.Int32 attribute_scheduleddurationminutes { get; set; }

	[CsvHelper.Configuration.Attributes.Name("attribute_scheduledend")]
	public System.DateTime attribute_scheduledend { get; set; }

	[CsvHelper.Configuration.Attributes.Name("attribute_scheduledstart")]
	public System.DateTime attribute_scheduledstart { get; set; }

	[CsvHelper.Configuration.Attributes.Name("attribute_statecode")]
	public Microsoft.Xrm.Sdk.OptionSetValue attribute_statecode { get; set; }

	[CsvHelper.Configuration.Attributes.Name("attribute_statuscode")]
	public Microsoft.Xrm.Sdk.OptionSetValue attribute_statuscode { get; set; }

	[CsvHelper.Configuration.Attributes.Name("attribute_subject")]
	public System.String attribute_subject { get; set; }

	[CsvHelper.Configuration.Attributes.Name("attribute_timezoneruleversionnumber")]
	public System.Int32 attribute_timezoneruleversionnumber { get; set; }

	[CsvHelper.Configuration.Attributes.Name("formatted_value_activitytypecode")]
	public string formatted_value_activitytypecode { get; set; }

	[CsvHelper.Configuration.Attributes.Name("formatted_value_actualdurationminutes")]
	public string formatted_value_actualdurationminutes { get; set; }

	[CsvHelper.Configuration.Attributes.Name("formatted_value_actualstart")]
	public string formatted_value_actualstart { get; set; }

	[CsvHelper.Configuration.Attributes.Name("formatted_value_createdby")]
	public string formatted_value_createdby { get; set; }

	[CsvHelper.Configuration.Attributes.Name("formatted_value_createdon")]
	public string formatted_value_createdon { get; set; }

	[CsvHelper.Configuration.Attributes.Name("formatted_value_isbilled")]
	public string formatted_value_isbilled { get; set; }

	[CsvHelper.Configuration.Attributes.Name("formatted_value_isregularactivity")]
	public string formatted_value_isregularactivity { get; set; }

	[CsvHelper.Configuration.Attributes.Name("formatted_value_isworkflowcreated")]
	public string formatted_value_isworkflowcreated { get; set; }

	[CsvHelper.Configuration.Attributes.Name("formatted_value_modifiedby")]
	public string formatted_value_modifiedby { get; set; }

	[CsvHelper.Configuration.Attributes.Name("formatted_value_modifiedon")]
	public string formatted_value_modifiedon { get; set; }

	[CsvHelper.Configuration.Attributes.Name("formatted_value_ownerid")]
	public string formatted_value_ownerid { get; set; }

	[CsvHelper.Configuration.Attributes.Name("formatted_value_owningbusinessunit")]
	public string formatted_value_owningbusinessunit { get; set; }

	[CsvHelper.Configuration.Attributes.Name("formatted_value_percentcomplete")]
	public string formatted_value_percentcomplete { get; set; }

	[CsvHelper.Configuration.Attributes.Name("formatted_value_prioritycode")]
	public string formatted_value_prioritycode { get; set; }

	[CsvHelper.Configuration.Attributes.Name("formatted_value_regardingobjectid")]
	public string formatted_value_regardingobjectid { get; set; }

	[CsvHelper.Configuration.Attributes.Name("formatted_value_scheduleddurationminutes")]
	public string formatted_value_scheduleddurationminutes { get; set; }

	[CsvHelper.Configuration.Attributes.Name("formatted_value_scheduledend")]
	public string formatted_value_scheduledend { get; set; }

	[CsvHelper.Configuration.Attributes.Name("formatted_value_scheduledstart")]
	public string formatted_value_scheduledstart { get; set; }

	[CsvHelper.Configuration.Attributes.Name("formatted_value_statecode")]
	public string formatted_value_statecode { get; set; }

	[CsvHelper.Configuration.Attributes.Name("formatted_value_statuscode")]
	public string formatted_value_statuscode { get; set; }

	[CsvHelper.Configuration.Attributes.Name("formatted_value_timezoneruleversionnumber")]
	public string formatted_value_timezoneruleversionnumber { get; set; }

}
