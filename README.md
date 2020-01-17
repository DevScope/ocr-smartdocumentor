
# ![SmartDocumentor Tools](./docs/media/smartdocumentor.png)

### [Click here to find out what's new](https://www.smartdocumentor.net)

## The Easiest way to capture data

Document processing is unavoidable for most companies. The process can be tough, rely heavily on manual workflows and limited by human capacity and speed. SmartDocumentor is an all-in-one data extraction solution that frees workers from the burden of analyzing and processing endless stacks of paper, PDFs, and images, greatly reducing manual labor and operational costs.

- Capture - SmartDocumentor’s scans any document for data, regardless of format. Be it paper, a PDF file, or a variety of image formats, SmartDocumentor can capture data from any documents, even if they are hard to read or show signs of heavy physical damage.

- Process - Once scanned, SmartDocumentor prepares documents to meet high-quality reading standards and converts them to searchable PDF files. It’s powerful engine then uses AI & Machine Learning to extract the data your company needs. The process is fast, seamless, and doesn't require human input.

- Integrate - After everything is reviewed and confirmed to be compliant, data is automatically integrated in your company’s ERP or ECM system. SmartDocumentor supports the most used systems in the market, including SAP, Microsoft Dynamics and Sage.

# SmartDocumentor Tools 
The SmartDocumentor tools are a collection of samples designed to cover end-to-end Smartdocumentor plugins development.

| Release   | Description |
|-----------------|--------------|
| 3.2.1 | Add authentication |

## Requirements:
**Pre-requisite:**

- Microsoft .NET Framework 4.6.1
- Microsoft Visual C++ 2013 Runtime Libraries
- Microsoft Visual C++ 2015 Runtime Libraries


**Firewall and Antivirus Exclusions:**

- Application installation path (by default C:\Program Files (x86)\DevScope\SmartDocumentor)
- Application configuration path (check SmartDocumentor ServiceConfig.exe file)
- Application installation logs (by default c:\temp\logs)
- User application data (%appdata%\Devscope)
- Applications installed on the application installation path (by default C:\Program Files (x86)\DevScope\SmartDocumentor)
  - SmartDocumentor.ScanStation.exe
  - SmartDocumentor.Scanner.Console.exe
  - SmartDocumentor.ManagementStation.exe
  - SmartDocumentor.ServiceConfig.exe
  - SmartDocumentor.TemplateEditor.exe
  - SmartDocumentor.ReviewStation.exe
  - SmartDocumentor.ProcessStation.Service.exe
  - SmartDocumentor.ProcessStation.Console.exe
  
  
**Local Admin Access (Read/Write):**

- SmartDocumentor ProcessStation Service
- Application installation path (by default C:\Program Files (x86)\DevScope\SmartDocumentor)
- Application configuration path (check SmartDocumentor ServiceConfig.exe file)
- Application installation logs (by default c:\temp\logs)
- Registry (64 bit machine)
  - HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\DevScope
  

**Database**

SmartDocumentor can work with SQL Server database providers. To use this type of provider you will need to have:
- SQL Server 2012 Standard Edition or higher installed
- Create a database (for example SmartDocumentor)
- Create a user with db owner permissions on the created database

In the SmartDocumentor.ServiceConfig.exe application you need to configure the provider for the created database


**Twain or ISIS Driver:**

Scanner drivers to be used on machines using the Scan Module (SmartDocumentor.ScanStation.exe) must be installed. The drivers vary depending on the scanner model.

## How-To Install:


## Reporting Security Issues
Security issues and bugs should be reported privately, via email, to the DevScope Support Team at [support@devscope.net](mailto:support@devscope.net). You should receive a response within 24 hours. If for some reason you do not, please follow up via email to ensure we received your original message. 

Copyright (c) DevScope. All rights reserved.
