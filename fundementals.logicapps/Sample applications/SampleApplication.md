# Sample application
This sample application was designed to illustrate a few real world concepts. THis includes message retries, security, monitoring and deployment.

## Scenario
The scenario hinges around the problem of abandoned vehicles. A (imagined) web site allows members of the public to report abandoned vehicles using an online form (not in the sample). 

Reports can optionally contain photos as evidence. Reports are processed into a back end system using an API. If the registration number already exists then the report is flagged up to let the person know that it has already been reported.

Vehicles may represent a safety risk, so any vehicles deemed as a safety risk are sent to a SharePoint document library with their images (if present) so they can be investigated as a priority.

Once the report is processed an online database is updated with the details so members of the public can see if someone else has reported the issue.

## Building out the sample
The service bus lab folder contains a console app that generates service bus messages and uploads files to blob storage.


