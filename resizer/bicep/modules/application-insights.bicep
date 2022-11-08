
param baseName string
param location string 

@allowed([
  'web'
  'ios'
  'java'
  'store'
  'other'
  'phone'
])
param kind string

@allowed([
  'web'
  'other'
])
param applicationType string

resource appi 'Microsoft.Insights/components@2020-02-02' = {
  name: 'appi-${baseName}'
  location: location
  kind: kind
  properties: {
    Application_Type: applicationType
  }
}

// outputs
output resourceId string = appi.id
output instrumentationKey string = appi.properties.InstrumentationKey
output resourceName string = appi.name
