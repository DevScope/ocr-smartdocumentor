---
layout: default
title: Home
---

SmartDocumentor - Technical Documentation

DevScope


|  #Version        | Date             | Changes          | Author          |
| ---------------- |:----------------:|:----------------:|----------------:|
| V 1.0            | 17-02-2020       | Initial Document |Luís Maia        |

                                          

Contacts


| DevScope | João Sousa                 |
| ---      | ---                        |
|          | <joao.sousa@devscope.net>  |
|          | +315 966 380 757           |


Copyright © DevScope

All information in this document is confidential and for the exclusive
access of SmartDocumentor\
customers. Access to this document is not permitted to any other entity
without prior from DevScope. Any entity with access to this document, is
obliged to the\
confidentiality of the same.

Index
=====

[1. Introduction ](#introduction)

[1.1 Overview ](#overview)

[1.1.1 Scanning - Scan Station ](#scanning---scan-station)

[1.1.2 Processing - Process Station ](#processing---process-station)

[1.1.3 Review - Review Station ](#review---review-station)

[1.1.4 Templates - Template Editor ](#templates---template-editor)

[1.1.5 Management - Management Station
](#management---management-station)

[1.1.6 SmartDocumentor's Assumptions ](#smartdocumentors-assumptions)

[1.1.7 Workflow ](#workflow)

[1.2 Extensibility and future versions
](#extensibility-and-future-versions)

[2. Customize SmartDocumentor ](#customize-smartdocumentor)

[2.1 Client Configuration ](#client-configuration)

[2.1.1 Service Configuration ](#service-configuration)

[2.1.2 Storage Configuration ](#storage-configuration)

[2.1.3 Plafond ](#plafond)

[2.2 Workflow ](#workflow-1)

[2.2.1 Workspace.config.xml ](#workspace.config.xml)

[2.2.2 Local Vs Global ](#local-vs-global)

[2.2.3 Scan Station ](#scan-station)

[2.2.4 Review Station ](#review-station)

[2.2.5 Process Station ](#process-station)

[2.3 Project Structure ](#project-structure)

[2.4 Workers ](#workers)

[2.4.1 SmartDocumentor ](#smartdocumentor)

[2.4.2 Custom Worker ](#custom-worker)

[2.5 Plugins ](#plugins)

[2.5.1 Scan Plugin ](#scan-plugin)

[2.5.2 Review Plugin ](#review-plugin)

[2.6 Debug ](#debug)

[3. Installation ](#installation)

[3.1 Checklist ](#checklist)

[3.1.1 Requirements ](#requirements)

[3.1.2 Firewall and Antivirus Exclusions (Servers & Clients)
](#firewall-and-antivirus-exclusions-servers-clients)

[3.2 Server Installation ](#server-installation)

[3.2.1 Install Setup ](#install-setup)

[3.2.2 Change the user running the windows service
](#change-the-user-running-the-windows-service)

[3.2.3 Copy the customized configuration
](#copy-the-customized-configuration)

[3.2.4 Configure the configuration path
](#configure-the-configuration-path)

[3.2.5 Extract the HDI to generate the license
](#extract-the-hdi-to-generate-the-license)

[3.2.6 Activate SmartDocumentor license
](#activate-smartdocumentor-license)

[3.2.7 Activate SmartDocumentor Plafond
](#activate-smartdocumentor-plafond)

[3.3 Review and Scan Station Installation
](#review-and-scan-station-installation)

[3.3.1 Install Setup ](#install-setup-1)

[3.3.2 Copy the customized configuration
](#copy-the-customized-configuration-1)

[3.3.3 Configure the configuration path
](#configure-the-configuration-path-1)

[3.3.4 Extract the HDI to generate the license
](#extract-the-hdi-to-generate-the-license-1)

[3.3.5 Activate SmartDocumentor license
](#activate-smartdocumentor-license-1)

[3.4 Update SmartDocumentor ](#update-smartdocumentor)

[3.5 Update SmartDocumentor Plugin ](#update-smartdocumentor-plugin)

[4. Troubleshooting Overview ](#troubleshooting-overview)

[4.1 Introduction ](#introduction-1)

[4.2 Log folder ](#log-folder)

[4.3 Database ](#database)

[4.3.1 Tables ](#tables)

[4.3.2 Queue States ](#queue-states)

[4.3.3 SmartDocumentor Task XML ](#smartdocumentor-task-xml)

[4.3.4 Documents in Tasks ](#documents-in-tasks)

Introduction
============

