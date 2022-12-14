
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
    baseName: 'resizerimagesjx01'
    location: location
    rbacAssignments: [
      {
        roleDefinitionId: 'acdd72a7-3385-48ef-bd42-f606fba81ae7'
        principalId: managedId.outputs.principalId
      }
    ]
    containers: [
      {
        name: 'raw'
        rbacAssignments: [
          {
            roleDefinitionId: 'ba92f5b4-2d11-453d-a403-e96b0029c9fe'
            principalId: managedId.outputs.principalId
          }
        ]
      }
      {
        name: 'resized'
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
    baseName: 'resizer-app-jx01'
    location: location
    kind: 'web'
    applicationType: 'web'
  }
}

module plan 'br:crbicepmodulesjx01.azurecr.io/microsoft.web/app-service-plan:1.0.0' = {
  name: 'function-app-service-plan-deploy'
  params: {
    baseName: 'resizer-app-jx01'
    location: location
    sku: {
      name: 'Y1'
      tier: 'Dynamic'
    }
    kind: 'functionapp'
    isLinux: true
  }
}

module func 'br:crbicepmodulesjx01.azurecr.io/microsoft.web/function-app:1.1.2' = {
  name: 'function-app-deploy'
  params: {
    baseName: 'resizer-app-jx01'
    location: location
    appServicePlanId: plan.outputs.planId
    isLinux: true
    identityType: 'managed'
    managedIdentityId: managedId.outputs.resourceId
    storageAccountName: sa.outputs.storageAccountName
    applicationInsightsInstrumentationKey: appi.outputs.instrumentationKey
    appSettings: [
      {
        name: 'FUNCTIONS_EXTENSION_VERSION'
        value: '~4'
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
