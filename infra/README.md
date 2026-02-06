# Infrastructure

This directory contains infrastructure-related resources and artifacts. Under normal circumstances, infrastructure-as-code (IaC) templates and configuration artifacts would also reside here. For the purposes of this demonstration, however, manual provisioning via the AWS Management Console is used instead (see the links below for additional provisioning documentation).

The provisioned infrastructure is intentionally limited in scope and focuses on the configuration and management of a regional AWS API Gateway that exposes public-facing mock HTTP endpoints. These endpoints simulate desired backend behavior without invoking any application compute (e.g., Lambda, EC2, etc.). Resources should be torn down once testing is complete. If this solution is expected to be deployed frequently, an IaC approach is recommended to ensure repeatability. That said, the primary goal of this proof-of-concept is to become familiar with the API Gateway service itself, and premature automation may detract from that objective.

## Notes on API Gateway Mock Integrations and VTL

This proof of concept relies heavily on **API Gateway mock integrations** and **Velocity Template Language (VTL)** to simulate realistic backend behavior, including pagination, branching responses, and error scenarios, without the use of Lambda or other compute services. The demonstration intentionally favors **clarity and determinism** over perfectly abstracted VTL patterns, as the objective is to illustrate the capabilities of API Gateway mocks rather than to build a generalized or reusable VTL framework.

## Helpful Resources

- AWS API Gateway Mapping Template Reference  
  https://docs.aws.amazon.com/apigateway/latest/developerguide/api-gateway-mapping-template-reference.html

- Apache Velocity Template Language Reference  
  https://velocity.apache.org/engine/devel/vtl-reference.html

- Mapping template transformations for REST APIs in API Gateway  
  https://docs.aws.amazon.com/apigateway/latest/developerguide/models-mappings.html

- Override request and response parameters and status codes for REST APIs in API Gateway  
  https://docs.aws.amazon.com/apigateway/latest/developerguide/apigateway-override-request-response-parameters.html
