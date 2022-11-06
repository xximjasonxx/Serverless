
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
param managedIdentityId string = ''
param appSettings array = []

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

// resources
resource func 'Microsoft.Web/sites@2022-03-01' = {
  name: 'func-${baseName}'
  location: location
  kind: appKind
  identity: identityBlock

  properties: {
    serverFarmId: appServicePlanId
    siteConfig: {
      appSettings: appSettings
    }
    httpsOnly: true
  }
}

// outputs
output funcName string = func.name
output funcId string = func.id
output funcDefaultHostName string = func.properties.defaultHostName
output funcKind string = func.kind
