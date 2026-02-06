# AWS API Gateway Mock API (Proof of Concept)
## poc-aws-api-gateway-mock

This repository contains a proof of concept exploring the use of AWS API Gateway as a viable Mock API solution to enable client development independent of backend dependency services.

## Overview

The artifacts in this repository include:

- A test project with a sampling of transport-agnostic test cases, using a fictitious **Library Catalog** scenario.
- A class library responsible for the Library Catalog business logic and AWS API Gateway transport communication.
- An `infra` directory containing supporting artifacts such as resource links and Velocity Template Language (`.vtl`) files used to configure API Gateway mock integrations.

## Scope and Intent

This code is provided for demonstration purposes only and should not be used in production. 

I chose to emphasize a few best practices that were of interest to me, such as keeping the Library Catalog business logic agnostic of AWS-specific details through the use of dependency injection within the tests. I also chose to develop this proof of concept using the newly released .NET 10 (LTS) to familiarize myself with some of the syntactic sugar introduced in recent versions of C#. 

For simplicity, the client is run locally, and the API endpoints are configured to be publicly accessible without any security or authentication.

## Cost and Resource Management

Using AWS API Gateway as a mock backend for testing without any provisioned backend services is relatively inexpensive (typically costing only pennies). Even so, resources should be torn down once testing is complete. Ideally, provisioning and teardown should be scripted to make the process repeatable and to minimize ongoing cost.

## Provisioning Approach

For the accompanying video demonstration, manual provisioning is performed via the AWS Management Console. However, an infrastructure-as-code (IaC) approach is strongly recommended. IaC enables rapid creation and teardown of resources, helping to reduce both operational risk and cost.
