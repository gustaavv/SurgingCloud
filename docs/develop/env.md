---
title: Environment Setup
---

This page covers how to set up the development environment for both the SurgingCloud .NET project and the documentation.

## SurgingCloud .NET project

The whole project is developed in .NET 6.

I am using JetBrains Rider with default settings to develop. Just import this project and you are ready to code.

## Documentation

This project use Material for MkDocs to build the documentation. Do the following steps to set it up:


Create a virtual environment:

```powershell
python -m venv venv
.\venv\Scripts\activate
```

Install the necessary packages:

```powershell
pip install -r .\requirements.txt
```

Start a live server for mkdocs:
```powershell
mkdocs serve
```
