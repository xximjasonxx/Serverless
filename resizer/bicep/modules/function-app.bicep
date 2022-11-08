
param baseName string
param location string

param appServicePlanId string = ''
param isLinux bool = false

@allowed([
  'none'
  'system'
  'managed'
])
param identityType string

@allowed([
  '1'
  '2'
  '3'
  '4'
])
param runtimeVersion string = '4'

param managedIdentityId string = ''
param appSettings array = []

param applicationInsightsInstrumentationKey string = ''
param storageAccountName string = ''

// existing
resource storageAccount 'Microsoft.Storage/storageAccounts@2022-05-01' existing = if (storageAccountName != '') {
  name: storageAccountName
}

// create values
var identityBlock = identityType == 'none' ? null : (identityType == 'system' ? {
  type: 'SystemAssigned'
} : {
  type: 'UserAssigned'
  userAssignedIdentities: {
    '${managedIdentityId}': {}
  }
})

var appKind = isLinux ? 'functionapp,linux' : 'functionapp'
var extensionVersionRuntime = '~${runtimeVersion}'
var storageAccountConnectionString = (storageAccountName != '') ? listKeys(storageAccount.id, '2022-05-01').keys[0].value : ''

// build common app settings
var insightsAppSettings = applicationInsightsInstrumentationKey == '' ? [] : [
  {
    name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
    value: applicationInsightsInstrumentationKey
  }
  {
    name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
    value: 'InstrumentationKey=${applicationInsightsInstrumentationKey}'
  }
]

var azureWebJobsStorageAppSettings = storageAccountConnectionString == '' ? [] : [
  {
    name: 'AzureWebJobsStorage'
    value: storageAccountConnectionString
  }
]

var runtimeVersionAppSetting = [
  {
    name: 'FUNCTIONS_EXTENSION_VERSION'
    value: extensionVersionRuntime
  }
]

var finalAppettings = union(appSettings, insightsAppSettings, azureWebJobsStorageAppSettings, runtimeVersionAppSetting)

// resources
resource func 'Microsoft.Web/sites@2022-03-01' = {
  name: 'func-${baseName}'
  location: location
  kind: appKind
  identity: identityBlock

  properties: {
    serverFarmId: appServicePlanId
    siteConfig: {
      appSettings: finalAppettings
    }
    httpsOnly: true
  }
}

// app

// outputs
output funcName string = func.name
output funcId string = func.id
output funcDefaultHostName string = func.properties.defaultHostName
output funcKind string = func.kind
