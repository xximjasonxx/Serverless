
targetScope = 'resourceGroup'

param location string = resourceGroup().location

// identity
module managedId 'br:crbicepmodulesjx01.azurecr.io/microsoft.identity/user-managed-identity:1.0.0' = {
  name: 'managed-identity-deploy'
  params: {
    baseName: 'resizer-app'
    location: location
  }
}

// storage account
module sa 'br:crbicepmodulesjx01.azurecr.io/microsoft.storage/account:1.0.0' = {
  name: 'storage-account-deploy'
  params: {
    baseName: 'serverlessimagesjx01'
    location: location
    rbacAssignments: [
      {
        roleDefinitionId: 'acdd72a7-3385-48ef-bd42-f606fba81ae7'
        principalId: managedId.outputs.principalId
      }
    ]
    containers: [
      {
        name: 'no-rbac'
      }
      {
        name: 'rbac'
        rbacAssignments: [
          {
            roleDefinitionId: 'ba92f5b4-2d11-453d-a403-e96b0029c9fe'
            principalId: managedId.outputs.principalId
          }
        ]
      }
    ]
  }
}


// function app
module appi 'br:crbicepmodulesjx01.azurecr.io/microsoft.insights/application-insights:1.0.0' = {
  name: 'application-insights-deploy'
  params: {
    baseName: 'image-api-jx01'
    location: location
    kind: 'web'
    applicationType: 'web'
  }
}

module plan 'br:crbicepmodulesjx01.azurecr.io/microsoft.web/app-service-plan:1.0.0' = {
  name: 'function-app-service-plan-deploy'
  params: {
    baseName: 'image-api-jx01'
    location: location
    sku: {
      name: 'Y1'
      tier: 'Dynamic'
    }
    kind: 'functionapp'
    isLinux: true
  }
}

module func 'br:crbicepmodulesjx01.azurecr.io/microsoft.web/function-app:1.0.0' = {
  name: 'function-app-deploy'
  params: {
    baseName: 'image-api-jx01'
    location: location
    appServicePlanId: plan.outputs.planId
    isLinux: true
    identityType: 'managed'
    managedIdentityId: managedId.outputs.resourceId
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
        value: appi.outputs.instrumentationKey
      }
      {
        name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
        value: 'InstrumentationKey=${appi.outputs.instrumentationKey}'
      }
      {
        name: 'AzureWebJobsStorage'
        value: 'DefaultEndpointsProtocol=https;AccountName=${sa.outputs.storageAccountName};AccountKey=${sa.outputs.storageAccountKey};EndpointSuffix=${environment().suffixes.storage}'
      }
      {
        name: 'StorageAccountConnection__clientId'
        value: managedId.outputs.clientId
      }
      {
        name: 'StorageAccountConnection__credential'
        value: 'managedidentity'
      }
      {
        name: 'StorageAccountConnection__serviceUri'
        value: 'https://${sa.outputs.storageAccountName}.blob.${environment().suffixes.storage}'
      }
      {
        name: 'WEBSITE_RUN_FROM_PACKAGE'
        value: '1'
      }
    ]
  }
}
