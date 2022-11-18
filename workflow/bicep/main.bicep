
targetScope = 'resourceGroup'
param location string = resourceGroup().location

// managed identity
module managedIdentity 'br:crbicepmodulesjx01.azurecr.io/microsoft.identity/user-managed-identity:1.0.0' = {
  name: 'managed-identity-deploy'
  params: {
    baseName: 'workfow-app'
    location: location
  }
}

resource cogText 'Microsoft.CognitiveServices/accounts@2022-03-01' = {
  name: 'cog-lang-serverless-jx03'
  location: location
  sku: {
    name: 'S0'
  }

  identity: {
    type: 'SystemAssigned'
  }

  kind: 'CognitiveServices'

  properties: {
    apiProperties: {}
    customSubDomainName: 'cog-serverless-jx03'
    networkAcls: {
        defaultAction: 'Allow'
        virtualNetworkRules: []
        ipRules: []
    }
    publicNetworkAccess: 'Enabled'
  }
}

// application insights
module appi 'br:crbicepmodulesjx01.azurecr.io/microsoft.insights/application-insights:1.0.0' = {
  name: 'application-insights-deploy'
  params: {
    baseName: 'workflow-app-jx02'
    location: location
    kind: 'web'
    applicationType: 'web'
  }
}

// server farm
module plan 'br:crbicepmodulesjx01.azurecr.io/microsoft.web/app-service-plan:1.0.0' = {
  name: 'function-app-service-plan-deploy'
  params: {
    baseName: 'workflow-app-jx02'
    location: location
    sku: {
      name: 'Y1'
      tier: 'Dynamic'
    }
    kind: 'functionapp'
    isLinux: true
  }
}

// storage account
module sa 'br:crbicepmodulesjx01.azurecr.io/microsoft.storage/account:1.0.1' = {
  name: 'storage-account-deploy'
  params: {
    baseName: 'serverlessimagesjx02'
    location: location
    rbacAssignments: [
      {
        roleDefinitionId: 'acdd72a7-3385-48ef-bd42-f606fba81ae7'
        principalId: managedIdentity.outputs.principalId
      }
    ]
    containers: [
      {
        name: 'received'
        rbacAssignments: [
          {
            roleDefinitionId: 'ba92f5b4-2d11-453d-a403-e96b0029c9fe'
            principalId: managedIdentity.outputs.principalId
          }
        ]
      }
    ]
  }

  dependsOn: [
    managedIdentity
    cogText
  ]
}

// cosmosdb
module cosmos 'br:crbicepmodulesjx01.azurecr.io/microsoft.documentdb/account:1.1.1' = {
  name: 'cosmosdb-deploy'
  params: {
    base_name: 'serverlessimages-ym01'
    location: location
    sql_databases: [
      {
        name: 'images'
        containers: [
          {
            name: 'image_data'
            partition_keys: [
              '/blobName'
            ]
          }
        ]
      }
    ]
    rbac_assignments: [
      {
        principalId: managedIdentity.outputs.principalId
        roleDefinitionId: '5bd9cd88-fe45-4216-938b-f97437e15450'
      }
    ]
  }
}

// function app
module func 'br:crbicepmodulesjx01.azurecr.io/microsoft.web/function-app:1.1.1' = {
  name: 'function-app-deploy'
  params: {
    baseName: 'image-api-jx02'
    location: location
    appServicePlanId: plan.outputs.planId
    isLinux: true
    identityType: 'managed'
    managedIdentityId: managedIdentity.outputs.resourceId
    storageAccountName: sa.outputs.storageAccountName
    applicationInsightsInstrumentationKey: appi.outputs.instrumentationKey
    appSettings: [
      {
        name: 'FUNCTIONS_EXTENSION_VERSION'
        value: '~4'
      }
      {
        name: 'StorageAccountConnection__clientId'
        value: managedIdentity.outputs.clientId
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
        name: 'CosmosDBConnection__clientId'
        value: managedIdentity.outputs.clientId
      }
      {
        name: 'CosmosDBConnection__credential'
        value: 'managedidentity'
      }
      {
        name: 'CosmosDBConnection__serviceUri'
        value: cosmos.outputs.cosmosdb_endpoint
      }
    ]
  }
}
