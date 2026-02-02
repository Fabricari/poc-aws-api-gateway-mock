# Infrastructure

This directory will contain infrastructure-as-code (IaC) used to provision the AWS resources required for this proof of concept.

The primary focus of the infrastructure layer is the configuration of AWS API Gateway to expose mock HTTP endpoints that simulate backend behavior without invoking application compute. Supporting resources (for example, IAM roles, logging, and deployment configuration) will be added as needed.

The specific IaC tooling (e.g., CloudFormation, CDK, SAM, Terraform) has not yet been selected and will be determined based on the requirements of the mock scenarios and the desired level of fidelity.
