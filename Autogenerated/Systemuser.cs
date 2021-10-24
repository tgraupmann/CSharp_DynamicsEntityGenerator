﻿[DynamicsEntityGenerator.TableName("systemuser")]
public class Systemuser : DynamicsEntityGenerator.IAutogeneratedClass
{
	public Microsoft.Xrm.Sdk.OptionSetValue attribute_accessmode { get; set; }
	public System.Guid attribute_address1_addressid { get; set; }
	public Microsoft.Xrm.Sdk.OptionSetValue attribute_address1_addresstypecode { get; set; }
	public System.String attribute_address1_composite { get; set; }
	public System.String attribute_address1_country { get; set; }
	public Microsoft.Xrm.Sdk.OptionSetValue attribute_address1_shippingmethodcode { get; set; }
	public System.Guid attribute_address2_addressid { get; set; }
	public Microsoft.Xrm.Sdk.OptionSetValue attribute_address2_addresstypecode { get; set; }
	public Microsoft.Xrm.Sdk.OptionSetValue attribute_address2_shippingmethodcode { get; set; }
	public System.Guid attribute_applicationid { get; set; }
	public System.String attribute_applicationiduri { get; set; }
	public System.Guid attribute_azureactivedirectoryobjectid { get; set; }
	public Microsoft.Xrm.Sdk.EntityReference attribute_businessunitid { get; set; }
	public Microsoft.Xrm.Sdk.EntityReference attribute_calendarid { get; set; }
	public Microsoft.Xrm.Sdk.OptionSetValue attribute_caltype { get; set; }
	public Microsoft.Xrm.Sdk.EntityReference attribute_createdby { get; set; }
	public System.DateTime attribute_createdon { get; set; }
	public System.Boolean attribute_defaultfilterspopulated { get; set; }
	public Microsoft.Xrm.Sdk.EntityReference attribute_defaultmailbox { get; set; }
	public System.String attribute_defaultodbfoldername { get; set; }
	public Microsoft.Xrm.Sdk.OptionSetValue attribute_deletedstate { get; set; }
	public System.Boolean attribute_displayinserviceviews { get; set; }
	public System.String attribute_domainname { get; set; }
	public Microsoft.Xrm.Sdk.OptionSetValue attribute_emailrouteraccessapproval { get; set; }
	public System.Decimal attribute_exchangerate { get; set; }
	public System.String attribute_firstname { get; set; }
	public System.String attribute_fullname { get; set; }
	public System.Int32 attribute_identityid { get; set; }
	public Microsoft.Xrm.Sdk.OptionSetValue attribute_incomingemaildeliverymethod { get; set; }
	public System.String attribute_internalemailaddress { get; set; }
	public Microsoft.Xrm.Sdk.OptionSetValue attribute_invitestatuscode { get; set; }
	public System.Boolean attribute_isdisabled { get; set; }
	public System.Boolean attribute_isemailaddressapprovedbyo365admin { get; set; }
	public System.Boolean attribute_isintegrationuser { get; set; }
	public System.Boolean attribute_islicensed { get; set; }
	public System.Boolean attribute_issyncwithdirectory { get; set; }
	public System.String attribute_lastname { get; set; }
	public Microsoft.Xrm.Sdk.EntityReference attribute_modifiedby { get; set; }
	public System.DateTime attribute_modifiedon { get; set; }
	public Microsoft.Xrm.Sdk.EntityReference attribute_modifiedonbehalfby { get; set; }
	public Microsoft.Xrm.Sdk.OptionSetValue attribute_msdyn_agentType { get; set; }
	public Microsoft.Xrm.Sdk.OptionSetValue attribute_msdyn_botprovider { get; set; }
	public System.Int32 attribute_msdyn_capacity { get; set; }
	public Microsoft.Xrm.Sdk.EntityReference attribute_msdyn_defaultpresenceiduser { get; set; }
	public System.Boolean attribute_msdyn_gdproptout { get; set; }
	public Microsoft.Xrm.Sdk.OptionSetValue attribute_msdyn_usertype { get; set; }
	public System.Guid attribute_organizationid { get; set; }
	public Microsoft.Xrm.Sdk.OptionSetValue attribute_outgoingemaildeliverymethod { get; set; }
	public Microsoft.Xrm.Sdk.OptionSetValue attribute_preferredaddresscode { get; set; }
	public Microsoft.Xrm.Sdk.OptionSetValue attribute_preferredemailcode { get; set; }
	public Microsoft.Xrm.Sdk.OptionSetValue attribute_preferredphonecode { get; set; }
	public Microsoft.Xrm.Sdk.EntityReference attribute_queueid { get; set; }
	public System.Boolean attribute_setupuser { get; set; }
	public System.Guid attribute_systemuserid { get; set; }
	public Microsoft.Xrm.Sdk.EntityReference attribute_transactioncurrencyid { get; set; }
	public System.Int32 attribute_userlicensetype { get; set; }
	public System.String attribute_userpuid { get; set; }
	public System.String attribute_windowsliveid { get; set; }
	public System.String attribute_yomifullname { get; set; }
	public string formatted_value_accessmode { get; set; }
	public string formatted_value_address1_addresstypecode { get; set; }
	public string formatted_value_address1_shippingmethodcode { get; set; }
	public string formatted_value_address2_addresstypecode { get; set; }
	public string formatted_value_address2_shippingmethodcode { get; set; }
	public string formatted_value_businessunitid { get; set; }
	public string formatted_value_caltype { get; set; }
	public string formatted_value_createdby { get; set; }
	public string formatted_value_createdon { get; set; }
	public string formatted_value_defaultfilterspopulated { get; set; }
	public string formatted_value_defaultmailbox { get; set; }
	public string formatted_value_deletedstate { get; set; }
	public string formatted_value_displayinserviceviews { get; set; }
	public string formatted_value_emailrouteraccessapproval { get; set; }
	public string formatted_value_exchangerate { get; set; }
	public string formatted_value_identityid { get; set; }
	public string formatted_value_incomingemaildeliverymethod { get; set; }
	public string formatted_value_invitestatuscode { get; set; }
	public string formatted_value_isdisabled { get; set; }
	public string formatted_value_isemailaddressapprovedbyo365admin { get; set; }
	public string formatted_value_isintegrationuser { get; set; }
	public string formatted_value_islicensed { get; set; }
	public string formatted_value_issyncwithdirectory { get; set; }
	public string formatted_value_modifiedby { get; set; }
	public string formatted_value_modifiedon { get; set; }
	public string formatted_value_modifiedonbehalfby { get; set; }
	public string formatted_value_msdyn_agentType { get; set; }
	public string formatted_value_msdyn_botprovider { get; set; }
	public string formatted_value_msdyn_capacity { get; set; }
	public string formatted_value_msdyn_defaultpresenceiduser { get; set; }
	public string formatted_value_msdyn_gdproptout { get; set; }
	public string formatted_value_msdyn_usertype { get; set; }
	public string formatted_value_outgoingemaildeliverymethod { get; set; }
	public string formatted_value_preferredaddresscode { get; set; }
	public string formatted_value_preferredemailcode { get; set; }
	public string formatted_value_preferredphonecode { get; set; }
	public string formatted_value_queueid { get; set; }
	public string formatted_value_setupuser { get; set; }
	public string formatted_value_transactioncurrencyid { get; set; }
	public string formatted_value_userlicensetype { get; set; }
}
