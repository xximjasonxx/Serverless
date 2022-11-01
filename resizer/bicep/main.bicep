
targetScope = 'resourceGroup'

param location string = resourceGroup().location

// identity
resource id 'Microsoft.ManagedIdentity/userAssignedIdentities@2022-01-31-preview' = {
  name: 'id-serverless-example'
  location: location
}

// storage account
module sa 'modules/storageAccount.bicep' = {
  name: 'storage-account-deploy'
  params: {
    location: location
    identityPrincipalId: id.properties.principalId
    containers: [
      'raw'
      'resized'
    ]
  }
}

// contianer role assignments
module raw_container_role_assignment 'modules/container-roleAssignment.bicep' = {
  name: 'raw-container-role-assignment'
  params: {
    storageAccountName: sa.outputs.storageAccountName
    containerName: 'raw'
    identityPrincipalId: id.properties.principalId
    roleDefinitionId: '2a2b9908-6ea1-4ae2-8e65-a410df84e7d1'
  }
}

module resized_container_role_assignment 'modules/container-roleAssignment.bicep' = {
  name: 'resized-container-role-assignment'
  params: {
    storageAccountName: sa.outputs.storageAccountName
    containerName: 'reszied'
    identityPrincipalId: id.properties.principalId
    roleDefinitionId: 'ba92f5b4-2d11-453d-a403-e96b0029c9fe'
  }
}

// function app
resource plan 'Microsoft.Web/serverfarms@2021-03-01' = {
  name: 'plan-func-image-api-jx01'
  location: location
  kind: 'linux'
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
  }
  properties: {
    reserved: true
  }
}

resource appi 'Microsoft.Insights/components@2020-02-02' = {
  name: 'appi-func-image-api-jx01'
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
  }
}

var webJobsStorageAccountConnectionString = 'DefaultEndpointsProtocol=https;AccountName=${sa.outputs.storageAccountName};AccountKey=${sa.outputs.storageAccountKey};EndpointSuffix=${environment().suffixes.storage}'
resource func 'Microsoft.Web/sites@2022-03-01' = {
  name: 'func-image-api-jx01'
  location: location
  kind: 'functionapp'
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${id.id}': {} 
    }
  }

  properties: {
    serverFarmId: plan.id
    siteConfig: {
      
      appSettings: [
        {
          name: 'AzureWebJobsStoageAccountConnection_serviceUri'
          value: 'https://${sa.outputs.storageAccountName}.blob.${environment().suffixes.storage}'
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: appi.properties.InstrumentationKey
        }
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: 'InstrumentationKey=${appi.properties.InstrumentationKey}'
        }
        {
          name: 'AzureWebJobsStorage'
          value: webJobsStorageAccountConnectionString
        }
      ]
    }
    httpsOnly: true
  }
}
