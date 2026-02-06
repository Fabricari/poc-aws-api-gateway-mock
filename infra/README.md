# Infrastructure

This directory will contain infrastructure-as-code (IaC) used to provision the AWS resources required for this proof of concept.

The primary focus of the infrastructure layer is the configuration of AWS API Gateway to expose mock HTTP endpoints that simulate backend behavior without invoking application compute. Supporting resources (for example, IAM roles, logging, and deployment configuration) will be added as needed.

The specific IaC tooling (e.g., CloudFormation, CDK, SAM, Terraform) has not yet been selected and will be determined based on the requirements of the mock scenarios and the desired level of fidelity.

## Notes on API Gateway Mock Integrations and VTL

This proof of concept relies heavily on **API Gateway Mock integrations** and **Velocity Template Language (VTL)** to simulate realistic backend behavior (pagination, branching responses, error cases) without Lambda or other compute.

This demo intentionally favors **clarity and determinism** over perfectly abstract VTL patterns, since the goal is to illustrate what API Gateway mocks can do rather than to build a reusable VTL framework.

## Helpful Resources

### Variables for data transformations in API Gateway

When you create a parameter mapping, you can use context variables as your data source. When you create mapping template transformations, you can use context variables, input, and util variables in scripts you write in Velocity Template Language (VTL).

- AWS API Gateway Mapping Template Reference  
  https://docs.aws.amazon.com/apigateway/latest/developerguide/api-gateway-mapping-template-reference.html

- Apache Velocity Template Language Reference  
  https://velocity.apache.org/engine/devel/vtl-reference.html

- Mapping template transformations for REST APIs in API Gateway
  https://docs.aws.amazon.com/apigateway/latest/developerguide/models-mappings.html

- Override your API's request and response parameters and status codes for REST APIs in API Gateway
  https://docs.aws.amazon.com/apigateway/latest/developerguide/apigateway-override-request-response-parameters.html

