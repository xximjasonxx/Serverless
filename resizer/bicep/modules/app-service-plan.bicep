
param baseName string
param location string
param sku object

@allowed([
  'api'
  'app'
  'functionapp'
])
param kind string
param isLinux bool = false

var planKind = isLinux ? '${kind},linux' : kind
resource plan 'Microsoft.Web/serverfarms@2021-03-01' = {
  name: 'plan-${baseName}'
  location: location
  kind: planKind
  sku: {
    name: sku.name
    tier: sku.tier
  }
  properties: {
    reserved: isLinux
  }
}

// outputs
output planName string = plan.name
output planId string = plan.id
output planKind string = planKind
